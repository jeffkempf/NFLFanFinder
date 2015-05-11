using GatoradeShowerPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;

//using System.Data;
//using System.Data.SqlClient;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GatoradeShowerPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamesItemPage : Page
    {
        public GamesItemPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GetGames();
        }

        #region ButtonHandlers
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        #endregion

        #region Getters
        public async void GetGames() //currently works but slow. Also fixes non-scrolling game box when creating groups
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Games");

                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<Games>>(s);
                    foreach (Games game in deserializedResponse)
                    {
                        gamesListBox.Items.Add(game.ToString());
                        if (!Games.GamesList.Contains(game))
                        {
                            Games.GamesList.Add(game);
                        }
                    }

                }

            }
            //PopulateGames();
            foreach (Games game in Games.GamesList) //GamesList successfully being populated
            {
                gamesListBox.Items.Add(game.ToString());
            }

        }

        //populates static GamesList list with games taken from db. Will need to call this in event creation to generate
        //games list for games page and for event creation page don't delete or modify populategames or getgames
        public async void PopulateGames()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Games");

                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<Games>>(s);
                    foreach (Games game in deserializedResponse)
                    {
                        //gamesListBox.Items.Add(game.ToString());
                        if (!Games.GamesList.Contains(game))
                        {
                            Games.GamesList.Add(game);
                        }
                    }

                }

            }
        }

        public async void getGameById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Games/" + id);

                if (response.IsSuccessStatusCode)
                {

                    var game = await response.Content.ReadAsStringAsync();
                    gamesListBox.Items.Add(game);

                    var myDeserializedObj = JsonConvert.DeserializeObject<WatchParties>(game);
                    gamesListBox.Items.Add(myDeserializedObj.ToString());
                    gamesListBox.Items.Add(myDeserializedObj);
                }

                foreach (var game in Games.GamesList)
                {
                    gamesListBox.Items.Add(game.ToString());
                }
            }
        }
#endregion
    }
}
