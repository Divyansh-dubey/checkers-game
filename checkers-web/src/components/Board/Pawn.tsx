import React from "react";
import classes from "./Pawn.module.css";
import classNames from "classnames";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCrown } from "@fortawesome/free-solid-svg-icons";

type PawnProps = {
  black?: boolean;
  canSelect?: boolean;
  selected?: boolean;
  isKing?: boolean;
};

export const Pawn: React.FC<PawnProps> = (props) => {
  return (
    <div
      className={classNames(
        classes.root,
        props.black ? classes.black : classes.white,
        {
          [classes.hightlighted]: props.canSelect,
          [classes.selected]: props.selected,
        }
      )}
    >
      {!!props.isKing && (
        <FontAwesomeIcon
          icon={faCrown}
          className={props.black ? classes.blackKing : classes.whiteKing}
        />
      )}
    </div>
  );
};
