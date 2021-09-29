using System.Windows;
using TodoListRemake.MVVM.Model;

namespace TodoListRemake.MVVM.ViewModel {
    public class ScheduleWrap : DependencyObject {

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(ScheduleWrap), new UIPropertyMetadata(-1));

        public int Index {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public Schedule Schedule { get; set; }
    }
}