Gilded Rose
==========================
The Solution
-------------------------
Our Innkeeper needed some desperate help to provide only the highest quality of goods at the right price to wary travelers and adventurers. What we have made manifest from spells untold is a magical orb loaded with tricks to help her along the way.

### Service Layer

The Service Layer is hosted in API Gateway, a simple proxy to a Lambda function (gilded-rose-service). The function utilizes Express 4 middleware to handle proxied requests.

As we entrusted our Innkeeper with this magical orb **we did not add additional security** against rouge wizards, thieves and other unlikeable characters. Therefore, make sure to hide under bed when turning in for the night!


### Web App

Built using create-react-app, utilizing Baobab.js (+baobab-react) for state management and React-Bootstrap. Icons are freely available ones from Font Awesome 4.

