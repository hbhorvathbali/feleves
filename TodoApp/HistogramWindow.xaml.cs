using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TodoApp.Models;

namespace TodoApp
{
    public partial class HistogramWindow : Window
    {
        private List<TodoTask> _tasks;
        private bool _isCompletedTasks;

        public HistogramWindow(List<TodoTask> tasks, bool isCompletedTasks)
        {
            InitializeComponent();
            _tasks = tasks;
            _isCompletedTasks = isCompletedTasks;

            TitleTextBlock.Text = isCompletedTasks ? "Completed Tasks by Date" : "Not Completed Tasks by Date";

            Loaded += HistogramWindow_Loaded;
        }

        private void HistogramWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawHistogram();
        }

        private void DrawHistogram()
        {
            HistogramCanvas.Children.Clear();

            var dateCounts = _tasks
                .GroupBy(t => t.DueDate.Date)
                .ToDictionary(g => g.Key, g => g.Count())
                .OrderBy(kvp => kvp.Key)
                .ToList();

            if (!dateCounts.Any())
            {
                TextBlock noDataLabel = new TextBlock
                {
                    Text = "No tasks to display",
                    FontSize = 16,
                    FontStyle = FontStyles.Italic,
                    Foreground = new SolidColorBrush(Colors.Gray)
                };

                Canvas.SetLeft(noDataLabel, HistogramCanvas.ActualWidth / 2 - 60);
                Canvas.SetTop(noDataLabel, HistogramCanvas.ActualHeight / 2);
                HistogramCanvas.Children.Add(noDataLabel);
                return;
            }

            int maxCount = dateCounts.Max(dc => dc.Value);
            if (maxCount == 0) maxCount = 1;

            double canvasWidth = HistogramCanvas.ActualWidth;
            double canvasHeight = HistogramCanvas.ActualHeight;

            double barWidth = Math.Max(30, (canvasWidth - 60) / dateCounts.Count);
            double maxBarHeight = canvasHeight - 80;

            DateTime today = DateTime.Today;

            int index = 0;
            foreach (var dateCount in dateCounts)
            {
                DateTime date = dateCount.Key;
                int count = dateCount.Value;
                double barHeight = (count / (double)maxCount) * maxBarHeight;
                double x = index * barWidth + 30;
                double y = canvasHeight - barHeight - 50;

                Color barColor;
                if (date < today)
                    barColor = Colors.Red;
                else if (date == today)
                    barColor = Colors.Red;
                else if (date == today.AddDays(1))
                    barColor = Colors.Orange;
                else
                    barColor = Colors.Green;

                Rectangle bar = new Rectangle
                {
                    Width = barWidth * 0.8,
                    Height = barHeight,
                    Fill = new SolidColorBrush(barColor),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1
                };

                Canvas.SetLeft(bar, x);
                Canvas.SetTop(bar, y);
                HistogramCanvas.Children.Add(bar);

                TextBlock valueLabel = new TextBlock
                {
                    Text = count.ToString(),
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Canvas.SetLeft(valueLabel, x + (barWidth * 0.4) - 5);
                Canvas.SetTop(valueLabel, y - 20);
                HistogramCanvas.Children.Add(valueLabel);

                TextBlock dateLabel = new TextBlock
                {
                    Text = date.ToString("MM/dd"),
                    FontSize = 10,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LayoutTransform = new RotateTransform(-45)
                };

                Canvas.SetLeft(dateLabel, x + (barWidth * 0.4) - 10);
                Canvas.SetTop(dateLabel, canvasHeight - 35);
                HistogramCanvas.Children.Add(dateLabel);

                index++;
            }

            DrawAxes(canvasWidth, canvasHeight, maxCount);          
            
        }

        private void DrawAxes(double canvasWidth, double canvasHeight, int maxCount)
        {
            Line yAxis = new Line
            {
                X1 = 25,
                Y1 = 20,
                X2 = 25,
                Y2 = canvasHeight - 50,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };
            HistogramCanvas.Children.Add(yAxis);

            Line xAxis = new Line
            {
                X1 = 25,
                Y1 = canvasHeight - 50,
                X2 = canvasWidth - 20,
                Y2 = canvasHeight - 50,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };
            HistogramCanvas.Children.Add(xAxis);

            int scaleSteps = Math.Min(maxCount, 5);
            for (int i = 0; i <= scaleSteps; i++)
            {
                int value = (int)Math.Round((i / (double)scaleSteps) * maxCount);
                double y = canvasHeight - 50 - (i / (double)scaleSteps) * (canvasHeight - 130);

                Line tick = new Line
                {
                    X1 = 20,
                    Y1 = y,
                    X2 = 25,
                    Y2 = y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1
                };
                HistogramCanvas.Children.Add(tick);

                TextBlock scaleLabel = new TextBlock
                {
                    Text = value.ToString(),
                    FontSize = 10,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Canvas.SetLeft(scaleLabel, 5);
                Canvas.SetTop(scaleLabel, y - 8);
                HistogramCanvas.Children.Add(scaleLabel);
            }
        }       
    }
}