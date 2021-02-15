import React from 'react';
import classes from './SubmitInput.module.css';

type SubmitInputProps = {
    id?: string;
    text?: string;
}

type SubmitInputActions = {
    onSubmit?: () => void;
}

export const SubmitInput: React.FC<SubmitInputProps & SubmitInputActions> = (props) => {
    return <input id={props.id} type="submit" value={props.text} onClick={(e) => props.onSubmit && props.onSubmit()} className={classes.root} />
}
