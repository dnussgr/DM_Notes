﻿using DM_Notes.Core;
using DM_Notes.MVVM.Model;
using DM_Notes.MVVM.View;
using DM_Notes.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DM_Notes.MVVM.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly INoteService _noteService;
        
        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }


        private string _userNote;
        public string UserNote
        {
            get => _userNote;
            set { _userNote = value; OnPropertyChanged(); }
        }


        private ObservableCollection<Note> _notes;
        public ObservableCollection<Note> Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }


        private Note _selectedNote;
        public Note SelectedNote
        {
            get => _selectedNote;
            set { _selectedNote = value; OnPropertyChanged(); }
        }


        public ISnackbarMessageQueue SnackbarMessageQueue { get; set; }

        public MainWindowViewModel(INoteService noteService, ISnackbarMessageQueue? snackbarQueue = null)
        {
            _noteService = noteService;
            
            Notes = new ObservableCollection<Note>();
            SelectedNote = new Note();
            AddNoteCommand = new RelayCommand(async () => await AddNote());
            RemoveNoteCommand = new RelayCommand(async () => await RemoveNote());
            NewNoteCommand = new RelayCommand(NewNote);
            OpenNoteCommand = new RelayCommand(OpenNote);

            SnackbarMessageQueue = snackbarQueue ?? new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            _ = LoadNotesAsync();
        }

        public ICommand AddNoteCommand { get;  }
        private async Task AddNote()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(UserNote))
            {
                SnackbarMessageQueue.Enqueue("Titel und Inhalt dürfen nicht leer sein!");
                return;
            }

            Note newNote = new Note
            {
                Title = Title,
                UserNote = UserNote,
                Date = DateTime.Now
            };

            try
            {
                Notes.Add(newNote);
                await _noteService.SaveNoteAsync(newNote);
                SnackbarMessageQueue.Enqueue("Notiz erfolgreich hinzugefügt!");

                SelectedNote = newNote;
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue("Fehler beim Speichern der Notiz!");
                await ErrorLogger.LogAsync("AddNote", ex);
            }

        }


        public ICommand RemoveNoteCommand { get; }
        private async Task RemoveNote()
        {
            if (SelectedNote != null && Notes.Contains(SelectedNote))
            {
                try
                {
                    var noteToRemove = SelectedNote;
                    Notes.Remove(noteToRemove);
                    SelectedNote = Notes.Count > 0 ? Notes[0] : new Note();
                    
                    await _noteService.DeleteNoteAsync(noteToRemove);
                    
                    SnackbarMessageQueue.Enqueue("Notiz gelöscht");
                }
                catch (Exception ex)
                {
                    SnackbarMessageQueue.Enqueue("Fehler beim Löschen der Notiz");
                    await ErrorLogger.LogAsync("RemoveNote", ex);
                }
            }
        }


        public ICommand NewNoteCommand { get; }
        private void NewNote()
        {
            SelectedNote = new Note();

            Title = string.Empty;
            OnPropertyChanged(nameof(Title));

            UserNote = string.Empty;
            OnPropertyChanged(nameof(UserNote));
        }


        public ICommand OpenNoteCommand { get; }
        private void OpenNote()
        {
            if (SelectedNote == null || string.IsNullOrWhiteSpace(SelectedNote.Title))
            {
                SnackbarMessageQueue.Enqueue("Keine Notiz ausgewählt!");
                return;
            }
            
            NoteDetailsWindow detailsWindow = new NoteDetailsWindow(SelectedNote, SaveEditedNote);
            detailsWindow.Show();
            
        }

        private async Task LoadNotesAsync()
        {
            var loadedNotes = await _noteService.LoadNotesAsync();
            Notes = loadedNotes;
            SnackbarMessageQueue.Enqueue("Notizen erfolgreich geladen");
        }

        private async void SaveEditedNote(Note editedNote)
        {
            try
            {
                var original = Notes.FirstOrDefault(n => n.Id == editedNote.Id);
                if (original == null ||
                    (original.Title == editedNote.Title && original.UserNote == editedNote.UserNote)) return;
                
                original.Title = editedNote.Title;
                original.UserNote = editedNote.UserNote;
                original.Date = DateTime.Now;

                await _noteService.SaveNoteAsync(editedNote);
                SnackbarMessageQueue.Enqueue("Notiz aktualisiert.");
            }
            catch (Exception ex)
            {
                SnackbarMessageQueue.Enqueue("Fehler beim Aktualisieren der Notiz!");
                await ErrorLogger.LogAsync("SaveEditedNote", ex);
            }
        }
    }
}
