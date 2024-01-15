/**
 * @typedef {import('./Types').IPlayer} IPlayer
 * /

/** @type {(left: IPlayer, right: IPlayer) => number} */
const playerCompare = (left, right) => {
  const leftValue = left.name.toLowerCase();
  const rightValue = right.name.toLowerCase();

  if (leftValue === rightValue) {
    return 0;
  }

  return leftValue < rightValue ? -1 : 1;
};

export default playerCompare;
