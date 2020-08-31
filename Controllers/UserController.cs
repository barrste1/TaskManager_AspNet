using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList_08272020.Models;

namespace ToDoList_08272020.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly UserToDoDbContext _context;
        public UserController(UserToDoDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var tasklist = _context.ToDoList.Where(x => x.UserId == id).ToList();

            return View(tasklist);

        }
        public IActionResult FilterView(List<ToDoList> searchList)
        {
            return View("../User/Index",searchList);
        }

        [HttpGet]//This action is for displaying the data in the database for the user
        public IActionResult AddTask()
        {

            return View();
        }


        [HttpPost]//This action validates the form entry, and adds it to the database if all required fields are filled in.
        public IActionResult AddTask(ToDoList newTask)
        {
            newTask.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                _context.ToDoList.Add(newTask);
                _context.SaveChanges();

            }

            //View should update with the modifications of the added task
            return RedirectToAction("Index");
        }


        //This prompts the user to confirm their deletion. if the yes button is clicked then the task will be deleted.
        //if no is pressed, User will be returned to the main menu
        public IActionResult ConfirmDeleteTask(int id)
        {
            var foundTask = _context.ToDoList.Find(id);
            return View(foundTask);
        }


        //this action deletes the selected task. 
        public IActionResult DeleteTask(int id,string delete)
        {
            if (delete=="No")
            {
                return RedirectToAction("Index");
            }

            var foundTask = _context.ToDoList.Find(id);

            if (foundTask != null)
            {
                //remove the super from the database
                _context.ToDoList.Remove(foundTask);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //This action takes in a bool (check box checked = true, not checked is false)
        public IActionResult SearchCompletion(bool status)
        {
            List<ToDoList> searchList = new List<ToDoList>();
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var tasklist = _context.ToDoList.Where(x => x.UserId == id).ToList();
            foreach (ToDoList task in tasklist)
            {
                if (task.Completed == status)
                {
                    searchList.Add(task);
                }

                //Do the nature of "bit" in SQL some falses are showing up as null, so the elseif here will catch null

                else if (task.Completed == null && status==false)
                {
                    searchList.Add(task);
                }
            }

            return View("../User/Index",searchList);
        }

        //this compares the date of input to the dates of tasks. if they are a match, a new list is constructed and sent to view
        public IActionResult SearchDate(DateTime status)
        {
            List<ToDoList> searchList = new List<ToDoList>();
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var tasklist = _context.ToDoList.Where(x => x.UserId == id).ToList();
            foreach (ToDoList task in tasklist)
            {
                if (task.DueDate <= status)
                {
                    searchList.Add(task);
                }
            }

            return View("../User/Index", searchList);
        }
    }
}
