"""
Gilded Rose Inventory Application
"""

import sys
import save_and_resume
import base_functions
import os
import argparse


def parse_input_arguments():
    argument_parser = argparse.ArgumentParser()
    argument_parser.add_argument('-useinput', action='store_true', default=False,
                                 help='Special mode to force re-import of inventory.txt file')
    args = argument_parser.parse_args()
    return args


def file_import_at_startup(special_mode):
    import_data_file_name = 'inventory.txt'
    import_data_file_exists = os.path.exists(import_data_file_name)
    native_data_file_exists = os.path.exists(save_and_resume.data_file_name)

    if special_mode and import_data_file_exists:
        print('\nSpecial mode to reimport from text file...\n')
        inventory = save_and_resume.import_data_on_first_run(import_data_file_name)
    elif native_data_file_exists:
        print('\nFound native data file, resuming...\n')
        inventory = save_and_resume.resume_objects()
    elif import_data_file_exists:
        print('\nFirst time loading, importing from text file...\n')
        inventory = save_and_resume.import_data_on_first_run(import_data_file_name)
    else:
        print('\nFile resume error\n')
        sys.exit()

    return inventory


def main_menu_loop(inventory):
    main_loop = True
    while main_loop:
        base_functions.main_menu_text_based()
        menu_selection = base_functions.main_menu_get_input()
        if menu_selection == 1:
            base_functions.print_inventory_to_screen(inventory)
        elif menu_selection == 2:
            base_functions.find_and_print_item(inventory)
        elif menu_selection == 3:
            inventory = base_functions.age_items_by_one_day(inventory)
        elif menu_selection == 4:
            base_functions.print_throw_out_items_to_screen(inventory)
        elif menu_selection == 5:
            inventory = base_functions.throw_out_low_quality_items(inventory)
        elif menu_selection == 6:
            inventory = base_functions.add_inventory_item(inventory)
        elif menu_selection == 7:
            print('\nSaving inventory and exiting application\n')
            save_and_resume.save_objects(inventory)
            sys.exit()
        elif menu_selection == 8:
            print('\nExiting application (no save of inventory)\n')
            sys.exit()
        else:
            print('\nInvalid Entry')


def main():
    print()
    print('*******************************')
    print('**** Gilded Rose Inventory ****')
    print('*******************************')
    print()
    input_arguments = parse_input_arguments()
    inventory_items = file_import_at_startup(input_arguments.useinput)
    main_menu_loop(inventory_items)


if __name__ == '__main__':
    main()
    pass
