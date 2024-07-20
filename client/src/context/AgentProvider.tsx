import React from "react";
import { createContext } from "use-context-selector";
import { getAgentService, IAgentService } from "../agent/agent-service";
import { ISession } from "../agent/agent-types";
import { asContextSelector } from "./as-context-selector";
import { IProfile } from "../agent/agent-types";
import { useCommandHistoryContext } from "./CommandHistoryProvider";

interface IAgentContext {
  service: IAgentService;
  sessions: ISession[];
  profiles: IProfile[];
  cache: SessionCache;
  closeSession: (sessionId: string) => void;
  onSessionData: (sessionId: string, handler: SessionDataHandler) => () => void;
  sessionId: string | undefined;
  session: ISession | undefined;
  setSessionId: (sessionId: string) => void;
}

type SessionDataHandler = (data: Uint8Array) => void;

type SessionCache = { [sessionId: string]: Uint8Array };

export const AgentContext = createContext<IAgentContext>(undefined!);

export const AgentProvider: React.FC<React.PropsWithChildren> = ({
  children,
}) => {
  const [service, setService] = React.useState<IAgentService>();
  const [sessions, setSessions] = React.useState<ISession[]>();
  const [profiles, setProfiles] = React.useState<IProfile[]>();
  const [sessionId, setSessionId] = React.useState<string>();
  const clearCommands = useCommandHistoryContext((c) => c.clearCommands);
  const handlers = React.useRef(
    {} as { [sessionId: string]: SessionDataHandler },
  );

  const cache = React.useRef({} as { [sessionId: string]: Uint8Array });

  const session = React.useMemo(() => {
    if (!sessions || !sessionId) {
      return;
    }

    return sessions.find((session) => session.id === sessionId);
  }, [sessionId, sessions]);

  React.useEffect(() => {
    getAgentService({
      ProfileList: [
        (data) => {
          setProfiles(data.profiles);
        },
      ],
      SessionList: [
        (data) => {
          setSessions(data.sessions);
          setSessionId((prev) => {
            if (prev && data.sessions.find((session) => session.id === prev)) {
              return prev;
            }

            return data.sessions[0]?.id;
          });
        },
      ],
      SessionData: [
        (sessionData) => {
          const { data } = sessionData;

          const decoded = Uint8Array.from(atob(data), (c) => c.charCodeAt(0));

          const cached = cache.current[sessionData.sessionId];

          // always append to the cache
          cache.current[sessionData.sessionId] = cached
            ? new Uint8Array([...cached, ...decoded])
            : decoded;

          const handler = handlers.current[sessionData.sessionId];
          if (handler) {
            handler(decoded);
          }
        },
      ],
    }).then((service) => {
      setService(service);
    });
  }, []);

  const onSessionData = React.useCallback(
    (sessionId: string, handler: SessionDataHandler) => {
      handlers.current[sessionId] = handler;

      const cached = cache.current[sessionId];
      if (cached) {
        handler(cached);
      }

      return () => {
        delete handlers.current[sessionId];
      };
    },
    [],
  );

  const closeSession = React.useCallback(
    (sessionId: string) => {
      service?.send({ type: "SessionClose", id: sessionId });

      const newActiveSessionId = sessions?.find(
        (session) => session.id !== sessionId,
      )?.id;

      setSessionId(newActiveSessionId ?? "");
      clearCommands(sessionId);
    },
    [service, sessions, setSessionId, clearCommands],
  );

  const context = React.useMemo((): IAgentContext | undefined => {
    if (!service || !sessions || !profiles) {
      return;
    }

    return {
      service,
      sessions,
      sessionId,
      setSessionId,
      profiles,
      cache: cache.current,
      closeSession,
      onSessionData,
      session,
    };
  }, [
    closeSession,
    onSessionData,
    profiles,
    service,
    session,
    sessionId,
    sessions,
  ]);

  if (!context) {
    return null;
  }

  return (
    <AgentContext.Provider value={context}>{children}</AgentContext.Provider>
  );
};

export const useAgentContext = asContextSelector(AgentContext);
