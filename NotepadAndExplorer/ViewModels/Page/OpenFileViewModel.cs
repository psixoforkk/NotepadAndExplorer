using NotepadAndExplorer.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace NotepadAndExplorer.ViewModels.Page
{
    public class OpenFileViewModel : ViewModelBase
    {
        public string FilePath;
        public FileEntity SelectedFileEntity { get; set; }
        public ObservableCollection<FileEntity> dirAndFiles { get; set; } = new ObservableCollection<FileEntity>();

        public ReactiveCommand<Unit, ushort> OpenCommand { get; }
        public ReactiveCommand<Unit, ushort> CancelCommand { get; }

        public OpenFileViewModel() 
        {
            FilePath = AppDomain.CurrentDomain.BaseDirectory;
            WriteToListBox();
            var parentDirInfo = Directory.GetParent(FilePath);
            FilePath = parentDirInfo.ToString();

            OpenCommand = ReactiveCommand.Create<Unit, ushort>(_ => 
                {
                if (SelectedFileEntity is DirectoryV dirV)
                {
                    FilePath = dirV.FullName;
                    dirAndFiles.Clear();
                    WriteToListBox();
                }
                if (SelectedFileEntity is BackEntity backV)
                {
                    var parentDirInfo = Directory.GetParent(FilePath);
                    if (parentDirInfo == null)
                    {
                        dirAndFiles.Clear();
                        foreach (var logicalDrive in Directory.GetLogicalDrives())
                        {
                            dirAndFiles.Add(new DirectoryV(logicalDrive));
                        }
                    }
                    else
                    {
                        FilePath = parentDirInfo.ToString();
                        dirAndFiles.Clear();
                        WriteToListBox();
                    }
                }
                    return 1;
            });
            CancelCommand = ReactiveCommand.Create<Unit, ushort>(_ =>
            {
                return 0;
            });

        }
        private void WriteToListBox()
        {
            var dirInfo = new DirectoryInfo(FilePath);
            dirAndFiles.Add(new BackEntity(".."));
            foreach (var dirs in dirInfo.GetDirectories())
            {
                dirAndFiles.Add(new DirectoryV(dirs));
            }
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                dirAndFiles.Add(new FileV(fileInfo));
            }
        }
    }
}
