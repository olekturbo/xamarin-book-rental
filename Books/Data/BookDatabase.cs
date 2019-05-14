using Books.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensionsAsync;
using SQLiteNetExtensionsAsync.Extensions;


namespace Books.Data
{
    public class BookDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public BookDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
           _database.CreateTablesAsync<Book, User, BookUser>().Wait();
        }

        public Task<List<Book>> GetBooksAsync()
        {
            return _database.Table<Book>().ToListAsync();
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }


        public Task<Book> GetBookAsync(int id)
        {
            return _database.Table<Book>()
                                .Where(i => i.Id == id)
                                .FirstOrDefaultAsync();
        }

        public Task<User> GetUserAsync(int id)
        {
            return _database.Table<User>()
                                .Where(i => i.Id == id)
                                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserWithChildren(User user)
        {
            return await _database.GetWithChildrenAsync<User>(user.Id);
        }

        public async Task UpdateUserModel(User user)
        {
            await _database.GetChildrenAsync<User>(user);
        }

        public async Task AddBookToUser(Book book, User user)
        {
            await _database.GetChildrenAsync<User>(user);
            user.Books.Add(book);
            await _database.UpdateWithChildrenAsync(user);
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            return _database.Table<User>()
                                .Where(i => i.Username == username)
                                .FirstOrDefaultAsync();
        }

        public Task<int> SaveBookAsync(Book book)
        {
            if(book.Id != 0)
            {
                return _database.UpdateAsync(book);
            }
            else
            {
                return _database.InsertAsync(book);
            }
        }

        internal Task GetUserByUsernameAsync(object p)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveUserAsync(User user)
        {
            if (user.Id != 0)
            {
                return _database.UpdateAsync(user);
            }
            else
            {
                return _database.InsertAsync(user);
            }
        }

        public Task<int> DeleteBookAsync(Book book)
        {
            return _database.DeleteAsync(book);
        }

        public Task<int> DeleteUserAsync(User user)
        {
            return _database.DeleteAsync(user);
        }

    }
}
