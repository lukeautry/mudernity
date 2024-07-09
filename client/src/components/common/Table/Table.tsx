import React from "react";
import { classNames } from "../../../utils/class-names";

interface ITableProps<K extends string, T> {
  data: ReadonlyArray<T>;
  columns: ITableColumns<K, T>;
  idKey: (item: T, index: number) => string | number;
  footer?: IFooterProps<K>;
}

export interface IFooterProps<K extends string> {
  render: RenderFooterRowFn<K>;
  rowStyle?: React.CSSProperties;
  cellStyle?: React.CSSProperties;
  rowClassName?: string;
  cellClassName?: string;
}

export type RenderFooterRowFn<K extends string> = {
  [P in K]: () => React.ReactNode;
};

export type ITableColumns<K extends string, T> = { [P in K]: ITableColumn<T> };

type ITableColumn<T> = {
  label: string;
  render: (item: T) => React.ReactNode;
  style?: React.CSSProperties;
  className?: string;
};

export const Table = <K extends string, T>(props: ITableProps<K, T>) => {
  const columns = Object.entries(props.columns) as [K, ITableColumn<T>][];
  const { footer } = props;

  return (
    <div className=" bg-stone-900 rounded-lg shadow-md overflow-hidden inline-block">
      <table className="text-left border-collapse">
        <thead className="text-gray-300">
          <tr className="font-bold bg-gray-950 text-white">
            {columns.map(([key, column]) => {
              const className = classNames(column.className, "p-3");

              return (
                <th key={key} className={className} style={column.style}>
                  {column.label}
                </th>
              );
            })}
          </tr>
        </thead>
        <tbody>
          {props.data.map((item, index) => {
            const key = props.idKey(item, index);

            return (
              <tr
                key={key}
                className={`${index % 2 === 0 ? "bg-stone-700" : "bg-stone-800"}`}
              >
                {columns.map(([, column], index) => {
                  return (
                    <td key={index} className="align-middle">
                      {column.render(item)}
                    </td>
                  );
                })}
              </tr>
            );
          })}
          {footer && (
            <tr
              className={classNames(
                "bg-stone-800 border-t border-dashed border-stone-600",
                footer.rowClassName,
              )}
              style={footer.rowStyle}
            >
              {columns.map(([key]) => {
                const renderFn = footer.render[key as K];

                return (
                  <td
                    key={key}
                    className={classNames("align-middle", footer.cellClassName)}
                    style={footer.cellStyle}
                  >
                    {renderFn()}
                  </td>
                );
              })}
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};
