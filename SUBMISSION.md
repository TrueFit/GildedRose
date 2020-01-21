![Gilded Rose](./assets/GildedRoseLogo.png)
# Rob E. Submission

## Tooling
* Development Tools
  * VS.NET 2019 Community Edition
* Application
   * .NET Core 3.1 [The SDK is required for Visual Studio](https://dotnet.microsoft.com/download/dotnet-core/3.1)
   * Web API
   * Entity Framework
   * Angular
   * XUnit
   * Moq

## Installation
* Pull down the repository from GitHub
* Load the solution ([GildedRose.sln](./src/GuildedRose.sln)) in Visual Studio then build and run it (with GildedRose.Web set as the StartUp Project in VS.NET).

Note that the application is configured to run on port 8888. If this port conflicts with your local environment, you can change the port by editing [launchSettings.json](./src/GildedRose.Web/Properties/launchSettings.json)

## Notes on the Submitted Solution
* More time was alotted to solving the core problem of managing inventory from the mid-tier/back end perspective than from a front end perspective.  Since the current end user tool is a sheet of paper, it didn't seem relevant to develop a "marketing showcase" level of front end. With that being said, the UI is lacking needed features such as adding and deleting items in addition to other typically expected features like column sorting and pagination of results.
* The "processing engine" allows for rules to be defined via data rather than requiring coding changes to adjust existing rules or add rules for new categories of items. The rules are currently stored in a [AgingRules JSON file](./AgingRules.json). This file is used by the application to load the aging rules.  In a real-world system, these rules would be persisted to a database and that application would have a user interface for maintaining the rules.
* The submitted application is using a SQLite database for persistance (except for the aging rules mentioned above). The database is recreated during application start up and the inventory will be re-read so stopping and starting the application can be used to reset and retry the application.
* While not defined in requirements/problem statement, I have added audit tracking via "Inventory History".  Any time an inventory item is updated, a history record is created.  This data could be used in the future by the innkeeper to identify trends and/or for input into inventory planning.  This iteration of the application UI **_does_** provide a view to see "Inventory History" so the innkeeper can use it immediately.  Starting the collection of this data, though, is most important because without historical data, it is difficult to review and identify past trends.
* The solution contains tests but tests were focused on two areas:
  * Areas that supported completing the development effort.  In some cases this meant writing integration tests where I might not normally check in integration tests for a real-world project.
  * Meaningful / high-value code just as the aging rules and related calculations.  No tests were written in the UI for this since I was testing everything I was changing as I went along (but that decision was in the context of this solution in the short term -- not with long term thoughts in mind)

## Assumptions
* I made the assumption that asking clarification on requirements was less important than documenting any assumptions that I made. In the real-world, there would be discussions to gain clarity.
* The Inventory.txt file had a record for "Raging Ogre,Backstage Pasess,10,10" -- I assumed that "Pasess" was a typo and corrected the data.
* Once Quality reaches 0, SellIn is also set to 0 if not already 0
* SellIn cannot be negative. To provide a more meaningful mechanism for tracking inventory, I added an "InventoryAddDate" attribute to the inventory. For the future, I would migrate away from "SellIn" as a pure number. Although, the UI could still show "number of days" to keep things user-friendly -- that type of conversion could just be done via simple date math.  Most important, though, is to have dates for looking back and performing analysis because a simple number field does not provide enough context.
* The "Additional Requirements" section defined one requirement that is not available via the UI -- "ask for the details of a single item by name". Based on the wording, it was assumed that providing an endpoint to solve this requirement without a user interface to access it was ok. The API endpoint is: /api/inventory/{name}/filter -- where {name} is the string of characters used for matching. The matching of the name uses "like" logic so more than one item can be returned if a non-unique name parameter is provided.
* As mentioned above in the "Notes" section, having the prettiest and/or most usable application was not a high priority since the intent is to replace a sheet of paper. The approach was to use an agile mindset -- we can always iterate on the solution to improve and refine it over time.