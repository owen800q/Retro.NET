# SAP GUI — UI Kit

A pixel-faithful recreation of an SAP-style desktop transaction screen. The kit demonstrates the full chrome stack and a transactional content area:

- `index.html` — the working prototype. Open this.
- `Window.jsx` — outer window shell (title bar, menu bar, toolbar, status bar)
- `Toolbar.jsx` — the SAP application command toolbar
- `Form.jsx` — fieldsets and form rows with labels + inputs
- `DataTable.jsx` — beveled table with selection / hover / status tags
- `TabStrip.jsx` — angled-cut tab strip
- `Dialog.jsx` — modal dialog overlay (OK / Cancel)
- `controls.jsx` — small primitives: Button, Input, Select, Checkbox, Radio, Tag, Switch

Each component is **cosmetic** — it imitates the look and minimal interaction (open dialog, switch tabs, select rows, edit text) but does not implement real persistence or business logic.

## Screens covered (one screen, multiple states)

The prototype lands on a **Shipment Maintenance** transaction (SAP `VL01N`-style), with three tabs:
1. **Header** — form view with carrier, service, payment, dates
2. **Items** — selectable data table with status tags
3. **Documents** — empty state placeholder

Click around — toolbar buttons trigger toasts/dialogs, table rows toggle selection, the **Delete** toolbar action opens a confirm dialog.
