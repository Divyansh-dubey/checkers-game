import React, { useEffect } from "react";
import { useRecoilValue } from "recoil";
import { useConnectionContext } from "../../api/Connection/ConnectionContext";
import classes from "./GameState.module.scss";
import { localPlayerState } from "./../../state/localPlayerState";
import { gameState } from "../../state/gameRoomState";
import { useRecoilState } from "recoil";
import { turnState } from "./../../state/turnState";
import classNames from "classnames";

type PlayerJoinedGame = {
  gameId: string;
  gameName: string;
  players: {
    id: string;
    name: string;
    color: "white" | "black";
  }[];
};

export const GameState: React.FC<{}> = (props) => {
  const connection = useConnectionContext();
  const [localPlayer, setLocalPlayer] = useRecoilState(localPlayerState);
  const [game, setGame] = useRecoilState(gameState);
  const currentTurn = useRecoilValue(turnState);

  useEffect(() => {
    subscribeToConnection();
    return unsubscribeFromConnection;
  }, [connection]);

  const subscribeToConnection = () => {
    connection.on("PlayerJoinedGame", (message: PlayerJoinedGame) => {
      console.log(message);
      const oponent = message.players.find((x) => x.id !== localPlayer.id);
      setGame({
        ...game,
        gameId: message.gameId,
        gameName: message.gameName,
        oponent,
      });
      setLocalPlayer({
        ...localPlayer,
        color: message.players.find((x) => x.id === localPlayer.id)?.color,
      });
    });
  };
  const unsubscribeFromConnection = () => {
    connection.off("PlayerJoinedGame");
  };

  return (
    <div className={classes.root}>
      <div
        data-text={`${localPlayer.name} (${localPlayer.color})`}
        className={classNames(classes.label, {
          [classes.glitch]: currentTurn.color === localPlayer.color,
        })}
      >{`${localPlayer.name} (${localPlayer.color})`}</div>
      <div className={classes.label}>{game.gameName}</div>
      <div
        data-text={`${game.oponent?.name} (${game.oponent?.color})`}
        className={classNames(classes.label, {
          [classes.glitch]: currentTurn.color === game.oponent?.color,
        })}
      >
        {!!game.oponent?.name
          ? `${game.oponent.name} (${game.oponent.color})`
          : "awaiting player..."}
      </div>
    </div>
  );
};
