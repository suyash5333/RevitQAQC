# RevitQAQC

A professional BIM Quality Assurance (QA/QC) add-in for Autodesk Revit, built using **C#**, **.NET 8**, **WPF**, and the **Autodesk Revit API**.

RevitQAQC automates model quality checks by identifying common BIM issues, calculating a model health score, and generating detailed PDF and JSON reports through a modern desktop interface.

---

## Features

- Automated QA/QC model validation
- Missing Mark Parameter Check
- Missing Comments Check
- Duplicate Mark Detection
- Wrong Level Assignment Check
- Model Standards Validation
- Element Count Analysis
- Model Health Score Calculation
- Interactive WPF Dashboard
- Live Search Filter
- Pass / Fail Status Filter
- PDF Report Export
- JSON Report Export
- Robust Error Handling

## Technology Stack

| Category | Technology |
|----------|------------|
| Language | C# |
| Framework | .NET 8 |
| BIM API | Autodesk Revit 2026 API |
| UI | WPF |
| Architecture | MVVM |
| Reporting | PDFsharp, Newtonsoft.Json |
| IDE | Visual Studio 2022 |

## Project Architecture

RevitQAQC
│
├── RevitQAQC.Addin          → Revit external command & ribbon integration
├── RevitQAQC.Engine         → QA engine, business logic, reports
├── RevitQAQC.Interfaces     → Interfaces for QA checks
├── RevitQAQC.Shared         → Shared models and data objects
├── RevitQAQC.WPF            → Dashboard UI (MVVM)
└── RevitQAQC.Tests          → Unit tests (future expansion)

## Workflow

Revit Model
      │
      ▼
Run QAQC Add-in
      │
      ▼
QA Engine
      │
      ▼
Execute QA Checks
      │
      ▼
Generate Results
      │
      ├──► Calculate Health Score
      │
      ├──► Generate PDF Report
      │
      ├──► Generate JSON Report
      │
      ▼
Display Results in WPF Dashboard


## QA Checks

The current version of RevitQAQC performs the following automated quality checks:

| Check | Description |
|-------|-------------|
| Missing Mark Parameter | Detects elements with empty or missing Mark values |
| Missing Comments | Detects elements without Comments |
| Duplicate Mark Detection | Finds duplicate Mark values in the model |
| Wrong Level Assignment | Identifies elements placed on incorrect levels |
| Model Standards Check | Verifies required project standards |
| Element Count Analysis | Counts model elements for health score calculation |

## Screenshots

### Dashboard

> *(Add dashboard screenshot here)*

### PDF Report

<img width="1031" height="790" alt="image" src="https://github.com/user-attachments/assets/ab6bd3f5-33d0-493c-8cb8-bf161cacbf71" />


### JSON Report

> *(Add generated JSON report screenshot here)*
