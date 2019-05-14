using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Books.ViewModels;

namespace Books
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBook : ContentPage
    {
        public AddBook()
        {
            InitializeComponent();
            var addBookViewModel = new AddBookViewModel(this);
            BindingContext = addBookViewModel;
        }
    }
}