using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadImage.model;
using UploadImage.modelview;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UploadImage.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListImage : ContentPage
    {
        List<Imagen> region;
        List<Imagen> service;
        RestService restService;
        public ListImage()
        {
            InitializeComponent();
            restService = new RestService();
        }
        protected async override void OnAppearing()
        {

            base.OnAppearing();
            service = await restService.GetRepositoriesAsync(DataConstants.urlGet);
            collectionView.ItemsSource = service;
          
        }

    }
}