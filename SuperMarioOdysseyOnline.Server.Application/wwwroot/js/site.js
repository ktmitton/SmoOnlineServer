import { render } from "preact";
import { useEffect, useState } from "preact/hooks";

import LobbyType from "./LobbyType.js";
import { Lobby as HideAndSeekLobby, ViewComponent as HideAndSeekViewComponent} from "./HideAndSeekLobby.js";
import CoopLobby from "./CoopLobby.js";
import html from "./html.js";

/**
 * @typedef {import('./Types').ILobby} ILobby
 */

/** @type {(lobbyType: LobbyType) => string} */
const getIconNameForLobbyType = (lobbyType) => {
  switch (lobbyType) {
    case LobbyType.HideAndSeek:
      return "search";
    case LobbyType.Coop:
      return "partner_exchange";
    default:
      return "videogame_asset";
  }
};

/** @type {(props: {lobby: ILobby}) => string} */
const Lobby = ({ lobby }) => {
  switch (lobby.type) {
    case LobbyType.HideAndSeek:
      return html`<${HideAndSeekViewComponent} lobby=${lobby} />`;
    case LobbyType.Coop:
      return html`<${Coop} lobby=${lobby} />`;
    default:
      return "";
  }
};

/** @type {(props: {lobbies: ILobby[]}) => string} */
const App = ({ lobbies }) => {
  const [ selectedLobby, setSelectedLobby ] = useState(lobbies.length === 0 ? null : lobbies[0]);

  return html`
    <div class="vh-100 d-flex">
      <div class="align-items-stretch position-sticky d-flex flex-column" style="width: 250px;">
        <div class="ps-3 navbar navbar-dark" style="background-color: #06163d;">
          <div class="text-center">
            <a class="navbar-brand" href="">Lobbies</a>
          </div>
        </div>
        <nav class="h-100 flex-grow-1" style="background-image: linear-gradient(180deg, rgb(5, 39, 103) 0%, #3a0647 70%);">
          ${lobbies.map(lobby => html`
            <div class="nav-item px-3">
              <span class="material-symbols-outlined align-text-top me-1">${getIconNameForLobbyType(lobby.type)}</span>
              ${lobby.name}
            </div>
          `)}
        </nav>
      </div>
      <main class="flex-grow-1 p-3" role="main">
        <${Lobby} lobby=${selectedLobby} />
      </main>
    </div>
  `;
};

/** @type {() => Promise<ILobby[]>} */
const getLobbyListing = async () => {
  const responseMessage = await fetch("/api/lobby/all");
  /** @type {{id: string, name: string, lobbyType: LobbyType}[]} */
  const responseContent = await responseMessage.json();

  return responseContent.map(lobby => {
    switch (lobby.lobbyType) {
      case LobbyType.HideAndSeek:
        return new HideAndSeekLobby(lobby.id, lobby.name);
      case LobbyType.Coop:
        return new CoopLobby(lobby.id, lobby.name);
    }
  });
}

/** @type {ILobby[]} */
const lobbies = await getLobbyListing();

render(html`<${App} lobbies=${lobbies} />`, document.body);
