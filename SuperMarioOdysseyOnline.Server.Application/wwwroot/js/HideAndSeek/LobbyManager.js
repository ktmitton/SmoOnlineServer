import { html } from "htm/preact";
import { useContext } from "preact/hooks";

import { LobbyContext } from "./LobbyContext.js";

/**
 * @typedef {import('./Types').ILobbyContext} ILobbyContext
 * @typedef {import('./Types').IPlayer} IPlayer
 */

/** @type {(left: IPlayer, right: IPlayer) => number} */
const playerCompare = (left, right) => {
  const leftValue = left.name.toLowerCase();
  const rightValue = right.name.toLowerCase();

  if (leftValue === rightValue) {
    return 0;
  }

  return leftValue < rightValue ? -1 : 1;
};

const LobbyManager = () => {
  /** @type {ILobbyContext} */
  const lobby = useContext(LobbyContext);

  /** @type {(playerId: string) => Promise} */
  const tagPlayer = (playerId) => fetch(`api/hideandseek/${lobby.id}/tag/${playerId}`, { method: "POST" });

  return html`
    <div class="card w-100">
      <div class="card-header">
        Lobby
        <button type="button" class="btn btn-sm ${lobby.isLocked.value ? "btn-danger" : "btn-success"} float-end py-0 px-1">
            <span class="material-symbols-outlined align-text-top">${lobby.isLocked.value ? "lock" : "lock_open"}</span>
        </button>
      </div>
      <div class="card-body">
        <table class="table table-sm table-hover">
          <thead>
            <tr>
              <th>${"\u00A0"}</th>
              <th>Name</th>
              <th>Time Hidden</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            ${lobby.players.value.sort(playerCompare).map(player => html`
              <tr class="align-middle" key=${player.id}>
                <td class="text-center">
                  ${player.isSeeking ? html`
                      <span class="material-symbols-outlined align-text-top">data_loss_prevention</span>
                  ` : html`
                      <span class="material-symbols-outlined align-text-top">directions_run</span>
                  `}
                </td>
                <td>${player.name}</td>
                <td>${player.timeHidden.split(".")[0]}</td>
                <td>
                  <button type="button" class="btn btn-secondary p-0" onClick=${() => tagPlayer(player.id)} title="Switch Role">
                      <span class="material-symbols-outlined align-text-top">swap_horiz</span>
                  </button>
                </td>
              </tr>
            `)}
          </tbody>
        </table>
      </div>
    </div>
  `;
};

export default LobbyManager;
