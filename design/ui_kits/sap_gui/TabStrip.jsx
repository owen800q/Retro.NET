/* global React */
// =============================================================
// TabStrip.jsx
// =============================================================

function TabStrip({ tabs, active, onChange }) {
  return (
    <div className="sap-tabs">
      {tabs.map((t) => (
        <span
          key={t.id}
          className={"sap-tab" + (t.id === active ? " is-active" : "") + (t.disabled ? " is-disabled" : "")}
          onClick={() => !t.disabled && onChange?.(t.id)}
        >
          {t.label}
        </span>
      ))}
    </div>
  );
}

Object.assign(window.SAP, { TabStrip });
