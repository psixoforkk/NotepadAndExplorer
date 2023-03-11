using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using NotepadAndExplorer.Models;
using NotepadAndExplorer.ViewModels.Page;
using System;
using System.IO;

namespace NotepadAndExplorer.Views.Page
{
    public partial class OpenFileView : UserControl
    {
        public OpenFileView()
        {
            InitializeComponent();
        }

        public void TextBoxChanged(object sender, RoutedEventArgs text)
        {
            if (DataContext is OpenFileViewModel openFileViewModel)
            {
                openFileViewModel.FileText = ((TextBox)sender).Text;
            }
        }
        public void DoubleTapp(object sender, RoutedEventArgs args)
        {
            if (DataContext is OpenFileViewModel openFileViewModel)
            {
                openFileViewModel.OpenCommand.Execute().Subscribe();
            }
        }
    }
}
