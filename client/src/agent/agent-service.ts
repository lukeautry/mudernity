import type {
  IProfileConnectionRequest,
  IProfileCreationRequest,
  IProfileDeletionRequest,
  IProfileList,
  IProfileUpdateRequest,
  ISessionCloseRequest,
  ISessionCommandRequest,
  ISessionConnectRequest,
  ISessionData,
  ISessionDisconnectRequest,
} from "./agent-types";
import type { ISessionList } from "./agent-types";

export interface IAgentService {
  on<K extends keyof IAgentMessages>(
    type: K,
    callback: (data: IAgentMessages[K]) => void,
  ): void;
  send(request: AgentRequest): void;
}

type Handlers = {
  [K in keyof IAgentMessages]?: ((data: IAgentMessages[K]) => void)[];
};

export const getAgentService = async (
  handlers: Handlers = {},
): Promise<IAgentService> => {
  const ws = new WebSocket("ws://localhost:5000");

  await new Promise<void>((resolve, reject) => {
    ws.onopen = () => {
      resolve();
    };

    ws.onerror = (error) => {
      reject(error);
    };

    ws.onmessage = (event) => {
      const payload = JSON.parse(
        event.data,
      ) as IAgentMessages[keyof IAgentMessages];

      const typeHandlers = handlers[payload.type];
      if (typeHandlers) {
        typeHandlers.forEach((handler) => handler(payload as never));
      }
    };
  });

  const service: IAgentService = {
    on(type, callback) {
      if (!handlers[type]) {
        handlers[type] = [];
      }

      handlers[type]!.push(callback);
    },
    send(request) {
      ws.send(JSON.stringify(request));
    },
  };

  return service;
};

export type AgentRequest =
  | IProfileCreationRequest
  | IProfileConnectionRequest
  | IProfileDeletionRequest
  | IProfileUpdateRequest
  | ISessionCloseRequest
  | ISessionConnectRequest
  | ISessionDisconnectRequest
  | ISessionCommandRequest;

interface IAgentMessages {
  SessionList: ISessionList;
  ProfileList: IProfileList;
  SessionData: ISessionData;
}
