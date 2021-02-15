import { atom } from "recoil";

type CurrentTurnState = {
  color: "white" | "black";
};

export const turnState = atom<CurrentTurnState>({
  key: "currentTurnState",
  default: { color: "white" },
});
