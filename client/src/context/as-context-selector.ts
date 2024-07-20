import type { Context } from "use-context-selector";
import { useContextSelector } from "use-context-selector";

export const asContextSelector = <T>(context: Context<T>) => {
  return <Result>(selector: (context: T) => Result) => {
    return useContextSelector(context, selector);
  };
};
