FROM node:12.2.0-alpine as build
WORKDIR /usr/src/app
ENV PATH /usr/src/app/node_modules/.bin:$PATH
ARG REACT_APP_API_URL
ENV REACT_APP_API_URL=${REACT_APP_API_URL}
COPY package.json /usr/src/app/package.json
RUN npm install
RUN npm install react-scripts@3.0.1 -g
CMD ["npm", "start"]
