/* global React */
const { useState: _useStateW } = React;

// =============================================================
// Window.jsx — title bar + menu bar + status bar shell
// =============================================================

function TitleBar({ title, icon = "document" }) {
  return (
    <div className="sap-titlebar">
      {window.SAP.ico(icon, 14)}
      <span>{title}</span>
      <span className="grow"></span>
      <span className="sap-titlebar-btn" title="Minimize">_</span>
      <span className="sap-titlebar-btn" title="Maximize">▢</span>
      <span className="sap-titlebar-btn" title="Close">×</span>
    </div>
  );
}

function MenuBar({ items = [] }) {
  return (
    <div className="sap-menubar">
      {items.map((m) => (
        <span key={m.label} dangerouslySetInnerHTML={{ __html: m.label }}></span>
      ))}
    </div>
  );
}

function StatusBar({ left, right }) {
  return (
    <div className="sap-statusbar">
      <span>{left}</span>
      <span style={{ marginLeft: "auto", fontFamily: "var(--font-mono)" }}>{right}</span>
    </div>
  );
}

function Window({ title, menu, toolbar, statusLeft, statusRight, children }) {
  return (
    <div className="sap-window" style={{ height: "100%", display: "flex", flexDirection: "column" }}>
      <TitleBar title={title} />
      <MenuBar items={menu} />
      {toolbar}
      <div style={{ flex: 1, overflow: "auto", padding: 6, background: "var(--surface)" }}>
        {children}
      </div>
      <StatusBar left={statusLeft} right={statusRight} />
    </div>
  );
}

Object.assign(window.SAP, { Window, TitleBar, MenuBar, StatusBar });
