# ChaosLoad
Introducing ChaosLoad because obviously out of chaos comes order.....

ChaosLoad allows you to define a demo SQL Server load in a JSON file and execute it like this...
```
{
    "ConnectionString": "Server=localhost\\sql2019;Database=WideWorldImporters;Trusted_Connection=True;",
    "Templates": [
        {
            "ScriptPath": "Scripts\\Demo1\\LotsOfCols.sql",
            "Sleep": 100,
            "Threads": 50,
            "RunCount": 1000
        },
        {
            "ScriptPath": "Scripts\\Demo1\\GetAllLogins.sql",
            "Sleep": 0,
            "Threads": 5,
            "RunCount": 5
        }
    ]
}
```

This is saying run
* LotOfCols.sql run concurrently across 50 threads for a total of 100 executions and pause for 100ms on each thread after each execution.
* GetAllLogs.sql run 5 times concurrently on 5 threads with no sleep required as each thread will only run the script once.

To run from source you need the .Net Core 2.0+ SDK found here [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

Then you from the project directory in the console you just need to run it passing in the location of your JSON file...

```
  dotnet run Scripts/MyJson.json
```

Alternatively head over to the release page here and download the latest built version which you can run in much the same way but is Windows only...

```
  ChaosLoad.exe Scripts/MyJson.json
 ```
 
 This project was mainly written to allow me to simulate load to test the performance of different things, it was written in one evening and is pretty scrappy (At some point I'll go back and tidy it up). It is however working great for me so I thought I'd share it.
