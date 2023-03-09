using NotepadAndExplorer.ViewModels.Page;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

namespace NotepadAndExplorer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;
        private NotepadViewModel notepadViewModel;
        private string fileText;
        public ReactiveCommand<Unit, Unit> SwitchToOpenFileViewModel { get; }
        public ReactiveCommand<Unit, Unit> SwitchToSaveFileViewModel { get; }
        public MainWindowViewModel()
        {
            Content = notepadViewModel = new NotepadViewModel();
            SwitchToSaveFileViewModel = ReactiveCommand.Create(() =>
            {
                OpenFileViewModel openFileViewModel = new OpenFileViewModel(1);
                Content = openFileViewModel;
                Observable.Merge(
                    openFileViewModel.OpenCommand,
                    openFileViewModel.CancelCommand).Subscribe(
                    returnedStr =>
                    {
                        if (returnedStr == "#.#.#")
                        {
                            Content = notepadViewModel;
                        }
                        if (returnedStr != string.Empty && returnedStr != "#.#.#")
                        {
                            string newFileText = FileText;
                            File.WriteAllText(returnedStr, newFileText);
                            Content = notepadViewModel;
                        }
                    }
                );
            });
            SwitchToOpenFileViewModel = ReactiveCommand.Create(() =>
            {
                OpenFileViewModel openFileViewModel = new OpenFileViewModel(0);
                Observable.Merge (
                    openFileViewModel.OpenCommand,
                    openFileViewModel.CancelCommand).Subscribe (
                    returnedStr =>
                        {
                            if (returnedStr != "#.#.#" && returnedStr != string.Empty)
                            {
                                if (returnedStr == "#.#.#.")
                                {
                                    string changed = returnedStr.Remove(5, 1);
                                    FileText = changed;
                                }
                                else
                                {
                                    FileText = returnedStr;
                                }
                                Content = notepadViewModel;
                            }
                            if (returnedStr == "#.#.#")
                            {
                                Content = notepadViewModel;
                            }

                        }
                    );
                Content = openFileViewModel;
            });
        }
        public string FileText
        {
            get { return fileText; }
            set { this.RaiseAndSetIfChanged(ref fileText, value); }
        }
        public ViewModelBase Content
        {
            get { return content; }
            set { this.RaiseAndSetIfChanged(ref content, value); }
        }
    }
}
