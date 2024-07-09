import React from "react";

interface ITabProps extends React.PropsWithChildren {
  active: boolean;
  onClick: () => void;
}

export const Tab: React.FC<ITabProps> = ({ children, active, onClick }) => {
  return (
    <button
      className={`px-2 outline-none ${active ? "bg-stone-700" : "hover:bg-stone-800"} h-8 flex items-center border-r border-r-stone-950`}
      onClick={onClick}
    >
      {children}
    </button>
  );
};
