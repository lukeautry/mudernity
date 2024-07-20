import React from "react";
import type { IFooterProps, ITableColumns } from "../../common/Table/Table";
import { Table } from "../../common/Table/Table";
import { EditableText } from "../../common/Controls/EditableText";
import { useAgentContext } from "../../../context/AgentProvider";
import { Input } from "../../common/Controls/Input";
import { useImmer } from "use-immer";
import { Button } from "../../common/Controls/Button";
import { IconCheck, IconTerminal, IconTrash } from "@tabler/icons-react";
import type { IProfileCreationRequest } from "../../../agent/agent-types";
import { produce } from "immer";
import { useAppContext } from "../../../context/AppProvider";

interface IProfilesProps {}

interface IProfile {
  id: string;
  name: string;
  hostname: string;
  port: number;
}

type ProfileColumns = "name" | "hostname" | "port" | "actions";

interface IProfileRequest {
  name: string;
  hostname: string;
  port: string;
}

const defaultRequest = (): IProfileRequest => ({
  name: "",
  hostname: "",
  port: "",
});

export const Profiles: React.FC<IProfilesProps> = () => {
  const profiles = useAgentContext((c) => c.profiles);
  const service = useAgentContext((c) => c.service);
  const setSection = useAppContext((c) => c.setSection);

  const [request, setRequest] = useImmer<IProfileRequest>(defaultRequest());

  const validatedRequest = React.useMemo(():
    | IProfileCreationRequest
    | undefined => {
    const portNum = parseInt(request.port);
    if (isNaN(portNum)) {
      return;
    }

    return {
      type: "ProfileCreation",
      name: request.name,
      hostname: request.hostname,
      port: parseInt(request.port),
    };
  }, [request]);

  const onProfileCreate = React.useCallback(() => {
    if (!validatedRequest) {
      return;
    }

    service.send(validatedRequest);
    setRequest(() => defaultRequest());
  }, [service, setRequest, validatedRequest]);

  const onKeyDown: React.KeyboardEventHandler<HTMLInputElement> =
    React.useCallback(
      (e) => {
        if (e.key === "Enter") {
          onProfileCreate();
        }
      },
      [onProfileCreate],
    );

  const columns = React.useMemo((): ITableColumns<ProfileColumns, IProfile> => {
    return {
      name: {
        label: "Name",
        render: (profile) => (
          <div className="py-2 pl-2">
            <EditableText
              value={profile.name}
              onChange={(name) => {
                if (!name) {
                  return;
                }

                service.send({
                  type: "ProfileUpdate",
                  profile: produce(profile, (draft) => {
                    draft.name = name;
                  }),
                });
              }}
            />
          </div>
        ),
        style: { minWidth: "200px" },
      },
      hostname: {
        label: "Host",
        render: (profile) => {
          return (
            <div className="py-1 pl-2">
              <EditableText
                value={profile.hostname}
                onChange={(value) => {
                  if (!value) {
                    return;
                  }

                  service.send({
                    type: "ProfileUpdate",
                    profile: produce(profile, (draft) => {
                      draft.hostname = value;
                    }),
                  });
                }}
              />
            </div>
          );
        },
        style: { minWidth: "140px" },
      },
      port: {
        label: "Port",
        render: (profile) => {
          return (
            <div className="py-1 pl-2">
              <EditableText
                value={profile.port.toString()}
                onChange={(value) => {
                  const port = parseInt(value);
                  if (isNaN(port)) {
                    return;
                  }

                  service.send({
                    type: "ProfileUpdate",
                    profile: produce(profile, (draft) => {
                      draft.port = port;
                    }),
                  });
                }}
              />
            </div>
          );
        },
        style: { minWidth: "80px" },
      },
      actions: {
        label: "",
        render: (profile) => {
          return (
            <div className="flex px-2 gap-1">
              <Button
                icon={IconTrash}
                title="Delete Profile"
                onClick={() => {
                  service.send({
                    type: "ProfileDeletion",
                    id: profile.id,
                  });
                }}
              />
              <Button
                icon={IconTerminal}
                title="Connect to Profile"
                className="outline-cyan-400 hover:bg-cyan-500"
                iconProps={{
                  className: "text-white",
                }}
                onClick={() => {
                  service.send({
                    type: "ProfileConnection",
                    id: profile.id,
                  });

                  setSection("console");
                }}
              />
            </div>
          );
        },
      },
    };
  }, [service, setSection]);

  const footerProps: IFooterProps<ProfileColumns> = React.useMemo(() => {
    return {
      render: {
        name: () => (
          <div className="pl-2">
            <Input
              placeholder="e.g. My Profile"
              value={request.name}
              onChange={(e) =>
                setRequest((draft) => {
                  draft.name = e.target.value;
                })
              }
              fullWidth
              onKeyDown={onKeyDown}
            />
          </div>
        ),
        hostname: () => (
          <div className="pl-2">
            <Input
              placeholder="e.g. localhost"
              value={request.hostname}
              onChange={(e) =>
                setRequest((draft) => {
                  draft.hostname = e.target.value;
                })
              }
              onKeyDown={onKeyDown}
            />
          </div>
        ),
        port: () => (
          <div className="pl-2">
            <Input
              placeholder="e.g. 4000"
              value={request.port}
              onChange={(e) =>
                setRequest((draft) => {
                  draft.port = e.target.value;
                })
              }
              onKeyDown={onKeyDown}
            />
          </div>
        ),
        actions: () => (
          <div className="px-2">
            <Button
              disabled={!validatedRequest}
              icon={IconCheck}
              iconProps={{
                className: "text-green-600 group-hover:text-green-400",
              }}
              onClick={onProfileCreate}
            />
          </div>
        ),
      },
      cellClassName: "py-2",
    };
  }, [
    onKeyDown,
    onProfileCreate,
    request.hostname,
    request.name,
    request.port,
    setRequest,
    validatedRequest,
  ]);

  return (
    <div className="flex flex-col h-full p-4 text-white">
      <div className="text-2xl pb-4">Profiles</div>
      <div className="flex-1">
        <Table
          data={profiles}
          idKey={(profile) => profile.id}
          columns={columns}
          footer={footerProps}
        />
      </div>
    </div>
  );
};
