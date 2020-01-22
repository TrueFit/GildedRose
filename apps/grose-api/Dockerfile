FROM python:3.7-alpine
WORKDIR /usr/src/app
COPY ./requirements.txt /usr/src/app/requirements.txt
COPY ./data/inventory.txt /urs/src/app/data/inventory.txt
RUN pip install -r requirements.txt
COPY . /usr/src/app
EXPOSE 5000
CMD ["gunicorn", "--bind", "0.0.0.0:5000", "app:app", "--capture-output"]
