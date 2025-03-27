using DM_Notes.MVVM.Model;
using System.Collections.ObjectModel;

namespace DM_Notes.Core
{
    public class NoteCollection
    {
        public ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();
    }
}
