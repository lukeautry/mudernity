import React from "react";

export const TabList: React.FC<React.PropsWithChildren> = ({ children }) => {
  return <div className="flex relative">{children}</div>;
};
