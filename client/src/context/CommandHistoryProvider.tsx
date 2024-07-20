import React from "react";
import { createContext } from "use-context-selector";
import { asContextSelector } from "./as-context-selector";
import { CommandHistoryManager } from "../components/Sections/Console/SessionTerminal/command-history-manager";

interface ICommandHistoryContext {
  addCommand(sessionId: string, command: string): void;
  getCommands(sessionId: string): string[];
  clearCommands(sessionId: string): void;
  getPreviousCommand(sessionId: string): string | undefined;
  getNextCommand(sessionId: string): string | undefined;
}

const CommandHistoryContext = createContext<ICommandHistoryContext>(undefined!);

export const CommandHistoryProvider: React.FC<React.PropsWithChildren> = ({
  children,
}) => {
  const history = React.useRef(new CommandHistoryManager(localStorage));

  const addCommand = React.useCallback((sessionId: string, command: string) => {
    history.current.addCommand(sessionId, command);
  }, []);

  const clearCommands = React.useCallback((sessionId: string) => {
    history.current.clearCommands(sessionId);
  }, []);

  const getCommands = React.useCallback((sessionId: string) => {
    return history.current.getCommands(sessionId);
  }, []);

  const getPreviousCommand = React.useCallback((sessionId: string) => {
    return history.current.getPreviousCommand(sessionId);
  }, []);

  const getNextCommand = React.useCallback((sessionId: string) => {
    return history.current.getNextCommand(sessionId);
  }, []);

  const context = React.useMemo((): ICommandHistoryContext => {
    return {
      addCommand,
      getCommands,
      clearCommands,
      getPreviousCommand,
      getNextCommand,
    };
  }, [
    addCommand,
    getCommands,
    clearCommands,
    getPreviousCommand,
    getNextCommand,
  ]);

  return (
    <CommandHistoryContext.Provider value={context}>
      {children}
    </CommandHistoryContext.Provider>
  );
};

export const useCommandHistoryContext = asContextSelector(
  CommandHistoryContext,
);
