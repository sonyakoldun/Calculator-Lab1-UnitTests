#!/bin/bash
# Script to run STRESS_LIGHT test
# Usage: ./run_stress.sh
# NOTE: Make sure 03_STRESS_LIGHT is enabled and others are disabled in GUI first!

cd "$(dirname "$0")"

echo "Cleaning previous results..."
rm -rf JMeter/HtmlReports/stress/*
rm -f JMeter/Results/stress_results.jtl

echo "Running STRESS_LIGHT test..."
jmeter -n -t JMeter/TestPlans/Lab4_ffl_performance.jmx \
  -l JMeter/Results/stress_results.jtl \
  -e -o JMeter/HtmlReports/stress/

echo ""
echo "Test completed! Results:"
echo "  - JTL file: JMeter/Results/stress_results.jtl"
echo "  - HTML report: JMeter/HtmlReports/stress/index.html"
echo ""
echo "To view HTML report:"
echo "  open JMeter/HtmlReports/stress/index.html"

