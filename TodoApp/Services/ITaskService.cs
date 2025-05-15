using System.Collections.Generic;
using TodoApp.Models;

namespace TodoApp.Services
{
    public interface ITaskService
    {
        List<TodoTask> GetAllTasks();
        TodoTask GetTaskById(int id);
        void AddTask(TodoTask task);
        void UpdateTask(TodoTask task);
        void DeleteTask(int id);
    }
}