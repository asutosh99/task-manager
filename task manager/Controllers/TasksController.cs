using Microsoft.AspNetCore.Mvc;
using task_manager.Models;
using System.Collections.Generic;
using System.Linq;


namespace task_manager.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class TasksController
    {
        public static List<TaskItem> Tasks = new List<TaskItem>();
        [HttpGet]
        public ActionResult<List<TaskItem>> GetTasks()
        {
            return Tasks;
        }
        [HttpPost]
        public ActionResult<TaskItem> CreateTask(TaskItem task)
        {
            task.Id = Tasks.Count + 1;
            Tasks.Add(task);
            return task;
        }
        [HttpPut("{id}")]
        public ActionResult<TaskItem> UpdateTask(int id, TaskItem updatedTask)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;
            return task;
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            Tasks.Remove(task);
            return new NoContentResult();

        }
    }
}
