# NFLFanFinder
Summary:
Windows Phone App that allows users to find other fans of their favorite team to watch games with. Users can create/join events.
This was a group project.  The roles were divided as follows:

Janci Dundigalla: database, stored procedures

Misha Claytor: UI design, navigation

Jeff Kempf: back-end code (server and client), validation, testing, analytics


Specifics:

Platform: Windows Phone 8.1

Client Code: C# and XAML

Server Code: C#

Server: Web API2 and Entity Framework

Database: Azure SQL Server


Server Info: 
We created our database first, then used Entity Framework to create a server from our database design.  Our server uses the MVC pattern.  I created additional methods to send/receive custom objects as Json strings so that the server could be used easier if we decided to use it again for a different platform (ie: Android, etc).  All stored procedures are referenced in the DbContext class.


Analytics:
Once user has joined 4+ events, any team that's present in 35%+ of those events is considered a "followed team", and any event featuring that team that the user hasn't already joined will appear as a suggested event on the user's home page.
