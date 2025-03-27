using System;

namespace DM_Notes.MVVM.Model
{
    public class Note
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string UserNote { get; set; }
    }
}
