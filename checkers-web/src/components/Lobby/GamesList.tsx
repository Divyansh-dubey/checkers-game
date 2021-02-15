import React from "react";
import classes from "./GamesList.module.css";
import { useRecoilState } from "recoil";
import { useConnectionContext } from "../../api/Connection/ConnectionContext";
import { gameListState } from "./../../state/gameListState";
import { useEffect } from "react";
import { useState } from "react";
import { Loader } from "../loader/Loader";
import { useHistory } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSignInAlt } from "@fortawesome/free-solid-svg-icons";

type GameListItem = {
  id: string;
  name: string;
};

export const GamesList = () => {
  const connection = useConnectionContext();
  const [gamesList, setGamesList] = useRecoilState(gameListState);

  const [isLoading, setIsLoading] = useState(false);

  const history = useHistory();

  const subscribeToConnection = () => {
    connection.on("ListRefreshed", (message: GameListItem[]) => {
      setGamesList(message);
    });
    connection.on("PlayerJoinedLobby", (message: GameListItem[]) => {
      setGamesList(message);
      setIsLoading(false);
    });
  };
  const unsubscribeFromConnection = () => {
    connection.off("ListRefreshed");
    connection.off("PlayerJoinedLobby");
  };

  useEffect(() => {
    setIsLoading(true);
    subscribeToConnection();
    return () => {
      unsubscribeFromConnection();
      connection.send("LeaveLobby");
    };
  }, [connection]);

  const joinGame = (gameId: string) => {
    unsubscribeFromConnection();
    history.push(`/${gameId}`);
  };

  return !isLoading ? (
    <ul className={classes.root}>
      {gamesList.length === 0 && (
        <li className={classes.item}>{`{ games: "no games" }`}</li>
      )}
      {gamesList.map((g) => (
        <li key={g.id} className={classes.item} onClick={() => joinGame(g.id)}>
          "{g.name}"
          <span>
            <br />
            .join() <FontAwesomeIcon icon={faSignInAlt} className={classes.icon} />
          </span>
          ,
        </li>
      ))}
    </ul>
  ) : (
    <Loader type="hacker" />
  );
};
