using System.Windows;
using TodoApp.Models;

namespace TodoApp
{
    public partial class TaskDetailWindow : Window
    {
        public TodoTask Task { get; private set; }

        public TaskDetailWindow(TodoTask task)
        {
            InitializeComponent();
            Task = task;
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}