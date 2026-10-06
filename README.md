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

<img width="1408" height="938" alt="image" src="https://github.com/user-attachments/assets/18d4d73f-d220-4ee6-b620-d3ddf25348e8" />


### PDF Report

<img width="1031" height="790" alt="image" src="https://github.com/user-attachments/assets/ab6bd3f5-33d0-493c-8cb8-bf161cacbf71" />


### JSON Report

<img width="1920" height="1033" alt="image" src="https://github.com/user-attachments/assets/70babf5e-b6c1-4647-a807-dd57986168d3" />

## Getting Started

### Prerequisites

- Autodesk Revit 2026
- .NET 8
- Visual Studio 2022

### Installation

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Build the solution.
4. Copy the generated `.addin` file to the Revit Addins folder.
5. Launch Autodesk Revit 2026.
6. Open a Revit project and run **RevitQAQC** from the ribbon.

## Future Improvements

- Add additional BIM QA/QC checks
- Support custom user-defined QA rules
- Integrate Autodesk Construction Cloud (ACC)
- Integrate Autodesk Platform Services (APS)
- Add AI-assisted model quality recommendations
- Export reports to Microsoft Excel
- Add charts and analytics dashboard
- Improve report customization

  ## Author

**Suyash Thakar**

Civil Engineer | BIM Automation Developer

- LinkedIn: www.linkedin.com/in/suyash-thakar-18549a2a8

