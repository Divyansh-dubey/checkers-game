import { atom } from "recoil";

type GameState = {
  gameId: string | undefined;
  gameName: string | undefined;
  oponent?: Player;
  isOver: boolean,
  winnerId?: string;
};

type Player = {
  id: string;
  name: string;
  color: string;
}

export const defaultGameState = {
  gameId: undefined,
  gameName: undefined,
  oponent: undefined,
  isOver: false
};

export const gameState = atom<GameState>({
  key: "gameRoomState",
  default: defaultGameState,
});
