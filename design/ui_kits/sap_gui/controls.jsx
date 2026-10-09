/* global React */
const { useState } = React;

// =============================================================
// controls.jsx — primitives
// =============================================================

const ico = (name, size = 16) => (
  <svg width={size} height={size} aria-hidden="true">
    <use href={"../../assets/icons.svg#" + name} />
  </svg>
);

function Button({ children, icon, variant = "default", onClick, disabled, pressed, style }) {
  const cls = [
    "sap-btn",
    variant === "toolbar" ? "toolbar" : "",
    icon && !children ? "icon-only" : "",
    pressed ? "is-pressed" : "",
    disabled ? "is-disabled" : "",
  ].filter(Boolean).join(" ");
  return (
    <button className={cls} onClick={disabled ? undefined : onClick} disabled={!!disabled} style={style}>
      {icon && ico(icon)}
      {children}
    </button>
  );
}

function Input({ value, onChange, error, disabled, width, type = "text", placeholder, style }) {
  const cls = ["sap-input", error ? "is-error" : "", disabled ? "is-disabled" : ""].filter(Boolean).join(" ");
  return (
    <input
      type={type}
      className={cls}
      value={value ?? ""}
      onChange={onChange}
      placeholder={placeholder}
      disabled={!!disabled}
      style={{ width, ...style }}
    />
  );
}

function Select({ value, onChange, options = [], width, disabled }) {
  return (
    <span className="sap-select" style={{ width }}>
      <select
        className="sap-input"
        value={value}
        onChange={onChange}
        disabled={!!disabled}
        style={{ width: "100%", appearance: "none", paddingRight: 18 }}
      >
        {options.map((o) => (
          <option key={o.value ?? o} value={o.value ?? o}>{o.label ?? o}</option>
        ))}
      </select>
      <span className="arrow" style={{ pointerEvents: "none" }}>▼</span>
    </span>
  );
}

function Checkbox({ checked, onChange, label, disabled }) {
  const cls = ["sap-check", checked ? "is-checked" : "", disabled ? "is-disabled" : ""].filter(Boolean).join(" ");
  return (
    <label className={cls} onClick={(e) => { if (!disabled) { e.preventDefault(); onChange?.(!checked); } }}>
      <span className="box"></span>{label}
    </label>
  );
}

function Radio({ checked, onChange, label, disabled, name }) {
  const cls = ["sap-radio", checked ? "is-checked" : "", disabled ? "is-disabled" : ""].filter(Boolean).join(" ");
  return (
    <label className={cls} onClick={(e) => { if (!disabled) { e.preventDefault(); onChange?.(true); } }}>
      <span className="box"></span>{label}
    </label>
  );
}

function Tag({ tone = "default", children, onClose }) {
  const cls = "sap-tag" + (tone !== "default" ? " " + tone : "") + (onClose ? " closable" : "");
  return (
    <span className={cls}>
      {children}
      {onClose && <span className="x" onClick={onClose}>×</span>}
    </span>
  );
}

function Switch({ on, onChange, disabled }) {
  const cls = "sap-switch" + (on ? " is-on" : "") + (disabled ? " is-disabled" : "");
  return <span className={cls} onClick={() => !disabled && onChange?.(!on)} role="switch" aria-checked={!!on}></span>;
}

function Fieldset({ legend, children, style }) {
  return (
    <div className="sap-fieldset" style={style}>
      {legend && <span className="legend">{legend}</span>}
      {children}
    </div>
  );
}

window.SAP = window.SAP || {};
Object.assign(window.SAP, { Button, Input, Select, Checkbox, Radio, Tag, Switch, Fieldset, ico });
