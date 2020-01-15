# Gilded Rose Solution

## Build

The following steps should work to run the application:

- Clone this repository.
- Open the solution in Visual Studio 2019.
- Set GildedRose.Web to be the startup project.
- Debug > Start Debugging.

This assumes that the `inventory.txt` file is in the same location (at the root of the cloned repository). After the initial inventory is loaded from the text file, inventory is stored in an `inventory.json` file in the web site directory.

I included some of the build artifacts in the commit that wouldn't usually be part of what's in version control so it should "just work". If it doesn't, you may have to run one of more of the following:

- Right-click solution: Restore Client-Side Libraries
- Right-click solution: Restore NuGet Packages
- Open command prompt to the GildedRose.Web project folder and run `npm install` and then `npx webpack`.

Tests are in the GildedRose.Tests project.

## Assumptions

Some assumptions I made based on the specifications. In a real-life situation I would get clarity from the customer, of course. But I figured part of the exercise was how to "read between the lines" and make reasonable guesses where specifications are open to interpretation.

1. "Sell In" is allowed to go negative. This would tell the user how far past the expiration date the item is.

2. "Sell In" is not required, stored, or displayed for items where it is meaningless. (For example, legendary items that have no need to be sold by any date.)

3. I was not clear on whether the required interfaces needed to be part of the user interface, or the API interface. I included both, although many of those functions in the API interface are not used by the client, since all of that work can be done trivially server-side.

4. Names and categories are not case-sensitive for searching / sorting purposes.

5. I didn't put much effort into the repository. I went for ease of testing (simple files) rather than trying to create a "production ready" layer (database, ORM, etc.), and didn't optimize much for performance. If I misunderstood the intention of the exercise and you wanted a full-fledged data access layer, please let me know.

6. The Gilded Rose is a modern inn. As such, they use a relatively modern browser.
