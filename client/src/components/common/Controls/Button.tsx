import type { IconProps } from "@tabler/icons-react";
import React from "react";
import { classNames } from "../../../utils/class-names";

interface IButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  icon?: React.FC<IconProps>;
  iconProps?: IconProps;
}

export const Button: React.FC<IButtonProps> = (props) => {
  const {
    className: overrideClassName,
    children,
    icon: Icon,
    iconProps,
    ...rest
  } = props;

  const iconContent = React.useMemo(() => {
    if (!Icon) {
      return;
    }

    const iconClassNames = classNames(
      iconProps?.className,
      props.disabled ? "!text-stone-500" : undefined,
    );

    return (
      <Icon stroke={4} size={14} {...iconProps} className={iconClassNames} />
    );
  }, [Icon, iconProps, props.disabled]);

  const classes = classNames(
    "rounded block bg-stone-800 p-2 outline outline-stone-600 outline-1 -outline-offset-1 hover:bg-stone-700 disabled:cursor-not-allowed disabled:hover:bg-stone-800 group",
    overrideClassName,
  );

  return (
    <button className={classes} {...rest}>
      {children}
      {iconContent}
    </button>
  );
};
