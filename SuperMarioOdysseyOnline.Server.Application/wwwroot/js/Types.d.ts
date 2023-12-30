import LobbyType from "./LobbyType";
import HideAndSeekRoundStatus from "./HideAndSeekRoundStatus.js";

export interface ILobby {
  id: string;
  name: string;
  type: LobbyType;
}

export interface ICancellationToken {
  isCancellationRequested: boolean;
}

export interface IHideAndSeekPlayer {
  id: string,
  name: string,
  isSeeking: boolean,
  timeHidden: number
}

export interface IHideAndSeekRound {
  initialSeekers: IHideAndSeekPlayer[],
  stage: {kingdom: string, stage: string},
  playTime: number,
  status: HideAndSeekRoundStatus
}

export interface IHideAndSeekRefreshResponse {
  isLocked: boolean,
  players: IHideAndSeekPlayer[],
  set: IHideAndSeekRound[],
  currentRound: {id: string, status: number}
}
