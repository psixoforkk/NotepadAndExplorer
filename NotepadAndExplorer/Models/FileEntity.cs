using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotepadAndExplorer.ViewModels;

namespace NotepadAndExplorer.Models
{
    public abstract class FileEntity : ViewModelBase
    {
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        protected FileEntity(string name)
        {
            Name = name;
        }
    }

    public class BackEntity : FileEntity
    {
        public BackEntity(string name) : base(name)
        {
            ImagePath = "Assets/closedir.png";
        }
    }
    public class DirectoryV : FileEntity
    {
        public DirectoryV(string dirName) : base(dirName)
        {
            FullName = dirName;
            ImagePath = "Assets/hdd.png";
        }
        public DirectoryV(DirectoryInfo dirName) : base(dirName.Name)
        {
            FullName = dirName.FullName;
            ImagePath = "Assets/opendir.png";
        }
    }
    public class FileV : FileEntity
    {
        public FileV(string name) : base(name) { }
        public FileV(FileInfo fileInfo) : base(fileInfo.Name)
        {
            FullName = fileInfo.FullName;
            ImagePath = "Assets/filefile.png";
        }
    }
}
