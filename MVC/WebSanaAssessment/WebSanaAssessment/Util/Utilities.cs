using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebSanaAssessment.Util
{
   public static class Utilities
   {
      /// <summary>
      /// Serialize a List<T> to XML file.
      /// </summary>
      /// <typeparam name="T">Type of class</typeparam>
      /// <param name="list">Collections to serialize</param>
      /// <param name="fileName">Full path of XML file</param>
      public static void Serialize<T>(this List<T> list, string fileName)
      {
         var serializer = new XmlSerializer(typeof(List<T>));
         using (var stream = File.OpenWrite(fileName))
         {            
            serializer.Serialize(stream, list);
         }
      }

      /// <summary>
      /// Deserialize a XML file to List<T>
      /// </summary>
      /// <typeparam name="T">Type of class</typeparam>
      /// <param name="fileName">Full path of XML file</param>
      /// <returns>Collection of T</returns>
      public static List<T> Deserialize<T>(string fileName)
      {         
         var list = new List<T>();

         XmlDocument doc = new XmlDocument();
         doc.Load(fileName);

         if(doc.HasChildNodes)
         {
            var serializer = new XmlSerializer(typeof(List<T>));
            using (var stream = File.OpenRead(fileName))
            {
               var other = (List<T>)(serializer.Deserialize(stream));
               list.Clear();
               list.AddRange(other);
            }
         }         

         return list;
      }

      /// <summary>
      /// Create a XmlElement based on object instance.
      /// </summary>
      /// <typeparam name="T">Type of class</typeparam>
      /// <param name="obj">Object instance</param>
      /// <returns>XmlElement</returns>
      public static XmlElement SerializeToXmlElement<T>(T obj)
      {
         XmlDocument doc = new XmlDocument();

         using (XmlWriter writer = doc.CreateNavigator().AppendChild())
         {
            new XmlSerializer(obj.GetType()).Serialize(writer, obj);
         }

         return doc.DocumentElement;
      }

      /// <summary>
      /// Get List<T> from CSV file
      /// </summary>
      /// <typeparam name="T">Type of class</typeparam>
      /// <param name="fileName">Full path of CSV file</param>
      /// <param name="separator">Character separator</param>
      /// <returns>Collection of T</returns>
      public static List<T> GetListFromCsv<T>(string fileName, char separator) where T : new()
      {
         List<T> list = new List<T>();

         using (StreamReader sr = new StreamReader(fileName))
         {
            T obj;
            PropertyInfo propertyInfo;
            Type t;
            object safeValue;
            string[] data;
            string[] fields = sr.ReadLine().Split(separator);

            while (!sr.EndOfStream)
            {
               data = sr.ReadLine().Split(separator);
               obj = new T();

               for (int i = 0; i < fields.Length; i++)
               {
                  propertyInfo = obj.GetType().GetProperty(fields[i]);
                  t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                  safeValue = (data[i] == null) ? null : Convert.ChangeType(data[i], t);
                  propertyInfo.SetValue(obj, safeValue, null);
               }
               list.Add(obj);
            }
         }

         return list;
      }

      /// <summary>
      /// Write T intance properties to CSV file
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="obj"></param>
      public static void WriteObjectToCsv<T>(T obj, string fileName, char separator)
      {
         string[] fields;

         using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
         {
            fields = sr.ReadLine().Split(separator);
         }

         using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8))
         {
            object value = null;
            StringBuilder newLine = new StringBuilder(Environment.NewLine);

            for (int i = 0; i < fields.Length; i++)
            {
               value = obj.GetType().GetProperty(fields[i]).GetValue(obj, null);
               newLine.Append($"{value}{separator}");
            }

            newLine.Length = newLine.Length - 1;
            sw.Write(newLine.ToString());
         }
      }

      //Thanks: https://stackoverflow.com/questions/11689337/net-file-writealllines-leaves-empty-line-at-the-end-of-file
      /// <summary>      
      /// Write Lines to File without add new empty line.
      /// </summary>
      /// <param name="path">Full path file</param>
      /// <param name="lines">Array of Lines</param>
      public static void WriteAllLinesBetter(string path, params string[] lines)
      {
         if (path == null)
            throw new ArgumentNullException("path");
         if (lines == null)
            throw new ArgumentNullException("lines");

         //using (var stream = File.OpenWrite(path))
         using (var stream = File.OpenWrite(path))
         {
            stream.SetLength(0);

            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
               if (lines.Length > 0)
               {
                  for (int i = 0; i < lines.Length - 1; i++)
                  {
                     writer.WriteLine(lines[i]);
                  }
                  writer.Write(lines[lines.Length - 1]);
               }
            }
         }
         
      }
   }
}