## Scripted Simulation of SQL Server Loads ##
ChaosLoad allows you to define a JSON template of the load you want to run that looks something like this...

```
{
   "ConnectionString": "Server=localhost\\sql2019;Database=WideWorldImporters;Trusted_Connection=True;",
   "Templates": [
      {
         "ScriptPath": "Scripts\\Demo1\\HammerTime.sql",
         "Sleep": 100,
         "Threads": 50,
         "RunCount": 1000
      },
      {
         "ScriptPath": "Scripts\\Demo1\\FindUser.sql",
         "Sleep": 0,
         "Threads": 5,
         "RunCount": 5
      }
   ]
}
```

You can define any number of scripts and have them execute a set number of times on a specified number of threads with a specified interval after each run. In the above example HammerTime.Sql runs 100 times on 50 threads and pauses for 50ms after each run, FindUser.Sql runs 5 times on 5 threads with no pause between each execution.

To run from source you'll need to first install [.Net Core SDK 2.1+](https://dotnet.microsoft.com/download), alternatively you can download the Windows release binaries under releases.

Assuming you've cloned the source then you can run ChaosLoad from command line by going to the directory the project is in and running

```
dotnet run PathToYourJson.Json
```

Or if you've downloaded the released version from GitHub...

```
ChaosLoad.exe PathToYourJson.exe
```

You can script some pretty cool labs with this to use in training sessions, For example hide a rogue query doing a crazy memory hogging sort in a bunch of innocent queries and play find the problem.

If you want a quick example you can run the one in the sample folder that can be run against the StackOverflow database. It looks like this...

```
{
   "ConnectionString": "Server=localhost\\sql2017;Database=StackOVerflow2013;Trusted_Connection=True;",
   "Templates": [{
      "ScriptPath": "Scripts\\Demo1\\GetAllUsersEndWithG.sql",
      "Sleep": 0,
      "Threads": 5,
      "RunCount": 10
   },
   {
      "ScriptPath": "Scripts\\Demo1\\GetCountPosts.sql",
      "Sleep": 0,
      "Threads": 5,
      "RunCount": 10
   },
   {
      "ScriptPath": "Scripts\\Demo1\\GetMostPopularBadge.sql",
      "Sleep": 10,
      "Threads": 5,
      "RunCount": 10
   }]
}
```

```
/*GetMostPopularBadge.sql*/
SELECT TOP 1
    COUNT(*), [Name]
FROM dbo.Badges
GROUP BY [name]
ORDER BY COUNT(*) DESC

/*GetAllUsersEndWithG.sql*/
SELECT *
FROM [users]
WHERE DisplayName LIKE '%G'

/*GetCountPosts.Sql*/
SELECT COUNT(*)
FROM Posts
```

Minor disclaimer, This project was written in an hour or so late one evening, it's pretty scrappy and whilst I fully intend to go back and tidy up there is a good chance I'll never get to it ;) Feel free to submit pull requests.


