using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace TodoListRemake.MVVM.Model {
    public class ScheduleDataBase {

        private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\yurisi\TodoListRemake\";

        private SQLiteConnectionStringBuilder _db;

        private List<Schedule> _schedules = new();

        public ScheduleDataBase() {
            Directory.CreateDirectory(_path);
            SetupDataBase();
            AddSchedulesList();
        }

        private void SetupDataBase() {
            _db = new SQLiteConnectionStringBuilder { DataSource = _path + "Schedule.db" };
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS todo(" +
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "title TEXT NOT NULL," +
                    "content TEXT NOT NULL," +
                    "date TEXT NOT NULL," +
                    "complete INTEGER NOT NULL," +
                    "notification INTEGER NOT NULL" +
                 ")";
            cmd.ExecuteNonQuery();
        }

        private void AddSchedulesList() {
            _schedules.Clear();
            using SQLiteConnection cn = new(_db.ToString());
            cn.Open();
            using SQLiteCommand cmd = new(cn);
            cmd.CommandText = "SELECT * FROM todo";
            using SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                _schedules.Add(
                    new Schedule {
                        Id = int.Parse(reader["id"].ToString()),
                        Title = (string)reader["title"],
                        Content = (string)reader["content"],
                        Date = DateTime.Parse((string)reader["date"]),
                        Complete = int.Parse(reader["complete"].ToString()) == 1,
                        Notification = int.Parse(reader["notification"].ToString()) == 1,
                    }
               );
            }
        }

        public void AddSchedule(Schedule schedule) {
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText = "INSERT INTO todo(title, content, date, complete, notification) " +
                "VALUES(" +
                $"'{schedule.Title}', '{schedule.Content}', '{schedule.Date}', {(schedule.Complete ? 1 : 0)}, {(schedule.Notification ? 1 : 0)})";
            cmd.ExecuteNonQuery();
            _schedules.Add(schedule);
            AddSchedulesList();
        }

        public void DeleteSchedule(Schedule schedule) {
            using var cn = new SQLiteConnection(_db.ToString());
            cn.Open();
            using var cmd = new SQLiteCommand(cn);
            cmd.CommandText = "DELETE FROM todo WHERE id = " + schedule.Id;
            cmd.ExecuteNonQuery();
            AddSchedulesList();
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
            AddSchedulesList();
        }

        public List<Schedule> GetScheduleByDate(DateTime date) {
            var schedules = _schedules.FindAll(n => n.Date.ToShortDateString() == date.ToShortDateString());
            schedules.Sort((a, b) => a.Date.CompareTo(b.Date));
            return schedules;
        }
    }
}
