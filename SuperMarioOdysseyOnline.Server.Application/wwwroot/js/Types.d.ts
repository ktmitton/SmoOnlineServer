import LobbyType from "./LobbyType";

export interface ILobby {
  id: string;
  name: string;
  type: LobbyType;
}

export interface ICancellationToken {
  isCancellationRequested: boolean;
}
