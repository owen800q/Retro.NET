/* global React */
// =============================================================
// Dialog.jsx — modal overlay with title + body + footer buttons
// =============================================================

function Dialog({ open, title, icon = "warning", children, buttons = [], onClose }) {
  if (!open) return null;
  const { Button, ico } = window.SAP;
  return (
    <div
      style={{
        position: "absolute", inset: 0, background: "rgba(0,0,0,.15)",
        display: "flex", alignItems: "center", justifyContent: "center", zIndex: 50,
      }}
      onClick={onClose}
    >
      <div className="sap-dialog" onClick={(e) => e.stopPropagation()} style={{ minWidth: 320 }}>
        <div className="sap-titlebar">
          <span>{title}</span>
          <span className="grow"></span>
          <span className="sap-titlebar-btn" onClick={onClose}>×</span>
        </div>
        <div className="body" style={{ display: "flex", gap: 8, alignItems: "flex-start" }}>
          {icon && ico(icon, 32)}
          <div style={{ flex: 1 }}>{children}</div>
        </div>
        <div className="footer">
          {buttons.map((b, i) => (
            <Button key={i} onClick={b.onClick}>{b.label}</Button>
          ))}
        </div>
      </div>
    </div>
  );
}

Object.assign(window.SAP, { Dialog });
