using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace TodoListRemake.MVVM.ViewModel {
    public class MainWindowViewModel : DependencyObject {
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
