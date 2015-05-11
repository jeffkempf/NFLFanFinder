using GatoradeShowerPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GatoradeShowerPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamsItemPage : Page
    {
        public TeamsItemPage()
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
            GetTeams();
            getTeamById(7);
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            GetTeams();
        }

        public async void GetTeams()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/teams");

                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<Teams>>(s);
                    foreach (Teams team in deserializedResponse)
                    {
                        teamsListBox.Items.Add(team.ToString());
                    }

                }
                teamsListBox.Items.Add(Teams.TeamsList.Count);
                foreach (var team in Teams.TeamsList)
                {
                    teamsListBox.Items.Add(team.ToString());
                }
            }
        }

        public async void getTeamById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/teams/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var team = await response.Content.ReadAsStringAsync();

                    var myDeserializedObj = JsonConvert.DeserializeObject<Teams>(team);
                    teamsListBox.Items.Add(myDeserializedObj.ToString());

                    teamsListBox.Items.Add(myDeserializedObj);
                }

                foreach (var Team in Teams.TeamsList)
                {
                    teamsListBox.Items.Add(Team.ToString());
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void itemSelected_Changed(object sender, SelectionChangedEventArgs e)
        {
            //panel2.Visibility = Visibility.Visible;
            instructions.Visibility = Visibility.Collapsed;

            string selectedItem = teamsListBox.Items[teamsListBox.SelectedIndex].ToString();
            //itemSelectedListBox.Items.Add(selectedItem);
            int tempId = teamsListBox.SelectedIndex;
            //itemSelectedListBox.Items.Add(tempId);
        }

        private void BackToTeams_Click(object sender, RoutedEventArgs e)
        {
            //panel2.Visibility = Visibility.Collapsed;
            instructions.Visibility = Visibility.Visible;
        }
    }
}
