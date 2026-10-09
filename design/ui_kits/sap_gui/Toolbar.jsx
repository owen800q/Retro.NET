/* global React */
// =============================================================
// Toolbar.jsx — the application command toolbar
// =============================================================

function Toolbar({ groups = [] }) {
  const { Button } = window.SAP;
  return (
    <div className="sap-toolbar">
      {groups.map((g, gi) => (
        <React.Fragment key={gi}>
          {g.map((b, bi) => (
            <Button
              key={bi}
              variant="toolbar"
              icon={b.icon}
              disabled={b.disabled}
              onClick={b.onClick}
              style={b.label ? { padding: "2px 6px" } : undefined}
            >
              {b.label}
            </Button>
          ))}
          {gi < groups.length - 1 && <span className="sep"></span>}
        </React.Fragment>
      ))}
    </div>
  );
}

Object.assign(window.SAP, { Toolbar });
