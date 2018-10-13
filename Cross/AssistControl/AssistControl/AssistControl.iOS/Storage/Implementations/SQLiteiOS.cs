using AssistControl.Storage;
using SQLite;
using Xamarin.Forms;
using AssistControl.iOS.Storage.Implementations;
using System.IO;

[assembly:Dependency(typeof(SQLiteiOS))]
namespace AssistControl.iOS.Storage.Implementations
{
    public class SQLiteiOS : ISQLite
    {
        private const string DatabaseName = "Base.db3";

        public SQLiteConnection GetConnection()
        {
            //SQLitePCL.Batteries.Init();
            string folderPathDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(folderPathDB, "..", "Library");
            string pathDB = Path.Combine(libraryPath, DatabaseName);
            return new SQLiteConnection(pathDB);
        }
    }
}