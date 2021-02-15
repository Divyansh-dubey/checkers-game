import { faEdit } from "@fortawesome/free-solid-svg-icons";
import React from "react";
import { IconButton } from "../buttons/IconButton";
import classes from "./PlayerNameDisplay.module.css";
import classNames from "classnames";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export const PlayerNameDisplay: React.FC<{
  name: string;
  isEditing: boolean;
  onClickEdit: () => void;
  className?: string;
}> = (props) => {
  return (
    <div className={classNames(classes.root, props.className)}>
      <div>{"{"}</div>
      <div className={classes.line}>yourName: "{props.name}"</div>
      <div
        className={classNames(classes.line, classes.item)}
        onClick={props.onClickEdit}
      >
        edit: <span>{props.isEditing.toString()}</span>
        <FontAwesomeIcon icon={faEdit} className={classes.icon} />
      </div>
      <div>{"}"}</div>
    </div>
  );
};
