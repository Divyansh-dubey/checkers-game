import { v4 as uuidv4 } from "uuid";

const PLAYER_ID_KEY = "CheckersPlayerId";
export const PLAYER_NAME_KEY = "CheckersPlayerName";

const getPlayerId = () => {
  const localPlayerId = localStorage.getItem(PLAYER_ID_KEY);
  if (!!localPlayerId) {
    console.log(`Player id ${localPlayerId}.`);
    return localPlayerId;
  }
  const newPlayerId = uuidv4();
  localStorage.setItem(PLAYER_ID_KEY, newPlayerId);
  console.log(`New player id ${newPlayerId} generated and saved.`);
  return newPlayerId;
};

const getPlayerName = () => {
  const localPlayerName = localStorage.getItem(PLAYER_NAME_KEY);
  if (!!localPlayerName) {
    console.log(`Player name ${localPlayerName}.`);
    return localPlayerName;
  }
  return "";
};

export type PlayerData = {
  id: string;
  name: string;
};

export const getPlayerData = (): PlayerData => {
  return {
    id: getPlayerId(),
    name: getPlayerName(),
  };
};
