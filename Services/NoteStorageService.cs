using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DM_Notes.MVVM.Model;

namespace DM_Notes.Services
{
    public static class NoteStorageService
    {
        // Pfad zu AppData
        private static readonly string StorageFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DM_Notes");

        
        // Pfad und Name der Speicherdatei
        private static readonly string StorageFile = Path.Combine(StorageFolder, "notes.json");
        
        
        /// <summary>
        /// Lädt die notes.json-Datei, erstellt sie, falls nicht vorhanden.
        /// </summary>
        /// <returns>Liste mit den Notizen, nach Datum sortiert</returns>
        public static async Task<List<Note>> LoadAsync()
        {
            try
            {
                if (!Directory.Exists(StorageFolder))
                    Directory.CreateDirectory(StorageFolder);

                if (!File.Exists(StorageFile))
                {
                    await SaveAsync(new List<Note>());
                    return new List<Note>();
                }

                var json = await Task.Run(() => File.ReadAllText(StorageFile));
                var notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
                return notes.OrderBy(n => n.Date).ToList();
            }
            catch (Exception ex)
            {
                await ErrorLogger.LogAsync("LoadAsync", ex);
                return new List<Note>();
            }
        }

        /// <summary>
        /// Speichert Notizen in die Speicherdatei, prüft zusätzlich, ob die Speicherdatei existiert.
        /// </summary>
        /// <param name="notes">ObservableCollection mit Notizen</param>
        public static async Task SaveAsync(IEnumerable<Note> notes)
        {
            try
            {
                if (!Directory.Exists(StorageFolder))
                    Directory.CreateDirectory(StorageFolder);

                var json = JsonSerializer.Serialize(notes, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                await Task.Run(() => File.WriteAllText(StorageFile, json));
            }
            catch (Exception ex)
            {
                await ErrorLogger.LogAsync("SaveAsync", ex);
            }
        }
    }
}
