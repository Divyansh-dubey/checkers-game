import React, { useEffect, useState } from "react";
import classes from "./Hacker.module.css";

export type HACKER_NAME = "hacker";

export const HackerLoader = () => {
  const [dotsCount, setDotsCount] = useState(1);
  const maxDots = 5;

  const [intervalId, setIntervalId] = useState<NodeJS.Timeout>();

  useEffect(() => {
    const interval = setInterval(
      () => setDotsCount((x) => (x + 1 <= maxDots ? x + 1 : 1)),
      600
    );
    setIntervalId(interval);
    return () => {
      clearInterval(interval);
    };
  }, []);

  return (
    <div className={classes.root}>
      {Array.from({ length: dotsCount }).map((x) => (
        <span>.</span>
      ))}
    </div>
  );
};

