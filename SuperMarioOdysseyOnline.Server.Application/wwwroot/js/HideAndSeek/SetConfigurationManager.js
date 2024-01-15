import { html } from "htm/preact";
import { useContext } from "preact/hooks";

import StageSelector from "./StageSelector.js";
import { SeekerSettingsContext } from "./SeekerSettingsContext.js";

/**
 * @typedef {import('./Types').ISeekerSettingsContext} ISeekerSettingsContext
 */

const SetConfigurationManager = () => {
  /** @type {ISeekerSettingsContext} */
  const seekerSettings = useContext(SeekerSettingsContext);

  /** @type {(event: InputEvent)} */
  const onSeekersPerRoundInput = (event) => seekerSettings.seekersPerRound.value = parseInt(event.target.value, 10);

  return html`
    <div class="card w-100">
      <div class="card-header">
        Settings
      </div>
      <div class="card-body">
        <div class="row mb-3">
          <div class="col-auto">
            <label class="form-label">Seekers Per Round</label>
            <input type="number" class="form-control" step="1" value=${seekerSettings.seekersPerRound} onInput=${onSeekersPerRoundInput} />
          </div>
        </div>
        <div>
          <label class="form-label">Kingdoms</label>
          <${StageSelector} />
        </div>
      </div>
    </div>
  `;
};

export default SetConfigurationManager;
