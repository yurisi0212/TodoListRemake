using System.Windows;
using TodoListRemake.MVVM.View;

namespace TodoListRemake.MVVM.ViewModel {
    public class AddScheduleViewModel : DependencyObject {

        private MainWindow _window;

        public AddScheduleViewModel(MainWindow window) {
            _window = window;
        }
    }
}
