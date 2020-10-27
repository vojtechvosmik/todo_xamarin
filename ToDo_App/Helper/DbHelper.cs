using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ToDo_App.Helper
{
    public class DbHelper : SQLiteOpenHelper
    {
        private static String DB_Name = "KalushiToDoApp";
        private static int DB_Ver = 1;
        public static String DB_Table = "Task";
        public static String DB_Column = "TaskName";

        public DbHelper(Context context): base(context, DB_Name, null, DB_Ver) { }
        public override void OnCreate(SQLiteDatabase db)
        {
            string query = $"CREATE TABLE {DbHelper.DB_Table}(ID INTEGER PRIMARY KEY AUTOINCREMENT, {DbHelper.DB_Column} TEXT NOT NULL)";
            db.ExecSQL(query);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            string query = $"DELETE TABLE IF EXISTS{DB_Table}";
            db.ExecSQL(query);
            OnCreate(db);
        }

        public void InsertNewTask(String task)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(DB_Column, task);
            db.InsertWithOnConflict(DB_Table, null, values, Android.Database.Sqlite.Conflict.Replace);
            db.Close();
        }

        public void DeleteTask(String task)
        {
            SQLiteDatabase db = this.WritableDatabase;
            db.Delete(DB_Table, DB_Column + "=?", new String[] { task });
            db.Close();
        }

        public List<string> getTaskList()
        {
            List<string> taskList = new List<string>();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.Query(DB_Table,new string[] { DB_Column }, null, null, null, null, null);
            while (cursor.MoveToNext())
            {
                int index = cursor.GetColumnIndex(DB_Column);
                taskList.Add(cursor.GetString(index));
            }
            return taskList;
        }
    }
}