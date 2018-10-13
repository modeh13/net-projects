using AssistControl.Droid.Storage.Implementations;
using AssistControl.Storage;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDroid))]
namespace AssistControl.Droid.Storage.Implementations
{  
    public class SQLiteDroid : ISQLite
    {
        private const string DatabaseName = "Base.db3";

        public SQLiteConnection GetConnection()
        {
            SQLitePCL.Batteries.Init();
            string folderPathDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string pathDB = Path.Combine(folderPathDB, DatabaseName);
            return new SQLiteConnection(pathDB);
        }
    }
}