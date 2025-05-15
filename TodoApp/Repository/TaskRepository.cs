using System.Collections.Generic;
using System.Linq;
using TodoApp.Models;

namespace TodoApp.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private static List<TodoTask> _tasks = new List<TodoTask>();

        public List<TodoTask> GetAllTasks()
        {
            return _tasks.ToList();
        }

        public TodoTask GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void AddTask(TodoTask task)
        {
            task.Id = GetNextId();
            _tasks.Add(task);
        }

        public void UpdateTask(TodoTask task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                var index = _tasks.IndexOf(existingTask);
                _tasks[index] = task;
            }
        }

        public void DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }

        public int GetNextId()
        {
            return _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
        }
    }
}