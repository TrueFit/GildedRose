"""
Base Function - This module contains the base functionality
for the inventory application.
"""


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
    print('\t6. Save inventory and exit application')
    print('\t7. Exit application (do not save any changes from session)')


def main_menu_get_input():
    while True:
        num = input('\nEnter action number and press enter: ')
        try:
            menu_selection = int(num)
            # print('Input number is: ', menu_selection)
            break
        except ValueError:
            print('\t\tThis is not a number. Please enter a valid number')

    return menu_selection


def print_inventory_to_screen(inventory):
    print('\nCurrent inventory...\n')
    for item in inventory:
        print(item)
    print(f'\t{len(inventory)} items total')


def find_and_print_item(inventory):
    print('\nFind and display item information...\n')
    name_to_search_for = input('\nEnter item name to search for: ')
    found_item = False

    for item in inventory:
        if item.item_name == name_to_search_for:
            print(item)
            found_item = True

    if not found_item:
        print(f'\tDid not find item, {name_to_search_for}, in inventory\n')


def age_items_by_one_day(inventory):
    print('\nAging items in inventory by one day')
    for item in inventory:
        item.adjust_quality_at_end_of_day()
        # print(item)

    return inventory


def print_throw_out_items_to_screen(inventory):
    print('\nThe following items can be discarded (quality of zero or less): ')
    for item in inventory:
        if item.item_quality <= 0:
            print(item)


def throw_out_low_quality_items(inventory):
    print('\nThrow out low quality items...\n')
    response = input('\nAre you sure you are ready to trash low quality items? (y for yes): ')
    if response == 'Y' or response == 'y':
        # use list comprehension to make new list to avoid iteration stepping potential
        adjusted_inventory = [item for item in inventory if item.item_quality > 0]
        count_of_low_quality_items = len(inventory) - len(adjusted_inventory)
        print(f'\tDisposed of {count_of_low_quality_items} items\n')
        return adjusted_inventory
    else:
        print('\tDid not throw out any items\n')
        return inventory


if __name__ == '__main__':
    print('module not runnable')
    pass
