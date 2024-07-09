import React from "react";

/**
 * Custom hook to get/set query params from the URL
 */
export const useQueryParams = (): [
  URLSearchParams,
  (fn: (params: URLSearchParams) => void) => void,
] => {
  const [searchParams, _setQueryParams] = React.useState(
    new URLSearchParams(window.location.search),
  );

  const setQueryParams = React.useCallback(
    (mutateFn: (params: URLSearchParams) => void) => {
      _setQueryParams((prev) => {
        const newSearchParams = new URLSearchParams(prev.toString());

        mutateFn(newSearchParams);
        const queryString = newSearchParams.toString();

        if (newSearchParams.toString() === prev.toString()) {
          return prev;
        }

        window.history.pushState(
          {},
          "",
          `${window.location.pathname}${queryString ? `?${queryString}` : ""}`,
        );

        return newSearchParams;
      });
    },
    [],
  );

  React.useEffect(() => {
    const onPopState = () => {
      _setQueryParams(new URLSearchParams(window.location.search));
    };

    window.addEventListener("popstate", onPopState);

    return () => {
      window.removeEventListener("popstate", onPopState);
    };
  }, []);

  return [searchParams, setQueryParams];
};
