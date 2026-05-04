#!/bin/bash

# IAM Module Migrations Script
# This script helps manage Entity Framework Core migrations for the IAM context

set -e

MIGRATION_NAME="${1:?Migration name is required. Usage: ./manage-migrations.sh <migration-name>}"
PROJECT_PATH="./IAM/IAM.Infrastructure"
STARTUP_PROJECT="./IAM/IAM.Interface"

echo "Creating migration: $MIGRATION_NAME"
echo "Project: $PROJECT_PATH"

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo "Error: dotnet CLI is not installed or not in PATH"
    exit 1
fi

# Create migration
dotnet ef migrations add "$MIGRATION_NAME" \
  --project "$PROJECT_PATH" \
  --startup-project "$STARTUP_PROJECT" \
  --context IamDbContext \
  --output-dir Migrations

echo "Migration '$MIGRATION_NAME' created successfully"
echo "Run 'dotnet ef database update' to apply the migration"
