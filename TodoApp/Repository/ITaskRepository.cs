using TodoApp.Models;

namespace TodoApp.Repository
{
    public interface ITaskRepository
    {
        List<TodoTask> GetAllTasks();
        TodoTask GetTaskById(int id);
        void AddTask(TodoTask task);
        void UpdateTask(TodoTask task);
        void DeleteTask(int id);
        int GetNextId();
    }
}