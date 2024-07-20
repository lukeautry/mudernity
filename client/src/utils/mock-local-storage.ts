import { ILocalStorage } from "./local-storage";

export const mockLocalStorage = (): ILocalStorage => {
  const storage: { [key: string]: string } = {};

  return {
    getItem: (key: string) => {
      return storage[key] || null;
    },
    setItem: (key: string, value: string) => {
      storage[key] = value;
    },
    clear: () => {
      Object.keys(storage).forEach((key) => {
        delete storage[key];
      });
    },
  };
};
