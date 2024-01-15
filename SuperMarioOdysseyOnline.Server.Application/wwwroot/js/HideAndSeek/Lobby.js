import { html } from "htm/preact";

import { StageContextProvider } from "./StageContext.js";
import { LobbyContextProvider } from "./LobbyContext.js";
import LobbyManager from "./LobbyManager.js";
import { SeekerSettingsContextProvider } from "./SeekerSettingsContext.js";
import SetConfigurationManager from "./SetConfigurationManager.js";
import SetStats from "./SetStats.js";

/** @type {(props: {id: string, name: string}) => string} */
const Lobby = ({ id, name }) => {
  return html`
    <${LobbyContextProvider} id=${id} name=${name}>
      <${SeekerSettingsContextProvider}>
        <${StageContextProvider}>
          <div class="row g-3">
            <div class="col-12 col-xl-4 d-flex align-items-stretch">
              <${LobbyManager} />
            </div>
            <div class="col-12 col-xl-8 d-flex align-items-stretch">
              <${SetStats} />
            </div>
            <div class="col-12">
              <${SetConfigurationManager} />
            </div>
          </div>
        <//>
      <//>
    <//>
  `;
};

export default Lobby;
