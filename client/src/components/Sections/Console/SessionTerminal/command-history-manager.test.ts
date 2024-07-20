import type { ILocalStorage } from "../../../../utils/local-storage";
import { mockLocalStorage } from "../../../../utils/mock-local-storage";
import { uuidv4 } from "../../../../utils/uuid-v4";
import { CommandHistoryManager } from "./command-history-manager";

describe("CommandHistoryManager", () => {
  let storage: ILocalStorage;
  let manager: CommandHistoryManager;
  let sessionId: string;

  beforeEach(() => {
    storage = mockLocalStorage();
    manager = new CommandHistoryManager(storage);
    sessionId = uuidv4();
  });

  const assertStorage = (expected: { [key: string]: string[] }) => {
    const entry = storage.getItem("command-history");
    if (!entry) {
      throw new Error("No entry found in storage");
    }

    expect(JSON.parse(entry)).toMatchObject(expected);
  };

  it("should add a command to the history", () => {
    const command = "test command";
    manager.addCommand(sessionId, command);
    expect(manager.getCommands(sessionId)).toContain(command);

    assertStorage({
      [sessionId]: [command],
    });
  });

  it("should clear the command history", () => {
    manager.addCommand(sessionId, "command 1");
    manager.addCommand(sessionId, "command 2");
    manager.clearCommands(sessionId);
    expect(manager.getCommands(sessionId)).toEqual([]);

    assertStorage({
      [sessionId]: [],
    });
  });

  it("should return the correct commands", () => {
    manager.addCommand(sessionId, "command 1");
    manager.addCommand(sessionId, "command 2");
    expect(manager.getCommands(sessionId)).toMatchObject([
      "command 2",
      "command 1",
    ]);

    assertStorage({
      [sessionId]: ["command 2", "command 1"],
    });
  });

  it("should return the previous command", () => {
    manager.addCommand(sessionId, "command 1");
    manager.addCommand(sessionId, "command 2");

    expect(manager.getPreviousCommand(sessionId)).toBe("command 2");
    expect(manager.getPreviousCommand(sessionId)).toBe("command 1");

    // it shouldn't go further back than the oldest command
    expect(manager.getPreviousCommand(sessionId)).toBe("command 1");
  });

  it("should return the next command", () => {
    manager.addCommand(sessionId, "command 1");
    manager.addCommand(sessionId, "command 2");

    expect(manager.getPreviousCommand(sessionId)).toBe("command 2");
    expect(manager.getPreviousCommand(sessionId)).toBe("command 1");

    // it shouldn't go further up than the most recent command
    expect(manager.getNextCommand(sessionId)).toBe("command 2");
    expect(manager.getNextCommand(sessionId)).toBe("command 2");
  });
});
