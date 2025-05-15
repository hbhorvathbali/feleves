using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp
{
    public partial class MainWindow : Window
    {
        private readonly ITaskService _taskService;
        public ObservableCollection<TodoTask> Tasks { get; set; }
        private DispatcherTimer _refreshTimer;
        private List<TodoTask> _allTasks;

        public MainWindow()
        {
            InitializeComponent();

            _taskService = App.TaskService;

            LoadTasks();

            DataContext = this;
            TasksListView.ItemsSource = Tasks;

            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromHours(1); 
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            foreach (var task in Tasks)
            {
                task.OnPropertyChanged(nameof(TodoTask.DeadlineStatus));
            }
        }

        private void LoadTasks()
        {
            _allTasks = _taskService.GetAllTasks();
            Tasks = new ObservableCollection<TodoTask>();
            ApplySorting();
        }

        private void RefreshTaskList()
        {
            _allTasks = _taskService.GetAllTasks();
            ApplySorting();
        }

        private void ApplySorting()
        {
            var tasksToSort = _allTasks.ToList();

            int selectedIndex = SortComboBox?.SelectedIndex ?? 0;
            IEnumerable<TodoTask> sortedTasks = selectedIndex switch
            {
                0 => tasksToSort.OrderBy(t => t.DueDate),
                1 => tasksToSort.OrderByDescending(t => t.DueDate),
                2 => tasksToSort.OrderByDescending(t => t.TaskPriority).ThenBy(t => t.DueDate), 
                3 => tasksToSort.OrderBy(t => t.TaskPriority).ThenBy(t => t.DueDate), 
                4 => tasksToSort.OrderBy(t => t.Title), 
                5 => tasksToSort.OrderByDescending(t => t.Title), 
                _ => tasksToSort.OrderBy(t => t.DueDate)
            };           

            Tasks.Clear();
            foreach (var task in sortedTasks)
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
                    TaskPriority = Priority.Medium,
                    CreatedDate = DateTime.Now
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
                task.OnPropertyChanged(nameof(TodoTask.DeadlineStatus));
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

        private void NotCompletedHistogram_Click(object sender, RoutedEventArgs e)
        {
            var notCompletedTasks = Tasks.Where(t => !t.IsCompleted).ToList();
            HistogramWindow histogramWindow = new HistogramWindow(notCompletedTasks, false);
            histogramWindow.Owner = this;
            histogramWindow.ShowDialog();
        }

        private void CompletedHistogram_Click(object sender, RoutedEventArgs e)
        {
            var completedTasks = Tasks.Where(t => t.IsCompleted).ToList();
            HistogramWindow histogramWindow = new HistogramWindow(completedTasks, true);
            histogramWindow.Owner = this;
            histogramWindow.ShowDialog();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tasks != null && _allTasks != null)
            {
                ApplySorting();
            }
        }       
    }
}