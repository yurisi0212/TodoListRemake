using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using TodoListRemake.MVVM.Model;
using TodoListRemake.MVVM.View;

namespace TodoListRemake.MVVM.ViewModel {

    public class MainWindowViewModel : DependencyObject {

        private MainWindow _window;

        private ScheduleDataBase _database;

        public static readonly DependencyProperty ShowCalendarIsOpenProperty =
            DependencyProperty.Register("ShowCalendarIsOpen", typeof(bool), typeof(MainWindowViewModel), new UIPropertyMetadata(false));

        public static readonly DependencyProperty TitleTextProperty = 
            DependencyProperty.Register("TitleText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("タイトル"));

        public static readonly DependencyProperty ContentsTextProperty =
            DependencyProperty.Register("ContentsText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("コンテンツ"));

        public static readonly DependencyProperty FooterTextProperty =
            DependencyProperty.Register("FooterText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("ようこそ"));

        public static readonly DependencyProperty CompleteButtonContentProperty =
            DependencyProperty.Register("CompleteButtonContent", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("完了にする"));

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(MainWindowViewModel), new UIPropertyMetadata(-1, OnSelectedIndexChanged));

        public static readonly DependencyProperty SelectedContentsDateTimeProperty =
            DependencyProperty.Register("SelectedContentsDateTime", typeof(DateTime), typeof(MainWindowViewModel), new UIPropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty ViewDateProperty =
            DependencyProperty.Register("ViewDate", typeof(DateTime), typeof(MainWindowViewModel), new UIPropertyMetadata(DateTime.Now));

        public ObservableCollection<ScheduleWrap> TodoList { get; }

        private RelayCommand listView_Loaded;

        public ICommand ListView_Loaded => listView_Loaded ??= new RelayCommand(PerformListView_Loaded);


        public bool ShowCalendarIsOpen {
            get => (bool)GetValue(ShowCalendarIsOpenProperty);
            set => SetValue(ShowCalendarIsOpenProperty, value);
        }

        public string TitleText {
            get => (string)GetValue(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }

        public string ContentsText {
            get => (string)GetValue(ContentsTextProperty);
            set => SetValue(ContentsTextProperty, value);
        }

        public string FooterText {
            get => (string)GetValue(FooterTextProperty);
            set => SetValue(FooterTextProperty, value);
        }

        public string CompleteButtonContent {
            get => (string)GetValue(CompleteButtonContentProperty);
            set => SetValue(CompleteButtonContentProperty, value);
        }

        public int SelectedIndex {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public DateTime SelectedContents {
            get => (DateTime)GetValue(SelectedContentsDateTimeProperty);
            set => SetValue(SelectedContentsDateTimeProperty, value);
        }

        public DateTime ViewDate {
            get => (DateTime)GetValue(ViewDateProperty);
            set => SetValue(ViewDateProperty, value);
        }

        public ICommand AddScheduleCommand { get; }
        public ICommand ShowCalendarCommand { get; }
        public ICommand ChangeDateCommand { get; }

        public MainWindowViewModel(MainWindow parentWindow) {

            _window = parentWindow;

            TodoList = new ObservableCollection<ScheduleWrap>();

            _database = new ScheduleDataBase();

            AddScheduleCommand = new RelayCommand(() => {
                AddScheduleWindow window = new(this, _database);
                window.Owner = _window;
                window.ShowDialog();
            });

            ShowCalendarCommand = new RelayCommand(() => {
                ShowCalendarIsOpen = true;
            });

            ChangeDateCommand = new RelayCommand<string>(date => {
                ShowCalendarIsOpen = false;
                ChangeDate(DateTime.Parse(date));
            });
        }

        private void PerformListView_Loaded() {
            ReloadListView();
        }

        private void ChangeDate(DateTime date) {
            ViewDate = date;
            ReloadListView();
        }

        public void ReloadListView() {
            TodoList.Clear();
            foreach (Schedule data in _database.GetScheduleByDate(ViewDate)) {
                TodoList.Add(new ScheduleWrap {
                    Index = TodoList.Count,
                    Schedule = data
                });
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {

        }
    }
}
