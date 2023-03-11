using Avalonia.Interactivity;
using NotepadAndExplorer.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;

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
        public int saveFlag = 1;
        public FileEntity selectedFileEntity;
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
                    if (FileText == "#.#.#" || FileText == "")
                    {
                        FileText += ".";
                    }
                    return FileText;
                }
                if (SelectedFileEntity is FileV fileVvvv && flag == 1)
                {
                    return FileNamePath;
                }
                if (SelectedFileEntity == null && flag == 1 && FileNamePath != null)
                {
                    return FileNamePath;
                }
                if (SelectedFileEntity == null && flag == 0 && FileNamePath != null)
                {
                    if (File.Exists(FileNamePath))
                    {
                        FileText = File.ReadAllText(FileNamePath);
                        return FileText;
                    }
                }
                return string.Empty;
            });
            CancelCommand = ReactiveCommand.Create<Unit, string>(str =>
            {
                return "#.#.#";
            });

        }
        public string FileNamePath
        {
            get { return fileNamePath; }
            set { this.RaiseAndSetIfChanged(ref fileNamePath, value); }
        }
        public string FileText
        {
            get { return fileText; }
            set { this.RaiseAndSetIfChanged(ref fileText, value); }
        }
        public string ButtonContent
        {
            get { return buttonContent; }
            set { this.RaiseAndSetIfChanged(ref buttonContent, value); }
        }
        public FileEntity SelectedFileEntity
        {
            get
            {
                swap = selectedFileEntity;
                if (swap is FileV && checkPage == 1 && saveFlag == 1)
                {
                    FileNamePath = swap.Name;
                    ButtonContent = "Сохранить";
                    saveFlag = 0;
                }
                else
                {
                    if (swap is FileV && checkPage == 0 && saveFlag == 0)
                    {
                        FileNamePath = swap.Name;
                    }
                    if (swap is DirectoryV && checkPage == 0 && saveFlag == 0)
                    {
                        FileNamePath = "";
                    }
                    ButtonContent = "Открыть";
                }
                return selectedFileEntity;
            }
            set { this.RaiseAndSetIfChanged(ref selectedFileEntity, value); }
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
