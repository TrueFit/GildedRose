python manage.py makemigrations
python manage.py migrate
DJANGO_SUPERUSER_PASSWORD=admin  python manage.py createsuperuser --username admin --email 'admin@gr.com' --noinput
python manage.py import_inventory ../../inventory.txt
python manage.py set_edge_cases

