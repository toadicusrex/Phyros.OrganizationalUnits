# Phyros.OrganizationalUnits

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Build Status](https://github.com/toadicusrex/Phyros.OrganizationalUnits/actions/workflows/ci.yml/badge.svg)](https://github.com/toadicusrex/Phyros.OrganizationalUnits/actions)
[![NuGet](https://img.shields.io/nuget/v/Phyros.OrganizationalUnits.svg?logo=nuget)](https://www.nuget.org/packages/Phyros.OrganizationalUnits)

A modern, configurable C# library for representing and manipulating hierarchical organizational structures as natural keys, as a replacement for synthetic/surrogate keys that have little meaning outside of a database. 

For example, let's say you have an organization with different regions and facilities; you take an order from a specific facility, and you want to know where that order came from.  Instead of having a lookup with a "RegisterId" that isn't a independently meaningful value, use something like myclient.westregion.mainstreetstore.customerservice.register; so you know which client it is, which region, which store, and which register. Want to see all of the orders from a specific store?  Just look for all orders with an organizational unit that starts with myclient.westregion.mainstreetstore.  Or for orders taken in the West Region, just look for all orders with an organizational unit that starts with myclient.westregion.  This allows you to easily query and filter orders based on their organizational context.

Targets both .NET 8 and .NET Standard 2.0.

## Features
- Parse, validate, and serialize hierarchical organizational unit strings
- Define natural keys for organizational structures
- Fully configurable base organizational unit alias and delimiter character
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

// Get fully qualified nodes - note the empty string as the first element
string[] nodes = ou.GetFullyQualifiedNodes();
// ["", "world", "world.country", "world.country.region", "world.country.region.city"]

// You can also implicitly convert between string and OrganizationalUnit
OrganizationalUnit implicitOu = "world.country";
string implicitString = implicitOu; // "world.country"
```

### Using Custom Configuration
```csharp
// Global configuration
OrganizationalUnitConfig.SetDefault(baseOrganizationalUnitAlias: "MyRoot", delimiter: '/');

// Parse with custom delimiter
var ou = OrganizationalUnit.Parse("world/country/region/city");
Console.WriteLine(ou.ToString());  // "world/country/region/city"

// Instance-specific configuration
var config = new OrganizationalUnitConfig { 
    BaseOrganizationalUnitAlias = "MyCompany", 
    Delimiter = '-' 
};

var customOu = OrganizationalUnit.Parse("dept-team-unit", config);
Console.WriteLine(customOu.ToUrlString());  // When empty: "MyCompany", otherwise: "dept-team-unit" 
```

### Working with Base Organizational Unit
```csharp
// Create a base organizational unit
var baseOu = new OrganizationalUnit();

// The base OU toString is empty string
Console.WriteLine(baseOu.ToString()); // ""

// The base OU toUrlString is the BaseOrganizationalUnitAlias
Console.WriteLine(baseOu.ToUrlString()); // "core"

// All of these are equivalent ways to create the base organizational unit
var baseOu1 = new OrganizationalUnit();
var baseOu2 = OrganizationalUnit.Parse("");
var baseOu3 = OrganizationalUnit.Parse("core");
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

### String Extensions
```csharp
// You can directly convert a string to an organizational unit
using Phyros.OrganizationalUnits;

string path = "world.country.region";
var ou = path.ToOrganizationalUnit();

// This is equivalent to
var ou2 = OrganizationalUnit.Parse(path);
```

## API Reference

### OrganizationalUnit Class

#### Constructors

##### `OrganizationalUnit(OrganizationalUnitConfig? config = null)`
Creates an empty organizational unit representing the base.

```csharp
var baseOu = new OrganizationalUnit();
Console.WriteLine(baseOu.ToUrlString()); // "core"
```

##### `OrganizationalUnit(string organizationalUnitString, OrganizationalUnitConfig? config = null)`
Creates an organizational unit from a string representation.

```csharp
var ou = new OrganizationalUnit("world.country.region");
Console.WriteLine(ou.ToString()); // "world.country.region"
```

#### Properties

##### `Nodes`
Gets the nodes of this organizational unit in order from least specific to most specific.

```csharp
var ou = new OrganizationalUnit("world.country.region");
foreach (var node in ou.Nodes)
{
    Console.WriteLine(node); // Outputs: "world", "country", "region"
}
```

#### Methods

##### `Parse(string? organizationalUnitString, OrganizationalUnitConfig? config = null)`
Parses a string into an OrganizationalUnit.

```csharp
var ou = OrganizationalUnit.Parse("world.country.region");
Console.WriteLine(ou.ToString()); // "world.country.region"
```

##### `GetFullyQualifiedNodes()`
Gets fully qualified nodes in order from most general to most specific, with empty string as the first element.

```csharp
var ou = OrganizationalUnit.Parse("world.country.region");
var nodes = ou.GetFullyQualifiedNodes();
// nodes = ["", "world", "world.country", "world.country.region"]
```

##### `ToString()`
Converts the organizational unit to a string representation.

```csharp
var ou = OrganizationalUnit.Parse("world.country.region");
string str = ou.ToString(); // "world.country.region"

// Base organizational unit
var baseOu = new OrganizationalUnit();
string baseStr = baseOu.ToString(); // ""
```

##### `ToUrlString()`
Converts the organizational unit to a URL-friendly string, using the base organizational unit alias for empty units.

```csharp
var ou = OrganizationalUnit.Parse("world.country.region");
string url = ou.ToUrlString(); // "world.country.region"

// Base organizational unit
var baseOu = new OrganizationalUnit();
string baseUrl = baseOu.ToUrlString(); // "core"
```

##### `Equals(object? obj)`
Determines whether this organizational unit is equal to another object.

```csharp
var ou1 = OrganizationalUnit.Parse("world.country");
var ou2 = OrganizationalUnit.Parse("world.country");
bool equal = ou1.Equals(ou2); // true
```

##### `GetHashCode()`
Gets a hash code for the organizational unit for dictionary storage.

##### Implicit Conversions
- `implicit operator OrganizationalUnit(string)`: Converts a string to an OrganizationalUnit.
- `implicit operator string(OrganizationalUnit)`: Converts an OrganizationalUnit to its string representation.

```csharp
// String to OrganizationalUnit
OrganizationalUnit ou = "world.country.region";

// OrganizationalUnit to string
string str = ou; // "world.country.region"
```

### OrganizationalUnitConfig Class

#### Properties

##### `Default`
Gets the default configuration.

```csharp
var defaultConfig = OrganizationalUnitConfig.Default;
Console.WriteLine(defaultConfig.BaseOrganizationalUnitAlias); // "core"
```

##### `BaseOrganizationalUnitAlias`
Gets or sets the alias for the base organizational unit (default: "core").

```csharp
var config = new OrganizationalUnitConfig { BaseOrganizationalUnitAlias = "myroot" };
var ou = new OrganizationalUnit(config);
Console.WriteLine(ou.ToUrlString()); // "myroot"
```

##### `Delimiter`
Gets or sets the delimiter used to separate nodes (default: '.').

```csharp
var config = new OrganizationalUnitConfig { Delimiter = '/' };
var ou = new OrganizationalUnit("world/country/region", config);
```

#### Methods

##### `SetDefault(string? baseOrganizationalUnitAlias = DefaultBaseOrganizationalUnitAlias, char? delimiter = DefaultDelimiter)`
Sets the default configuration values globally.

```csharp
OrganizationalUnitConfig.SetDefault(baseOrganizationalUnitAlias: "myroot", delimiter: '/');
var ou = new OrganizationalUnit();
Console.WriteLine(ou.ToUrlString()); // "myroot"
```

### OrganizationalUnitExtensions Class

#### Methods

##### `IsDescendantOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialParent)`
Determines whether an organizational unit is a descendant of another.

```csharp
var parent = OrganizationalUnit.Parse("world.country");
var child = OrganizationalUnit.Parse("world.country.region.city");
bool isDescendant = child.IsDescendantOf(parent); // true
```

##### `IsAncestorOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialDescendent)`
Determines whether an organizational unit is an ancestor of another.

```csharp
var parent = OrganizationalUnit.Parse("world.country");
var child = OrganizationalUnit.Parse("world.country.region.city");
bool isAncestor = parent.IsAncestorOf(child); // true
```

##### `IsChildOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialParent)`
Determines whether an organizational unit is a direct child of another.

```csharp
var parent = OrganizationalUnit.Parse("world.country");
var child = OrganizationalUnit.Parse("world.country.region");
bool isChild = child.IsChildOf(parent); // true
```

##### `IsParentOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialChild)`
Determines whether an organizational unit is a direct parent of another.

```csharp
var parent = OrganizationalUnit.Parse("world.country");
var child = OrganizationalUnit.Parse("world.country.region");
bool isParent = parent.IsParentOf(child); // true
```

### StringExtensions Class

#### Methods

##### `ToOrganizationalUnit(this string organizationalUnitString, OrganizationalUnitConfig? config = null)`
Converts a string to an OrganizationalUnit.

```csharp
string path = "world.country.region";
var ou = path.ToOrganizationalUnit();
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
  - Base OU Alias: Set at runtime via `OrganizationalUnitConfig.Default.BaseOrganizationalUnitAlias` or `OrganizationalUnitConfig.SetDefault("MyRoot")`
  - Delimiter: Set at runtime via `OrganizationalUnitConfig.Default.Delimiter` or `OrganizationalUnitConfig.SetDefault(delimiter: '/')`
  
## Contributing
PRs and issues welcome! Please use generic test data with intuitive organizational structure representations.

---

© 2025 toadicusrex. MIT License.