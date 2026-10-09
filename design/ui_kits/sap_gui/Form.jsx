/* global React */
// =============================================================
// Form.jsx — label-on-left form rows
// =============================================================

function FormRow({ label, required, error, help, children }) {
  return (
    <>
      <label style={{ textAlign: "right", paddingTop: 3, color: error ? "var(--status-error)" : "inherit" }}>
        {label}
        {required && <span style={{ color: "var(--status-error)" }}> *</span>}
        {label ? ":" : ""}
      </label>
      <div>
        {children}
        {(help || error) && (
          <div style={{ fontSize: 10, color: error ? "var(--status-error)" : "var(--fg-muted)", marginTop: 1 }}>
            {error || help}
          </div>
        )}
      </div>
    </>
  );
}

function Form({ children, columns = "80px 1fr", gap = "3px 8px", style }) {
  return (
    <div style={{ display: "grid", gridTemplateColumns: columns, gap, alignItems: "start", ...style }}>
      {children}
    </div>
  );
}

Object.assign(window.SAP, { Form, FormRow });
