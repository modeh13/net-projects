using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLiteDemo
{
    public class DataAccess<T> where T : class, new()
    {
        //Attributes
        private static DataAccess<T> Instance;
        private string FolderDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private const string NameDB = "Base.db3";
        private SQLiteConnection Connection;

        //Properties
        public string PathDB { get; private set; }

        //Constructor
        public DataAccess()
        {
            PathDB = Path.Combine(FolderDB, NameDB);
            Connection = new SQLiteConnection(PathDB);            
            Connection.CreateTable<T>();
        }

        public static DataAccess<T> GetInstance()
        {
            return Instance = Instance ?? new DataAccess<T>();
        }

        //Methods
        public List<T> GetList()
        {
            return Connection.Table<T>().ToList();
        }

        public int Insert(object obj)
        {
            return Connection.Insert(obj);
        }

        public int Update(object obj)
        {
            return Connection.Update(obj);
        }

        public int Delete(object primaryKey)
        {
            return Connection.Delete<T>(primaryKey);
        }
    }
}