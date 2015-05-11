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
using Windows.UI.Popups;
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
    public sealed partial class SignUpPage : Page
    {
        List<Members> memberList; //will eventually get rid of this
        int memID;
        public SignUpPage()
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
            GetMembers();
        }

        private async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            bool notNull = false;

            //null string check 4-9-2015
            #region NullStringCheck
            if (string.IsNullOrWhiteSpace(usernameBox.Text))
            {
                MessageDialog msg = new MessageDialog("username cannot be empty.");
                await msg.ShowAsync();
            }
            else if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageDialog msg1 = new MessageDialog("password cannot be empty");
                await msg1.ShowAsync();
            }
            else if (string.IsNullOrWhiteSpace(confirmPasswordBox.Password))
            {
                MessageDialog msg2 = new MessageDialog("confirm password cannot be empty");
                await msg2.ShowAsync();
            }
            else if (string.IsNullOrWhiteSpace(cityBox.Text))
            {
                MessageDialog msg3 = new MessageDialog("city cannot be left empty");
                await msg3.ShowAsync();
            }
            else if (string.IsNullOrWhiteSpace(stateBox.Text))
            {
                MessageDialog msg4 = new MessageDialog("state cannot be left empty");
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

                //values obtained through popup
                string username = usernameBox.Text; 
                string password = null;
                if (passwordBox.Password.Equals(confirmPasswordBox.Password))
                {
                    password = passwordBox.Password;
                }
                else
                {
                    //what should happen if passwords invalid?
                    //They should be electrocuted for forgetting.... 
                    MessageDialog msg5 = new MessageDialog("passwords don't match");
                    await msg5.ShowAsync();
                    return; //stops method and prompts for error
                }
                string city = cityBox.Text;
                string state = stateBox.Text;
                int team = 0;
                //teamIDs are 65 - 96. Need to create a combo box for this
                //team = Convert.ToInt32(teamBox.Text);
                ComboBoxItem typeItem = (ComboBoxItem)teamBox.SelectedItem;
                string teamStr = typeItem.Content.ToString();
                //string teamStr = t.ToString();
                team = Teams.getIdFromTeamName(teamStr);

                //trying to catch exception thrown from not filling in all fields
                 Members addedMember = new Members(username, password, city, state, team);
                    if (addedMember.usernameIsUnique(username))
                    {
                        if (addedMember.completelyFilled())
                        {
                            //MembersListBox.Items.Add("username is unique");
                            String newMemberStr = JsonConvert.SerializeObject(addedMember);
                            HttpContent postStr = new StringContent(newMemberStr);
                            var response = await client.PostAsync("api/Members?newMember=" + WebUtility.UrlEncode(newMemberStr), postStr);
                            HttpResponseMessage memberResponse = await client.GetAsync("api/Members?MemberStr=" + username);

                            //if memberId displays as -1, means response failed. 0 means page not receiving value
                            int memberId = -1;
                            if (memberResponse.StatusCode == HttpStatusCode.OK)
                            {
                                // got the memberId...
                                memberId = Convert.ToInt32(memberResponse.Content.ReadAsStringAsync().Result);
                            }
                                //internet connection test
                            else
                            {
                                MessageDialog msg = new MessageDialog("You need internet connection to continue");
                                await msg.ShowAsync();
                                return;
                            }
                            if (notNull == true)
                            {
                                MessageDialog msg = new MessageDialog("Account created successfully");
                                await msg.ShowAsync();
                                this.Frame.Navigate(typeof(HomeHub), memberId);
                            }
                            
                        }
                    }
                    else
                    {
                        MessageDialog msg6 = new MessageDialog("username is already taken");
                        await msg6.ShowAsync();
                    }

            }
           
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void viewMembers_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewMembers));
        }

        public async void GetMembers()
        {
            using (var client = new HttpClient())
            {
                //MembersListBox.Items.Add("using block entered");
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Members"); //debugger leaves method after this line
                if (response.IsSuccessStatusCode) //code not making it to here
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
            }
        }

    }
}
