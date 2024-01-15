import { html } from "htm/preact";

import HideAndSeekLobby from "./HideAndSeek/Lobby.js";
import LobbyType from "./LobbyType.js";

/**
 * @typedef {import('./Types').ILobby} ILobby
 */

/** @type {(props: {lobby: ILobby}) => string} */
const Lobby = ({ lobby }) => {
  switch (lobby.type) {
    case LobbyType.HideAndSeek:
      return html`<${HideAndSeekLobby} id=${lobby.id} name=${lobby.name} />`;
    default:
      return "";
  }
};

export default Lobby;
