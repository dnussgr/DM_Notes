using System;
using System.Windows.Input;

namespace DM_Notes.Core
{
    /// <summary>
    /// Implementiert das ICommand-Interface für die Verwendung in WPF-ViewModels
    /// Ermöglicht die Bindung von Methoden an UI-Elemente
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Erstellt ein neues <see cref="RelayCommand"/>-Objekt
        /// </summary>
        /// <param name="execute">Die auszuführende Aktion</param>
        /// <param name="canExecute">Eine Funktion, die bestimmt, ob der Befehl ausführbar ist (optional)</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        /// <summary>
        /// Wird ausgelöst, wenn sich die Ausführbarkeit des Befehls ändert
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }


        /// <summary>
        /// Überprüft, ob der Befehl ausgeführt werden kann
        /// </summary>
        /// <param name="parameter">Ein optionaler Parameter</param>
        /// <returns>True, wenn der Befehl ausführbar ist, andernfalls False</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();


        /// <summary>
        /// Führt den Befehl aus
        /// </summary>
        /// <param name="parameter">Ein optionaler Parameter</param>
        public void Execute(object parameter) => _execute();
    }

}