import { Console } from "./Console/Console";
import type { IconProps } from "@tabler/icons-react";
import { IconTerminal2, IconUserScan } from "@tabler/icons-react";
import { Profiles } from "./Profiles/Profiles";

interface ISectionConfig {
  label: string;
  component: React.FC;
  icon: React.FC<IconProps>;
}

export type Section = "console" | "profiles";

export const sections: { [K in Section]: ISectionConfig } = {
  profiles: {
    label: "Profiles",
    component: Profiles,
    icon: IconUserScan,
  },
  console: {
    label: "Console",
    component: Console,
    icon: IconTerminal2,
  },
};

export const isValidSection = (section: unknown): section is Section =>
  typeof section === "string" && section in sections;
