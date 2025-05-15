using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp
{
    public partial class MainWindow : Window
    {
        private readonly ITaskService _taskService;
        public ObservableCollection<TodoTask> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _taskService = App.TaskService;

            LoadTasks();

            DataContext = this;
            TasksListView.ItemsSource = Tasks;
        }

        private void LoadTasks()
        {
            var tasks = _taskService.GetAllTasks();
            Tasks = new ObservableCollection<TodoTask>(tasks);
        }

        private void RefreshTaskList()
        {
            Tasks.Clear();
            foreach (var task in _taskService.GetAllTasks())
            {
                Tasks.Add(task);
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddNewTask();
        }

        private void NewTaskTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNewTask();
            }
        }

        private void AddNewTask()
        {
            string title = NewTaskTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(title))
            {
                var newTask = new TodoTask
                {
                    Title = title,
                    Description = "",
                    DueDate = DateTime.Now.AddDays(1),
                    IsCompleted = false,
                    TaskPriority = Priority.Medium
                };

                _taskService.AddTask(newTask);
                RefreshTaskList();
                NewTaskTextBox.Clear();
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TodoTask task)
            {
                OpenTaskDetailWindow(task);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is TodoTask task)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete task '{task.Title}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _taskService.DeleteTask(task.Id);
                    RefreshTaskList();
                }
            }
        }

        private void CheckBox_StatusChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TodoTask task)
            {
                _taskService.UpdateTask(task);
            }
        }

        private void TasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksListView.SelectedItem is TodoTask selectedTask)
            {
                OpenTaskDetailWindow(selectedTask);
                TasksListView.SelectedItem = null;
            }
        }

        private void OpenTaskDetailWindow(TodoTask task)
        {
            TodoTask taskCopy = new TodoTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                TaskPriority = task.TaskPriority
            };

            TaskDetailWindow detailWindow = new TaskDetailWindow(taskCopy);
            detailWindow.Owner = this;

            if (detailWindow.ShowDialog() == true)
            {
                _taskService.UpdateTask(detailWindow.Task);
                RefreshTaskList();
            }
        }
    }
}