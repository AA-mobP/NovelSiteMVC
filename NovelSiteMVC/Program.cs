using CLTelegramBot;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.BussinessLogic;
using NovelSiteMVC.Controllers;
using NovelSiteMVC.Models;
using System.Configuration;

namespace NovelSiteMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false; ;
                options.Password.RequireDigit = false; ;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddDbContext<AppDbContext>(contextOptions =>
             {
                 //contextOptions.UseSqlServer(builder.Configuration.GetConnectionString("localSqlServer"));
                 contextOptions.UseSqlServer(builder.Configuration.GetConnectionString("HostSqlServer"));
             });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            builder.Services.AddMemoryCache();
            //builder.Services.AddHangfire(options =>
            //{
            //    //options.UseSqlServerStorage(builder.Configuration.GetConnectionString("localSqlServer"));
            //    options.UseSqlServerStorage(builder.Configuration.GetConnectionString("HostSqlServer"));
            //});
            //builder.Services.AddHangfireServer();

            //Custom Services
            builder.Services.AddScoped<IONUtility, ONUtility>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            TelegramUtilities.ConfigBot();

            app.MapGet("/api/check-daily-tasks", async (HttpContext context) =>
            {
                // في أي مكان يناسب بدء التشغيل (مثل متوسط البرمجيات middleware)

                // عندما يتم الوصول إلى التطبيق بعد إعادة التشغيل
                IONUtility utility = context.RequestServices.GetRequiredService<IONUtility>();

                // تحقق مما إذا كان وقت إرسال الإشعار قد فات ولم يتم الإرسال
                DateTime now = DateTime.UtcNow;
                // تحقق من السجلات أو قاعدة البيانات لمعرفة ما إذا كان الإشعار قد تم إرساله بالفعل
                bool alreadySent = ONUtility.CheckIfNotificationSentToday();
                if (!alreadySent)
                {
                    // إرسال الإشعار مباشرة عند الوصول
                    await utility.SendDailyTaskList();
                }
                //app.UseHangfireDashboard("/hangfire");


            });
            //RecurringJob.AddOrUpdate<IONUtility>("SendDailyTaskList", x => x.SendDailyTaskList(), Cron.Daily(3));
            ////RecurringJob.RemoveIfExists("SendDailyTaskList");

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
