#!/bin/bash

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Sword",
"category": "Weapon",
"sellin": 30,
"quality": 50
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Axe",
"category": "Weapon",
"sellin": 40,
"quality": 50
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Halberd",
"category": "Weapon",
"sellin": 60,
"quality": 40
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Aged Brie",
"category": "Food",
"sellin": 50,
"quality": 10
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Aged Milk",
"category": "Food",
"sellin": 20,
"quality": 20
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Mutton",
"category": "Food",
"sellin": 10,
"quality": 10
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Hand of Ragnaros",
"category": "Sulfuras",
"sellin": 80,
"quality": 80
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "I am Murloc",
"category": "Backstage Passes",
"sellin": 20,
"quality": 10
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Raging Ogre",
"category": "Backstage Pasess",
"sellin": 10,
"quality": 10
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Giant Slayer",
"category": "Conjured",
"sellin": 15,
"quality": 50
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Storm Hammer",
"category": "Conjured",
"sellin": 20,
"quality": 50
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Belt of Giant Strength",
"category": "Conjured",
"sellin": 20,
"quality": 40
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Cheese",
"category": "Food",
"sellin": 5,
"quality": 5
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Potion of Healing",
"category": "Potion",
"sellin": 10,
"quality": 10
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Bag of Holding",
"category": "Misc",
"sellin": 10,
"quality": 50
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "TAFKAL80ETC Concert",
"category": "Backstage Passes",
"sellin": 15,
"quality": 20
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Elixir of the Mongoose",
"category": "Potion",
"sellin": 5,
"quality": 7
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "+5 Dexterity Vest",
"category": "Armor",
"sellin": 0,
"quality": 20
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Full Plate Mail",
"category": "Armor",
"sellin": 50,
"quality": 50
}'

curl -k --insecure --header "Content-Type: application/json" \
     --request POST \
     --url https://localhost:44320/api/Store \
--data '{
"name": "Wooden Shield",
"category": "Armor",
"sellin": 10,
"quality": 30
}
]