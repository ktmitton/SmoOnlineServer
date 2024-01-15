import { html } from "htm/preact";
import { createContext } from "preact";
import { useEffect } from "preact/hooks";
import { signal, batch } from "preact/signals";

import { CancellationTokenSource } from "../CancellationToken.js";

/**
 * @typedef {import('./Types').ILobbyContext} ILobbyContext
 * @typedef {import('./Types').IPlayer} IPlayer
 * @typedef {import('./Types').IRefreshResponse} IRefreshResponse
 * @typedef {import('./Types').IRound} IRound
 */

const LobbyContext = createContext(signal({}));

/** @type {({id: string, name: string, children: any}) => string} */
const LobbyContextProvider = ({id, name, children}) => {
  /** @type {{value: boolean}} */
  const isLocked = signal(false);
  /** @type {{value: IPlayer[]}} */
  const players = signal([]);
  /** @type {{value: IRound[]}} */
  const set = signal([]);
  /** @type {{value: IRound?}} */
  const currentRound = signal(null);

  /** @type {ILobbyContext} */
  const context = {
    id,
    name,
    isLocked,
    players,
    set,
    currentRound
  };

  useEffect(() => {
    const cancellationTokenSource = new CancellationTokenSource();
    const cancellationToken = cancellationTokenSource.token;

    const refreshLobby = async () => {
      if (cancellationToken.isCancellationRequested) {
        return;
      }

      const responseMessage = await fetch(`api/hideandseek/${id}`);
      /** @type {IRefreshResponse} */
      const responseContent = await responseMessage.json();

      if (cancellationToken.isCancellationRequested) {
        return;
      }

      batch(() => {
        players.value = responseContent.players;
        set.value = responseContent.set;
        isLocked.value = responseContent.isLocked;
        currentRound.value = responseContent.currentRound;
      });

      setTimeout(refreshLobby, 1000);
    };

    setTimeout(refreshLobby, 1000);

    return () => cancellationTokenSource.cancel();
  }, [id, isLocked, players, set, currentRound]);

  return html`<${LobbyContext.Provider} value=${context}>${children}<//>`
};

export { LobbyContext, LobbyContextProvider };
