using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAppWebAPI.Models;

namespace ToDoAppWebAPI.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ToDoApplicationContext _context;

        public TasksController(ToDoApplicationContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            
            var userId = User.Identity.Name;
            var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
            foreach(var t in tasks)
            {
                if (t.DueDate > System.DateTime.Now)
                    t.Status = "in-progress";
                else
                    t.Status = "completed";
            }
            return Ok(tasks);
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var userId = User.Identity.Name; 
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

      
        [HttpPost]
        public async Task<ActionResult<Task>> CreateTask(Tasks task)
        {
            task.UserId = User.Identity.Name; 
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Tasks task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            var userId = User.Identity.Name; 
            var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;

            await _context.SaveChangesAsync();

            return Ok();
        }

  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = User.Identity.Name; 
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
