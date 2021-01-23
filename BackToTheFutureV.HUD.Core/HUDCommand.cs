using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BackToTheFutureV.HUD.Core
{
    [Serializable]
    public class HUDCommand
    {
        private static BinaryFormatter formatter = new BinaryFormatter();

        public string Verb { get; }
        public byte[] Payload { get; }

        public int Count => Payload.Length;

        public HUDCommand(string verb, object payload)
        {
            Verb = verb;

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, payload);
                Payload = stream.ToArray();
            }
        }

        public T Get<T>()
        {
            using (var stream = new MemoryStream(Payload))
            {
                try
                {
                    return (T)formatter.Deserialize(stream);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        public static HUDCommand FromData(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                try
                {
                    return (HUDCommand)formatter.Deserialize(stream);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static implicit operator byte[](HUDCommand command)
        {
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, command);
                return stream.ToArray();
            }
        }
    }
}
