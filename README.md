# Gilded Rose

## Thank you for the opportunity.  This was fun.  Wish I had more time to spend on it.

## Notes
- I approached this exercise as if this code was part of a larger system so I did not focus on doing a UI.
- For simplicity, I assumed all names were uique and that there are no typos in the supplied list of items.
- All data is ASCII not Unicode. Disclaimers about a more robust system apply.
- Authorization and Authentication are assumed.
- All data returned is JSON.

## Needed Improvements
- Implement PATCH for partial updates.
- Logging - logging should be added for try/catches and other places where we'd like to capture information (i.e. startup, shutdown, data updates, etc.) Logs should go to something like ElasticStack.
- Unit testing
- Add EF migration code against a real database like SQL Server.
- Referential Integrity: the In-memory data store doesn't support it because it is meant for demos.
- Implement Authorization and Authentication. We could use OAth, API keys or various other methods if we wanted to properly flesh this out.
- See my notes in the code regarding improvements to the factory

## Testing the API

To add data via the API, run the bash shell script supplied in the LoadItems.sh script in the Tests. I used git bash but any bash shell should work. 

I used Swagger to implement a UI for testing.  If you want to do an end of day, enter 'yes' as a parameter (withj no quotes).  

## Apologies
I ran out of time so I just did a massive put into Github.  Sorry there are no comments.
