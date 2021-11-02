# Gilded Rose
----------------------------------------------------------------------------------------------
**About**  
This is a .NET Core (.NET 6) Blazor Web App.   

**How To Run:**  
1. Open the project in VS 2019 or 2022  
2. If not already, set the startup project as "GildedRose-Blazor.Server"
3. Run the application 
  
## Notes:  
Items to consider:  
  
1. Days can be incremented by passing an int parameter denoting the number of days you would like to increment. I have provided buttons for 1 and 5 days.  
2. I added some integration tests, but could use more.  
3. I did not include parsing out the inventory text file. I added some additional fields, and used a built in SQLite database, so parsing the text file into new objects ended up being an ugly operation. May add it back in if needed, but given the setup I am using, and that the list never changes, creating a default object list that gets populated on startup seemed to suffice and be cleaner / easier to understand. 
4. I used a local SQLite database to emulate a more traditional interaction with a database without requiring the provisioning of one. To keep deployment simple, I didn't want to have to roll out multiple docker containers for various services. Given the scope of the project, it seemed fitting to "spice it up" a little, without overdoing it.
5. Many architectural choices make no "real world" sense. Namely the use of SQLite. I also wouldn't choose a Blazor application for production. My main goal was to demonstrate the use of Entity Framework, .NET Core, have some integration tests, throw in a few more functions and most of all have the main functionality of incrementing the days be accurate to the specifications.   

----------------------------------------------------------------------------------------------
## The Problem
Hi and welcome to team Gilded Rose. As you know, we are a small inn with a prime location in a prominent city run by a friendly innkeeper named Allison. We also buy and sell only the finest goods. Unfortunately, our goods are constantly degrading in quality as they approach their sell by date. We need you to write a system that allows us to manage our inventory, so that we are able to service all of the adventurers who frequent our store (we don't want to run out of healing potions when an tiefling comes in unlike last time - poor Leeroy).

Here are the basic rules for the system that we need:

1. All items have a SellIn value which denotes the number of days we have to sell the item
2. All items have a Quality value which denotes how valuable the item is
3. At the end of each day our system lowers both values for every item

Since this is the real world, there are some edge cases we need for you to account for as well:

1. Once the sell by date has passed, Quality degrades twice as fast
2. The Quality of an item is never negative
3. "Aged Brie" actually increases in Quality the older it gets
4. The Quality of an item is never more than 50
5. "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
6. "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but Quality drops to 0 after the concert
7. "Conjured" items degrade in Quality twice as fast as normal items
8. An item can never have its Quality increase above 50, however "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters.

We currently keep our inventory in a hand written list. Since Allison wants to get home at night, we keep the writing to a minimum. Each line has the following information, in order:

1. Item Name
2. Item Category
3. Sell In
4. Quality

### Additional Requirements:
1. There is no requirement for what you choose as your interface into the system, however whatever interface you choose should, at a minimum, provide for the following commands:
	1. Ask for the entire list of inventory
	2. Ask for the details of a single item by name
	3. Progress to the next day
	4. List of trash we should throw away (Quality = 0)
2. In this repo, you will find an inventory.txt file. This is the initial inventory your solution should load. After that, you may store the data however you wish.

## The Fine Print
Please use whatever technology and techniques you feel are applicable to solve the problem. We suggest that you approach this exercise as if this code was part of a larger system. The end result should be representative of your abilities and style.

Please fork this repository, then when you have completed your solution, issue a pull request to notify us that you are ready for us to review your submission.

Have fun.

## Things To Consider
Here are a couple of thoughts about the domain that could influence your response:

* The world is a magical place - you never know when the next "special requirement" might pop up - how can you make this painless?
* Keep in mind that accurate inventory is a must for the shop, how might you ensure that the future programmer who takes over the code while you are off adventuring doesn't mistakenly mess things up?
