using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tuto2
{
    class Logger
    {
        private static void Log(string error)
        {
            string path = @"log.txt";
            if (!File.Exists(path))
            {
                using StreamWriter sw = File.CreateText(path);
                sw.WriteLine(error);
            }
            else
            {
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(error);
            }
        }

        public static void LogMissingData(string student)
        {
            Log("Missing data: " + student);
        }

        public static void LogDataError(string student, string field)
        {
            Log("Error in data(" + field + "): " + student);
        }

        public static void LogAlreadyExists(Student student)
        {
            Log("Already exists: " + student.ToString());
        }

        public static void LogFileNotFound(string path)
        {
            Log("File not found: " + path);
        }

        public static void LogIncorrectPath(string path)
        {
            Log("Incorrect path: " + path);
        }

        public static void LogNoStudents()
        {
            Log("No students");
        }
    }
}
