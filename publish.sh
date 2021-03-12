#!/usr/bin/env bash
set -e
if [[ "$#" -ne 1 ]]; then
  echo "Usage: $0 VERSION" >&2
  exit 1
fi

echo Creating packages $1
dotnet pack -p:PackageVersion=$1 -c Release ./Digizuite
dotnet pack -p:PackageVersion=$1 -c Release ./Digizuite.Core

echo Publishing packages version $1
dotnet nuget push ./Digizuite/bin/Release/Digizuite.Sdk.$1.nupkg -s github
dotnet nuget push ./Digizuite.Core/bin/Release/Digizuite.Sdk.Core.$1.nupkg -s github