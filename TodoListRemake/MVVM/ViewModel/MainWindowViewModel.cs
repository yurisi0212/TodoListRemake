using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace TodoListRemake.MVVM.ViewModel {
    public class MainWindowViewModel : DependencyObject {

        public static readonly DependencyProperty TitleTextProperty = 
            DependencyProperty.Register("TitleText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("タイトル"));

        public static readonly DependencyProperty ContentsTextProperty =
            DependencyProperty.Register("ContentsText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("コンテンツ"));

        public static readonly DependencyProperty FooterTextProperty =
            DependencyProperty.Register("FooterText", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata("ようこそ"));

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

        public ICommand AddScheduleCommand { get; }
        public ICommand ShowCalendarCommand { get; }

        public MainWindowViewModel() {

            AddScheduleCommand = new RelayCommand(() => {
                _ = MessageBox.Show("予定の追加");
            });

            ShowCalendarCommand = new RelayCommand(() => {
                _ = MessageBox.Show("カレンダーの表示");
            });
        }
    }
}
