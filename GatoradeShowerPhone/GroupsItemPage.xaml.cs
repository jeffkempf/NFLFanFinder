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
    public sealed partial class GroupsItemPage : Page
    {
        int memID;
        string selectedItem;

        public GroupsItemPage()
        {
            this.InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            #region navigationCheck
            pageTitle.Visibility = Visibility.Visible;
            firstViewer.Visibility = Visibility.Visible;
            groupsListBox.Visibility = Visibility.Visible;
            #endregion

            memID = (int)e.Parameter;
            groupsListBox.Items.Add("current member ID: " + memID);
            GetGroups();

            eventTypeBox.Items.Add("Bar Meetup");
            eventTypeBox.Items.Add("Watch Party");
            eventTypeBox.Items.Add("Stadium Meetup");

            GamesItemPage gp = new GamesItemPage();
            gp.PopulateGames();
            //groupGameBox.ItemsSource = Games.GamesList; //this works
            GetGames();

        }

        #region Getters
        public async void GetGames()
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
                        groupGameBox.Items.Add(game.ToString());
                        if (!Games.GamesList.Contains(game))
                        {
                            Games.GamesList.Add(game);
                        }
                    }

                }
            }
        }

        public async void GetGroups()
        {
            groupsListBox.Items.Clear(); //clear out listbox to avoid duplicate listings
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/groups");

                //if successfully get all groups from db
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<Groups>>(s);
                    foreach (Groups party in deserializedResponse)
                    {
                        if (!Groups.GroupsList.Contains(party))
                        {
                            Groups.GroupsList.Add(party);
                        }
                        groupsListBox.Items.Add(party.ToString());
                    }
                }
            }
        }

        //updates the mgs in MemberGroupList to reflect any recent changes in db. Will call this often.
        public async void populateMemberGroupList()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response4 = await client.GetAsync("api/MemberGroups");

                //if getting all membergroups is successful - doing this for validation
                if (response4.IsSuccessStatusCode)
                {
                    //reads server response in as Json string, then convert to MemberGroups list
                    string s1 = await response4.Content.ReadAsStringAsync();
                    var allMGs = JsonConvert.DeserializeObject<List<MemberGroups>>(s1);
                    MemberGroups.MemberGroupList.Clear();
                    foreach (MemberGroups mg in allMGs)
                    {
                        MemberGroups.MemberGroupList.Add(mg);
                    }
                }
            }
        }

        public async void getGroupById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Groups/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<Groups>(s);
                }
            }
        }

        //returns list of members enrolled in specified group
        public async void getMembersPerGroup()
        {
            itemSelectedListBox.Items.Clear();
            itemSelectedListBox.Items.Add("\t" + selectedItem);
            itemSelectedListBox.Items.Add("Members List: ");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Groups?GroupName=" + selectedItem);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<List<string>>(s);
                    foreach (string member in deserializedResponse)
                    {
                        itemSelectedListBox.Items.Add(member);
                    }
                }
            }
        }

        //helper method to get home and away strings out of whole game's toString. Used to find gameID
        public List<string> getHomeAndAway(string whole)
        {
            List<string> homeAway = new List<string>();
            int b1 = whole.IndexOf("@");
            string away = whole.Substring(0, b1);
            int b2 = whole.IndexOf(":");
            string home = whole.Substring(b1, (b2 - b1));
            homeAway.Add(away);
            homeAway.Add(home);
            return homeAway;
        }

#endregion

        #region ButtonHandlers
        private async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //values obtained through popup
                string groupName = groupNameBox.Text;
                //string cdate = DateTime.Now.ToString();
                DateTime cdate = DateTime.Now;
                int gameID = 0;
                string gameStr = groupGameBox.SelectedItem.ToString(); //currently throws an error
                List<string> homeAway = getHomeAndAway(gameStr); //might need to change this to 2 strings
                string away = homeAway[0];
                string home = homeAway[1];
                if (gameStr == null)
                {
                    MessageDialog emptyMsg = new MessageDialog("You must select a game");
                    await emptyMsg.ShowAsync();
                    return;
                }
                //gameID = gameStr.gameID;//will need to 
                var teamsMsg = await client.GetAsync("api/Games?home=" + home + "&away=" + away);
                if (teamsMsg.IsSuccessStatusCode)
                {
                    var testing = teamsMsg.Content.ReadAsStringAsync().Result;
                    gameID = Convert.ToInt32(teamsMsg.Content.ReadAsStringAsync().Result);

                }
                string eventType = (String)eventTypeBox.SelectedItem;
                if (eventType == null)
                {
                    MessageDialog emptyMsg = new MessageDialog("You must select an event type");
                    await emptyMsg.ShowAsync();
                    return;
                }
                string groupLocation = groupLocationBox.Text;

                bool conf = false; //what are we going to do about party privacy?
                Groups addedGroup = new Groups(groupName, memID, cdate, gameID, eventType, groupLocation, conf);

                //checking that all fields filled out
                if (addedGroup.completelyFilled())
                {
                    //checks that group name is unique
                    if (addedGroup.hasUniqueName())
                    {
                        //convert Groups obj to Json string for server to receive it
                        String newGroupStr = JsonConvert.SerializeObject(addedGroup);
                        HttpContent postGroupStr = new StringContent(newGroupStr);
                        var response = await client.PostAsync("api/Groups?groupStr=" + WebUtility.UrlEncode(newGroupStr), postGroupStr); //causing HTTP 500 error

                        //if posting new group successful
                        if (response.IsSuccessStatusCode)
                        {
                            //get groupID then create a new MemberGroup entity using gameID and memID
                            int newGroupID = 0;
                            HttpResponseMessage groupResponse = await client.GetAsync("api/Groups?byName=" + groupName);
                            if (groupResponse.StatusCode == HttpStatusCode.OK)
                            {
                                // got the memberId...
                                newGroupID = Convert.ToInt32(groupResponse.Content.ReadAsStringAsync().Result);
                                MemberGroups newMemGrp = new MemberGroups(newGroupID, memID, gameID);

                                //convert MemberGroups obj to Json string for server to receive
                                string newMG = JsonConvert.SerializeObject(newMemGrp);

                                //post new memberGroup obj to db
                                HttpContent postStr = new StringContent(newMG);
                                HttpResponseMessage response2 = await client.PostAsync("api/MemberGroups?newMG=" + WebUtility.UrlEncode(newMG), postStr);
                                //if posting new membergroup is successful
                                if (response2.IsSuccessStatusCode)
                                {
                                    //following 2 methods perform same functionality as calling viewMembers_Click
                                    populateMemberGroupList();
                                    getMembersPerGroup();
                                    MessageDialog successMsg = new MessageDialog("You have successfully created and joined the group");
                                    await successMsg.ShowAsync();
                                }
                                else
                                {
                                    itemSelectedListBox.Items.Add("Failed to post new membergroup"); //should never display
                                }
                            }
                            //displays groups again
                            panel2.Visibility = Visibility.Collapsed;
                            pageTitle.Visibility = Visibility.Visible;
                            firstViewer.Visibility = Visibility.Visible;
                            groupsListBox.Visibility = Visibility.Visible;

                            //GetGroups();

                            addAnEventPop.IsOpen = false;
                        }
                        else
                        {
                            MessageDialog failMsg = new MessageDialog("Group failed to create");
                            await failMsg.ShowAsync();
                        }

                    }
                    else
                    {
                        MessageDialog notUniqueMsg = new MessageDialog("Name is already taken");
                        await notUniqueMsg.ShowAsync();
                    }
                }
                else
                {
                    errorListBox.Items.Add("You must fill out all values to create a group");
                    addAnEventPop.IsOpen = true;
                }
            }
            GetGroups();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            addAnEventPop.IsOpen = false;
            panel2.Visibility = Visibility.Visible;
            //viewMembers.Visibility = Visibility.Visible;
            itemSelectedListBox.Visibility = Visibility.Visible;
            toGroups.Visibility = Visibility.Visible;
        }

        //works correctly and validated
        private async void joinButton_Click(object sender, RoutedEventArgs e)
        {
            pageTitle.Visibility = Visibility.Collapsed;
            firstViewer.Visibility = Visibility.Collapsed;
            groupsListBox.Visibility = Visibility.Collapsed;

            populateMemberGroupList(); //should repopulate membergroups before creating/validating new membergroups
            int currGroupID = 0;

            using (var client = new HttpClient())
            {
                //first need to find groupID. Following foreach not tested yet
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage groupResponse = await client.GetAsync("api/Groups?byName=" + selectedItem);
                if (groupResponse.StatusCode == HttpStatusCode.OK)
                {
                    // got the memberId...
                    currGroupID = Convert.ToInt32(groupResponse.Content.ReadAsStringAsync().Result);
                }
                HttpResponseMessage response = await client.GetAsync("api/groups/" + currGroupID);

                //if getting current group is successful
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    Groups currGroup = JsonConvert.DeserializeObject<Groups>(s);
                    MemberGroups newMemGrp = new MemberGroups(currGroupID, memID, currGroup.gameID);

                    //validate that member is not already in group
                    if (newMemGrp.notInGroup()) //correct values being passed
                    {
                        //convert MemberGroups obj to Json string for server to receive
                        string newMG = JsonConvert.SerializeObject(newMemGrp);

                        //post new memberGroup obj to db
                        HttpContent postStr = new StringContent(newMG);
                        HttpResponseMessage response2 = await client.PostAsync("api/MemberGroups?newMG=" + WebUtility.UrlEncode(newMG), postStr);

                        //if posting new membergroup is successful
                        if (response2.IsSuccessStatusCode)
                        {
                            //following 2 methods perform same functionality as calling viewMembers_Click
                            populateMemberGroupList();
                            getMembersPerGroup();
                            MessageDialog successMsg = new MessageDialog("You have successfully joined the group");
                            await successMsg.ShowAsync();
                        }
                        else
                        {
                            itemSelectedListBox.Items.Add("Failed to post new membergroup"); //should never display
                        }
                    }
                    else
                    {
                        MessageDialog inGroupErrorMsg = new MessageDialog("You're already in this group");
                        await inGroupErrorMsg.ShowAsync();
                    }
                }
                else
                {
                    itemSelectedListBox.Items.Add("Failed to get current group");//should never display
                }

                //returns to group detail page
                pageTitle.Visibility = Visibility.Visible;
                firstViewer.Visibility = Visibility.Visible;
                groupsListBox.Visibility = Visibility.Visible;
            }
        }

        //works correctly and validated
        private async void leaveButton_Click(object sender, RoutedEventArgs e)
        {
            populateMemberGroupList();
            int gID = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage groupResponse = await client.GetAsync("api/Groups?byName=" + selectedItem);

                if (groupResponse.StatusCode == HttpStatusCode.OK)
                {
                    // got the memberId...
                    gID = Convert.ToInt32(groupResponse.Content.ReadAsStringAsync().Result);
                }
            }

            //search membergroups for entry containing both memID and groupID
            int index1 = 0; //index within MemberGroupList, not MemGrpID

            //when not realizing already in group, because not being found in list
            foreach (MemberGroups mg in MemberGroups.MemberGroupList)
            {
                if (mg.groupID == gID && mg.memberID == memID)
                {
                    break;
                }
                index1++;
            }

            //if iterate through entire list without finding mg, reset index to 0
            if (index1 >= MemberGroups.MemberGroupList.Count)
            {
                index1 = 0;
            }

            //delete membergroup (if found) or notification if not
            if (index1 != 0)
            {
                //avoid creating a MemberGroups obj for this assignment. Will crash app if do so.
                int deleteID = MemberGroups.MemberGroupList[index1].memGroupID; //PK ID of data object in db
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.DeleteAsync("api/MemberGroups/" + deleteID);

                    //if membergroup object successfully deleted
                    if (response.IsSuccessStatusCode)
                    {
                        MessageDialog leaveSuccessMsg = new MessageDialog("You have left this group");
                        await leaveSuccessMsg.ShowAsync(); //these 2 methods will perform entire viewMembers_click method, which fixes join/leave issues
                        //populateMemberGroupList(); //for some reason deleted mg stays in list
                        MemberGroups.MemberGroupList.RemoveAt(index1);

                        //checks if group is empty after this member leaves. If so, deletes group
                        //problem occurs because mg getting deleted from db but not from MemberGroupList, so always returns true
                        if (!groupHasMembers(gID))
                        {
                            deleteGroup(gID);
                            //don't want to nav back to groups page with deleted group selected. Maybe that's causing error
                            groupsListBox.Items.Clear();

                            this.Frame.Navigate(typeof(GroupsItemPage), memID);
                        }
                        else
                        {
                            getMembersPerGroup(); //only need to display members if group not getting deleted
                        }
                    }
                    else
                    {
                        MessageDialog leaveFailMsg = new MessageDialog("delete response failed");
                        await leaveFailMsg.ShowAsync();
                    }
                }
            }
            else
            {
                MessageDialog notInGroupErrorMsg = new MessageDialog("You're not in this group");
                await notInGroupErrorMsg.ShowAsync();
            }

        }

        #endregion

        #region ListboxHandlers
        private void selected_Item(object sender, SelectionChangedEventArgs e)
        {
            int temp = groupsListBox.SelectedIndex;
        }

        public void SelectedItem_Changed(object sender, SelectionChangedEventArgs e)
        {
            clickOnGroup();
            
        }

        public void clickOnGroup()
        {
            //setting visibilities
            panel2.Visibility = Visibility.Visible;
            itemSelectedListBox.Visibility = Visibility.Visible;

            pageTitle.Visibility = Visibility.Collapsed;
            firstViewer.Visibility = Visibility.Collapsed;
            groupsListBox.Visibility = Visibility.Collapsed;
            joinleavepanel.Visibility = Visibility.Visible;
           
            itemSelectedListBox.Items.Clear();

            //This is working so far 
            //Displays the name
            ////trying the following if statement
            if (groupsListBox.SelectedIndex == -1)
            {
                return; //want to ensure that deleted SelectedIndex isn't getting used upon returning to groups page
            }
            else
            {
                selectedItem = groupsListBox.Items[groupsListBox.SelectedIndex].ToString();
                Groups selectedGroup = Groups.getGroupByName(selectedItem);
                itemSelectedListBox.Items.Add(selectedGroup.getGroupDetails());
            }
        }

        //this was supposed to handle the user click from home hub where the user click would force open panel2
        //I cannot currently get it to auto select the selectedIndex because when firing this handler manually 
        //the groupsListBox is null. And I do not know how to repopulate the list to select the index like the 
        //index and continue with showing the group details just like SelectedItem_Changed does
        public void ItemSent_Changed(object sender, SelectionChangedEventArgs e)
        {
            GetGroups();
            //setting visibilities
            panel2.Visibility = Visibility.Visible;
            itemSelectedListBox.Visibility = Visibility.Visible;

            pageTitle.Visibility = Visibility.Collapsed;
            firstViewer.Visibility = Visibility.Collapsed;
            groupsListBox.Visibility = Visibility.Collapsed;

            itemSelectedListBox.Items.Clear();
        }

        #endregion

        #region viewHandlers
        private void BackToGroups_Click(object sender, RoutedEventArgs e)
        {
            groupsListBox.SelectedIndex = -1;
            panel2.Visibility = Visibility.Collapsed;

            pageTitle.Visibility = Visibility.Visible;
            firstViewer.Visibility = Visibility.Visible;
            groupsListBox.Visibility = Visibility.Visible;
        }

        private void viewMembers_Click(object sender, RoutedEventArgs e) //removed async
        {
            joinleavepanel.Visibility = Visibility.Visible;
            vieweventspanel.Visibility = Visibility.Collapsed;

            populateMemberGroupList(); //try only calling this when first click view members and first click join/leave
            getMembersPerGroup();

        }

        private void createEvent_Click(object sender, RoutedEventArgs e)
        {
            addAnEventPop.IsOpen = true;
            errorListBox.Items.Clear();

            pageTitle.Visibility = Visibility.Collapsed;
            firstViewer.Visibility = Visibility.Collapsed;
            groupsListBox.Visibility = Visibility.Collapsed;
            panel2.Visibility = Visibility.Collapsed;

        }

        public async void deleteGroup(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://nflff.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync("api/Groups/" + id);

                //if group successfully deleted
                if (response.IsSuccessStatusCode)
                {
                    MessageDialog deleteSuccessMsg = new MessageDialog("This group has been deleted");
                    await deleteSuccessMsg.ShowAsync(); //these 2 methods will perform entire viewMembers_click method, which fixes join/leave issues
                    GetGroups();
                }
                else
                {
                    MessageDialog deleteFailMsg = new MessageDialog("Failed to delete group");
                    await deleteFailMsg.ShowAsync(); //these 2 methods will perform entire viewMembers_click method, which fixes join/leave issues
                    GetGroups();
                }
            }
        }

        #endregion

        //checks if specified group has any members by iterating through memberGroupList looking for group's id
        public bool groupHasMembers(int id)
        {
            foreach (MemberGroups mg in MemberGroups.MemberGroupList)
            {
                if (mg.groupID == id)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
