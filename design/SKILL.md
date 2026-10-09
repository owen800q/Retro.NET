---
name: retro-sap-gui-design
description: Use this skill to generate well-branded interfaces and assets for the Retro SAP GUI design system, either for production or throwaway prototypes/mocks/etc. Contains essential design guidelines, colors, type, fonts, assets, and UI kit components for prototyping SAP-style desktop transaction screens, beveled Windows-9x-flavored enterprise tools, and "ironic enterprise" software aesthetics.
user-invocable: true
---

Read the README.md file within this skill, and explore the other available files (`colors_and_type.css`, `components.css`, `assets/icons.svg`, `ui_kits/sap_gui/`).

If creating visual artifacts (slides, mocks, throwaway prototypes, etc), copy assets out and create static HTML files for the user to view. Always link `colors_and_type.css` and `components.css` for the bevel system, and `assets/icons.svg` for iconography. Never invent new SVG icons — extend `icons.svg` instead.

If working on production code, you can copy assets and read the rules in README.md to become an expert in designing with this aesthetic. Pay particular attention to: 0px corner radii, 1px two-tone bevels (no drop shadows), MS-Sans-Serif type stack, the SAP amber (#F0AB00) used for selection/highlight, and the dense 4px-grid layout with label-on-left forms.

If the user invokes this skill without any other guidance, ask them what they want to build or design (a transaction screen, a settings dialog, a fake login, an "enterprise terminal" promo mock, etc), ask a few questions about scope and tone, and act as an expert designer who outputs HTML artifacts _or_ production code, depending on the need.
