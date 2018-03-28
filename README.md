Approach:

This solution is build using dotnet core.  It contains a Web and Console interface.
The inventory information is stored in a MySql database that is held in a docker container 
(this simulates a distrubuted architecture)
The Web interface is build with ASP.Net core and react with redux.


This solution has multiple pieces:
1. GRBusinessLogic - This project handled the Business Logic and Database connection of the the solution
2. GRConsoleApp - A Command line interface to the application
2. GRWeb - The API and Web Page front end for this solution 

Requirements:
1. node js (and npm)
2. dotnet core version 2
3. docker

-- Build and Run the Application
1. Run the following command to build the docker image:
	docker run --name GildedRose-mysql -e "MYSQL_ROOT_PASSWORD=mypassword" -p 6606:3306 -d mysql
	 
	This create a mysql docker image named GildedRose-mysql with port 3306 mapped to localhost:6606 so
	that we can connect externally to the image

2. Intall node packages in GRWeb
	from ../GRWeb run:
	 npm install 

3. Build the dotnet core components 
	run:
	 dotnet build 
	 
	 in the following directories in this order
	../GRBusinessLogic
	../GRConsoleApp
	../GRWeb

4. Run the following in the ../GRBusinessLogic to create the database tables on the mysql dstabase:
	dotnet ef database update
	 
5. Use the console app to import the text file in the the database
   	from ../GRConsoleApp
   	dotnet run - to bring up the command line interface
   	from promp: Giled Rose CLI>
	import 

	you can type: 
	inventory 
	to get a list of inventory from the CLI.

	type: 
	exit 
	to exit the CLI

6. Run GRWeb to launch the Web Interface
	from ../GRWeb
	dotnet run - start the web site.
	in a browser go to localhost:5000 to bring up the web interface 
	
7. Have Fun ;)


-- Future Considerations (Phase 2):
1. Allow for restock of Inventory and deal with the same item that is stocked at different times. (i.e. the same Item with a different SellIn and Quality).
2. Allow for the addition of more categories by storing them in the database.
  - Create fields on the categories that can store End Of Day Process properties. 
3. Enhance End OfDay Process to to keep log of when / if it happened and notify user if it has already been done for the day.



----- Inital Requirements --------
Gilded Rose
==========================
The Problem
-------------------------
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

We currently keep our inventory in a hand written list. Since Allison's wants to get home at night, we keep the writing to a minimum. Each line has the following information, in order:

1. Item Name
2. Item Category
3. Sell In
4. Quality

Additional Requirements:
-------------------------
1. There is no requirement for what you choose as your interface into the system, however whatever interface you choose should, at a minimum, provide for the following commands:
	1. Ask for the entire list of inventory
	2. Ask for the details of a single item by name
	3. End the day
	4. List of trash we should throw away (Quality = 0)
2. In this repo, you will find an inventory.txt file. This is the initial inventory your solution should load. After that, you may store the data however you wish.


The Fine Print
-------------------------
Please use whatever technology and techniques you feel are applicable to solve the problem. We suggest that you approach this exercise as if this code was part of a larger system. The end result should be representative of your abilities and style.

Please fork this repository, then when you have completed your solution, issue a pull request to notify us that you are ready for us to review your submission.

Have fun.
