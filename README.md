# Checkers game
### A simple checkers game wrapped with docker containers

## Repository structure

This is a monorepository, containing both backend and frontend of the game. You can find a docker compose file responsible for building the entire project in the root directory. Building the project from the root directory will set up the database and build both frontend and the backend part for you.

## Technology used

1. React (create-react-app) + typescript
2. ASP .net core + signalR
3. Mongo db
4. Docker

## Running locally

To run this on your local machine, you need to do the following:
1. Clone this repo
2. In the root directory run:

`docker-compose build`

once done, run:

`docker stack deploy -c docker-compose.yml checkers-stack`

when no longer needed, you can remove the stack by running:

`docker stack rm checkers-stack`

3. you should be able to access the application via localhost:3000 in your browser

**HAVE FUN**
