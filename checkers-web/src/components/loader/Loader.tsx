import React from 'react';
import { BallLoader, BALL_NAME } from "./Ball/Ball";
import { BricksLoader, BRICKS_NAME } from "./Bricks/Bricks";
import { FrameLoader, FRAME_NAME } from "./Frame/Frame";
import { HackerLoader, HACKER_NAME } from "./Hacker/Hacker";

type LoaderProps = {
    type?: BRICKS_NAME | BALL_NAME | FRAME_NAME | HACKER_NAME;
}

export const Loader: React.FC<LoaderProps> = (props) => {
    switch(props.type) {
        case "bricks":
            return <BricksLoader />
        case "ball":
            return <BallLoader />
        case "frame":
            return <FrameLoader />
        case "hacker":
            return <HackerLoader />
        default:
            return <BricksLoader />
    }
}