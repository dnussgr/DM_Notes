using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DM_Notes.MVVM.Model;

namespace DM_Notes.Services;
/// <summary>
/// Interface für Notiz-Operationen
/// </summary>
public interface INoteService
{
    Task<ObservableCollection<Note>> LoadNotesAsync();
    Task SaveNoteAsync(Note note);
    Task DeleteNoteAsync(Note note);
}