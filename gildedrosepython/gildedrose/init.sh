python manage.py makemigrations
python manage.py migrate
python manage.py createsuperuser --username admin --email ''
python manage.py import_inventory ../../inventory.txt
