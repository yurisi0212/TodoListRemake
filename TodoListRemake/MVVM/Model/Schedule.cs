using System;

namespace TodoListRemake.MVVM.Model {
    public class Schedule {

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool Complete { get; set; }

        public bool Notification { get; set; }

    }
}
