from .models import Item

class InventoryService:
    """
    Business logic related to the Inventory app
    """

    @staticmethod
    def end_day():
        """
        This method closes out the current day and updates
        the sell_in and quality fields for all items in
        the inventory
        """

        import logging
        logging.error('UPDATING INVENTORY!!!')