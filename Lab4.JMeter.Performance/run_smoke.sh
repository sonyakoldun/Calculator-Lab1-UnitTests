#!/bin/bash
# Script to run SMOKE test
# Usage: ./run_smoke.sh

cd "$(dirname "$0")"

echo "Cleaning previous results..."
rm -rf JMeter/HtmlReports/smoke/*
rm -f JMeter/Results/smoke_results.jtl

echo "Running SMOKE test..."
jmeter -n -t JMeter/TestPlans/Lab4_ffl_performance.jmx \
  -l JMeter/Results/smoke_results.jtl \
  -e -o JMeter/HtmlReports/smoke/

echo ""
echo "Test completed! Results:"
echo "  - JTL file: JMeter/Results/smoke_results.jtl"
echo "  - HTML report: JMeter/HtmlReports/smoke/index.html"
echo ""
echo "To view HTML report:"
echo "  open JMeter/HtmlReports/smoke/index.html"

