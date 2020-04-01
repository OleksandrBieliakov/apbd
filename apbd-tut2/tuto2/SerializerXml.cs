using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace tuto2
{
    class SerializerXml : ISerializer
    {
        public void Serialize(string toPath, University university)
        {
            try
            {
                using FileStream writer = new FileStream(toPath, FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(University));
                serializer.Serialize(writer, university);
            }
            catch (ArgumentException)
            {
                Logger.LogIncorrectPath(toPath);
            }
        }
    }

}
