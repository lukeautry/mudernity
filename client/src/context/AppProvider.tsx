import React from "react";
import { createContext } from "use-context-selector";
import { asContextSelector } from "./as-context-selector";
import { Section, isValidSection } from "../components/Sections/sections";
import { AgentProvider } from "./AgentProvider";
import { useQueryParams } from "../hooks/use-query-params";
import { CommandHistoryProvider } from "./CommandHistoryProvider";

interface IAppContext {
  section: Section;
  setSection: (section: Section) => void;
}

const AppContext = createContext<IAppContext>(undefined!);

const sectionKey = "section";

const defaultSection: Section = "console";

export const AppProvider: React.FC<React.PropsWithChildren> = ({
  children,
}) => {
  const [queryParams, setQueryParams] = useQueryParams();

  const section = React.useMemo(() => {
    const fromQuery = queryParams.get(sectionKey);
    if (isValidSection(fromQuery)) {
      return fromQuery;
    }

    return defaultSection;
  }, [queryParams]);

  const setSection = React.useCallback(
    (section: Section) => {
      setQueryParams((params) => {
        if (section !== defaultSection) {
          params.set(sectionKey, section);
        } else {
          params.delete(sectionKey);
        }
      });
    },
    [setQueryParams],
  );

  const context = React.useMemo((): IAppContext => {
    return {
      section,
      setSection,
    };
  }, [section, setSection]);

  return (
    <AppContext.Provider value={context}>
      <CommandHistoryProvider>
        <AgentProvider>{children}</AgentProvider>
      </CommandHistoryProvider>
    </AppContext.Provider>
  );
};

export const useAppContext = asContextSelector(AppContext);
