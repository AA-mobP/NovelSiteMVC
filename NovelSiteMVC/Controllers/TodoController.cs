using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLTelegramBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;
using Polly;

namespace NovelSiteMVC.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Todo
        public IActionResult Index()
        {
            return View();
        }

        // GET: Todo/GetItems
        public IActionResult GetItems()
        {
            var todoList = _context.tblTodos
                .AsNoTracking()
                .Where(x => x.isDeleted == false)
                .Select(x => new { x.Id, x.Task, x.DueDate, x.Status })
                .OrderBy(x => x.Status)
                .ToList();
            return Json(new { list = todoList });
        }

        // POST: Todo/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(string task, DateTime? dueDate)
        {
            if (string.IsNullOrEmpty(task))
                return Json(new { success = false });

            var todoModel = new TodoModel();
            todoModel.Task = task;
            todoModel.DueDate = dueDate?.ToUniversalTime();

            if (dueDate is null)
            {
                dueDate = DateTime.UtcNow;
                todoModel.Status = StatusType.Active;
            }
            else
                todoModel.Status = StatusType.HasDueDate;

            _context.Add(todoModel);
            _context.SaveChanges();
            int i = todoModel.Id;

            return Json(new { id = todoModel.Id, status = todoModel.Status });
        }

        // POST: Todo/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditTask(int id, string task)
        {
            var original = _context.tblTodos.Find(id);
            if (original is null || original.Id != id || string.IsNullOrEmpty(task))
                return Json(new { success = false });

            original.Task = task;
            _context.SaveChanges();

            return Json(new { success = true });
        }
        
        // POST: Todo/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditStatus(int id, StatusType status)
        {
            var original = _context.tblTodos.Find(id);
            if (original is null || original.Id != id)
                return Json(new { success = false });

            original.Status = status;
            _context.SaveChanges();

            return Json(new { success = true });
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var todoModel = _context.tblTodos.Find(id);
            if (todoModel is null)
                return BadRequest();

            todoModel.isDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool TodoModelExists(int id)
        {
            return _context.tblTodos.Any(e => e.Id == id);
        }
    }
}
