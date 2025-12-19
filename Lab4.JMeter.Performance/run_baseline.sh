#!/bin/bash
# Script to run BASELINE test
# Usage: ./run_baseline.sh
# NOTE: Make sure 02_BASELINE is enabled and others are disabled in GUI first!

cd "$(dirname "$0")"

echo "Cleaning previous results..."
rm -rf JMeter/HtmlReports/baseline/*
rm -f JMeter/Results/baseline_results.jtl

echo "Running BASELINE test..."
jmeter -n -t JMeter/TestPlans/Lab4_ffl_performance.jmx \
  -l JMeter/Results/baseline_results.jtl \
  -e -o JMeter/HtmlReports/baseline/

echo ""
echo "Test completed! Results:"
echo "  - JTL file: JMeter/Results/baseline_results.jtl"
echo "  - HTML report: JMeter/HtmlReports/baseline/index.html"
echo ""
echo "To view HTML report:"
echo "  open JMeter/HtmlReports/baseline/index.html"

