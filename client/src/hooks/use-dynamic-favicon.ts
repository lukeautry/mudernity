import React from "react";
import { useAgentContext } from "../context/AgentProvider";
import { useAppContext } from "../context/AppProvider";

export const useDynamicFavicon = () => {
  const section = useAppContext((c) => c.section);
  const session = useAgentContext((c) => c.session);

  React.useEffect(() => {
    const favicon = document.querySelector(
      'link[rel="icon"]',
    ) as HTMLLinkElement;

    const href = (() => {
      if (section === "console" && session) {
        if (session.state === "active") {
          return "connected.svg";
        } else {
          return "disconnected.svg";
        }
      }

      return "favicon.svg";
    })();

    favicon.href = href;
  }, [section, session]);
};
