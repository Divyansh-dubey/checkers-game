import React from "react";
import classes from "./Bricks.module.css";

export type BRICKS_NAME = 'bricks';

export const BricksLoader = () => {
  return (
    <div className={classes.root}>
      <div className={classes.brick} />
      <div className={classes.brick} />
      <div className={classes.brick} />

      <div className={classes.brick} />
      <div className={classes.brick} />

      <div className={classes.brick} />
      <div className={classes.brick} />
      <div className={classes.brick} />
    </div>
  );
};
