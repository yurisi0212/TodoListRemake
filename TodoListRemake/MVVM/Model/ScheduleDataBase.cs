using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace TodoListRemake.MVVM.Model {
    public class ScheduleDataBase {

        private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\yurisi\TodoListRemake\";

        private SQLiteConnectionStringBuilder _db;

        private List<Schedule> _schedules = new();

        public ScheduleDataBase() {
            Directory.CreateDirectory(_path);
            SetupDataBase();
        }

        private void SetupDataBase() {
            _db = new SQLiteConnectionStringBuilder { DataSource = _path + "Schedule.db" };
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS todo(" +
                    "id INTEGER NOT NULL PRIMARY KEY," +
                    "title TEXT NOT NULL," +
                    "content TEXT NOT NULL," +
                    "date TEXT NOT NULL," +
                    "complete INTEGER NOT NULL," +
                    "notification INTEGER NOT NULL" +
                 ")";
            cmd.ExecuteNonQuery();
        }

        private void AddListSchedules() {
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText = "SELECT * FROM todo";
            using (SQLiteDataReader reader = cmd.ExecuteReader());
            
        }

        public void AddSchedule(Schedule schedule) {
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText = "INSERT INTO todo(id, title, content, date, complete, notification) " +
                "VALUES(" +
                $"{schedule.Id}, '{schedule.Title}', '{schedule.Content}', '{schedule.Date}', {(schedule.Complete ? 1 : 0)}, {(schedule.Notification ? 1 : 0)})";
            cmd.ExecuteNonQuery();
            _schedules.Add(schedule);
        }

        public void DeleteSchedule(Schedule schedule) {
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText = "DELETE FROM todo WHERE id = " + schedule.Id;
            cmd.ExecuteNonQuery();
        }

        public void UpdateSchedule(Schedule oldSchedule, Schedule newSchedule) {
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText = "REPLACE INTO todo " +
                "VALUES(" +
                $"{newSchedule.Id}, '{newSchedule.Title}', '{newSchedule.Content}', '{newSchedule.Date}', {(newSchedule.Complete ? 1 : 0)}, {(newSchedule.Notification ? 1 : 0)})";
            cmd.ExecuteNonQuery();
            _schedules.Remove(oldSchedule);
            _schedules.Add(newSchedule);
        }

        public List<Schedule> GetScheduleByDate(DateTime date) {
            return _schedules.FindAll(n => n.Date.ToShortDateString() == date.ToShortDateString());
        }
    }
}
