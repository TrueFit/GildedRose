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

## Docker
 1. `docker-compose build`
 2. `docker-compose up`

## Run Application
 1.  from GildedRose/gildedrosepython/gildedrose
 2.  `python manage.py runserver`
 3.  open http://127.0.0.1:8000/ and login with admin:admin
 4.  REST api available at http://127.0.0.1:8000/inventory/api/ same u/p

## Simulating Day Change
1. Quit the server with CONTROL-C.
2. set the enviroment variable GR_DAYS to a number of days to add to current date and run the server   
        `GR_DAYS=7 python manage.py runserver`
3. With Docker
        `GR_DAYS=7 docker-compose up`

## Quality Models
The calculations are done through quality models. This allows rapid additions and adjustments through the interface. The model has the following parameters:
  1. item -- this links the model to a single item. Null if not used
  2. category -- this links the model to entire category of items. Null if not used
  3. valid_from_n_days_before_expire -- Sets start of when the model is to be applied. Inclusive of the day.
  4. valid_until_n_days_before_expire -- Sets end of when the model is to be applied. Exclusive of the day.
  5. quality_delta_per_day -- how much the quality changes per day. Negaiive reduces quality, Positive increased quality
  6. expired_scale -- A multiplication factor of the quality_delta_per_day that is applied after the sell_in is less than 0