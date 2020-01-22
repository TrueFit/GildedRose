import csv
import os
import redis

r = None
REDIS_URL = os.environ.get('REDIS_URL', 'redis') 
REDIS_KEY = os.environ.get('REDIS_KEY')

try:
    if REDIS_KEY:
        r = redis.StrictRedis(
                        host=REDIS_URL,
                        port=6379,
                        password=REDIS_KEY)
    else:
        r = redis.Redis(
                        host=REDIS_URL,
                        port=6379)
    r.ping()
    print("Redis ok")
except redis.ConnectionError:
    print("Redis connection failure")

def get_keys():
    inventory = {}
    for key in r.scan_iter("user:*"):
        inventory[key] = r.get(key)
    return inventory.items()

def get_key(key):
    return r.get(key)

def set_key(key, newVal):
    r.set(key, newVal)
    return newVal

# reading in the CSV at startup
with open('data/inventory.txt') as rosefile:
    rosereader = rosefile.readlines()
    for row in rosereader:
        item_props = row.split(',')
        try:
            item = {
                'name': item_props[0],
                'category': item_props[1],
                'sellIn': item_props[2],
                'quality': item_props[3],
            }
            set_key(item['name'], item)
        except:
            print("Row parsing failed, discarding...")

