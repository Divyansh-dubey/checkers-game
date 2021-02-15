import { PLAYER_NAME_KEY } from "./getPlayerData"

export const updatePlayerName = (name: string) => {
    localStorage.setItem(PLAYER_NAME_KEY, name);
    console.log(`New player name ${name} saved.`);
}