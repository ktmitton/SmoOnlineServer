import { html } from "htm/preact";
import { createContext } from "preact";
import { signal } from "preact/signals";

/**
 * @typedef {import('./Types').ISeekerSettingsContext} ISeekerSettingsContext
 */

const SeekerSettingsContext = createContext({seekersPerRound: signal(2)});

const SeekerSettingsContextProvider = ({children}) => {
  /** @type {{value: number}} */
  const seekersPerRound = signal(2);

  /** @type {ISeekerSettingsContext} */
  const context = {
    seekersPerRound
  };

  return html`
    <${SeekerSettingsContext.Provider} value=${context}>${children}<//>
  `
};

export { SeekerSettingsContext, SeekerSettingsContextProvider };
