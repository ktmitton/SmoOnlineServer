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
  totalTimeHidden: number
}

export interface IHideAndSeekRound {
  seekers: IHideAndSeekPlayer[],
  stage: string,
  playTime: number,
  status: HideAndSeekRoundStatus
}

export interface IHideAndSeekRefreshResponse {
  isLocked: boolean,
  players: IHideAndSeekPlayer[],
  set: IHideAndSeekRound[],
  currentRound: {id: string, status: number}
}
