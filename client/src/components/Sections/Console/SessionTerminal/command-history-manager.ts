import type { ILocalStorage } from "../../../../utils/local-storage";

type CommandHistory = { [sessionId: string]: string[] };
type CursorPosition = { [sessionId: string]: number | undefined };

export const commandHistoryKey = "command-history";
const maxCommandHistory = 100;

const defaultCursor = -1;

export class CommandHistoryManager {
  private readonly history: CommandHistory;
  private readonly cursor: CursorPosition;

  constructor(private readonly storage: ILocalStorage) {
    this.storage = storage;
    this.history = this.getDefaultHistory();
    this.cursor = {};
  }

  public addCommand(sessionId: string, command: string): void {
    if (!this.history[sessionId]) {
      this.history[sessionId] = [];
    }

    this.history[sessionId].unshift(command);
    this.history[sessionId] = this.history[sessionId].slice(
      0,
      maxCommandHistory,
    );
    this.cursor[sessionId] = defaultCursor; // Reset cursor position
    this.saveHistory();
  }

  public clearCommands(sessionId: string): void {
    this.history[sessionId] = [];
    this.cursor[sessionId] = defaultCursor; // Reset cursor position
    this.saveHistory();
  }

  public getCommands(sessionId: string): string[] {
    return this.history[sessionId] || [];
  }

  public getPreviousCommand(sessionId: string): string | undefined {
    return this.getSurroundingCommand(sessionId, 1);
  }

  public getNextCommand(sessionId: string): string | undefined {
    return this.getSurroundingCommand(sessionId, -1);
  }

  private getDefaultHistory(): CommandHistory {
    const history = this.storage.getItem(commandHistoryKey);

    if (!history) {
      return {};
    }

    return JSON.parse(history);
  }

  private saveHistory() {
    this.storage.setItem(commandHistoryKey, JSON.stringify(this.history));
  }

  private getSurroundingCommand(sessionId: string, change: number) {
    const sessionCommands = this.history[sessionId] || [];
    if (sessionCommands.length === 0) {
      return;
    }

    const cursor = this.cursor[sessionId] ?? defaultCursor;
    const newCursor = cursor + change;

    const command = sessionCommands[newCursor];
    if (!command) {
      return sessionCommands[cursor];
    }

    this.cursor[sessionId] = newCursor;
    return command;
  }
}
