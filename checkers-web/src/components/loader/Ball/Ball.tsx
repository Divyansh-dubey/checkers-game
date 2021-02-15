import React from "react";
import classes from "./Ball.module.css";

export type BALL_NAME = 'ball';

export const BallLoader = () => {
  return (
    <div className={classes.root}>
    <div className={classes.shadow} />
      <div className={classes.ball} />
    </div>
  );
};
