using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NovelSiteMVC.Models;

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
            var todoList = _context.TodoModel.Where(x => x.isDeleted == false).ToList();
            return View(todoList);
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

            return Json(new { success = true });
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Task,DueDate,Status")] TodoModel todoModel)
        {
            if (id != todoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoModel);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoModelExists(todoModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoModel);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var todoModel = _context.TodoModel.Find(id);
            if (todoModel is null)
                return BadRequest();

            todoModel.isDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool TodoModelExists(int id)
        {
            return _context.TodoModel.Any(e => e.Id == id);
        }
    }
}
