export class CliError extends Error {
  constructor(code, message, details) {
    super(message);
    this.name = 'CliError';
    this.code = code;
    this.details = details;
  }
}
export const fail = (code, message, details) => { throw new CliError(code, message, details); };
