using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace CursoBackend.Util
{
    public class Util
    {
        public static T JsonToObject<T>(JObject json)
        {
            JsonSerializer serializer = new JsonSerializer();
            return (T)serializer.Deserialize(new JTokenReader(json), typeof(T));
        }        
    }
}