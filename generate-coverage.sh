#!/bin/bash

outdir="$(pwd)/Test/TestResults"
coveragefile="$outdir/coverage.cobertura.xml"

# Run tests with coverage
dotnet test Test/Test.csproj \
  -p:CollectCoverage=true \
  -p:CoverletOutputFormat=cobertura \
  -p:CoverletOutput="$outdir/coverage" \
  -p:Exclude=\"[xunit*]*,[*.Test]*\" \
  --logger "console;verbosity=detailed" || true

# Generate HTML report
reportgenerator "-reports:$coveragefile" "-targetdir:$outdir/Reports" "-reporttypes:Html"

# Open report in default browser (Windows)
if [[ -f "Test/TestResults/Reports/index.html" ]]; then
    start "Test/TestResults/Reports/index.html"
else
    echo "Error: Report generation failed - file not found"
fi