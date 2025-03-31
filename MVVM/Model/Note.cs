using System;

namespace DM_Notes.MVVM.Model
{
    public class Note
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string UserNote { get; set; }
    }
}
