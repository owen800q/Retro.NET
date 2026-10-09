/* global React */
// =============================================================
// DataTable.jsx — beveled table with row selection
// =============================================================

function DataTable({ columns, rows, selectedIds, hoveredId, onSelect, onHover, getId }) {
  const id = getId || ((r, i) => r.id ?? i);
  return (
    <table className="sap-table">
      <thead>
        <tr>
          <th style={{ width: 22 }}></th>
          {columns.map((c) => (
            <th key={c.key} style={{ textAlign: c.align || "left", width: c.width }}>{c.label}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {rows.map((r, i) => {
          const rid = id(r, i);
          const sel = selectedIds?.has(rid);
          const hov = hoveredId === rid;
          const cls = [sel ? "is-selected" : "", hov && !sel ? "is-hovered" : ""].filter(Boolean).join(" ");
          return (
            <tr
              key={rid}
              className={cls}
              onMouseEnter={() => onHover?.(rid)}
              onMouseLeave={() => onHover?.(null)}
              onClick={() => onSelect?.(rid)}
              style={{ cursor: "default" }}
            >
              <td style={{ textAlign: "center" }}>
                <input type="checkbox" readOnly checked={!!sel} style={{ margin: 0 }} />
              </td>
              {columns.map((c) => (
                <td key={c.key} style={{ textAlign: c.align || "left", fontVariantNumeric: c.numeric ? "tabular-nums" : "normal" }}>
                  {c.render ? c.render(r) : r[c.key]}
                </td>
              ))}
            </tr>
          );
        })}
      </tbody>
    </table>
  );
}

Object.assign(window.SAP, { DataTable });
