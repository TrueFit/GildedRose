# Gilded Rose

## Assumptions
  Running from a UNIX like enviromnet. The following was tested on MacOS

## Get Code
   
    git clone git@github.com:snyderra/GildedRose.git
    
## Quickstart
 1. `bash go.sh`

## Setup Environment
 1.  `cd GildedRose`
 2.  `python3 -m venv venv`
 3.  `source venv/bin/activate`
 4.  `python -m pip install -r gildedrosepython/requirements.txt`
 5.  `cd gildedrosepython/gildedrose`
 6.  `bash init.sh`

## Run Application
 1.  from GildedRose/gildedrosepython/gildedrose
 2.  `python manage.py runserver`
 3.  open http://127.0.0.1:8000/ and login with admin:admin
 4.  REST api available at http://127.0.0.1:8000/inventory/api/ same u/p

## Simulating Day Change
1. Quit the server with CONTROL-C.
2. set the enviroment variable GR_DAYS to a number of days to add to current date and run the server   
        `GR_DAYS=7 python manage.py runserver`
