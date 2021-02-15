import React, { useState } from "react";
import { useRecoilState } from "recoil";
import { updatePlayerName } from "../../api/updatePlayerName";
import { localPlayerState } from "../../state/localPlayerState";
import { Editor } from "../editor/Editor";

export const PlayerNameEditor: React.FC<{ onClose: () => void }> = (props) => {
  const [localPlayer, setLocalPlayer] = useRecoilState(localPlayerState);

  const [tempName, setTempName] = useState(localPlayer.name);

  const submitName = () => {
    if (tempName.length < 5 || tempName.length > 50) {
      return;
    }
    updatePlayerName(tempName);
    setLocalPlayer({ ...localPlayer, name: tempName });
    props.onClose();
  };
  const updateName = (newName: string) => {
    if (newName.length > 20) {
      return;
    }
    setTempName(newName);
  };

  const canSave = tempName.length > 5 && tempName.length <= 20;
  const canClose = tempName.length > 5;

  return <Editor
      label="Your nickname"
      value={tempName}
      onValueChange={updateName}
      onSubmit={submitName}
      onClose={props.onClose}
      canSave={canSave}
      canClose={canClose}
    />;
};
