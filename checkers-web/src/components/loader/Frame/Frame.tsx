import React from "react";
import classes from "./Frame.module.css";

export type FRAME_NAME = 'frame';

export const FrameLoader = () => {
  return (
    <div className={classes.root}>
      <div className={classes.circle1} />
      <div className={classes.circle2} />
      <div className={classes.circle3} />
      <div className={classes.circle4} />
    </div>
  );
};
