import React, { useContext } from 'react';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const newConnection: HubConnection = new HubConnectionBuilder()
.withUrl(process.env.REACT_APP_HUB_ENDPOINT as string)
.withAutomaticReconnect()
.build();

const ConnectionContext = React.createContext(newConnection);

export const ConnectionProvider = ConnectionContext.Provider;
export const ConnectionConsumer = ConnectionContext.Consumer;
export const useConnectionContext = () => useContext(ConnectionContext);