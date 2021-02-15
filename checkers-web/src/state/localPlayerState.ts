import { atom } from "recoil";
import { getPlayerData } from "../api/getPlayerData";

type LocalPlayerData = {
    id: string;
    name: string;
    color?: "white" | "black" ;
  };

export const localPlayerState = atom<LocalPlayerData>({
    key: 'localPlayerState',
    default: getPlayerData()
})