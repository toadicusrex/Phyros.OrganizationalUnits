# Phyros.OrganizationalUnits

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Build Status](https://github.com/toadicusrex/Phyros.OrganizationalUnits/actions/workflows/ci.yml/badge.svg)](https://github.com/toadicusrex/Phyros.OrganizationalUnits/actions)
[![NuGet](https://img.shields.io/nuget/v/Phyros.OrganizationalUnits.svg?logo=nuget)](https://www.nuget.org/packages/Phyros.OrganizationalUnits)

A modern, configurable C# library for representing and manipulating hierarchical organizational structures as natural keys.  The idea is to not use some GUID or synthetic key that doesn't have meaning outside of whatever database you're using; instead, use a naturally unique key! 

For example, let's say you have an organization with different regions and facilities; you take an order from a specific facility, and you want to know where that order came from.  Instead of having a lookup with a "RegisterId" that isn't a meaningful value, use something like MyClient.WestRegion.MainStreetStore.LeftRegister; so you know which client it is, which region, which store, and which register. Want to see all of the orders from a specific store?  Just look for all orders with an organizational unit that starts with MyClient.WestRegion.MainStreetStore.  Or for orders taken in the West Region, just look for all orders with an organizational unit that starts with MyClient.WestRegion.  This allows you to easily query and filter orders based on their organizational context.

Targets both .NET 8 and .NET Standard 2.0.

## Features
- Parse, validate, and serialize hierarchical organizational unit strings
- Define natural keys for organizational structures
- Fully configurable base OU and delimiter character
- Instance-based or global configuration
- Clean, testable API with XML documentation
- Multi-targeting: .NET 8 and .NET Standard 2.0
- Symbol package generation for debugging
- Generalized test data with intuitive structure representations

## Usage

### Basic Usage
```csharp
using Phyros.OrganizationalUnits;

// Parse an OU string
var ou = OrganizationalUnit.Parse("world.country.region.city");

// Convert to string or URL-friendly string
string ouString = ou.ToString();          // "world.country.region.city"
string urlString = ou.ToUrlString();      // "world.country.region.city"

// Get fully qualified nodes
string[] nodes = ou.GetFullyQualifiedNodes();
// ["world.country.region.city", "world.country.region", "world.country", "world", ""]
```

### Using Custom Configuration
```csharp
// Global configuration
OrganizationalUnitConfig.SetDefault(baseOrganizationalUnit: "MyRoot", delimiter: '/');

// Parse with custom delimiter
var ou = OrganizationalUnit.Parse("world/country/region/city");
Console.WriteLine(ou.ToString());  // "world/country/region/city"

// Instance-specific configuration
var config = new OrganizationalUnitConfig { 
    BaseOrganizationalUnit = "MyCompany", 
    Delimiter = '-' 
};

var customOu = OrganizationalUnit.Parse("dept-team-unit", config);
Console.WriteLine(customOu.ToUrlString());  // When empty: "MyCompany", otherwise: "dept-team-unit" 
```

### Hierarchical Relationships
```csharp
var parent = OrganizationalUnit.Parse("world.country");
var child = OrganizationalUnit.Parse("world.country.region");

// Check relationships
bool isDescendant = child.IsDescendantOf(parent);   // true
bool isAncestor = parent.IsAncestorOf(child);       // true
bool isChild = child.IsChildOf(parent);             // true
bool isParent = parent.IsParentOf(child);           // true
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
- NuGet packages published to [GitHub Packages](https://github.com/toadicusrex?tab=packages) and [NuGet.org](https://www.nuget.org/packages/Phyros.OrganizationalUnits)
- [MIT License](LICENSE)

## Configuration
- **Global Settings:**
  - Base OU: Set at runtime via `OrganizationalUnitConfig.Default.BaseOrganizationalUnit` or `OrganizationalUnitConfig.SetDefault("MyRoot")`
  - Delimiter: Set at runtime via `OrganizationalUnitConfig.Default.Delimiter` or `OrganizationalUnitConfig.SetDefault(delimiter: '/')`
  
## Contributing
PRs and issues welcome! Please use generic test data with intuitive organizational structure representations.

---

© 2025 toadicusrex. MIT License.