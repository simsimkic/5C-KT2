using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIMS_Projekat.Serializer
{
    public class Serializer<T> where T : ISerializable, new()
    {
        private const char Delimiter = '|';

        public bool ToCSV(string fileName, List<T> objects)
        {
            StringBuilder csv = new StringBuilder();

            foreach (T obj in objects)
            {
                if (obj.ToCSV() == null)
                {
                    return false;
                }
                string line = string.Join(Delimiter.ToString(), obj.ToCSV());
                csv.AppendLine(line);
            }

            File.WriteAllText(fileName, csv.ToString());
            return true;
        }

        public List<T> FromCSV(string fileName)
        {
            List<T> objects = new List<T>();

            foreach (string line in File.ReadLines(fileName))
            {
                string[] csvValues = line.Split(Delimiter);
                T obj = new T();
                obj.FromCSV(csvValues);
                if (!obj.IsDeleted)
                    objects.Add(obj);
            }

            return objects;
        }
    }
}
