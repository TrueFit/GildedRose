import redis

# TODO until we get some basic hookup operations,
# this will be a mock data adapter, a python dictionaries
# Impl should be similar to a k/v store like redis
inventory = {}

def get_keys():
    # TODO a k/v store will look different
    return inventory.items()

def get_key(key):
    # TODO a k/v store should swallow this logic
    return [i for i in inventory if i.get('name') == key]

def set_key(key, newVal):
    # TODO what if there are multiple items of the same name?
    # for now this won't support that...
    # a k/v store should swallow most of this logic
    inventory.update({key: newVal})
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
                'sellIn': int(item_props[2]),
                'quality': int(item_props[3]),
            }
            set_key(item['name'], item)
        except:
            print("Row parsing failed, discarding...")
