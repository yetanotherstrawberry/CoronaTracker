using SQLite;
using System;
using System.IO;

namespace CoronaTracker.Services
{
    public static class Constants
    {

        public const SQLiteOpenFlags DatabaseFlags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        public static string DatabaseFile
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLite.db3");

    }
}
