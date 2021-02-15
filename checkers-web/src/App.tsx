import React, { useEffect, useState } from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import { useConnectionContext } from "./api/Connection/ConnectionContext";
import classes from "./App.module.css";
import { Game } from "./components/Game/Game";
import { Loader } from "./components/loader/Loader";
import { Lobby } from "./components/Lobby/Lobby";

const App: React.FC = () => {
  const connection = useConnectionContext();

  const [isConnecting, setIsConnecting] = useState(true);

  useEffect(() => {
    console.log(`connection state: ${connection.state}`);
    if (connection.state === "Disconnected") {
      console.log("connecting...")
      connection
        .start()
        .then(() => { 
          setIsConnecting(() => false);
        })
        .catch((error) => console.log(error));
    }
    if (connection.state === "Connected") {
      setIsConnecting(false);
    }
  }, [connection]);
  return (
    <div className={classes.root}>
      {!isConnecting ? <RoutedApp /> : <Loader type="bricks" />}
    </div>
  );
};

const RoutedApp: React.FC = () => {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/:gameId">
          <Game />
        </Route>
        <Route path="/lobby" exact>
          <Lobby />
        </Route>
        <Route path="/">
          <Lobby />
        </Route>
      </Switch>
    </BrowserRouter>
  );
};

export default App;
