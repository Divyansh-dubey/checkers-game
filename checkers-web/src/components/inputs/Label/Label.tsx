import React from "react";
import classes from "./Label.module.css";

type LabelProps = {
  for: string;
};

export const Label: React.FC<LabelProps> = (props) => {
  return <label htmlFor={props.for} className={classes.root}>{props.children}</label>;
};
