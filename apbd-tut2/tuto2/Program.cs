using System;
using System.Collections.Generic;
using System.IO;

namespace tuto2
{
    class Program
    {
        public static void Process(string fromPath, string toPath, string extention)
        {
            University university = new Reader().ReadFile(fromPath);
            university.Author = "Oleksandr Bieliakov";
            university.CreatedAt = DateTime.Now;
            if (university != null)
            {
                if (university.IsEmpty())
                {
                    Logger.LogNoStudents();
                }
                else if (extention.Equals("xml"))
                {
                    new SerializerXml().Serialize(toPath, university);
                }
                else if (extention.Equals("json"))
                {
                    new SerializerJson().Serialize(toPath, university);
                }
            }
        }

        static void Main(string[] args)
        {
            string fromPath, toPath, extention;
            if (args.Length != 3)
            {
                fromPath = @"data.csv";
                toPath = @"result.xml";
                extention = "xml";
            }
            else
            {
                fromPath = args[0];
                toPath = args[1];
                extention = args[2].ToLower();
            }
            Process(fromPath, toPath, extention);
        }
    }
}
