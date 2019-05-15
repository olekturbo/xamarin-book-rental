using Books.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Books.ViewModels
{
    public class CheckBooksViewModel : NotifyBase
    {
        private readonly CheckBooks _checkBooks;

        private ObservableCollection<Book> _listOfBooks = new ObservableCollection<Book>();
        private bool _isWorker;

        public bool IsWorker
        {
            get => _isWorker;
            set
            {
                _isWorker = value;
                NotifyPropertyChange(nameof(IsWorker));
            }
        }
        public ObservableCollection<Book> ListOfBooks
        {
            get => _listOfBooks;
            set
            {
                _listOfBooks = value;
                NotifyPropertyChange(nameof(ListOfBooks));
            }
        }

        private async Task GetAllBooks()
        {
            var foundBooks = await App.Database.GetBooksAsync();
            var foundUser = await App.Database.GetUserByUsernameAsync(Preferences.Get("Username", "default"));
            if(foundUser.Role.Contains("Pracownik"))
            {
                _isWorker = true;
            }
            else
            {
                _isWorker = false;
            }

            foreach (var item in foundBooks)
            {
                _listOfBooks.Add(item);
            }

        }

        public CheckBooksViewModel(CheckBooks checkBooks)
        {
            _checkBooks = checkBooks;
            Task.Run(async () => { await GetAllBooks(); });
        }

        public ICommand Borrow => new Command<int>(OnClickBorrow);
        public ICommand Edit => new Command<int>(OnClickEdit);
        public ICommand Remove => new Command<int>(OnClickRemove);

        private async void OnClickEdit(int id)
        {
            _checkBooks.Navigation.PushAsync(new EditBook(id));
        }

        private async void OnClickRemove(int id)
        {
            var foundBook = await App.Database.GetBookAsync(id);
            await App.Database.DeleteBookAsync(foundBook);

            _checkBooks.DisplayAlert("Komunikat", "Ksiazka zostala usunieta.", "OK");
        }
        private async void OnClickBorrow(int id)
        {
            var foundUser = await App.Database.GetUserByUsernameAsync(Preferences.Get("Username", "default"));
            var foundBook = await App.Database.GetBookAsync(id);
            await App.Database.UpdateUserModel(foundUser);
            var userBooks = foundUser.Books;
            var result = userBooks.Find(x => x.Title.Contains(foundBook.Title));
            if (result != null)
            {
                _checkBooks.DisplayAlert("Komunikat", "Ta ksiazka jest juz przez ciebie wypozyczona.", "OK");
            }
            else
            {
                if (foundBook.Count > 0)
                {
                    await App.Database.AddBookToUser(foundBook, foundUser);
                    _checkBooks.DisplayAlert("Komunikat", "Ksiazka " + foundBook.Title + " zostala wypozyczona.", "OK");
                    foundBook.Count--;
                    await App.Database.SaveBookAsync(foundBook);
                }
                else
                {
                    _checkBooks.DisplayAlert("Komunikat", "Brak ksiazek na stanie.", "OK");

                }
            }
        }
    }
}
