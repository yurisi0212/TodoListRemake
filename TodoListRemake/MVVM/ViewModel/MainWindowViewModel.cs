using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
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

        public ObservableCollection<ScheduleWrap> TodoList { get; }

        public static readonly DependencyProperty ShowCalendarIsOpenProperty =
            DependencyProperty.Register("ShowCalendarIsOpen", typeof(bool), typeof(MainWindowViewModel), new UIPropertyMetadata(false));

        public static readonly DependencyProperty TitleTextProperty = 
            DependencyProperty.Register("TitleText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata(""));

        public static readonly DependencyProperty ContentsTextProperty =
            DependencyProperty.Register("ContentsText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata(""));

        public static readonly DependencyProperty FooterTextProperty =
            DependencyProperty.Register("FooterText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata(""));

        public static readonly DependencyProperty CompleteButtonContentProperty =
            DependencyProperty.Register("CompleteButtonContent", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata(""));

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(MainWindowViewModel), new UIPropertyMetadata(-1, OnSelectedIndexChanged));

        public static readonly DependencyProperty SelectedContentsDateTimeProperty =
            DependencyProperty.Register("SelectedContentsDateTime", typeof(DateTime), typeof(MainWindowViewModel), new UIPropertyMetadata());

        public static readonly DependencyProperty ViewDateProperty =
            DependencyProperty.Register("ViewDate", typeof(DateTime), typeof(MainWindowViewModel), new UIPropertyMetadata(DateTime.Now));

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

        private RelayCommand listView_Loaded;

        public ICommand ListView_Loaded => listView_Loaded ??= new RelayCommand(PerformListView_Loaded);
        public ICommand AddScheduleCommand { get; }
        public ICommand ShowCalendarCommand { get; }
        public ICommand ChangeDateCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainWindowViewModel(MainWindow parentWindow) {
            _window = parentWindow;
            TodoList = new ObservableCollection<ScheduleWrap>();
            _database = new ScheduleDataBase();

            AddScheduleCommand = new RelayCommand(() => {
                ShowAddWindow();
            });

            ShowCalendarCommand = new RelayCommand(() => {
                ShowCalendarIsOpen = true;
            });

            ChangeDateCommand = new RelayCommand<string>(date => {
                ShowCalendarIsOpen = false;
                ChangeDate(DateTime.Parse(date));
            });

            SaveCommand = new RelayCommand(() => {
                Save();
            });

            DeleteCommand = new RelayCommand(() => {
                Delete();
            });
        }

        private static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {
            MainWindowViewModel vm = (MainWindowViewModel)sender;
            vm.TitleText = vm.TodoList[vm.SelectedIndex].Schedule.Title;
            vm.ContentsText = vm.TodoList[vm.SelectedIndex].Schedule.Content;
            vm.SelectedContents = vm.TodoList[vm.SelectedIndex].Schedule.Date;
            vm.CompleteButtonContent = vm.TodoList[vm.SelectedIndex].Schedule.Complete ? "未完了に戻す" : "完了にする";
        }

        private void PerformListView_Loaded() {
            ReloadListView(null);
        }

        private void ChangeDate(DateTime date) {
            ViewDate = date;
            ReloadListView(null);
            ResetForm();
        }

        private void ShowAddWindow() {
            AddScheduleWindow window = new(this, _database);
            window.Owner = _window;
            window.ShowDialog();
        }

        private void Save() {
            if(SelectedIndex == -1) return;

            Schedule oldSchedule = TodoList[SelectedIndex].Schedule;
            Schedule newSchedule = new() {
                Id = oldSchedule.Id,
                Title = TitleText,
                Content = ContentsText,
                Date = SelectedContents,
                Complete = oldSchedule.Complete,
                Notification = oldSchedule.Notification
            };
            _database.UpdateSchedule(oldSchedule, newSchedule);
            FooterText = oldSchedule.Title + "をアップデートしました。";
            SelectedIndex = ReloadListView(newSchedule);
        }

        private async void Delete() {
            var msgbox = await _window.ShowMessageAsync("TodoList", TodoList[SelectedIndex].Schedule.Title + "を削除してよろしいですか？", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() {
                AffirmativeButtonText = "はい",
                NegativeButtonText = "いいえ"
            });
            if (msgbox == MessageDialogResult.Negative) return;
            _database.DeleteSchedule(TodoList[SelectedIndex].Schedule);
            FooterText = TodoList[SelectedIndex].Schedule.Title + "を削除しました。";
            TodoList.RemoveAt(SelectedIndex);
            SelectedIndex = -1;
            ReloadListView(null);
            ResetForm();
        }

        public int ReloadListView(Schedule? schedule) {
            var result = -1;
            TodoList.Clear();
            foreach (Schedule data in _database.GetScheduleByDate(ViewDate)) {
                var wrap = new ScheduleWrap {
                    Index = TodoList.Count,
                    Schedule = data
                };
                TodoList.Add(wrap);
                if (schedule == null) continue;
                if (wrap.Schedule.Id == schedule.Id)
                    result = wrap.Index;
            }
            return result;
        }

        private void ResetForm() {
            TitleText = "";
            ContentsText = "";
            SelectedContents = DateTime.MinValue;
            CompleteButtonContent = "";
        }
    }
}