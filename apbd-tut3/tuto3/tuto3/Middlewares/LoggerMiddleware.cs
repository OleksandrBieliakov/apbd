using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tuto3.DAL;
using tuto3.Models;

namespace tuto3.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDbService dbService)
        {
            if (context.Request != null)
            {
                LogEntry logEntry = await AsyncGetLogEntry(context);
                logEntry = await AsyncLogToDb(dbService, logEntry);
                await AsyncLogToFile(logEntry);
            }
            if (_next != null)
            {
                await _next(context);
            }
        }

        private async Task<LogEntry> AsyncGetLogEntry(HttpContext context)
        {
            DateTime time = DateTime.Now;
            string path = context.Request.Path;
            string method = context.Request.Method;
            string queryString = context.Request.QueryString.ToString();
            string bodyString = "";

            // Need this line to make further "reader.BaseStream.Seek(0, SeekOrigin.Begin);" work.
            // Enble it before Reading from the stream.
            context.Request.EnableBuffering();

            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyString = await reader.ReadToEndAsync();

                // Need this line, otherwise occurs the Error: "The input does not contain any JSON tokens..." for any POST method with JSON body.
                // because after reading the body, the position of stream is at the end of it, and when the body is read further in the pipeline it returns an empty line
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            LogEntry logEntry = new LogEntry
            {
                Time = time,
                Path = path,
                Method = method,
                QueryString = queryString,
                Body = bodyString
            };
            return logEntry;
        }

        private async Task<LogEntry> AsyncLogToDb(IDbService dbService, LogEntry logEntry)
        {
            LogEntry loggedInDb = logEntry;
            int idLogEntry = await Task.Run(() => dbService.InsertLogEntry(logEntry));
            loggedInDb.Id = idLogEntry;
            return loggedInDb;
        }

        private async Task AsyncLogToFile(LogEntry logEntry)
        {
            await Task.Run(() =>
            {
                string jsonString = JsonSerializer.Serialize(logEntry);

                string path = @"log.txt";

                if (!File.Exists(path))
                {
                    using StreamWriter sw = File.CreateText(path);
                    sw.WriteLine(jsonString);
                }
                else
                {
                    using StreamWriter sw = File.AppendText(path);
                    sw.WriteLine(jsonString);
                }
            });
        }

    }
}
