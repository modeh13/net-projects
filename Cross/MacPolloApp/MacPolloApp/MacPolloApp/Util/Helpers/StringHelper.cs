using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacPolloApp.Util.Helpers
{
    public static class StringHelper
    {
        public static IList<string> GetLines(int charactersByLine, string message)
        {
            IList<string> list = new List<string>();
            string[] words;
            string line = string.Empty;
            string word;
            message = message.Trim();
            words = message.Split(' ');

            for (short i = 0; i < words.Length; i++)
            {
                word = words[i].Trim();

                if (line.Length + word.Length + 1 <= charactersByLine)
                {
                    line += " " + word;
                }
                else {
                    list.Add(line);
                    line = word;
                }
            }

            if (line.Length > 0) list.Add(line);

            return list;
        }
    }
}