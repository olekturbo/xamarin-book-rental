using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Books.ViewModels;
using Xamarin.Essentials;

namespace Books
{
    public partial class DataPage : ContentPage
    {
        public DataPage()
        {
            InitializeComponent();
            var dataPageViewModel = new DataPageViewModel(this);
            BindingContext = dataPageViewModel;
        }

    }
}