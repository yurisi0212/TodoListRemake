using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace TodoListRemake.MVVM.ViewModel {
    public class MainWindowViewModel : DependencyObject {
        public ICommand AddScheduleCommand { get; }
        public ICommand ShowCalendarCommand { get; }

        public MainWindowViewModel() {

            AddScheduleCommand = new RelayCommand(() => {
                MessageBox.Show("押せてる");
            });

        }
    }
}
