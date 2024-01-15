import RoundStatus from "./RoundStatus";

export interface IStage {
  kingdom: string,
  stage: string,
  label: string
  isSelected: boolean
}

export interface IStageContext {
  stages: {value: IStage[]},
  toggleStage: (name: string) => void
}

export interface ILobbyContext  {
  id: string,
  name: string,
  isLocked: {value: boolean},
  players: {value: IPlayer[]},
  set: {value: IRound[]},
  currentRound: {value: IRound}
}

export interface IPlayer {
  id: string,
  name: string,
  isSeeking: boolean,
  timeHidden: number
}

export interface IRefreshResponse {
  isLocked: boolean,
  players: IPlayer[],
  set: IRound[],
  currentRound: {id: string, status: number}
}

export interface IRound {
  initialSeekers: IPlayer[],
  stage: {kingdom: string, stage: string},
  playTime: number,
  status: RoundStatus
}

export interface ISeekerSettingsContext {
  seekersPerRound: {value: number}
}
