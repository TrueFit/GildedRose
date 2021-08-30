"""SaveAndResume module

This module will contain all logic required to save and resume
inventory data and import of first run data file.

"""

import pickle
import csv
import inventory_item

data_file_name = 'inventoryData.pickle'


def save_objects(objects):
    try:
        with open(data_file_name, 'wb') as file:
            pickle.dump(objects, file, protocol=pickle.HIGHEST_PROTOCOL)
    except Exception as exception:
        print('Error during save of object: ', exception)


def resume_objects():
    try:
        with open(data_file_name, 'rb') as file:
            list_of_objects = pickle.load(file)
    except Exception as exception:
        print('Error during resume of object: ', exception)
    return list_of_objects


def import_data_on_first_run(data_file):
    inventory_items = []
    csv_file = open(data_file, mode='r')
    # csvFileFieldNames = ['Item Name', 'Item Category', 'Sell In', 'Quality']
    csv_reader = csv.reader(csv_file)
    for line in csv_reader:
        # print(type(line))
        # print(line)
        item = inventory_item.create_item(line[0], line[1], int(line[2]), int(line[3]))
        # print(item)
        inventory_items.append(item)

    return inventory_items


if __name__ == '__main__':
    print('module not runnable')
    pass
