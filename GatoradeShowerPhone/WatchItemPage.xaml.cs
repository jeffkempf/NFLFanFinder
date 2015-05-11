using GatoradeShowerPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GatoradeShowerPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WatchItemPage : Page
    {
        int memID;
        public WatchItemPage()
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
            memID = (int)e.Parameter;
            //getPartyById(4);
            GetParties();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public async void getPartyById(int id)
        {
            //ListBox GroupsListBox = new ListBox();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/watchparties/" + id);
                //PartyListBox.Items.Add("before if");
                if (response.IsSuccessStatusCode)
                {
                    //PartyListBox.Items.Add("if entered");                   
                    var party = await response.Content.ReadAsStringAsync();
                    PartyListBox.Items.Add(party);
                    //must use JsonProperty annotations in model to successfully link json string values to respected properties
                    //will generate objects with null values if dont.
                    var myDeserializedObj = JsonConvert.DeserializeObject<WatchParties>(party); 
                    PartyListBox.Items.Add(myDeserializedObj.ToString());
                    //PartyListBox.Items.Add("string deserialized into obj");
                    //Groups newGroup = new Groups(id, name, owner, cDate, game, eType, eID, priv);
                    //WatchParties.PartiesList.Add(myDeserializedObj);
                    PartyListBox.Items.Add(myDeserializedObj);
                }

                //GroupsListBox.Items.Add("after if");
                foreach (var party in WatchParties.PartiesList)
                {
                    PartyListBox.Items.Add(party.ToString());
                }
            }
        }

        public async void GetParties()
        {
            //ListBox GroupsListBox = new ListBox(); //why did you do this? It doesn't sync to anything in the xaml.
            //printing to this is pointless because the user will never see it.
            using (var client = new HttpClient())
            {
                //PartyListBox.Items.Add("using block entered");
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //PartyListBox.Items.Add("client's defaultrequestheaders done");

                HttpResponseMessage response = await client.GetAsync("api/watchparties");
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<WatchParties>>(s);
                    foreach (WatchParties party in deserializedResponse)
                    {
                        PartyListBox.Items.Add(party.ToString());
                    }
                    
                }
                //GroupsListBox.Items.Add(Groups.GroupsList.Count);
                foreach (var party in WatchParties.PartiesList)
                {
                    PartyListBox.Items.Add(party.ToString());
                }
            }
        }

        private async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //values obtained through popup
                //int member = 1; //will need to change this
                string street = streetBox.Text;
                string city = cityBox.Text;
                string state = stateBox.Text;
                bool priv = false;
                if ((bool)publicRadio.IsChecked)
                {
                    priv = false;
                }
                else
                {
                    priv = true;
                }

                WatchParties addedParty = new WatchParties(memID, street, city, state, priv);

                String newPartyStr = JsonConvert.SerializeObject(addedParty);
                HttpContent postStr = new StringContent(newPartyStr);
                var response = await client.PostAsync("api/WatchParties?partyStr=" + WebUtility.UrlEncode(newPartyStr), postStr);
                GetParties();

            }

        }


         private void addWatchParty_Click(object sender, RoutedEventArgs e)
         {
             addWatchPartyPop.IsOpen = true;
         }

         private void cancelButton_Click(object sender, RoutedEventArgs e)
         {
             addWatchPartyPop.IsOpen = false;
         }

    }
}
