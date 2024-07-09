import React from "react";
import { sections } from "./components/Sections/sections";
import { Sidebar } from "./components/Sidebar/Sidebar";
import { useAppContext } from "./context/AppProvider";

export const App = () => {
  const section = useAppContext((c) => c.section);

  const Component = sections[section].component;

  return (
    <div className="flex h-screen font-mono text-xs text-white">
      <Sidebar />
      <div className="flex-1 bg-stone-900">
        <Component />
      </div>
    </div>
  );
};
