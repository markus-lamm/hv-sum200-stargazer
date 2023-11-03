using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.JSInterop;
using sum200_project_stargazer.Model;

namespace sum200_project_stargazer.Data
{
    internal class ApiService
    {
        public List<ApodImage> galleryApod { get; set; }

        public ApodImage dailyApod { get; set; }

        public async Task<ApodImage> GetDailyApod()
        {
            if (dailyApod == null)
            {
                dailyApod = await ApiCallDailyApod();
                return dailyApod;
            }
            else
            {
                return dailyApod;
            }
        }

        public async Task<List<ApodImage>> GetGalleryApod()
        {
            if (galleryApod == null)
            {
                galleryApod = await ApiCallGalleryApod();
                return galleryApod;
            }
            else
            {
                return galleryApod;
            }
        }

        public async Task<ApodImage> GetApodById(string id)
        {
            if (galleryApod == null)
            {
                galleryApod = await ApiCallGalleryApod();
            }
            if (dailyApod == null)
            {
                dailyApod = await ApiCallDailyApod();
            }
            if (dailyApod.Id == id)
            {
                return dailyApod;
            }
            else
            {
                return galleryApod.FirstOrDefault(x => x.Id == id);
            }
        }

        public async Task<List<ApodImage>> ForceGalleryRefresh()
        {
            galleryApod = await ApiCallGalleryApod();
            return galleryApod;
        }

        public async Task<ApodImage> ApiCallDailyApod()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("https://api.nasa.gov/planetary/apod?api_key=BUFPNqGtnzJxnA0vIh3SmvifO74YDIpWcoHNoj3y");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApodImage>(json);
                    result.Id = Guid.NewGuid().ToString();
                    return result;
                }
                else
                {
                    var toast = Toast.Make("An error has occured, check your internet connection", ToastDuration.Short, 14);
                    await toast.Show();
                    return null;
                }
            }
            catch
            {
                var toast = Toast.Make("An error has occured, check your internet connection", ToastDuration.Short, 14);
                await toast.Show();
                return null;
            }
        }

        public async Task<List<ApodImage>> ApiCallGalleryApod()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("https://api.nasa.gov/planetary/apod?api_key=BUFPNqGtnzJxnA0vIh3SmvifO74YDIpWcoHNoj3y&count=5");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<ApodImage>>(json);
                    foreach (var image in result)
                    {
                        image.Id = Guid.NewGuid().ToString();
                    }
                    return result;
                }
                else
                {
                    var toast = Toast.Make("An error has occured, check your internet connection", ToastDuration.Short, 14);
                    await toast.Show();
                    return null;
                }
            }
            catch
            {
                var toast = Toast.Make("An error has occured, check your internet connection", ToastDuration.Short, 14);
                await toast.Show();
                return null;
            }
        }
    }
}