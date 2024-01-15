import { html } from "htm/preact";

import RoundStatus from "./RoundStatus.js";

/**
 * @typedef {import('./Types').IRound} IRound
 */

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

  if (round.status === RoundStatus.Completed) {
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
    case RoundStatus.Queued:
      return html`
        <button type="button" class="btn btn-secondary btn-sm py-0 px-1" onClick=${loadRound}>
          <span class="material-symbols-outlined align-text-top">flight_takeoff</span>
        </button>
      `;
    case RoundStatus.Loading:
    case RoundStatus.Paused:
      return html`
        <button type="button" class="btn btn-secondary btn-sm py-0 px-1" onClick=${playRound}>
          <span class="material-symbols-outlined align-text-top">play_arrow</span>
        </button>
      `;
    case RoundStatus.Playing:
      return html`
        <button type="button" class="btn btn-secondary btn-sm py-0 px-1" onClick=${pauseRound}>
          <span class="material-symbols-outlined align-text-top">pause</span>
        </button>
      `;
  }

  return "\u00A0";
};

export default RoundAction;
