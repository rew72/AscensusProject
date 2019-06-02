using System.Collections.Generic;
using api.Models;

namespace api
{
    public interface IFileHelper
    {
        string GetLatestFile(string filePath, string fileName);
        List<T> GetData<T>(string fileName);
        void SetData<T>(string fileName, List<Contact> updatedList);
    }
}