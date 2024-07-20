import React from "react";
import { useAppContext } from "../context/AppProvider";
import { useAgentContext } from "../context/AgentProvider";
import { sections } from "../components/Sections/sections";

export const useDynamicTitle = () => {
  const section = useAppContext((c) => c.section);
  const session = useAgentContext((c) => c.session);

  React.useEffect(() => {
    if (section === "console" && session) {
      document.title = `${session.hostname}:${session.port} | Mudernity`;
    } else {
      document.title = `${sections[section].label} | Mudernity`;
    }
  }, [section, session]);
};
