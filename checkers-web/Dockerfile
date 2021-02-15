# build environment
FROM node:14 as build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY package.json ./
COPY yarn.lock ./
RUN yarn install --frozen-lockfile
RUN yarn global add react-scripts@3.4.1
COPY . ./
RUN yarn build

# production environment
FROM nginx:stable-alpine
LABEL author="Demolition Chico"
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]