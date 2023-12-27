class CancellationToken {
  /** @type {CancellationTokenSource} */
  #cancellationTokenSource;

  get isCancellationRequested() {
    this.#cancellationTokenSource.isCancellationRequested;
  }

  constructor(cancellationTokenSource) {
    this.#cancellationTokenSource = cancellationTokenSource;
  }
}

class CancellationTokenSource {
  #isCancellationRequested = false;
  get isCancellationRequested() {
    return this.#isCancellationRequested;
  }

  #token = new CancellationToken(this);
  get token() {
    return this.#token;
  }

  cancel() {
    this.#isCancellationRequested = true;
  }
}

export { CancellationTokenSource, CancellationToken }
