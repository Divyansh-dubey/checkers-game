import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import { IconButton } from "../buttons/IconButton";
import { Label } from "../inputs/Label/Label";
import { TextInput } from "../inputs/Text/TextInput";
import classes from "./Editor.module.css";

export const Editor: React.FC<{
  label: string;
  value: string;
  onValueChange: (value: string) => void;
  onSubmit: () => void;
  onClose: () => void;
  canSave?: boolean;
  canClose?: boolean;
  id?: string;
}> = (props) => {
  const canSave = props.canSave === undefined || props.canSave === true;
  const canClose = props.canClose === undefined || props.canClose === true;

  const submit = () => {
    if (!canSave) {
      return;
    }
    props.onSubmit();
  };

  const update = (event: React.ChangeEvent<HTMLInputElement>) => {
    props.onValueChange(event.target.value);
  };

  return (
    <div className={classes.root}>
      <div className={classes.code}>
        <div>{`{`}</div>
        <div>
          <label htmlFor={props.id}>
            {props.label
              .split(" ")
              .map((x, i) =>
                i === 0
                  ? `${x[0].toLocaleLowerCase()}${x.slice(1)}`
                  : `${x[0].toUpperCase()}${x.slice(1)}`
              )
              .join("")}
            :{" "}
          </label>
          "
          <input
            style={{ width: `${((props.value.length ?? 0) + 1) * 0.525}em` }}
            className={classes.input}
            type="text"
            id={props.id}
            value={props.value}
            onChange={update}
            autoFocus
          />
          ",
        </div>
        <div className={classes.save} onClick={submit}>
          submit: save()
          <FontAwesomeIcon icon={faCheck} className={classes.icon} />
        </div>
        <div>{`}`}</div>
      </div>
      <IconButton
        icon={faTimes}
        className={classes.closeBtn}
        onClick={props.onClose}
        disabled={!canClose}
      />
    </div>
  );
};
