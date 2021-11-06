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

## Solution

I have provided the source code and the executables. If you want to use the application before looking at the code, please start the GildedRose.Server.exe from the Executables\Server\ folder, before launching GildedRose.Client.exe in Executables\Client\.

### Architecture

I wanted to develop an application in C# .NET, because it's the programming environment I worked with most over the past ten years.

I chose Windows Presentation Forms (WPF) for the user interface with a clean MVVM pattern meaning the view itself is only bound to view models which are connected to the actual data models. This enables adjustments in the user interface without changing data models and vice versa.

The task could have been done in a fat client without data base or services, but I hope Allison has success in her shopkeeper career and will soon have enough gold coins to hire more employees and add new cash registers. Therefore, I added a basic backend system using gRPC with ADO.NET 5 and a SQLite data base. Any company I worked for already had some sort of abstract service infrastructure that was ready-to-use. I have never done this myself and I took this programming test as an opportunity to learn about gRPC which I always wanted to do. Data storage could have been done in plain text files, but I decided to use a SQLite data base. I wanted to make a system that can be used by you directly without starting any containers, creating data bases, etc. So, a file based data base seemed to be the best fit.

Most software products will be improved and expanded over time. I therefore wanted to make it as flexible as possible. I already mentioned the MVVM pattern for WPF that I used. The client is connected to an inventory system interface that can be anything. In my pull request there is an implementation to connect the client to a gRPC service, but it could be anything including professional warehouse systems. The backend system has an interface to data sources which can be inventory lists like the provided text file or a SQLite data base. This interface can be implemented for MS SQL, Oracble, MongoDB, clouds or any other type of data storage.

### Assumptions

1. The requirements do not specify how fast items decay each day. It is assumed the decay for SellIn and Qualitz of an item is 1 per day.
2. The item "Sulfuras, Hand of Ragnaros" is stored in a format in the inventory file that is valid with "Hand of Ragnaros" being the name and Sulfuras being the category. It is therefore being processed as such.
3. It is not specified how items in different categories would behave. In my generalized approach, a conjured aged brie would get a high quality very quickly.
4. I assumed the quality of backstage passes drops to zero, once the SellIn value is lower than zero meaning the tickets can be sold on the day of concert but not after.

### Assumptions

To answer the second question from "Things To Consider": Using unit tests, gated check-ins and a build server. Unit tests can assure that the written code works in expected parameters and behave as expected in corner cases. A gated check-in that checks the code before committing it to a repository ensures that no code ends up in the repository that does not compile or fails unit tests. A nightly build server can provide executables or setups that are ready to use - if given Unit Tests succeed.