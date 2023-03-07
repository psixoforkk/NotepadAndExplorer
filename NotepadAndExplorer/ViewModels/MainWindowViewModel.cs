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
        public ReactiveCommand<Unit, Unit> SwitchToOpenFileViewModel { get; }
        public ReactiveCommand<Unit, Unit> SwitchToSaveFileViewModel { get; }
        public MainWindowViewModel()
        {

            Content = notepadViewModel = new NotepadViewModel();

            SwitchToSaveFileViewModel = ReactiveCommand.Create(() =>
            {
                OpenFileViewModel openFileViewModel = new OpenFileViewModel();
                Content = openFileViewModel;
                Observable.Merge(
                    openFileViewModel.OpenCommand,
                    openFileViewModel.CancelCommand).Subscribe(
                    cringe =>
                    {
                        if (cringe == 0)
                        {
                            Content = notepadViewModel;
                        }

                    }
                );
            });
            SwitchToOpenFileViewModel = ReactiveCommand.Create(() =>
            {
                OpenFileViewModel openFileViewModel = new OpenFileViewModel();
                Observable.Merge (
                    openFileViewModel.OpenCommand,
                    openFileViewModel.CancelCommand).Subscribe (
                    cringe =>
                        {
                            if (cringe == 0)
                            {
                                Content = notepadViewModel;
                            }

                        }
                    );
                Content = openFileViewModel;
            });
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
