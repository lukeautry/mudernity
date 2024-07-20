import React from "react";
import { sections } from "./components/Sections/sections";
import { Sidebar } from "./components/Sidebar/Sidebar";
import { useAppContext } from "./context/AppProvider";
import { useDynamicTitle } from "./hooks/use-dynamic-title";
import { useDynamicFavicon } from "./hooks/use-dynamic-favicon";

export const App = () => {
  const section = useAppContext((c) => c.section);
  const Component = sections[section].component;

  useDynamicTitle();
  useDynamicFavicon();

  return (
    <div className="flex h-screen font-mono text-xs text-white">
      <Sidebar />
      <div className="flex-1 bg-stone-900">
        <Component />
      </div>
    </div>
  );
};
