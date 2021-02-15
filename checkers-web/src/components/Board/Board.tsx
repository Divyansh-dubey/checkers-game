import classNames from "classnames";
import React, { useEffect, useState } from "react";
import { useRecoilState, useRecoilValue } from "recoil";
import { useConnectionContext } from "../../api/Connection/ConnectionContext";
import { BoardData, boardState, MoveData } from "./../../state/boardState";
import { gameState } from "./../../state/gameRoomState";
import classes from "./Board.module.css";
import { Field } from "./Field";
import { localPlayerState } from "./../../state/localPlayerState";
import { turnState } from "../../state/turnState";

type BoardProps = {
  className?: string;
  reverse?: boolean;
};

export const Board: React.FC<BoardProps> = (props) => {
  const connection = useConnectionContext();
  const localPlayer = useRecoilValue(localPlayerState);
  const [board, setBoard] = useRecoilState(boardState);
  const [game, setGame] = useRecoilState(gameState);
  const [turn, setTurn] = useRecoilState(turnState);

  const [selectedMoves, setSelectedMoves] = useState<MoveData[]>([]);

  useEffect(() => {
    subscribeToConnection();
    connection.send("RefreshBoard", { gameId: game.gameId });
    return unsubscribeFromConnection;
  }, [connection]);

  const subscribeToConnection = () => {
    connection.on("BoardRefreshed", (message: BoardData) => {
      setBoard(message);
      setSelectedMoves([]);
    });
    connection.on("TurnRefreshed", (message: string) => {
      setTurn({ color: message as "white" | "black" });
    });
    connection.on("GameOver", (message: { id: string }) => {
      connection.off("BoardRefreshed");
      setGame({ ...game, winnerId: message.id });
    });
  };
  const unsubscribeFromConnection = () => {
    connection.off("BoardRefreshed");
    connection.off("GameOver");
  };

  const isMyTurn = () => turn.color === localPlayer.color;

  const canSelectIndex = (index: number) => {
    return isMyTurn() && [
      ...board.availableMoves.map((m) => m.startIndex),
      ...selectedMoves.map((s) => s.targetIndex),
    ].includes(index);
  };

  const isPawnSelected = (index: number) => {
    return isMyTurn() && selectedMoves.map((m) => m.startIndex).includes(index);
  };

  const selectField = (index: number) => {
    const move = selectedMoves.find((m) => m.targetIndex === index);
    if (isMyTurn() && move !== undefined) {
      connection.send("MakeMove", { gameId: game.gameId, move });
      return;
    }
    setSelectedMoves(
      board.availableMoves.filter((m) => m.startIndex === index)
    );
  };

  return (
    <div className={classNames(classes.root, props.className)}>
      {board.fields.map((field) => {
        const row = !props.reverse
          ? Math.floor(field.index / board.size)
          : board.size - Math.floor(field.index / board.size) - 1;
        const column = !props.reverse
          ? field.index % board.size
          : board.size - (field.index % board.size) - 1;
        const isBlack = field.isUsable;
        const player = field.pawnColor;
        const fieldSize = (1 / board.size) * 100;
        return (
          <Field
            key={field.index}
            row={row}
            column={column}
            sizePercent={fieldSize}
            black={isBlack}
            // label={field.index.toString()}
            pawn={player}
            isKing={field.isKing}
            canSelect={canSelectIndex(field.index)}
            onSelect={() => selectField(field.index)}
            selected={isPawnSelected(field.index)}
          />
        );
      })}
    </div>
  );
};
