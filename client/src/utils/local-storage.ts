export interface ILocalStorage {
  getItem(key: string): string | null;
  setItem(key: string, value: string): void;
  clear(): void;
}

export class LocalStorage implements ILocalStorage {
  getItem(key: string): string | null {
    return localStorage.getItem(key);
  }

  setItem(key: string, value: string): void {
    localStorage.setItem(key, value);
  }

  clear(): void {
    localStorage.clear();
  }
}
