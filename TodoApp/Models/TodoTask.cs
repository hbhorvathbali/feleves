using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoApp.Models
{
    public class TodoTask : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _description;
        private DateTime _dueDate;
        private bool _isCompleted;
        private Priority _priority;
        private DateTime _createdDate = DateTime.Now;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (_dueDate != value)
                {
                    _dueDate = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DeadlineStatus));
                }
            }
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DeadlineStatus));
                }
            }
        }

        public Priority TaskPriority
        {
            get => _priority;
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged();
                }
            }
        }

        public DeadlineStatus DeadlineStatus
        {
            get
            {
                if (IsCompleted)
                    return DeadlineStatus.Normal;

                DateTime today = DateTime.Today;
                DateTime dueDate = DueDate.Date;

                if (dueDate < today)
                    return DeadlineStatus.Overdue;
                else if (dueDate == today)
                    return DeadlineStatus.DueToday;
                else if (dueDate == today.AddDays(1))
                    return DeadlineStatus.DueTomorrow;
                else
                    return DeadlineStatus.Normal;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum DeadlineStatus
    {
        Normal,
        DueTomorrow,
        DueToday,
        Overdue
    }
}