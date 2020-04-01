using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace tuto2
{
    class SerializerJson : ISerializer
    {
        public void Serialize(string toPath, University university)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(university);
                File.WriteAllText(toPath, jsonString);
            }
            catch (ArgumentException)
            {
                Logger.LogIncorrectPath(toPath);
            }
        }
    }
}
