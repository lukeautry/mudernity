import React from "react";
import { SessionTerminal } from "./SessionTerminal/SessionTerminal";
import { SessionTabs } from "./SessionTabs/SessionTabs";
import { useAgentContext } from "../../../context/AgentProvider";
import { IconVersionsOff } from "@tabler/icons-react";

export const Console: React.FC = () => {
  const sessions = useAgentContext((c) => c.sessions);

  if (sessions.length === 0) {
    return (
      <div className="flex justify-center items-center h-full bg-black">
        <div className="flex gap-4 items-center">
          <IconVersionsOff />
          <div>
            <div className="text-lg">No active sessions</div>
            <div className="text-sm text-gray-500">
              Start a new session to see it here
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="flex h-full flex-col">
      <SessionTabs />
      <SessionTerminal />
    </div>
  );
};
