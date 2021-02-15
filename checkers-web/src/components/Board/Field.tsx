import React from "react";
import classes from "./Field.module.css";
import classNames from "classnames";
import { Pawn } from "./Pawn";

type FieldProps = {
  row: number;
  column: number;
  sizePercent: number;
  black?: boolean;
  label?: string;
  pawn: "white" | "black" | undefined;
  canSelect?: boolean;
  selected?: boolean;
  onSelect: () => void;
  isKing?: boolean;
};

export const Field: React.FC<FieldProps> = (props) => {
  const fieldStyle = {
    height: `${props.sizePercent}%`,
    width: `${props.sizePercent}%`,
  };
  const select = () => {
    props.canSelect && props.onSelect();
  }
  return (
    <div
      className={classNames(
        classes.root,
        !!props.black ? classes.black : classes.white,
        {
          [classes.hightlighted]: !props.pawn && props.canSelect,
          [classes.canSelect]: !props.pawn && props.canSelect,
        }
      )}
      style={{
        ...fieldStyle,
        top: `${props.row * props.sizePercent}%`,
        left: `${props.column * props.sizePercent}%`,
      }}
      onClick={select}
    >
      {!!props.pawn && (
        <Pawn black={props.pawn === "black"} canSelect={props.canSelect} selected={props.selected} isKing={props.isKing} />
      )}
      {!props.pawn && props.label}
    </div>
  );
};
