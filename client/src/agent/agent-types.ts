export interface IProfileList {
  type: "ProfileList";
  profiles: IProfile[];
}

export interface IProfile {
  id: string;
  hostname: string;
  port: number;
  name: string;
}

export interface IProfileCreationRequest {
  type: "ProfileCreation";
  hostname: string;
  port: number;
  name: string;
}

export interface IProfileConnectionRequest {
  type: "ProfileConnection";
  id: string;
}

export interface IProfileUpdateRequest {
  type: "ProfileUpdate";
  profile: IProfile;
}

export interface IProfileDeletionRequest {
  type: "ProfileDeletion";
  id: string;
}

export interface ISessionList {
  type: "SessionList";
  sessions: ISession[];
}

export interface ISessionData {
  type: "SessionData";
  sessionId: string;
  // base64 encoded data
  data: string;
}

export interface ISession {
  id: string;
  hostname: string;
  state: SessionState;
  port: number;
}

export type SessionState = "active" | "inactive";

export interface ISessionCloseRequest {
  type: "SessionClose";
  id: string;
}

export interface ISessionConnectRequest {
  type: "SessionConnect";
  id: string;
}

export interface ISessionDisconnectRequest {
  type: "SessionDisconnect";
  id: string;
}

export interface ISessionCommandRequest {
  type: "SessionCommand";
  id: string;
  command: string;
}
