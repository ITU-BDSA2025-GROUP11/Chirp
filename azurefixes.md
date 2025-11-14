look at pr 138
we got azure to work by: 
changing back to 8.0
removing reference to db from Chirp.Web.csproj
altering the connection string in appsettings.json to /tmp/Chirp.db this way azure explicitly knows where to find it
added a startup command to azure config 'dotnet Chirp.Web.dll'
