import React from "react";
import classes from "./AlertMessage.module.scss";

type AlertMessageProps = {
  message: string;
  buttons?: React.ReactNode;
};

export const AlertMessage: React.FC<AlertMessageProps> = ({
  message,
  buttons,
}) => {
  return (
    <div className={classes.root}>
      <h1 className={classes.glitch} data-text={message}>
        {message}
      </h1>
      {!!buttons && <div className={classes.buttons}>{buttons}</div>}
    </div>
  );
};
 