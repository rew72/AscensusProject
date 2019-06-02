using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using CsvHelper;

namespace api
{

    public class FileHelper : IFileHelper
    {
        private readonly string _filePath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName).FullName;

        public FileHelper()
        {

        }


        public string GetLatestFile(string filePath, string fileName)
        {
            return new DirectoryInfo(filePath).GetFiles(fileName + "*.csv", SearchOption.TopDirectoryOnly)
                .OrderByDescending(f => f.LastWriteTime)
                .First().FullName;
        }

        public List<T> GetData<T>(string fileName)
        {
            var reader = new StreamReader(GetLatestFile(_filePath, fileName));
            var csv = new CsvReader(reader);
            var listT = csv.GetRecords<T>().ToList();
            reader.Close();

            return listT;
        }

        public void SetData<T>(string fileName, List<Contact> updatedList)
        {
            System.IO.File.WriteAllBytes(GetLatestFile(_filePath, fileName), new byte[0]);
            var writer = new StreamWriter(GetLatestFile(_filePath, fileName));
            var csv = new CsvWriter(writer);
            csv.WriteRecords(updatedList);
            writer.Flush();
            writer.Close();
        }
    }
}
