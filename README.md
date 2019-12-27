# Gilded Rose

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


Compiling
---------
The project was built in Visual Studio 2019 Community. The target framework is `.NET Core 3.1`.

1. Open `GildedRose.sln`.
2. Click "Start" in Visual Studio or press (F6).
3. The code should build without errors.

Projects
--------
*GildedRose* - This is a console application of the main app.

*GildedRoseInventory* - This is a DLL that holds the logic of the inventory system.

Running from Visual Studio
--------------------------

1. Open `GildedRose.sln` in Visual Studio 2019.
2. Press F5 to run in debug mode.
3. You should see output like the following:
```bash
_____ _ _     _          _  ______
|  __ (_) |   | |        | | | ___ \
| |  \/_| | __| | ___  __| | | |_/ /___  ___  ___
| | __| | |/ _` |/ _ \/ _` | |    // _ \/ __|/ _ \
| |_\ \ | | (_| |  __/ (_| | | |\ \ (_) \__ \  __/
 \____/_|_|\__,_|\___|\__,_| \_| \_\___/|___/\___|

Inventory System
[..\..\..\..\..\..\inventory.txt]

1. List entire inventory
2. Details of a single item
3. Progress to the next day
4. List of trash to throw away
5. Exit

Enter number (1-5):
```
4. Type a number, from 1 to 5, followed by hitting the enter key to go into the different options of the application.

Running from the Command Prompt
-------------------------------
1. Make sure you have built the project.
2. Open a command prompt and change directory into
`GildedRose\Source\GildedRose\GildedRose\bin\Debug\netcoreapp3.1`
3. Run the following to start the application using the inventory.txt file in the repo.
```bash
dotnet GildedRose.dll "..\..\..\..\..\..\inventory.txt"
```
4. The application loads the data in the inventory.txt file, but doesn't save back to the file. Exiting the application and restarting will start from the beginning.

Assumptions
-----------

I have made the following assumptions:

* I assume that the Quantity of Aged Brie should stay at 50 when the Sell In is 0. This means that it is never trash. Maybe this is something that should change later.
* I assume that the Sell In will continue to count down when the Quality is 0. Not sure if that is what we want or not.
