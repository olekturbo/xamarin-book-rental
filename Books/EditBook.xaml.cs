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
    public partial class EditBook : ContentPage
    {
        public EditBook(int id)
        {
            InitializeComponent();
            var editBookViewModel = new EditBookViewModel(this, id);
            BindingContext = editBookViewModel;
        }
    }
}