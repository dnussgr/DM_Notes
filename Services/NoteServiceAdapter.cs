using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DM_Notes.MVVM.Model;

namespace DM_Notes.Services;

/// <summary>
/// Adapter für das ViewModel-Interface INoteService, nutzt NoteStorageService
/// </summary>
public class NoteServiceAdapter : INoteService
{
    public async Task<ObservableCollection<Note>> LoadNotesAsync()
    {
        var notes = await NoteStorageService.LoadAsync();
        return new ObservableCollection<Note>(notes);
    }

    public async Task SaveNoteAsync(Note note)
    {
        var notes = await NoteStorageService.LoadAsync();
        var existing = notes.FirstOrDefault(n => n.Id == note.Id);

        if (existing != null)
        {
            existing.Title = note.Title;
            existing.UserNote = note.UserNote;
            existing.Date = note.Date;
        }
        else
        {
            notes.Add(note);
        }

        await NoteStorageService.SaveAsync(notes);
    }

    public async Task DeleteNoteAsync(Note note)
    {
        var notes = await NoteStorageService.LoadAsync();
        var updated = notes.Where(n => n.Id != note.Id).ToList();
        await NoteStorageService.SaveAsync(updated);
    }
}