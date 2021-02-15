import React from 'react';
import classes from './TextInput.module.css';

type TextInputProps = {
    id?: string;
    text?: string;
}

type TextInputActions = {
    onTextUpdate?: (text: string) => void;
}

export const TextInput: React.FC<TextInputProps & TextInputActions> = (props) => {
    return <input id={props.id} type="text" value={props.text} onChange={(e) => props.onTextUpdate && props.onTextUpdate(e.target.value)} className={classes.root} />
}
