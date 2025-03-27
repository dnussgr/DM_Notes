using System.Windows;
using DM_Notes.MVVM.ViewModel;
using DM_Notes.Core;

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
            DataContext = new MainWindowViewModel(new NoteCollection());
        }
    }
}
