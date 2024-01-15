import { html } from "htm/preact";
import { useContext, useId } from "preact/hooks";

import { StageContext } from "./StageContext.js";

/**
 * @typedef {import('./Types').IStageContext} IStageContext
 */

const StageSelector = () => {
  /** @type {IStageContext} */
  const context = useContext(StageContext);

  /** @type {(previousValue: IStage[][], currentValue: IStage, currentIndex: number) => IStage[][]} */
  const stageChunkReducer = (previousValue, currentValue, currentIndex) => {
    const chunkId = Math.floor(currentIndex / 6);
    if (previousValue.length < (chunkId + 1)) {
      previousValue.push([])
    }

    previousValue[chunkId].push(currentValue);

    return previousValue;
  }

  const stageGroups = context.stages.value.reduce(stageChunkReducer, []);

  return html`
    <div class="row g-3">
      ${stageGroups.map((group, index) => html`
        <div class="col-sm-12 col-lg-6 col-xxl" key=${index}>
          <div class="list-group">
            ${group.map(stage => {
              const checkboxId = useId();

              return html`
                <button class="list-group-item list-group-item-action" key=${stage.stage} onClick=${() => context.toggleStage(stage.stage)}>
                  <div class="form-check form-switch">
                    <input class="form-check-input me-1" type="checkbox" role="switch" id=${checkboxId} checked=${stage.isSelected} />
                    <label class="form-check-label w-100" for=${checkboxId} onClick=${() => context.toggleStage(stage.stage)}>${stage.label}</label>
                  </div>
                </button>
              `;
            })}
          </div>
        </div>
      `)}
    </div>
  `;
};

export default StageSelector;
