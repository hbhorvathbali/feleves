using System.Windows;
using TodoApp.Repository;
using TodoApp.Services;

namespace TodoApp
{
    public partial class App : Application
    {
        public static ITaskRepository TaskRepository { get; private set; }
        public static ITaskService TaskService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TaskRepository = new TaskRepository();
            TaskService = new TaskService(TaskRepository);
        }
    }
}