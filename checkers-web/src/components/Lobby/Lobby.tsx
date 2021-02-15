import React, { useEffect, useState } from "react";
import classes from "./Lobby.module.css";
import { IconButton } from "../buttons/IconButton";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { GameNameEditor } from "../GameName/GameNameEditor";
import { Overlay } from "../overlay/Overlay";
import { useConnectionContext } from "../../api/Connection/ConnectionContext";
import { PlayerNameEditor } from "../PlayerName/PlayerNameEditor";
import { PlayerNameDisplay } from "../PlayerName/PlayerNameDisplay";
import { GamesList } from "./GamesList";
import { localPlayerState } from "./../../state/localPlayerState";
import { useRecoilState, useRecoilValue } from "recoil";
import { useHistory } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { defaultGameState, gameState } from "../../state/gameRoomState";

type Game = {
  id: string;
  name: string;
};

type GameCreated = Game & {
  creatorId: string;
  creatorName: string;
};

export const Lobby: React.FC = () => {
  const connection = useConnectionContext();
  const localPlayer = useRecoilValue(localPlayerState);
  const [game, setGame] = useRecoilState(gameState);

  const [gameEditorOpen, setGameEditorOpen] = useState(false);
  const [nameEditorOpen, setNameEditorOpen] = useState(false);

  const history = useHistory();

  useEffect(() => {
    setNameEditorOpen(!localPlayer.name);
    setGame(defaultGameState);
    subscribeToConnection();
    connection.send("JoinLobby", {});
    return unsubscribeFromConnection;
  }, [connection]);
  const subscribeToConnection = () => {
    connection.on("GameSuccessfulyCreated", (message: GameCreated) => {
      console.log("created!");
      history.push(`/${message.id}`);
    });
  };
  const unsubscribeFromConnection = () => {
    connection.off("GameSuccessfulyCreated");
  };
  const createGame = (gameName: string) => {
    connection
      .send("CreateGame", {
        gameName,
        creatorId: localPlayer.id,
        creatorName: localPlayer.name,
      })
      .then(closeGameEditor);
  };
  const openGameEditor = () => setGameEditorOpen(true);
  const closeGameEditor = () => setGameEditorOpen(false);
  const openNameEditor = () => setNameEditorOpen(true);
  const closeNameEditor = () => setNameEditorOpen(false);
  return (
    <div className={classes.root}>
      {!!localPlayer.name && (
        <PlayerNameDisplay
          isEditing={nameEditorOpen}
          name={localPlayer.name}
          onClickEdit={openNameEditor}
          className={classes.nameDisplay}
        />
      )}
      <div className={classes.gamesList}>
        <div>{`const list = { games: [`}</div>
        <GamesList />
        <div>{`],`}</div>
        <div className={classes.new} onClick={openGameEditor}>
          new: create(){" "}
          <FontAwesomeIcon icon={faPlus} className={classes.icon} />
        </div>
        <div>{`}`}</div>
        {gameEditorOpen && (
          <Overlay>
            <GameNameEditor
              onCreateGame={createGame}
              onClose={closeGameEditor}
            />
          </Overlay>
        )}
        {nameEditorOpen && (
          <Overlay>
            <PlayerNameEditor onClose={closeNameEditor} />
          </Overlay>
        )}
      </div>
    </div>
  );
};
