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
        public FileEntity swap;
        private string buttonContent;
        public string fileText;
        public string fileNamePath;
        public int checkPage;
        public FileEntity selectedFileEntity;
        public FileEntity SelectedFileEntity 
        {
            get
            {
                swap = selectedFileEntity;
                if (swap is FileV && checkPage == 1)
                {
                    FileNamePath = swap.Name;
                    ButtonContent = "Сохранить";
                }
                else
                {
                    ButtonContent = "Открыть";
                }
                return selectedFileEntity;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedFileEntity, value);
            }
        }
        public ObservableCollection<FileEntity> dirAndFiles { get; set; } = new ObservableCollection<FileEntity>();

        public ReactiveCommand<Unit, string> OpenCommand { get; }
        public ReactiveCommand<Unit, string> CancelCommand { get; }

        public OpenFileViewModel(int flag) 
        {
            FilePath = AppDomain.CurrentDomain.BaseDirectory;
            WriteToListBox();
            var parentDirInfo = Directory.GetParent(FilePath);
            FilePath = parentDirInfo.ToString();

            checkPage = flag;

            OpenCommand = ReactiveCommand.Create<Unit, string>(str => 
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
                if (SelectedFileEntity is FileV fileVvv && flag == 0)
                {
                    FileText = File.ReadAllText(fileVvv.FullName);
                    return FileText;
                }
                if (SelectedFileEntity is FileV fileVvvv && flag == 1)
                {
                    return fileVvvv.FullName;
                }
                return "#.#.#";
            });
            CancelCommand = ReactiveCommand.Create<Unit, string>(str =>
            {
                return string.Empty;
            });

        }
        public string FileNamePath
        {
            get { return fileNamePath; }
            set
            {
                this.RaiseAndSetIfChanged(ref fileNamePath, value);
            }
        }
        public string FileText
        {
            get { return fileText;}
            set
            {
                this.RaiseAndSetIfChanged(ref fileText, value);
            }
        }
        public string ButtonContent
        {
            get { return buttonContent; }
            set
            {
                this.RaiseAndSetIfChanged(ref buttonContent, value);
            }
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
