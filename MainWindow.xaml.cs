using System.Windows;
using DM_Notes.MVVM.ViewModel;
using DM_Notes.Core;
using DM_Notes.Services;

namespace DM_Notes
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindowViewModel(new NoteServiceAdapter());
            DataContext = viewModel;
        }
    }
}
