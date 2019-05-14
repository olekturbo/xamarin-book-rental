using Books.InterfacesDS;
using Books.Models;
using Books.Validation;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Books.ViewModels
{
    class AddBookViewModel : NotifyBase
    {
        private readonly AddBook _addBook;
        private ValidatableObject<string> _title = new ValidatableObject<string>();
        private ValidatableObject<string> _description = new ValidatableObject<string>();
        private ValidatableObject<int> _count = new ValidatableObject<int>();
        private ValidatableObject<string> _image = new ValidatableObject<string>();

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
        public ICommand AddNew => new Command(OnClickAddNew);

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

        async private void OnClickAddNew(object obj)
        {
            ValidateEntries();

            var isError = _title.Errors.Any() || _description.Errors.Any() ||
                _count.Errors.Any();

            if (isError)
                return;

            Book book = new Book();
            book.Title = _title.Value;
            book.Description = _description.Value;
            book.Count = _count.Value;
            book.Image = _image.Value;
            await App.Database.SaveBookAsync(book);

            _addBook.DisplayAlert("Komunikat", "Dziękujemy - książka została dodana.", "OK");
            _addBook.Navigation.PopAsync();

        }

        private void ValidateEntries()
        {
            _title.Validate();
            _description.Validate();
            _count.Validate();

        }

        private void OnClickBack(object obj)
        {
            _addBook.Navigation.PopAsync();
        }

        public AddBookViewModel(AddBook addBook)
        {
            _addBook = addBook;
            AddValidationRules();
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
