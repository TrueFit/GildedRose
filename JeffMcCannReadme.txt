Hello!

Welcome to my implementation of the GildedRose Inventory application.

The implementation is split into 2 solutions.  The first is a WebAPI and the second is a MVC Web application.  

WebAPI
- the GildedRose.Api solution is based on an onion architecture.  The Business Objects and "Helpers" are the core for the project.
- They are stand-alone, but are referenced by Translators which connect the generic classes to the entities found in the Entities project. 
- The repository layer allows for CRUD operations with a context that can be set upon construction.  This was done to allow for unit
tests to be created against it.
- Similarly, the services layer is another abstraction that uses the repository layer and is unit tested in the same manner.  It
returns business objects to the controller layer.
- The controllers in the API project expose the RESTFul endpoints that are used in the GildedRose.Web application.


MVC Web Application
- The MVC Web Application is used to house an Angular 4 app with C# backend that calls the GildedRose.Api
- The Application uses client helpers to call the GildedRose.Api asynchronously.
- Also, the Angular app, is using Observables to add another layer of asynchronous calls.
- The web form and elements are using html, AngularJS, and some bootstrap formatting.

I hope the functionality of the web front end is not too difficult to understand.  I didn't set up any distinctive css or clever html.

Thank you for taking the time to look at my implementation.

Sincerely,

Jeff McCann
jefe101073@gmail.com
412-215-8550 