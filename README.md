# ChaosLoad
## Scripted Simulation of SQL Server Loads ##
When blogging, presenting or testing an idea, one issue I constantly have is that my local SQL Server used for all these things has no real load on it making it hard to test how things I do work concurrently or under stress. I used to use Adam Machanic's [SQLQueryStress](https://github.com/ErikEJ/SqlQueryStress) and this was great for running single scripts but made it hard to make more realistic loads.

Enter ChaosLoad (Out of Chaos comes order and all that)...

ChaosLoad allows you to define a JSON template of the load you want to run that looks something like this...

```
{
   "ConnectionString": "Server=localhost\\sql2019;Database=WideWorldImporters;Trusted_Connection=True;",
   "Templates": [
      {
         "ScriptPath": "Scripts\\Demo1\\HammerTim.sql",
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

The source can be found on the [ChaosLoad GitHub Repository](https://github.com/gavdraper/ChaosLoad), to run from source you'll need to first install [.Net Core SDK 2.1+](https://dotnet.microsoft.com/download), alternatively you can download the Windows release binaries from GitHub and run without installing anything.

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

Before I run this the server has nothing going on...

![Empty sp_whoisactive]({https://gavindraper.com/content/images/2019-ChaosLoad\NoLoad.PNG)

Let's then fire it up...

![Starting From Console](https://gavindraper.com//content/images/2019-ChaosLoad\Start.PNG)

Now our server looks like this...

![Loaded sp_whoisactive](https://gavindraper.com//content/images/2019-ChaosLoad\Load.PNG)

Minor disclaimer, This project was written in an hour or so late one evening, it's pretty scrappy and whilst I fully intend to go back and tidy up there is a good chance I'll never get to it ;) Feel free to submit pull requests.


