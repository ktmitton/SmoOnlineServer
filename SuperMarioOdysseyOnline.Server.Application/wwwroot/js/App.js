import { html } from "htm/preact";
import { render } from "preact";
import { useState } from "preact/hooks";

import Lobby from "./Lobby.js";
import LobbyType from "./LobbyType.js";

/**
 * @typedef {import('./Types').ILobby} ILobby
 */

/** @type {(lobbyType: LobbyType) => string} */
const getIconNameForLobbyType = (lobbyType) => {
  switch (lobbyType) {
    case LobbyType.HideAndSeek:
      return "search";
    case LobbyType.Coop:
      return "sprint";
    default:
      return "videogame_asset";
  }
};

/** @type {(props: {lobbies: ILobby[]}) => string} */
const App = ({ lobbies }) => {
  const [ selectedLobby, setSelectedLobby ] = useState(lobbies.length === 0 ? null : lobbies[0]);

  return html`
    <nav class="position-fixed top-0 start-0 vh-100" style="width: 250px;">
      <div class="ps-3 navbar navbar-dark" style="background-color: #06163d;">
        <div class="text-center">
          <span class="navbar-brand">Lobbies</span>
        </div>
      </div>
      <div class="h-100 px-3 py-2" style="background-image: linear-gradient(180deg, rgb(5, 39, 103) 0%, #3a0647 70%);">
        <div class="nav flex-columns gap-2">
            ${lobbies.map(lobby => html`
              <div class="nav-item w-100" key=${lobby.id}>
                <button type="button" class="btn btn-outline-light w-100 opacity-75 text-start py-2 ${lobby === selectedLobby  ? "active" : " "}" onClick=${() => setSelectedLobby(lobby)}>
                  <span class="material-symbols-outlined align-text-top me-1">${getIconNameForLobbyType(lobby.type)}</span>
                  ${lobby.name}
                </button>
              </div>
            `)}
        </div>
      </div>
    </nav>
    <main class="p-3" role="main" style="margin-left: 250px;">
      <${Lobby} lobby=${selectedLobby} />
    </main>
  `;
};

const setTheme = () => {
  document.documentElement.setAttribute(
    "data-bs-theme",
    window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light"
  );
}

window.matchMedia("(prefers-color-scheme: dark)").addEventListener("change", setTheme);

setTheme();

const responseMessage = await fetch("/api/lobby/all");
/** @type {ILobby[]} */
const lobbies = await responseMessage.json();

render(html`<${App} lobbies=${lobbies} />`, document.body);
