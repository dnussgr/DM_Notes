using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DM_Notes.Core
{
    /// <summary>
    /// Basisklasse für alle ViewModels, die Änderungen an Eigenschaften an die UI weitergeben.
    /// Implementiert das <see cref="INotifyPropertyChanged"/>-Interface.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Wird ausgelöst, wenn sich eine Eigenschaft ändert.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Benachrichtigt die UI, dass sich eine Eigenschaft geändert hat.
        /// </summary>
        /// <param name="name">Der Name der geänderten Eigenschaft (wird automatisch ermittelt).</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
