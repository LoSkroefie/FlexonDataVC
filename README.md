
# Flexon-DataVC

Flexon-DataVC is a Git-like version control system for datasets. It allows developers to track, diff, and rollback changes to structured data.

## Features
- Dataset versioning (add, commit, rollback).
- Lightweight and fast.
- Works on all platforms.

## Usage
1. **Initialize a repository**:
   ```
   datagit init
   ```

2. **Add a dataset**:
   ```
   datagit add dataset.json
   ```

3. **Commit changes**:
   ```
   datagit commit -m "Initial commit"
   ```

4. **View differences**:
   ```
   datagit diff 1
   ```

5. **Rollback to a version**:
   ```
   datagit rollback 1
   ```

## Requirements
- .NET 6.0 or higher.

## Author
JVR Software - Innovating data management.
