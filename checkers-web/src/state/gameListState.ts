import { atom } from "recoil";

type GameListItem = {
    id: string;
    name: string;
  };

export const gameListState = atom<GameListItem[]>({
    key: 'gameListState',
    default: []
});