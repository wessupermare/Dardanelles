using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Dardanelles
{
    class Config
    {
        private const char SEPARATOR = '\0';

        private readonly FileInfo ConfFile;

        private List<string> FileContents
        {
            get { return File.ReadAllLines(ConfFile.FullName, Encoding.UTF8).ToList(); }

            set { File.WriteAllLines(ConfFile.FullName, value, Encoding.UTF8); }
        }

        public Config(string filename)
        {
            ConfFile = new FileInfo(filename);
            if (!Directory.Exists(ConfFile.DirectoryName))
                Directory.CreateDirectory(ConfFile.DirectoryName);
            if (!File.Exists(ConfFile.FullName))
                File.Create(ConfFile.FullName).Close();
        }

        public void Delete(string key)
        {
            List<string> data = FileContents;
            data.RemoveAll((string s) => s.Split(SEPARATOR)[0] == key);
            FileContents = data;
        }

        public void DeleteAll()
        {
            File.Delete(ConfFile.FullName);
        }

        public bool ContainsKey(string key)
        {
            List<string> data = FileContents;
            return data.Count((string s) => s.Split(SEPARATOR)[0] == key) > 0;
        }

        public object this[string key]
        {
            get
            {
                foreach (string item in FileContents)
                {
                    string[] data = item.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                    if (data[0] == key)
                        return JsonConvert.DeserializeObject(data[1]);
                }
                throw new KeyNotFoundException($"Key {key} not found in config file.");
            }

            set
            {
                List<string> data = FileContents;
                string item = key + SEPARATOR + JsonConvert.SerializeObject(value, Formatting.Indented);
                int datIndex = data.FindIndex((string s) => s.Split(SEPARATOR)[0] == key);
                if (datIndex >= 0)
                    data[datIndex] = item;
                else
                    data.Add(item);
                FileContents = data;
            }
        }
    }
}