import React from "react";

interface IInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  inputRef?: React.RefObject<HTMLInputElement>;
  fullWidth?: boolean;
}

export const Input: React.FC<IInputProps> = (props) => {
  const { className: overrideClassName, fullWidth, inputRef, ...rest } = props;

  return (
    <input
      className={`block rounded bg-stone-800 p-2 outline outline-stone-600 outline-1 -outline-offset-1 focus:outline-blue-500 focus:placeholder-white ${overrideClassName || ""} ${fullWidth ? "w-full" : ""}`}
      ref={inputRef}
      {...rest}
    />
  );
};
