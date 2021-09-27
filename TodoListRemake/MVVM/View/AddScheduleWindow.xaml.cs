using MahApps.Metro.Controls;
using TodoListRemake.MVVM.Model;
using TodoListRemake.MVVM.ViewModel;

namespace TodoListRemake.MVVM.View {
    /// <summary>
    /// AddScheduleWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AddScheduleWindow : MetroWindow {
        public AddScheduleWindow(MainWindow window,ScheduleDataBase database) {
            InitializeComponent();
            DataContext = new AddScheduleViewModel(window,database);
        }
    }
}
