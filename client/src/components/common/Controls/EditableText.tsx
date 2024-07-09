import React from "react";
import { Input } from "./Input";
import { IconPencil } from "@tabler/icons-react";

interface IEditableTextProps {
  value: string;
  onChange: (value: string) => void;
}

export const EditableText: React.FC<IEditableTextProps> = ({
  value,
  onChange,
}) => {
  const [isEditing, setIsEditing] = React.useState(false);
  const [localValue, setLocalValue] = React.useState(value);
  const inputRef = React.useRef<HTMLInputElement>(null);

  React.useEffect(() => {
    setLocalValue(value);
  }, [value]);

  const handleBlur = React.useCallback(() => {
    setIsEditing(false);

    if (localValue === value) {
      return;
    }

    onChange(localValue);
  }, [localValue, onChange, value]);

  React.useEffect(() => {
    if (isEditing) {
      inputRef.current?.select();
    }
  }, [isEditing]);

  const handleClick = React.useCallback(() => {
    setIsEditing(true);
  }, []);

  return (
    <div onClick={handleClick} className="group">
      {isEditing ? (
        <Input
          type="text"
          inputRef={inputRef}
          value={localValue}
          onChange={(e) => setLocalValue(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              handleBlur();
            }
          }}
          onBlur={handleBlur}
          autoFocus
          fullWidth
        />
      ) : (
        <div className="p-2 hover:underline cursor-pointer flex gap-2 items-center">
          <div>{value}</div>
          <IconPencil size=".75rem" className="hidden group-hover:inline" />
        </div>
      )}
    </div>
  );
};
