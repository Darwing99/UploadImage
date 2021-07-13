using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UploadImage.model;
using UploadImage.modelview;
using UploadImage.views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UploadImage
{
    public partial class MainPage : ContentPage
    {
        List<Imagen> service;
        RestService restService;
        byte[] imageArray = null;

        public MainPage()
        {
            InitializeComponent();
            restService = new RestService();
            enviarInfo.Clicked += EnviarInfo_Clicked;
          
        }


        private async void EnviarInfo_Clicked(object sender, EventArgs e)
        {
            
         
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported )
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
            var camera = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                Name = "img",
                Directory = "MiApp",
                DefaultCamera= Plugin.Media.Abstractions.CameraDevice.Front
               
            });


            if (camera != null)
            {
             
                image.Source = ImageSource.FromStream(() => {

                    return camera.GetStream();



                });
                using (MemoryStream memory = new MemoryStream())
                {

                    Stream stream = camera.GetStream();
                    stream.CopyTo(memory);
                    imageArray = memory.ToArray();
                }
            }
            if (camera == null)
                return;
            var compartirfoto = camera.Path;
          
          
            Guardar.IsEnabled = true;

        }

        protected async override void OnAppearing()
        {

            base.OnAppearing();
            service = await restService.GetRepositoriesAsync(DataConstants.urlGet);
            var datos = service;
        }

        private async void guardar_Clicked(object sender, EventArgs e)
        {

          
            if (string.IsNullOrEmpty(imageArray.ToString()))
            {
                await DisplayAlert("Alerta", "No ha tomado fotografias", "ok");
                return;

            }
            else
            {
                 var data = new Imagen
                  {
                      id = 0,
                      image = imageArray
                  };

                  var client = new HttpClient();
                  var json = JsonConvert.SerializeObject(data);
                  var contentJSON = new StringContent(json, Encoding.UTF8, "application/json");
                  var response = await client.PostAsync(DataConstants.urlPost, contentJSON);
                  if (response.StatusCode == HttpStatusCode.OK)
                  {
                      await DisplayAlert("Datos", "Se guardo correctamente la información", "OK");

                  }
                 
               
                
            }


        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListImage());
        }
    }

}
