import React, { useState } from "react";
import { Editor } from "../editor/Editor";

export const GameNameEditor: React.FC<{
  onCreateGame: (name: string) => void;
  onClose: () => void;
}> = (props) => {
  const [gameName, setGameName] = useState("");

  const submitName = () => {
    if (gameName.length < 5 || gameName.length > 50) {
      return;
    }
    props.onCreateGame(gameName);
  };

  const updateName = (newName: string) => {
    if (newName.length > 20) {
      return;
    }
    setGameName(newName);
  };
  const canSave = gameName.length > 5 && gameName.length <= 20;

  return (
    <Editor
      label="Game name"
      value={gameName}
      onValueChange={updateName}
      onSubmit={submitName}
      onClose={props.onClose}
      canSave={canSave}
      canClose={true}
    />
  );
};
