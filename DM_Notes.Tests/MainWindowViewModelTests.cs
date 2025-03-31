using DM_Notes.MVVM.Model;
using DM_Notes.MVVM.ViewModel;
using DM_Notes.Services;
using MaterialDesignThemes.Wpf;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DM_Notes.Tests
{
    [TestFixture]
    public class MainWindowViewModelTest
    {
        private Mock<INoteService> _mockNoteService;
        private MainWindowViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _mockNoteService = new Mock<INoteService>();
            _mockNoteService
                .Setup(s => s.LoadNotesAsync())
                .ReturnsAsync(new ObservableCollection<Note>());

            var dummyQueue = new Mock<ISnackbarMessageQueue>().Object;

            _viewModel = new MainWindowViewModel(_mockNoteService.Object, dummyQueue)
            {
                Title = "Testtitel",
                UserNote = "Inhalt"
            };
        }

        [Test]
        public async Task AddNote_ValidInput_AddNoteToCollectionAndSaves()
        {
            // Act
            await Task.Run(() => _viewModel.AddNoteCommand.Execute(null));

            // Assert
            Assert.AreEqual(1, _viewModel.Notes.Count);
            _mockNoteService.Verify(s => s.SaveNoteAsync(It.IsAny<Note>()), Times.Once);
        }

        [Test]
        public async Task RemoveNote_ValidSelectedNote_RemovesAndDeletes()
        {
            //Arrange
            var note = new Note { Title = "Löschen", UserNote = "Test" };
            _viewModel.Notes.Add(note);
            _viewModel.SelectedNote = note;

            // Act
            await Task.Run(() => _viewModel.RemoveNoteCommand.Execute(null));

            // Assert
            Assert.IsFalse(_viewModel.Notes.Contains(note));
            _mockNoteService.Verify(s => s.DeleteNoteAsync(note), Times.Once);
        }

        [Test]
        public async Task Addnote_EmptyTitle_DoesNotSave()
        {
            // Arrange
            _viewModel.Title = "";
            _viewModel.UserNote = "Inhalt";

            // Act
            await Task.Run(() => _viewModel.AddNoteCommand.Execute(null));

            // Assert
            Assert.AreEqual(0, _viewModel.Notes.Count);
            _mockNoteService.Verify(s => s.SaveNoteAsync(It.IsAny<Note>()), Times.Never);
        }

        [Test]
        public async Task SaveEditedNote_ChangeContent_CallService()
        {
            // Arrange
            var note = new Note()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Alt",
                UserNote = "Inhalt"
            };

            _viewModel.Notes.Add(note);
            _viewModel.SelectedNote = note;

            var edited = new Note()
            {
                Id = note.Id,
                Title = "Neu",
                UserNote = "Bearbeitet"
            };

            // Act
            var method = typeof(MainWindowViewModel)
                .GetMethod("SaveEditedNote", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method.Invoke(_viewModel, new object[] { edited });

            // Assert
            _mockNoteService.Verify(s => s.SaveNoteAsync(It.Is<Note>(n => n.Id == note.Id)), Times.Once);
        }

        [Test]
        public async Task SaveEditedNote_UnchangedContent_DoesNotCallService()
        {
            // Arrange
            var note = new Note
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Original",
                UserNote = "Inhalt"
            };

            _viewModel.Notes.Add(note);
            _viewModel.SelectedNote = note;

            var unchanged = new Note
            {
                Id = note.Id,
                Title = "Original",
                UserNote = "Inhalt"
            };

            // Act
            var method = typeof(MainWindowViewModel)
                .GetMethod("SaveEditedNote", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method.Invoke(_viewModel, new object[] { unchanged });

            // Assert
            _mockNoteService.Verify(s => s.SaveNoteAsync(It.IsAny<Note>()), Times.Never);
        }


        [Test]
        public void NewNote_ResetsFieldsAndSelection()
        {
            // Arrange
            _viewModel.Title = "Test";
            _viewModel.UserNote = "Inhalt";
            _viewModel.SelectedNote = new Note { Title = "ABC" };

            // Act
            _viewModel.NewNoteCommand.Execute(null);

            // Assert
            NUnit.Framework.Assert.That(_viewModel.Title, Is.Empty);
            NUnit.Framework.Assert.That(_viewModel.UserNote, Is.Empty);
            Assert.IsNotNull(_viewModel.SelectedNote);
        }
    }

}
