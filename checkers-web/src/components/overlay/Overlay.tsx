import React from "react";
import classes from "./Overlay.module.css";

type OverlayProps = {
  onClickOutside?: () => void;
};

export const Overlay: React.FC<OverlayProps> = (props) => {
  return (
    <div
      className={classes.root}
      onClick={(e) => {
        props.onClickOutside && e.preventDefault() && props.onClickOutside();
      }}
    >
      <div className={classes.background} />
      {props.children}
    </div>
  );
};
