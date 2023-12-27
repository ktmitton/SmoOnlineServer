import LobbyType from "./LobbyType.js";

/**
 * @typedef {import('./Types').ILobby} ILobby
 */

/**
 * @implements {ILobby}
 */
class CoopLobby {
  /** @type {string} */
  #id;
  get id() {
    return this.#id;
  }

  /** @type {string} */
  #name;
  get name() {
    return this.#name;
  }

  get type() {
    return LobbyType.Coop;
  }

  constructor(
    /** @type {string} */
    id,
    /** @type {string} */
    name
  ) {
    this.#id  = id;
    this.#name  = name;
  }
}

export default CoopLobby;
