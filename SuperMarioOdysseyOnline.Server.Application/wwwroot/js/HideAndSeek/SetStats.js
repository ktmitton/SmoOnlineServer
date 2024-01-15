import { useContext } from "preact/hooks";
import { html } from "htm/preact";

import { StageContext } from "./StageContext.js";
import { LobbyContext } from "./LobbyContext.js";
import playerCompare from "./playerCompare.js";
import RoundAction from "./RoundAction.js";
import { SeekerSettingsContext } from "./SeekerSettingsContext.js";

/**
 * @typedef {import('./Types').IStageContext} IStageContext
 * @typedef {import('./Types').ILobbyContext} ILobbyContext
 * @typedef {import('./Types').ISeekerSettingsContext} ISeekerSettingsContext
 */

const SetStats = () => {
  /** @type {IStageContext} */
  const stage = useContext(StageContext);
  /** @type {ILobbyContext} */
  const lobby = useContext(LobbyContext);
  /** @type {ISeekerSettingsContext} */
  const seekerSettings = useContext(SeekerSettingsContext);

  const createNewSet = () => fetch(
      `api/hideandseek/${lobby.id}/createnewset`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          seekersPerRound: seekerSettings.seekersPerRound.value,
          stages: stage.stages.value.filter(x => x.isSelected).map(x => x.stage)
        })
      }
    );

  const extendSet = () => fetch(
      `api/hideandseek/${lobby.id}/extendcurrentset`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          seekersPerRound: seekerSettings.seekersPerRound.value,
          stages: stage.stages.value.filter(x => x.isSelected).map(x => x.stage)
        })
      }
    );

  return html`
    <div class="card w-100">
        <div class="card-header">
          Set Stats
          <button type="button" class="btn btn-sm btn-primary float-end py-0 px-1" onClick=${createNewSet}>
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
                  ${lobby.set.value.map((round, index) => html`
                    <tr class="align-middle" key=${round.id}>
                      <td>${index + 1}</td>
                      <td>
                        ${round.initialSeekers.sort(playerCompare).map(player => html`
                          <span class="badge rounded-pill bg-success me-2">${player.name}</span>
                        `)}
                      </td>
                      <td>${round.stage.label}</td>
                      <td>${round.playTime.split(".")[0]}</td>
                      <td>
                        <${RoundAction} lobbyId=${lobby.id} round=${round} isCurrentRound=${round.id === lobby.currentRound.value.id} />
                      </td>
                    </tr>
                  `)}
                </tbody>
            </table>
            ${lobby.set.value.length === 0 ? "" : html`
              <button type="button" class="btn btn-primary w-100" onClick=${extendSet}>Extend Set</button>
            `}
        </div>
    </div>
  `;
};

export default SetStats;
