import { html } from "htm/preact";
import { createContext } from "preact";
import { signal } from "preact/signals";

/**
 * @typedef {import('./Types').IStage} IStage
 * @typedef {import('./Types').IStageContext} IStageContext
 */

const responseMessage = await fetch("api/hideandseek/kingdoms/");
/** @type {{kingdom: string, stage: string, label: string}[]} */
const stages = await responseMessage.json();
const defaultDeselectedStageLabels = ["Deep Woods", "Cloud Kingdom", "Dark Side", "Darker Side", "Odyssey"];
const defaultStages = stages.map(stage => {
  return {
    stage: stage.stage,
    kingdom: stage.kingdom,
    label: stage.label,
    isSelected: !defaultDeselectedStageLabels.includes(stage.label)
  }
});

const StageContext = createContext(signal([...defaultStages]));

const StageContextProvider = ({children}) => {
  /** @type {{value: IStage[]}} */
  const stages = signal([...defaultStages]);

  /** @type {(stageName: string) => void} */
  const toggleStage = (stageName) => {
    stages.value = stages.value.map(stage => {
      if (stage.stage === stageName) {
        stage.isSelected = !stage.isSelected;
      }

      return stage;
    });
  };

  /** @type {IStageContext} */
  const context = {
    stages,
    toggleStage
  };

  return html`
    <${StageContext.Provider} value=${context}>${children}<//>
  `
};

export { StageContext, StageContextProvider };
