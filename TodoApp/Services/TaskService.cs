using System.Collections.Generic;
using TodoApp.Models;
using TodoApp.Repository;

namespace TodoApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public List<TodoTask> GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        public TodoTask GetTaskById(int id)
        {
            return _taskRepository.GetTaskById(id);
        }

        public void AddTask(TodoTask task)
        {
            _taskRepository.AddTask(task);
        }

        public void UpdateTask(TodoTask task)
        {
            _taskRepository.UpdateTask(task);
        }

        public void DeleteTask(int id)
        {
            _taskRepository.DeleteTask(id);
        }
    }
}