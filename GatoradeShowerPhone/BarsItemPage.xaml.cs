using GatoradeShowerPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GatoradeShowerPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BarsItemPage : Page
    {
        int memID;

        
        public BarsItemPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            memID = (int)e.Parameter;
            GetBars(); 
        }


        public async void GetBars()
        {
            using (var client = new HttpClient())
            {
                //PartyListBox.Items.Add("using block entered");
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //PartyListBox.Items.Add("client's defaultrequestheaders done");

                HttpResponseMessage response = await client.GetAsync("api/bars");
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<Bars>>(s);
                    foreach (Bars bar in deserializedResponse)
                    {
                        barsListBox.Items.Add(bar.ToString());
                    }
                }
                //GroupsListBox.Items.Add(Groups.GroupsList.Count);
                foreach (var bar in Bars.barsList)
                {
                    barsListBox.Items.Add(bar.ToString());
                }
            }
        }

        public async void getBarById(int id) 
        {
            //ListBox barsListBox = new ListBox();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/bars/" + id);
                barsListBox.Items.Add("before if");
                if (response.IsSuccessStatusCode)
                {
                    //barsListBox.Items.Add("if entered");
                    //int start = 0;
                    var bar = await response.Content.ReadAsStringAsync();
                    barsListBox.Items.Add(bar);
                    //int waste = stringToGroup(Group, start);
                    //barsListBox.Items.Add("readasstring executed");
                    //must use JsonProperty annotations in model to successfully link json string values to respected properties
                    //will generate objects with null values if dont.
                    var myDeserializedObj = JsonConvert.DeserializeObject<WatchParties>(bar);
                    barsListBox.Items.Add(myDeserializedObj.ToString());
                    //barsListBox.Items.Add("string deserialized into obj");
                    //Groups newGroup = new Groups(id, name, owner, cDate, game, eType, eID, priv);
                    //WatchParties.PartiesList.Add(myDeserializedObj);
                    barsListBox.Items.Add(myDeserializedObj);
                }

                //barsListBox.Items.Add("after if");
                foreach (var bar in Bars.barsList)
                {
                    barsListBox.Items.Add(bar.ToString());
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

        private void barsButton_Click(object sender, RoutedEventArgs e)
        {
            //I did not make this button. It does load when you open the page... It just takes a few seconds
            getBarById(10);
        }

        private void pop_Click(object sender, RoutedEventArgs e)
        {
            popUp.IsOpen = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            popUp.IsOpen = false;
        }

        private async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            bool notNull = false; 

                //null string check 4-9-2015
                #region NullStringCheck
                if (string.IsNullOrWhiteSpace(barNameBox.Text))
                {
                    MessageDialog msg = new MessageDialog("The bar name cannot be empty.");
                    await msg.ShowAsync();
                }
                else if (string.IsNullOrWhiteSpace(barStreetBox.Text))
                {
                    MessageDialog msg1 = new MessageDialog("The street address cannot be empty");
                    await msg1.ShowAsync();
                }
                else if (string.IsNullOrWhiteSpace(barCityBox.Text))
                {
                    MessageDialog msg2 = new MessageDialog("The city location cannot be left empty");
                    await msg2.ShowAsync();
                }
                else if (string.IsNullOrWhiteSpace(barStateBox.Text))
                {
                    MessageDialog msg3 = new MessageDialog("The state location cannot be left empty");
                    await msg3.ShowAsync();
                }
                else if (string.IsNullOrWhiteSpace(barPhoneBox.Text))
                {
                    MessageDialog msg4 = new MessageDialog("The phone number cannot be left empty");
                    await msg4.ShowAsync();
                }
                else
                {
                    notNull = true;
                }
                #endregion

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //get values from gui fields
                string name = barNameBox.Text;
                string street = barStreetBox.Text;
                string city = barCityBox.Text;
                string state = barStateBox.Text;
                string phone = barPhoneBox.Text;
                string zip = barZipBox.Text;
                Bars addedBar = new Bars(name, street, city, state, phone, zip);

                //barsListBox.Items.Add("before creating stringcontent");
                String newBarStr = JsonConvert.SerializeObject(addedBar);
                //barsListBox.Items.Add("newBarStr created");
                HttpContent postStr = new StringContent(newBarStr);
                //postStr.Headers.Add("Content-Type", "application/json");
                //barsListBox.Items.Add("postStr created");

                var response = await client.PostAsync("api/Bars?barstr=" + WebUtility.UrlEncode(newBarStr), postStr);
                //barsListBox.Items.Add("response created");
                GetBars();

            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            
            //Only if the response is ok.
            if (notNull == true)
            {
                popUp.IsOpen = false;
            }
        }
    }
}
