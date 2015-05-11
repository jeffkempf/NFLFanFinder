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
    public sealed partial class ViewMembers : Page
    {
        int memID;
        List<Members> memberList;

        public ViewMembers()
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
            if (memID != 0) //an int will never be null so memID != null will always be true
            {
                memID = (int)e.Parameter;
            }
            //getMemberById(101);
            GetMembers();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }


        public async void GetMembers()
        {
            using (var client = new HttpClient())
            {
                //MembersListBox.Items.Add("using block entered");
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.GetAsync("api/Members");
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    memberList = JsonConvert.DeserializeObject<List<Members>>(s);
                    foreach (var member in memberList)
                    {
                        if (!Members.MembersList.Contains(member))
                        {
                            Members.MembersList.Add(member);
                        }
                    }

                }

                //MembersListBox.Items.Add("The following is a list of Members.MembersList \n");
                //displays all registered members for testing. Works correctly
                //foreach (var member in Members.MembersList)
                //{
                //    MembersListBox.Items.Add(member.ToString());
                //}
            }
        }

        public async void getMemberById(int id)
        {
            //ListBox MembersListBox = new ListBox();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Members/" + id);
                //MembersListBox.Items.Add("before if");
                if (response.IsSuccessStatusCode)
                {
                    //MembersListBox.Items.Add("if entered");
                    var member = await response.Content.ReadAsStringAsync();
                    MembersListBox.Items.Add(member);
                    //must use JsonProperty annotations in model to successfully link json string values to respected properties
                    //will generate objects with null values if dont.
                    var myDeserializedObj = JsonConvert.DeserializeObject<Members>(member);
                    MembersListBox.Items.Add(myDeserializedObj.ToString());
                    //MembersListBox.Items.Add("string deserialized into obj");
                    //Groups newGroup = new Groups(id, name, owner, cDate, game, eType, eID, priv);
                    //WatchParties.PartiesList.Add(myDeserializedObj);
                    MembersListBox.Items.Add(myDeserializedObj);
                }
                //MembersListBox.Items.Add("after if");
                foreach (var member in Members.MembersList)
                {
                    MembersListBox.Items.Add(member.ToString());
                }
            }
        }

        private async void addMember_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Members addedMember = new Members("tester1", "test", "Raleigh", "NC", 66);
                String newMemberStr = JsonConvert.SerializeObject(addedMember);
                var postStr = new StringContent(addedMember.ToString());
                //postStr.ReadAsStringAsync(newMemberStr);
                var response = await client.PostAsync("api/Members?NewMember=" + WebUtility.UrlEncode(newMemberStr), postStr);
                GetMembers();

            }
        }

        private void pop_Click(object sender, RoutedEventArgs e)
        {
            //popUp.IsOpen = true;
        }

        //private async void enterButton_Click(object sender, RoutedEventArgs e)
        //{
           
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        //values obtained through popup
        //        string username = usernameBox.Text;
        //        string password = null;
        //        if(passwordBox.Text.Equals(confirmPasswordBox.Text))
        //        {
        //            password = passwordBox.Text;
        //        }
        //        else
        //        {
        //            //what should happen if passwords invalid?
        //            //They should be electrocuted for forgetting.... 
        //            passwordBox.Text = "Passwords don't match";
        //        }
        //        string city = cityBox.Text;
        //        string state = stateBox.Text;
        //        int team = 0;
        //        //teamIDs are 65 - 96. Need to create a combo box for this
        //        team = Convert.ToInt32(teamBox.Text);

        //        Members addedMember = new Members(username, password, city, state, team);
        //        if (addedMember.usernameIsUnique(username))
        //        {
        //            if (addedMember.completelyFilled())
        //            {
        //                //MembersListBox.Items.Add("username is unique");
        //                String newMemberStr = JsonConvert.SerializeObject(addedMember);
        //                HttpContent postStr = new StringContent(newMemberStr);
        //                var response = await client.PostAsync("api/Members?newMember=" + WebUtility.UrlEncode(newMemberStr), postStr);
        //                GetMembers();
        //                popUp.IsOpen = false;
        //            }
        //            else
        //            {
        //                //will need to find better place to put this
        //                usernameBox.Text = "All fields are required";
        //            }
                    
        //        }
        //        else
        //        {
        //            usernameBox.Text = " ---> username already taken <---";
        //        }
                
        //        //GetMembers();
                
        //    }
        //    //popUp.IsOpen = false;

        //}

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //popUp.IsOpen = false;
            //popUp2.IsOpen = false; 
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            //popUp.IsOpen = false;
            //popUp2.IsOpen = true;
        }

        private void backPop_Click(object sender, RoutedEventArgs e)
        {
            //popUp2.IsOpen = false;
            //popUp.IsOpen = true;
        }

       
    }
}
