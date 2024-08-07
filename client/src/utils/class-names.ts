/**
 * Simple utility function to concatenate class names together.
 */
export const classNames = (...classes: (string | undefined)[]): string => {
  return classes.filter(Boolean).join(" ");
};
