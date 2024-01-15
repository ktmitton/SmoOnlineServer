import { render } from "preact";
import { html } from "htm/preact";

import App from "./App.js";

/**
 * @typedef {import('./Types').ILobby} ILobby
 */

const responseMessage = await fetch("/api/lobby/all");
/** @type {ILobby[]} */
const lobbies = await responseMessage.json();

render(html`<${App} lobbies=${lobbies} />`, document.body);
