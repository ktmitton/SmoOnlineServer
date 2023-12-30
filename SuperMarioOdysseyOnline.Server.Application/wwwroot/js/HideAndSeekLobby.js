import { useState, useEffect } from "preact/hooks";

import LobbyType from "./LobbyType.js";
import HideAndSeekRoundStatus from "./HideAndSeekRoundStatus.js";
import { CancellationToken, CancellationTokenSource } from "./CancellationToken.js";
import html from "./html.js";

/**
 * @typedef {import('./Types').ILobby} ILobby
 * @typedef {import('./Types').ICancellationToken} ICancellationToken
 * @typedef {import('./Types').IHideAndSeekPlayer} IPlayer
 * @typedef {import('./Types').IHideAndSeekRound} IRound
 * @typedef {import('./Types').IHideAndSeekRefreshResponse} IRefreshResponse
 */

/**
 * @implements {ILobby}
 */
class Lobby {
  /** @type {string} */
  #id;
  get id() {
    return this.#id;
  }

  /** @type {string} */
  #name;
  get name() {
    return this.#name;
  }

  get type() {
    return LobbyType.HideAndSeek;
  }

  /** @type {boolean} */
  #isLocked;
  get isLocked() {
    return this.#isLocked;
  }

  /** @type {IPlayer[]} */
  #players = [];
  get players() {
    return this.#players;
  }

  /** @type {IRound[]} */
  #set = [];
  get set() {
    return this.#set;
  }

  /** @type {{id: string}} */
  #currentRound = null;
  get currentRound() {
    return this.#currentRound;
  }

  constructor(
    /** @type {string} */
    id,
    /** @type {string} */
    name
  ) {
    this.#id  = id;
    this.#name  = name;
  }

  /**
   *
   * @param {CancellationToken} cancellationToken
   */
  async refreshAsync(cancellationToken) {
    if (cancellationToken.isCancellationRequested) {
      return;
    }

    const responseMessage = await fetch(`api/hideandseek/${this.#id}`);
    /** @type {IRefreshResponse} */
    const responseContent = await responseMessage.json();

    this.#players = responseContent.players;
    this.#set = responseContent.set;
    this.#isLocked = responseContent.isLocked;
    this.#currentRound = responseContent.currentRound;
  }
}

/** @type {(props: {lobbyId: string, round: IRound, isCurrentRound: boolean}) => string} */
const RoundAction = ({lobbyId, round, isCurrentRound}) => {
  const loadRound = async () => {
    await fetch(`api/hideandseek/${lobbyId}/load`, { method: "POST" });
  };

  const playRound = async () => {
    await fetch(`api/hideandseek/${lobbyId}/play`, { method: "POST" });
  };

  const pauseRound = async () => {
    await fetch(`api/hideandseek/${lobbyId}/pause`, { method: "POST" });
  };

  if (round.status === HideAndSeekRoundStatus.Completed) {
    return html`
      <button type="button" class="btn btn-link btn-sm py-0 px-1" disabled>
        <span class="material-symbols-outlined align-text-top">check_circle</span>
      </button>
    `;
  }

  if (!isCurrentRound) {
    return html`
      <button type="button" class="btn btn-secondary btn-sm py-0 px-1 invisible" disabled>
        <span class="material-symbols-outlined align-text-top">flight_takeoff</span>
      </button>
    `;
  }

  switch (round.status) {
    case HideAndSeekRoundStatus.Queued:
      return html`
        <button type="button" class="btn btn-secondary btn-sm py-0 px-1" onClick=${loadRound}>
          <span class="material-symbols-outlined align-text-top">flight_takeoff</span>
        </button>
      `;
    case HideAndSeekRoundStatus.Loading:
    case HideAndSeekRoundStatus.Paused:
      return html`
        <button type="button" class="btn btn-secondary btn-sm py-0 px-1" onClick=${playRound}>
          <span class="material-symbols-outlined align-text-top">play_arrow</span>
        </button>
      `;
    case HideAndSeekRoundStatus.Playing:
      return html`
        <button type="button" class="btn btn-secondary btn-sm py-0 px-1" onClick=${pauseRound}>
          <span class="material-symbols-outlined align-text-top">pause</span>
        </button>
      `;
  }

  return "\u00A0";
};

/** @type {(left: IPlayer, right: IPlayer) => number} */
const playerCompare = (left, right) => {
  const leftValue = left.name.toLowerCase();
  const rightValue = right.name.toLowerCase();

  if (leftValue === rightValue) {
    return 0;
  }

  return leftValue < rightValue ? -1 : 1;
};

/** @type {(props: {lobby: Lobby}) => string} */
const ViewComponent = ({ lobby }) => {
  const [ lastRefreshedTimestamp, setLastRefreshedTimestamp ] = useState(new Date());
  const [ showCreateNewSetModal, setShowCreateNewSetModal ] = useState(false);
  const [ seekersPerRound, setSeekersPerRound ] = useState(2);

  useEffect(() => {
    const cancellationTokenSource = new CancellationTokenSource();
    const cancellationToken = cancellationTokenSource.token;

    const intervalId = setTimeout(async () => {
      if (cancellationToken.isCancellationRequested) {
        clearInterval(intervalId);

        return;
      }

      await lobby.refreshAsync(cancellationToken);

      if (!cancellationToken.isCancellationRequested) {
        setLastRefreshedTimestamp(new Date());
      }
    }, 1000);

    return () => cancellationTokenSource.cancel();
  }, [lastRefreshedTimestamp]);

  /** @type {(event: InputEvent)} */
  const onSeekersPerRoundInput = (event) => setSeekersPerRound(parseInt(event.target.value, 10));

  const createNewSet = async () => {
    await fetch(`api/hideandseek/${lobby.id}/createnewset/${seekersPerRound}`, { method: "POST" });

    setShowCreateNewSetModal(false);
  };

  const extendSet = async () => {
    await fetch(`api/hideandseek/${lobby.id}/extendcurrentset/${seekersPerRound}`, { method: "POST" });
  };

  /** @type {(playerId: string) => Promise} */
  const tagPlayer = async (playerId) => {
    await fetch(`api/hideandseek/${lobby.id}/tag/${playerId}`, { method: "POST" });
  };

  return html`
    <div class="row">
      <div class="col-12 col-xl-4 d-flex align-items-stretch mb-3">
        <div class="card w-100">
          <div class="card-header">
            Lobby
            <button type="button" class="btn btn-sm ${lobby.isLocked ? "btn-danger" : "btn-success"} float-end py-0 px-1">
                <span class="material-symbols-outlined align-text-top">${lobby.isLocked ? "lock" : "lock_open"}</span>
            </button>
          </div>
            <div class="card-body">
              <table class="table table-sm table-hover">
                <thead>
                  <tr>
                    <th>${"\u00A0"}</th>
                    <th>Name</th>
                    <th>Time Hidden</th>
                  </tr>
                </thead>
                <tbody>
                  ${lobby.players.sort(playerCompare).map(player => html`
                    <tr class="align-middle">
                      <td class="text-center">
                        ${player.isSeeking ? html`
                          <span class="btn btn-link p-0 disabled">
                            <span class="material-symbols-outlined align-text-top">search</span>
                          </span>
                        ` : html`
                          <button type="button" class="btn btn-secondary p-0" onClick=${() => tagPlayer(player.id)}>
                            <span class="material-symbols-outlined align-text-top">touch_app</span>
                          </button>
                        `}
                      </td>
                      <td>${player.name}</td>
                      <td>${player.timeHidden.split(".")[0]}</td>
                    </tr>
                  `)}
                </tbody>
              </table>
            </div>
        </div>
      </div>
      <div class="col-12 col-xl-8 d-flex align-items-stretch mb-3">
        <div class="card w-100">
            <div class="card-header">
              Set Stats
              <button type="button" class="btn btn-sm btn-primary float-end py-0 px-1" onClick=${() => setShowCreateNewSetModal(true)}>
                  <span class="material-symbols-outlined align-text-top">shuffle</span>
              </button>
            </div>
            <div class="card-body">
                <table class="table table-sm table-hover">
                    <thead>
                        <tr>
                            <th>Round</th>
                            <th>Seekers</th>
                            <th>Stage</th>
                            <th>Time</th>
                            <th>${"\u00A0"}</th>
                        </tr>
                    </thead>
                    <tbody>
                      ${lobby.set.map((round, index) => html`
                        <tr class="align-middle">
                          <td>${index + 1}</td>
                          <td>
                            ${round.initialSeekers.sort(playerCompare).map(player => html`
                              <span class="badge rounded-pill bg-success me-2">${player.name}</span>
                            `)}
                          </td>
                          <td>${round.stage.kingdom}</td>
                          <td>${round.playTime.split(".")[0]}</td>
                          <td>
                            <${RoundAction} lobbyId=${lobby.id} round=${round} isCurrentRound=${round.id === lobby.currentRound.id} />
                          </td>
                        </tr>
                      `)}
                    </tbody>
                </table>
                ${lobby.set.length === 0 ? "" : html`
                  <button type="button" class="btn btn-primary w-100" onClick=${extendSet}>Extend Set</button>
                `}
            </div>
        </div>
      </div>
    </div>
    ${showCreateNewSetModal ? html`
      <div class="modal-backdrop show">
      </div>
      <div class="modal d-block">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">Build New Set</h5>
            </div>
            <div class="modal-body">
              <label class="form-label">Seekers Per Round</label>
              <input type="number" class="form-control" value="${seekersPerRound}" onInput=${onSeekersPerRoundInput} step="1" />
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" onClick=${() => setShowCreateNewSetModal(false)}>Close</button>
              <button type="button" class="btn btn-primary" onClick=${createNewSet}>Build</button>
            </div>
          </div>
        </div>
      </div>
    ` : ""
    }
  `;
};

export { Lobby, ViewComponent };
