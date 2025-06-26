# Phyros.OrganizationalUnits

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Build Status](https://github.com/toadicusrex/Phyros.OrganizationalUnits/actions/workflows/ci.yml/badge.svg)](https://github.com/toadicusrex/Phyros.OrganizationalUnits/actions)
[![NuGet](https://img.shields.io/nuget/v/Phyros.OrganizationalUnits.svg?logo=nuget)](https://www.nuget.org/packages/Phyros.OrganizationalUnits)

A modern, configurable C# library for representing and manipulating hierarchical organizational units (OUs) in a generic, privacy-friendly way.

## Features
- Parse, validate, and serialize organizational unit strings
- Configurable base OU (default: `Core`)
- Clean, testable API with XML documentation
- Modern build and CI/CD with Nuke and GitHub Actions
- Symbol package generation for debugging
- Generalized, non-location-specific test data

## Usage
```csharp
using Phyros.OrganizationalUnits;

// Parse an OU string
// override base OU if needed:
OrganizationalUnitConfig.BaseOrganizationalUnit = "MyRoot";
var ou = OrganizationalUnit.Parse("world.country.region.city");

// Convert to string or URL-friendly string
string ouString = ou.ToString();
string urlString = ou.ToUrlString();

// Get fully qualified nodes
string[] nodes = ou.GetFullyQualifiedNodes();
```

## Build & Test
- **Build locally:**
  - `Ctrl+Shift+B` in VS Code (runs Nuke build)
  - Or: `./build.cmd` (Windows) or `./build.sh` (Linux/macOS)
- **Pack NuGet:**
  - `./build.cmd Pack`
- **Publish (CI only):**
  - Handled by GitHub Actions on push/PR

## CI/CD
- Automated with GitHub Actions
- Versioning via GitVersion (see `GitVersion.yml`)
- NuGet packages published to [GitHub Packages](https://github.com/toadicusrex?tab=packages)
- [MIT License](LICENSE)

## Configuration
- **Base OU:** Set at runtime via `OrganizationalUnitConfig.BaseOrganizationalUnit`
- **NuGet API Key:** Add `PACKAGES_API_KEY` as a repository secret for publishing

## Contributing
PRs and issues welcome! Please use generic, non-sensitive test data.

---

© 2025 toadicusrex. MIT License.