# Implementation of Gilded Rose Inventory

This is my implementation of the Gilded Rose Inventory code kata, in Python.

See README_OriginalProblem.md for the problem description, requirements, and requested rules.

Assumptions made in my implementation:

* All strings are in English
* All strings are in ascii (no Unicode support for now)
* All string cases must match (important for category processing and item finding)
* "Once the sell by date has passed..." means that sell_in is < 0
* Since not explicitly mentioned in problem, the default quality adjustment is a decrease of 1 unit per day
* Edge case rule #7, ""Conjured" items degrade in Quality twice as fast as normal items", does not stack with edge
  case #1, "Once the sell by date has passed, Quality degrades twice as fast" (i.e. decrease for Conjured items
  after sell by date is not 4 but only 2)
* While adding a new item...
    - String entries cannot be empty
    - Quality entry cannot be negative
    - The letter case you use will be retained


## Version Information

ver 1.0 :: Initial release :: released 2021.xx.xx :: git repo tag 'xx'

## Running the Gilded Rose Inventory

### On a system with Python 2 and Python 3 using command line:

`cd GildedRose/app`

`python3 main_app.py`


### Using Windows and Linux supplied binaries (*coming soon*)

Find the package corresponding to your OS in the /dist folder of this repository.

Extract the contents, both files, to the same folder.

Run the executable file as you would any other executable.

### Special run mode

A special application run mode is provided for the purpose of reimporting the original
inventory.txt file. Running the Python script in the command line method above, pass an 
argument of `-useinput` will instruct the app to use the inventory.txt file if it exists.

