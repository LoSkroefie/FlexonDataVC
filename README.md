# FlexonDataVC

A Git-like version control system for datasets, providing efficient storage and version management for data files.

## Features

- **Dataset Versioning**: Track changes to datasets over time
- **Efficient Storage**: Uses delta encoding and compression
- **Smart Diffing**: Compare datasets at row level
- **Command Line Interface**: Git-like commands for easy use

## Installation

```bash
dotnet tool install --global FlexonDataVC
```

## Usage

### Initialize a Repository
```bash
datagit init
```

### Add a Dataset
```bash
datagit add dataset.csv
```

### Commit Changes
```bash
datagit commit -m "Added initial dataset"
```

### View Differences
```bash
datagit diff 1
```

### Rollback to Previous Version
```bash
datagit rollback 1
```

## Data Format Support

Supported data types:
- Integer (int)
- Long (long)
- Float (float)
- Double (double)
- String (string)
- DateTime (datetime)
- Boolean (bool)

## Architecture

### Storage Format
The binary storage format includes:
- Header with metadata
- Schema section
- Data section with compressed rows
- Delta section for version changes

### Components
- **BinarySerializer**: Handles data serialization/deserialization
- **DatasetComparer**: Provides smart diffing capabilities
- **Command Interface**: Processes user commands

## Development

### Building
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
