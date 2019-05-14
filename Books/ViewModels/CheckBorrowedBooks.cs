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
    public class CheckBorrowedBooksViewModel : NotifyBase
    {
        private readonly CheckBorrowedBooks _checkBorrowedBooks;

        private ObservableCollection<Book> _listOfBooks = new ObservableCollection<Book>();
        public ObservableCollection<Book> ListOfBooks
        {
            get => _listOfBooks;
            set
            {
                _listOfBooks = value;
                NotifyPropertyChange(nameof(ListOfBooks));
            }
        }

        private async Task GetBorrowedBooks()
        {
            var foundUser = await App.Database.GetUserByUsernameAsync(Preferences.Get("Username", "default"));
            var foundBooks = await App.Database.GetUserWithChildren(foundUser);

            foreach(var item in foundBooks.Books)
            {
                _listOfBooks.Add(item);
            }

        }

        public CheckBorrowedBooksViewModel(CheckBorrowedBooks checkBorrowedBooks)
        {
            _checkBorrowedBooks = checkBorrowedBooks;
            Task.Run(async () => { await GetBorrowedBooks(); });
        }
    }
}
