# syntax=docker/dockerfile:1
FROM python:3
ENV PYTHONUNBUFFERED=1
WORKDIR /gr
COPY gildedrosepython/requirements.txt /gr/
RUN pip install -r requirements.txt
COPY gildedrosepython/gildedrose /gr/
COPY inventory.txt /
RUN cd /gr && bash init.sh
