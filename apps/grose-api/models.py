import datastore

def get_items():
    return [val for key, val in datastore.get_keys()]

def get_item_by_name(name):
    # TODO: with a k/v store this can probably hit metal
    # directly through the data adapter
    return datastore.get_key(name)

def get_trash():
    return [item for item in get_items() if item.get('quality', 0) <= 0]

def set_degrade_items():
    return [datastore.set_key(item.get('name'), _degrade_item(item)) for item in get_items()]


def _degrade_item(item):
    # TODO classlike behavior, de/serialize if there's time
    # Verbose read/sets... maybe more elegant yeah?

    sellin_floor = 0
    quality_ceiling = 50
    quality_floor = 0

    # # Special sellIn rules

    # Everything but 'Sulfuras' type items age
    if item['category'] != 'Sulfuras':
        item['sellIn'] -= 1

    # Let's apply a floor to the sell by dates (and not track negative
    # expiration) since I'm not sure which is more likely, a fantasy
    # merchant keeping inventory for sys.maxsize *days* and this api being
    # backported to python2, or tracking days-since-expired becomes
    # important
    if item['sellIn'] < sellin_floor:
        item['sellIn'] = sellin_floor

    # # Special quality rules

    # 'Aged Brie' nameed 'Food' type items get better over time
    if item['name'] == 'Aged Brie' and item['category'] == 'Food':
        item['quality'] += 1

    # 'Backstage Passes' type items have special rules
    elif item['category'] == 'Backstage Passes':
        if item['sellIn'] > 10:
            item['quality'] += 1
        elif item['sellIn'] > 5:
            item['quality'] += 2
        elif item['sellIn'] > 0:
            item['quality'] += 3
        else:
            item['quality'] = 0

    # 'Conjured' type items have special rules
    elif item['category'] == 'Conjured':
        item['quality'] -= 2

    # 'Sulfuras' type items have special rules
    elif item['category'] == 'Sulfuras':
        item['quality'] = 80
        quality_ceiling = 80

    # most things
    else:
        item['quality'] -= 1

    # Apply quality ceiling and floor
    if item['quality'] < quality_floor:
        item['quality'] = quality_floor
    elif item['quality'] > quality_ceiling:
        item['quality'] = quality_ceiling 

    return item