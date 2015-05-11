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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
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

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        //Login page handler
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //obtain inputted username and password
            String currUser = userBox.Text;
            String currPass = passwordBox.Password;
            //return list of all existing users, then loop through to find a matching username/password
            GetMembers(currUser, currPass);
        }

        public async void GetMembers(String currUser, String currPass)
        {
            using (var client = new HttpClient())
            {
                //MembersListBox.Items.Add("using block entered");
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //MembersListBox.Items.Add("client's defaultrequestheaders done");

                //gets all members from table
                HttpResponseMessage response = await client.GetAsync("api/Members");
                //MembersListBox.Items.Add("after response reached");
                if (response.IsSuccessStatusCode)
                {
                    //reads all member objs from table as a json string
                    string s = await response.Content.ReadAsStringAsync();
                    //how can we pass the user's login credentials (including ID) to other pages? via HttpClient?
                    //converts string of members into a list of member objs
                    var deserializedResponse = JsonConvert.DeserializeObject<List<Members>>(s);
                    int memberId = 0;
                    foreach (Members member in deserializedResponse)
                    {
                        //if current member matches a member found in list
                        if(member.compareUserAndPassword(currUser, currPass)) {
                            //MembersListBox.Items.Add(currUser + " found.");
                            //MembersListBox.Items.Add(member.userName + " " + member.password);
                            Members currMember = member; //this works
                            #region out
                            //MembersListBox.Items.Add("Current member: " + currMember.ToString());
                            //how should memberID be remembered for user?
                            //client.DefaultRequestHeaders.Authorization = CreateBasicHeader(currUser, currPass);
                            //MembersListBox.Items.Add(client.DefaultRequestHeaders.Authorization);
                            #endregion
                            HttpResponseMessage memberResponse = await client.GetAsync("api/Members?MemberStr=" + currUser);
                            
                            if (memberResponse.StatusCode == HttpStatusCode.OK)
                            {
                                // got the memberId...
                                memberId = Convert.ToInt32(memberResponse.Content.ReadAsStringAsync().Result);
                            }
                            else if (memberResponse.StatusCode == HttpStatusCode.NotFound)
                            {
                                // member not found...
                                MessageDialog msg = new MessageDialog("incorrect username and/or password");
                                await msg.ShowAsync();
                                memberId = -1;
                            }
                            else
                            {
                                memberId = -2; //for testing
                            }
                            
                            //will need to call server's getidbyname method and pass result instead of currMember.memberID
                            this.Frame.Navigate(typeof(HomeHub), memberId);
                        } 
                    }
                    if (memberId == 0)
                    {
                        MessageDialog msg = new MessageDialog("Incorrect username and/or password");
                        await msg.ShowAsync();
                    }
                }
                else
                {
                    MessageDialog msg = new MessageDialog("You need internet connection to continue");
                    await msg.ShowAsync();
                }
                #region commentout
                //MembersListBox.Items.Add(Members.MembersList.Count);
                //foreach (var member in Members.MembersList)
                //{
                //   MembersListBox.Items.Add(member.ToString());
                //}
                #endregion
            }
        }
        #region commentout
        //creates authentication header value that stores username:password in httpclient header
        //public AuthenticationHeaderValue CreateBasicHeader(string username, string password)
        //{
        //    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(username + ":" + password);
        //    return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        //} 
        #endregion
    }
}
