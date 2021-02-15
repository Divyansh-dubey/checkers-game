# Containerized checkers game
### A simple checkers game wrapped with docker containers

## Technology used

1. react (create-react-app) + typescript
2. asp .net core + signalR
3. docker

## Running locally

To run this on youtr local machine, you need to do the following:
1. clone this repo
2. in the root directory run:

`docker-compose build`

once done, run:

`docker stack rm checkers-stack`

3. you should be able to access the application via localhost:3000 in your browser

**HAVE FUN**
