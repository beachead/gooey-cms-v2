using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Gooeycms.Business.Util
{
    public static class Serializer
    {
        public static byte [] ToByteArray<T>(T obj) 
        {
            byte[] result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                result = stream.ToArray();
            }

            return result;
        }

        public static T ToObject<T>(byte [] data) 
        {
            T result;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                result = (T)formatter.Deserialize(stream);
            }

            return result;
        }
    }
}
