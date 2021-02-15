import { IconDefinition } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import classNames from "classnames";
import React from "react";
import classes from "./IconButton.module.css";

type Icon = IconDefinition;

export const IconButton: React.FC<{
  icon: Icon;
  className?: string;
  onClick?: () => void;
  disabled?: boolean;
}> = (props) => {
  return (
    <button
      className={classNames(classes.root, props.className, {
        [classes.disabled]: props.disabled,
      })}
      onClick={() => {!props.disabled && props.onClick && props.onClick();}}
    >
      <FontAwesomeIcon icon={props.icon} />
    </button>
  );
};
