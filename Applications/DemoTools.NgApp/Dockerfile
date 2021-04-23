### STAGE 1: Build ###
FROM node:12.7-alpine AS build
WORKDIR /usr/src/app
COPY './Applications/DemoTools.NgApp/package*.json' ./
RUN npm install
COPY './Applications/DemoTools.NgApp/.' .
#RUN npm run build --prod
RUN $(npm bin)/ng build --prod --output-path=dist

### STAGE 2: Run ###
FROM nginx:1.17.1-alpine
COPY './Applications/DemoTools.NgApp/nginx.conf' /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist /usr/share/nginx/html
