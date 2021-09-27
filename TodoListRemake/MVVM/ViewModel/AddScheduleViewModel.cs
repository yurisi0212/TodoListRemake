using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Input;
using TodoListRemake.MVVM.Model;
using TodoListRemake.MVVM.View;

namespace TodoListRemake.MVVM.ViewModel {
    public class AddScheduleViewModel : DependencyObject {

        private MainWindow _window;

        private ScheduleDataBase _database;

        public static readonly DependencyProperty TitleTextBoxProperty =
            DependencyProperty.Register("TitleTextBox", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("タイトル"));

        public static readonly DependencyProperty ScheduleDateTimeProperty =
            DependencyProperty.Register("ScheduleDateTime", typeof(DateTime), typeof(MainWindowViewModel), new UIPropertyMetadata(DateTime.Now));


        public static readonly DependencyProperty ContentTextBoxProperty =
            DependencyProperty.Register("ContentTextBox", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("概要"));

        public string TitleTextBox {
            get => (string)GetValue(TitleTextBoxProperty);
            set => SetValue(TitleTextBoxProperty, value);
        }

        public DateTime ScheduleDateTime {
            get => (DateTime)GetValue(ScheduleDateTimeProperty);
            set => SetValue(ScheduleDateTimeProperty, value);
        }

        public string ContentTextBox {
            get => (string)GetValue(ContentTextBoxProperty);
            set => SetValue(ContentTextBoxProperty, value);
        }

        public ICommand SaveButtonCommand { get; }

        public AddScheduleViewModel(MainWindow window,ScheduleDataBase database) {
            _window = window;
            _database = database;
            SaveButtonCommand = new RelayCommand(() => {
                MessageBox.Show(TitleTextBox + "\n" + ScheduleDateTime.ToString() + "\n" + ContentTextBox);
            });

            Schedule schedule = new(){
                Id = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                Title = TitleTextBox,
                Date = ScheduleDateTime,
                Content = ContentTextBox,
                Complete = false,
                Notification = false
            };
            _database.AddSchedule(schedule);
        }
    }
}
