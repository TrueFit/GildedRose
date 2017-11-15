Gilded Rose
==========================
The Solution
-------------------------
Our Innkeeper needed some desperate help to provide only the highest quality of goods at the right price to wary travelers and adventurers. What we have made manifest from spells untold is a magical orb loaded with tricks to help her along the way.

### Service Layer

The Service Layer is hosted in API Gateway, a simple proxy to a Lambda function (gilded-rose-service). The function utilizes Express 4 middleware to handle proxied requests.

As we entrusted our Innkeeper with this magical orb **we did not add additional security** against rouge wizards, thieves and other unlikeable characters. Therefore, make sure to hide under bed when turning in for the night!

#### Features & Omissions

The honest truth, here are the features and omissions.

Features:
* Serverless and auto scaling
* Get the current inventory
* Delete and item
* Create a new item
* Validation when an item is created
* Move forward a day
* Reset to the initial data set

Omissions:
* No bulk operations - create and delete are one by one
* Uses S3 for persistence and therefore lacks adherance to the ACID conditions and has no transaction concept
* No security layer
* No CI system integration (built/deployed manually)

### Data Layer

This is merely a JSON file kept in S3 for the time being. To scale this and ensure we adhere to ACID this could be loaded into DynamoDB or a relational database in RDS (Relational Database Service). If the desire is to show that level of capability I'd be MORE than happy to add it!

### Web App

Built using create-react-app, utilizing Baobab.js (+baobab-react) for state management and React-Bootstrap. Icons are freely available ones from Font Awesome 4. Also takes advantage of React-Router for navigation and building of the overall hierarchy.

Baobab React is very much like Redux in that it allows certain parts of the UI to be connected to a part of the state, sine Baobab inccludes immutability and we're taking advantage of React PureComponent this means that only certain parts of the UI update when a change occurs and only if it is truly a change. React will disqualify any references (Objects) that have the same address as prior to the update.

#### Features & Omissions

The honest truth, here are the features and omissions.

Features:
* See a list of items that have a quality > 0, which we deem as good items
* See a separate list of items with a quality of 0 or below
* Remove items from either list
* Add new items (to the current inventory)
* End the day - this will take us to The World Of Tomorrooooow!
* Reset it all - taking us back to day 1

Omissions:
* Didn't add authentication
* Didn't add validation for inputs when creating a new item, the service will validate and block creation of invalid items, however there is no visual feedback
* Didn't enable to filter down search capability for the table component
* Didn't add the sort capabilities for the table
* No CI system integration (built/deployed manually)

#### Running It

In order to run the app locally there are certain dependencies:

* node.js (6+) + npm 3+ (Comes bundled)

Then navigate to the root folder of the app (gilded-rose-app) and run:

```npm i```

This will install all dependent modules.

Next to launch the app:

```npm start```

This should now launch it on localhost:3000 and it will connect to the Remote Service on AWS.
