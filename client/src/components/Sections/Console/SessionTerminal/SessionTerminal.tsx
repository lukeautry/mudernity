import React from "react";
import { FitAddon } from "@xterm/addon-fit";
import { Terminal } from "@xterm/xterm";
import { Input } from "../../../common/Controls/Input";
import { Button } from "../../../common/Controls/Button";
import { lightBorderClasses } from "../../../../constants/styles";
import { useAgentContext } from "../../../../context/AgentProvider";
import "./SessionTerminal.scss";

interface ITerminalSessionProps {}

export const SessionTerminal: React.FC<ITerminalSessionProps> = () => {
  const sessionCache = useAgentContext((c) => c.cache);
  const onSessionData = useAgentContext((c) => c.onSessionData);
  const session = useAgentContext((c) => c.session);
  const service = useAgentContext((c) => c.service);
  const containerRef = React.useRef<HTMLDivElement>(null);
  const inputRef = React.useRef<HTMLInputElement>(null);
  const [command, setCommand] = React.useState("");

  React.useEffect(() => {
    if (!containerRef.current || !session?.id) {
      return;
    }

    const term = new Terminal({
      disableStdin: true,
    });
    const fitAddon = new FitAddon();
    term.loadAddon(fitAddon);

    term.open(containerRef.current);

    fitAddon.fit();

    const dataDispose = onSessionData(session.id, (arr) => {
      term.write(arr);
    });

    return () => {
      term.dispose();
      fitAddon.dispose();
      dataDispose();
    };
  }, [onSessionData, session?.id, sessionCache]);

  const onSend = React.useCallback(() => {
    if (!session?.id || !command) {
      return;
    }

    service.send({
      type: "SessionCommand",
      id: session.id,
      command,
    });

    // select all input text
    inputRef.current?.select();
  }, [command, service, session?.id]);

  const onKeyDown: React.KeyboardEventHandler<HTMLInputElement> =
    React.useCallback(
      (e) => {
        if (e.key === "Enter") {
          onSend();
        }
      },
      [onSend],
    );

  const toggleConnectionButton = React.useMemo(() => {
    if (!session) {
      return;
    }

    if (session.state === "active") {
      return (
        <Button
          onClick={() => {
            service.send({
              type: "SessionDisconnect",
              id: session.id,
            });
          }}
        >
          Disconnect
        </Button>
      );
    }

    return (
      <Button
        onClick={() => {
          service.send({
            type: "SessionConnect",
            id: session.id,
          });
        }}
      >
        Connect
      </Button>
    );
  }, [service, session]);

  return (
    <div className="session-terminal flex flex-1 flex-col">
      <div className="flex-1 bg-black">
        <div className="h-full w-full" ref={containerRef} />
      </div>
      <div className={`flex gap-2 border-t p-2 ${lightBorderClasses}`}>
        <Input
          inputRef={inputRef}
          className="flex-1"
          type="text"
          placeholder="Message"
          value={command}
          onChange={(e) => setCommand(e.target.value)}
          onKeyDown={onKeyDown}
        />
        {toggleConnectionButton}
      </div>
    </div>
  );
};
