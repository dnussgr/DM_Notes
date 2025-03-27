using System;
using System.ComponentModel;
using System.Windows;
using DM_Notes.MVVM.Model;

namespace DM_Notes.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für NoteDetailsWindow.xaml
    /// </summary>
    public partial class NoteDetailsWindow : Window
    {
        private readonly Note _originalNote;
        private readonly Action<Note> _onClose;
        private readonly Note _editableNote;

        public NoteDetailsWindow(Note selectedNote, Action<Note> onClose = null)
        {
            InitializeComponent();
            _originalNote = selectedNote;
            _onClose = onClose;

            _editableNote = new Note
            {
                Id = selectedNote.Id,
                Title = selectedNote.Title,
                UserNote = selectedNote.UserNote,
                Date = selectedNote.Date
            };

            DataContext = _editableNote;
        }

        private void EditToggle_Checked(object sender, RoutedEventArgs e)
        {
            TitleBox.IsReadOnly = false;
            NoteBox.IsReadOnly = false;
        }

        private void EditToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            TitleBox.IsReadOnly = true;
            NoteBox.IsReadOnly = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_originalNote.Title != _editableNote.Title || _originalNote.UserNote != _editableNote.UserNote)
            {
                _onClose?.Invoke(_editableNote);
            }
        }
    }
}
