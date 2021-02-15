import React, { useEffect, useState } from "react";
import { useRecoilState, useRecoilValue } from "recoil";
import { useConnectionContext } from "../../api/Connection/ConnectionContext";
import { Board } from "../Board/Board";
import { defaultGameState, gameState } from "./../../state/gameRoomState";
import { localPlayerState } from "./../../state/localPlayerState";
import classes from "./Game.module.css";
import { AlertMessage } from "./../AlertMessage/AletMessage";
import { GameState } from "../GameState/GameState";
import { useHistory, useParams } from "react-router-dom";
import { Loader } from "../loader/Loader";

type PlayerJoinedGame = {
  gameId: string;
  gameName: string;
  players: {
    id: string;
    name: string;
    color: "white" | "black";
  }[];
};

export const Game: React.FC<{}> = () => {
  const connection = useConnectionContext();
  const [localPlayer, setLocalPlayer] = useRecoilState(localPlayerState);
  const [game, setGame] = useRecoilState(gameState);

  const [isLoading, setIsLoading] = useState(true);
  const { gameId } = useParams<{ gameId: string }>();

  const history = useHistory();

  useEffect(() => {
    subscribeToConnection();
    connection.send("JoinGame", {
      gameId,
      playerId: localPlayer.id,
      playerName: localPlayer.name,
    });
    return unsubscribeFromConnection;
  }, [connection]);

  const subscribeToConnection = () => {
    connection.on("PlayerJoinedGame", (message: PlayerJoinedGame) => {
      if (gameId !== message.gameId) {
        throw new Error("Invalid game id received");
      }
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
      setIsLoading(false);
    });
  };
  const unsubscribeFromConnection = () => {
    connection.off("PlayerJoinedGame");
  };

  const quit = () => {
    setGame(defaultGameState);
    history.push("/");
  }

  return (
    <div className={classes.root}>
      <GameState />
      {!isLoading ? (
        <Board
          className={classes.board}
          reverse={localPlayer.color === "white"}
        />
      ) : (
        <Loader type="bricks" />
      )}
      {!!game.winnerId && (
        <AlertMessage
          message={game.winnerId === localPlayer.id ? "YOU WIN" : "YOU LOSE"}
          buttons={
            <div style={{ textAlign: "center" }}>
              <button className={classes.backBtn} onClick={quit}>Return to lobby</button>
            </div>
          }
        />
      )}
    </div>
  );
};
