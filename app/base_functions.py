"""
Base Function - This module contains the base functionality
for the inventory application.
"""

import inventory_item
from enum import Enum


class ValidationType(Enum):
    NoValidation = 1
    NotToBeNegative = 2

# define Python user-defined exceptions


class Error(Exception):
    """Base class for other exceptions"""
    pass


class ValueNotToBeNegative(Error):
    """Raised when the input value is negative"""
    pass


def main_menu_text_based():
    print()
    print()
    print('**** Gilded Rose Inventory Menu ****')
    print()
    print('\t1. Print complete inventory to screen')
    print('\t2. Display details of an item by name')
    print('\t3. Age inventory by one day')
    print('\t4. Display inventory items than can be thrown out')
    print('\t5. Discard items that can be thrown out')
    print('\t6. Add inventory item')
    print('\t7. Save inventory and exit application')
    print('\t8. Exit application (do not save any changes from session)')


def obtain_valid_numerical_input(validation, display_string):
    while True:
        user_input = input(display_string)
        try:
            numerical_value = int(user_input)
            # print('Input number is: ', menu_selection)
            if validation == ValidationType.NotToBeNegative and numerical_value < 0:
                raise ValueNotToBeNegative
            break
        except ValueError:
            print('\t\tThis is not a number. Please enter a valid number')
        except ValueNotToBeNegative:
            print('\t\tThis value is not allowed to be negative. Please enter a valid number')

    return numerical_value


def obtain_valid_string_input(display_string):
    should_loop = True
    while should_loop:
        user_input = input(display_string)

        if len(user_input) == 0:
            print('\t\tEmpty strings are not allowed. Please enter text')
            should_loop = True
        else:
            should_loop = False

    return user_input


def print_inventory_to_screen(inventory):
    print('\nCurrent inventory...\n')
    for item in inventory:
        print(item)
    print(f'\t{len(inventory)} items total')


def find_and_print_item(inventory):
    print('\nFind and display item information...\n')
    name_to_search_for = obtain_valid_string_input('\nEnter item name to search for: ')
    found_item = False

    item_count = 0
    for item in inventory:
        if item.item_name == name_to_search_for:
            print(item)
            found_item = True
            item_count += 1

    if not found_item:
        print(f'\tDid not find item, {name_to_search_for}, in inventory\n')
    else:
        print(f'\t{item_count} items\n')


def age_items_by_one_day(inventory):
    print('\n\tAging items in inventory by one day')
    for item in inventory:
        item.adjust_quality_at_end_of_day()
        # print(item)

    return inventory


def print_throw_out_items_to_screen(inventory):
    print('\nThe following items can be discarded (quality of zero): ')
    item_count = 0
    for item in inventory:
        if item.item_quality <= 0:
            print('\t' + item.__str__())
            item_count += 1
    print(f'\t{item_count} items\n')


def throw_out_low_quality_items(inventory):
    print('\nThrow out low quality items...\n')
    print_throw_out_items_to_screen(inventory)
    response = input('\nAre you sure you are ready to trash low quality items? (y for yes, n for no): ')
    if response == 'Y' or response == 'y':
        # use list comprehension to make new list to avoid iteration stepping potential for slowness
        adjusted_inventory = [item for item in inventory if item.item_quality > 0]
        count_of_low_quality_items = len(inventory) - len(adjusted_inventory)
        print(f'\tDisposed of {count_of_low_quality_items} items\n')
        return adjusted_inventory
    else:
        print('\tDid not throw out any items\n')
        return inventory


def add_inventory_item(inventory):
    print('\nAdd an item to the inventory...\n')
    print('\tYou will be asked for four inputs in order to add an item.\n'
          '\tBe mindful of character case.')
    print()
    name = obtain_valid_string_input('\n\tEnter the name of the item: ')
    category = obtain_valid_string_input('\n\tEnter the category of item: ')
    sell_in = obtain_valid_numerical_input(ValidationType.NoValidation,
                                           '\n\tEnter the sell by of the item (in days remaining): ')
    quality = obtain_valid_numerical_input(ValidationType.NotToBeNegative,
                                           '\n\tEnter the quality of the item: (0 to 50, unless special): ')

    item = inventory_item.create_item(name, category, int(sell_in), int(quality))
    print()
    print('\tThe item you have entered looks as follows:\n')
    print('\t\t' + item.__str__())
    response = input('\n\tDo you wish to add this item? (y for yes, n for no) ')
    if response == 'Y' or response == 'y':
        inventory.append(item)
        print('\t\t...item added\n')
    else:
        print('\t\t...item not added\n')

    return inventory


if __name__ == '__main__':
    print('module not runnable')
    pass
