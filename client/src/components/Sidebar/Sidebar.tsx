import React from "react";
import type { Section } from "../Sections/sections";
import { sections } from "../Sections/sections";
import { lightBorderClasses } from "../../constants/styles";
import { useAppContext } from "../../context/AppProvider";

export const Sidebar = () => {
  const currentSection = useAppContext((c) => c.section);
  const setSection = useAppContext((c) => c.setSection);

  return (
    <div
      className={`flex flex-col bg-stone-900 border-r ${lightBorderClasses} `}
    >
      {Object.entries(sections).map(([key, section]) => (
        <div
          key={key}
          className={`flex cursor-pointer items-center gap-2 p-3 ${key === currentSection ? "bg-stone-600" : "hover:bg-stone-800"}`}
          onClick={() => setSection(key as Section)}
          title={section.label}
        >
          <section.icon />
        </div>
      ))}
    </div>
  );
};
