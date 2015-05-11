using GatoradeShowerPhone.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GatoradeShowerPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeHub : Page
    {
        int memID;
        List<string> teamList = new List<string>();
        Members currMember;

        public HomeHub()
        {
            this.InitializeComponent();

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            memID = (int)e.Parameter;
            App.gSelected = null;
            Task t = getMemberById(memID);
            await t; //need to assign these methods to tasks then await tasks to ensure they execute in correct order

            //start of analytics portion
            Task y = getTeamsForMember();
            await y;
            int tc = getTeamCount();
            List<string> followedTeams = new List<string>();

            //must have joined 4+ groups before analytics tracks teams
            if (tc > 7)
            {
                currMember.removeAts(teamList);
                Dictionary<string, int> teamCount = currMember.getMemberTeams();
                if (teamCount.Count > 0)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        followedTeams = determineFollowedTeams(teamCount, tc);
                        if (followedTeams.Count > 0)
                        {
                            activityList.Items.Add("Based on your previous activity,\nwe think you might enjoy the \nfollowing groups:");
                        }
                        else
                        {//never reached
                        }
                        foreach (string team in followedTeams)
                        {
                            HttpResponseMessage response = await client.GetAsync("api/Groups?teamName=" + team + "&memID=" + memID);

                            //if successfully gets list of groups containing followed teams
                            if (response.IsSuccessStatusCode)
                            {
                                string s = await response.Content.ReadAsStringAsync();
                                var deserializedResponse = JsonConvert.DeserializeObject<List<string>>(s);
                                foreach (string group in deserializedResponse)
                                {
                                    //this really slows down logging in. Would have been better if Janci updated her stored procedure
                                    response = await client.GetAsync("api/Groups?GroupName=" + group);
                                    string mems = await response.Content.ReadAsStringAsync();
                                    var groupMemList = JsonConvert.DeserializeObject<List<string>>(mems);
                                    if(!groupMemList.Contains(currMember.ToString())) {
                                        groupSelectionbox.Items.Add(group);
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                
            }
            
        }


        #region ButtonHandlers
        private void barsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BarsItemPage), memID);
        }

        private void groupsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupsItemPage), memID);
        }

        private void teamsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TeamsItemPage), memID);
        }

        private void gamesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamesItemPage), memID);
        }

        private void watchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WatchItemPage), memID);
        }

        //temp button members
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewMembers), memID);
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
#endregion

        #region Getters
        public async Task getMemberById(int id) //try making this not async so it will complete before getTeamsForMember starts
        {
            //ListBox MembersListBox = new ListBox();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Members/" + id);
                //await causes getTeamsForMember to execute before this finishes. This prevents analytics.
                if (response.IsSuccessStatusCode)
                {
                    var member = await response.Content.ReadAsStringAsync();
                    //homeList.Items.Add(member);

                    currMember = JsonConvert.DeserializeObject<Members>(member);
                    homeList.Items.Add("Welcome, " + currMember.ToString() + "!");
                    //homeList2.Items.Add("Click Events to join an \nexisting event or create \n your own");
                    //homeList.Items.Add(myDeserializedObj);
                }

            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        //returning Task is sim to returning void except can assign return value to a thread to ensure method finishes executing
        //before next async method executes
        public async Task getTeamsForMember()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/MemberGroups?mg=" + memID);

                if (response.IsSuccessStatusCode)
                {
                    var teams = await response.Content.ReadAsStringAsync();
                    teamList = JsonConvert.DeserializeObject<List<string>>(teams);
                }
            }
        }

        public int getTeamCount()
        {
            int count = 0;
            foreach (string team in teamList)
            {
                count++;
            }
            return count;
        }

        //any team that appears in 35%+ of user's groups is considered a followed team. Nothing special about 35. Just seemed good.
        public List<string> determineFollowedTeams(Dictionary<string, int> teams, int tc)
        {
            List<string> followedTeams = new List<string>();
            foreach (var team in teams)
            {
                if (team.Value >= (tc * 0.35))
                {
                    followedTeams.Add(team.Key);
                }
            }
            return followedTeams;
        }

        #endregion

        #region ListboxHandlers
        //redirects to the game pop up
        private void SelectedItem_Changed(object sender, SelectionChangedEventArgs e)
        {
            var current = Window.Current.Content as Frame;
            var page = current.Content as GroupsItemPage;
            GroupsItemPage gp = new GroupsItemPage();

            //I wanted this part to automatically select the group for the user. 
            //however I cannot make it work
            App.gSelected = groupSelectionbox.SelectedItem.ToString();

            //GroupsItemPage page1 = new GroupsItemPage();
            gp.clickOnGroup();

            //page1.ItemSent_Changed(this, null);
            this.Frame.Navigate(typeof(GroupsItemPage), memID);
        }

        #endregion
    }
}