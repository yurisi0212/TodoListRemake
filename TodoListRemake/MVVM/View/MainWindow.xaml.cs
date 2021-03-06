using MahApps.Metro.Controls;
using TodoListRemake.MVVM.ViewModel;

namespace TodoListRemake.MVVM.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        public MainWindow() {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }
    }
}
