using Avalonia.Controls;
using DynamicData;
using Microsoft.VisualBasic;
using NotepadAndExplorer.ViewModels.Page;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

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
                    cringe =>
                    {
                        if (cringe == string.Empty)
                        {
                            Content = notepadViewModel;
                        }
                        if (cringe != string.Empty && cringe != "#.#.#")
                        {
                            string newFileText = FileText;
                            File.WriteAllText(cringe, newFileText);
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
                    cringe =>
                        {
                            if (cringe != "#.#.#" && cringe != string.Empty)
                            {
                                FileText = cringe;
                                Content = notepadViewModel;
                            }
                            if (cringe == string.Empty)
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
            set
            {
                this.RaiseAndSetIfChanged(ref content, value);
            }
        }
    }
}
