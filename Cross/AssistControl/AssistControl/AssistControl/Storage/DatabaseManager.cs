using AssistControl.Model.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AssistControl.Storage
{
    public class DatabaseManager
    {
        SQLiteConnection Connection;

        public DatabaseManager()
        {
            Connection = DependencyService.Get<ISQLite>().GetConnection();
            Connection.CreateTable<Student>();
        }

        public void Save<T>(T entry) where T : IKeyObject, new()
        {
            Connection.Insert(entry);
        }

        public void Update<T>(T entry) where T : IKeyObject, new()
        {
            Connection.Update(entry);
        }

        public void Delete<T>(T entry) where T : IKeyObject, new()
        {
            Connection.Delete(entry);
        }

        public List<TSource> GetList<TSource>() where TSource : IKeyObject, new()
        {
            return Connection.Table<TSource>().AsEnumerable().ToList();
        }

        public TSource GetItemByKey<TSource>(string key) where TSource : IKeyObject, new()
        {
            return Connection.Table<TSource>().AsEnumerable().FirstOrDefault(x => x.Key == key);
        }
    }
}