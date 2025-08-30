using CLTelegramBot;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;
using System.Globalization;
using System.Security.Policy;
using System.Text;

namespace NovelSiteMVC.BussinessLogic
{
    public class IOUtility
    {
        
    }
    public class TextUtility
    {

    }
    public class DBUtility
    {

    }
    public interface IONUtility
    {
        public Task SendDailyTaskList();
        public Task SendDailyTaskList(long chatId);
    }
    /// <summary>
    /// Over Network Utilites like send a message to an external api
    /// </summary>
    public class ONUtility : IONUtility
    {
        private AppDbContext context;
        public static long MeenaId = 1986364230;
        public static long MeId = 2062729545;
        //MeenaId = 1986364230;
        //MeId = 2062729545;
        
        public ONUtility(AppDbContext _context)
        {
            context = _context;
        }
        public async Task SendDailyTaskList()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ar-IQ");
            
            var tasks = await context.tblTodos.AsNoTracking()
                .Where(x => !x.isDeleted
                && (x.Status == StatusType.Active||
                    x.Status == StatusType.HasDueDate ||
                    x.Status == StatusType.Passed) &&
                (x.DueDate == null||
                    x.DueDate.Value.Date == DateTime.UtcNow.Date))
                .Select(x => new Todo_DueDateTask(){ Task = x.Task, DueDate = x.DueDate?? DateTime.Now })
                .OrderBy(t => t.DueDate)
                .ToListAsync();
            
            if (!tasks.Any())
            {
                await TelegramUtilities.SendTextMessage(MeenaId, "لا توجد مهام لهذا اليوم");
                return;
            }

            string formattedMessage = FormatDailyTaskMessage(tasks);
            // إرسال الرسالة المنسقة
            await TelegramUtilities.SendTextMessage(MeenaId, formattedMessage);
        }
        public async Task SendDailyTaskList(long chatId)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ar-IQ");
            
            var tasks = await context.tblTodos.AsNoTracking()
                .Where(x => !x.isDeleted && (x.DueDate == null || x.DueDate.Value.Date == DateTime.UtcNow.Date))
                .Select(x => new Todo_DueDateTask(){ Task = x.Task, DueDate = x.DueDate?? DateTime.Now })
                .OrderBy(t => t.DueDate)
                .ToListAsync();
            
            if (!tasks.Any())
            {
                await TelegramUtilities.SendTextMessage(MeenaId, "لا توجد مهام لهذا اليوم");
                return;
            }

            string formattedMessage = FormatDailyTaskMessage(tasks);
            // إرسال الرسالة المنسقة
            await TelegramUtilities.SendTextMessage(chatId, formattedMessage);
        }

        private string FormatDailyTaskMessage(List<Todo_DueDateTask> tasks)
        {
            // إنشاء نص الرسالة مع تنسيق Markdown
            var messageBuilder = new StringBuilder();

            // العنوان الملون داخل نص بارز
            messageBuilder.AppendLine(">مهام اليوم مرتبة `" + DateTime.Today.ToString("dd/MM/yyyy") + "`");
            messageBuilder.AppendLine();

            // إضافة المهام مع تنسيقها
            foreach (var task in tasks)
            {
                if (task.DueDate is null)
                    task.DueDate = DateTime.Now;
                // تنسيق الوقت بخط ثقيل
                messageBuilder.Append($"الساعة* {task.DueDate.Value.ToLocalTime().ToString("hh:mm tt")}: *");

                // عنوان المهمة بخط عادي
                messageBuilder.Append($"||{task.Task}||");

                messageBuilder.AppendLine();
                messageBuilder.AppendLine();
            }
            return messageBuilder.ToString();
        }

        public static bool CheckIfNotificationSentToday()
        {
            try
            {
                string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "notification_logs.txt");

                // إذا لم يكن الملف موجوداً، فإن الإشعار لم يتم إرساله
                if (!File.Exists(logFilePath))
                    return false;

                // قراءة الملف وتحقق من تاريخ اليوم
                string todayDate = DateTime.Today.ToString("yyyy-MM-dd");
                string[] logs = File.ReadAllLines(logFilePath);

                return logs.Any(log => log.Contains($"DailyTaskList:{todayDate}"));
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                Console.WriteLine($"Error checking notification log: {ex.Message}");
                return false; // في حالة الخطأ، نفترض أن الإشعار لم يتم إرساله
            }
        }

        public static void LogNotificationSent()
        {
            try
            {
                string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "notification_logs.txt");
                string logEntry = $"DailyTaskList:{DateTime.Today.ToString("yyyy-MM-dd")}";

                // إضافة سجل جديد إلى الملف
                File.AppendAllLines(logFilePath, new[] { logEntry });
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                Console.WriteLine($"Error logging notification: {ex.Message}");
            }
        }
    }
}
