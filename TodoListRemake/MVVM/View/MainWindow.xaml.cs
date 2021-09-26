using MahApps.Metro.Controls;
using TodoListRemake.MVVM.Model;
using TodoListRemake.MVVM.ViewModel;

namespace TodoListRemake.MVVM.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        private ScheduleDataBase _dataBase;

        public MainWindow() {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
            _dataBase = new ScheduleDataBase();
        }
    }
}
