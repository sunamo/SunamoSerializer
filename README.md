# SunamoSerializer

## Short description

Knihovna pro správu textových souborů s daty včetně hlaviček a oddělovačů. Umí je načíst a zapsat. Obsahuje Runner a testy.


Management of txt files with data, including support for headers and delimited content.

## Overview

SunamoSerializer is part of the Sunamo package ecosystem, providing modular, platform-independent utilities for .NET development. It handles serialization and deserialization of text-based data files using configurable delimiters.

## Main Components

### Key Classes

- **SF** - Static facade for all serialization and deserialization operations.

### Key Methods

- `ParseUpToRequiredElementsLine()` - Parse a line ensuring exact element count.
- `GetAllElementsFile()` - Read all elements from a delimited file.
- `GetAllElementsFileAdvanced()` - Read file with separate header and data rows.
- `RemoveComments()` - Filter out comment and empty lines.
- `WriteAllElementsToFile()` - Write serialized data to a file.
- `PrepareToSerialization()` - Serialize elements into a delimited line.
- `ToDictionary()` - Parse element lists into a typed dictionary.

## Installation

```bash
dotnet add package SunamoSerializer
```

## Dependencies

- **Microsoft.Extensions.Logging.Abstractions** (v10.0.2)

## Package Information

- **Package Name**: SunamoSerializer
- **Version**: 26.2.7.2
- **Target Frameworks**: net10.0, net9.0, net8.0
- **Category**: Platform-Independent NuGet Package
- **License**: MIT

## Related Packages

This package is part of the Sunamo package ecosystem. For more information about related packages, visit the main repository.

## License

See the repository root for license information.
