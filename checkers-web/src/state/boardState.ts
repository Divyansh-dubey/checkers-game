import { atom } from "recoil";

export type BoardData = {
  size: number;
  fields: FieldData[];
  availableMoves: MoveData[];
}

export type FieldData = {
    index: number;
    isUsable: boolean;
    pawnColor: "white" | "black" | undefined;
    isKing: boolean;
  };

export type MoveData = {
  startIndex:number;
  targetIndex:number;
  jumpOverIndex?:number;
}

export const boardState = atom<BoardData>({
    key: 'boardState',
    default: { size: 0, fields: [], availableMoves: [] }
});