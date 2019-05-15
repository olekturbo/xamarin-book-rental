using Books.InterfacesDS;
using Books.Models;
using Books.Validation;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Books.ViewModels
{
    class EditBookViewModel : NotifyBase
    {
        private readonly EditBook _editBook;
        private ValidatableObject<string> _title = new ValidatableObject<string>();
        private ValidatableObject<string> _description = new ValidatableObject<string>();
        private ValidatableObject<int> _count = new ValidatableObject<int>();
        private ValidatableObject<string> _image = new ValidatableObject<string>();
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyPropertyChange(nameof(Id));
            }
        }

        public ValidatableObject<string> Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChange(nameof(Title));
            }
        }

        public ValidatableObject<string> Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChange(nameof(Description));
            }
        }

        public ValidatableObject<int> Count
        {
            get => _count;
            set
            {
                _count = value;
                NotifyPropertyChange(nameof(Count));
            }
        }

        public ValidatableObject<string> Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyPropertyChange(nameof(Image));
            }
        }

        public ICommand Back => new Command(OnClickBack);
        public ICommand ChooseImage => new Command(OnClickImage);
        public ICommand Update => new Command(OnClickUpdate);

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async void OnClickImage(object obj)
        {
            Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                var bytes = ReadFully(stream);
                var name = $@"{Guid.NewGuid()}.jpg";

                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string filePath = path + "\\" + name;
                File.WriteAllBytes(filePath, bytes);
                _image.Value = filePath;

            }

        }

        private async Task GetCurrentBook()
        {
            var foundBook = await App.Database.GetBookAsync(_id);
            _title.Value = foundBook.Title;
            _description.Value = foundBook.Description;
            _count.Value = foundBook.Count;
            _image.Value = foundBook.Image;

        }

        async private void OnClickUpdate(object obj)
        {
            ValidateEntries();

            var isError = _title.Errors.Any() || _description.Errors.Any() ||
                _count.Errors.Any();

            if (isError)
                return;

            Book book = await App.Database.GetBookAsync(_id);
            book.Title = _title.Value;
            book.Description = _description.Value;
            book.Count = _count.Value;
            book.Image = _image.Value;
            await App.Database.SaveBookAsync(book);

            _editBook.DisplayAlert("Komunikat", "Dziękujemy - książka została edytowana.", "OK");
            _editBook.Navigation.PopAsync();

        }

        private void ValidateEntries()
        {
            _title.Validate();
            _description.Validate();
            _count.Validate();

        }

        private void OnClickBack(object obj)
        {
            _editBook.Navigation.PopAsync();
        }

        public EditBookViewModel(EditBook editBook, int id)
        {
            _editBook = editBook;
            _id = id;
            AddValidationRules();
            Task.Run(async () => { await GetCurrentBook(); });
        }

        private void AddValidationRules()
        {
            _title.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Tytuł jest wymagany."
            });

            _description.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Opis jest wymagany."
            });
        }
    }
}
