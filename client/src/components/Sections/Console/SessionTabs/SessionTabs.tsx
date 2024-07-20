import React from "react";
import { useAgentContext } from "../../../../context/AgentProvider";
import { TabList } from "../../../common/Tabs/TabList";
import { Tab } from "../../../common/Tabs/Tab";
import type { SessionState } from "../../../../agent/agent-types";
import { IconX } from "@tabler/icons-react";

const StatusIndicator: React.FC<{ status: SessionState }> = ({ status }) => {
  const size = "8px";

  return (
    <div
      style={{ width: size, height: size }}
      className={`rounded-full ${
        status === "active" ? "bg-green-500" : "bg-red-500"
      }`}
    />
  );
};

export const SessionTabs: React.FC = () => {
  const sessions = useAgentContext((c) => c.sessions);
  const sessionId = useAgentContext((c) => c.sessionId);
  const setSessionId = useAgentContext((c) => c.setSessionId);
  const closeSession = useAgentContext((c) => c.closeSession);

  const tabs = React.useMemo(() => {
    return sessions.map((session) => {
      return (
        <Tab
          key={session.id}
          active={session.id === sessionId}
          onClick={() => setSessionId(session.id)}
        >
          <div className="flex items-center gap-2">
            <StatusIndicator status={session.state} />
            {session.hostname}:{session.port}
            <div
              className="flex hover:bg-black rounded"
              style={{ padding: ".1rem" }}
              onClick={(event) => {
                event.stopPropagation();
                event.preventDefault();
                closeSession(session.id);
              }}
            >
              <IconX size={16} title="Close" className="text-stone-200" />
            </div>
          </div>
        </Tab>
      );
    });
  }, [closeSession, sessionId, sessions, setSessionId]);

  return <TabList>{tabs}</TabList>;
};
