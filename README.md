# Containerized checkers game
### A simple checkers game wrapped with docker containers

## Repo structure

This is a monorepo, containing both backend and frontend of the game. You can find a docker compose file responsible for building the entire project in th root directory. Building the project from the root directory will set up the database and build both front and the backend part for you.

## Technology used

1. react (create-react-app) + typescript
2. asp .net core + signalR
3. mongo db
4. docker

## Running locally

To run this on youtr local machine, you need to do the following:
1. clone this repo
2. in the root directory run:

`docker-compose build`

once done, run:

`docker stack rm checkers-stack`

3. you should be able to access the application via localhost:3000 in your browser

**HAVE FUN**
