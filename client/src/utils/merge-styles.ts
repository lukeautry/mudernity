export const mergeStyles = (
  ...styles: React.CSSProperties[]
): React.CSSProperties => {
  return styles.reduce((acc, style) => ({ ...acc, ...style }), {});
};
