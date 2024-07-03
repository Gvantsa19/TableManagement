FROM node:alpine  AS build

WORKDIR /usr/src/app

COPY package*.json .
RUN npm install
COPY . .
EXPOSE 4200
CMD ./node_modules/.bin/ng serve --host 0.0.0.0 --disable-host-check
