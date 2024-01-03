# Stage 1: Build the application
FROM node:latest as build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run ng build

# Stage 2: Serve the application
FROM nginx:alpine
COPY --from=build /app/dist/inquiry-demo-app /usr/share/nginx/html