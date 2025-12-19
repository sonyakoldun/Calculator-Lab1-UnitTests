# Lab #4: Performance Testing with Apache JMeter

This lab implements performance testing of https://ffl.org.ua/ using Apache JMeter.

## Folder Structure

```
Lab4.JMeter.Performance/
├── README.md
├── Report_Lab4_Text.md
├── JMeter/
│   ├── TestPlans/
│   │   └── Lab4_ffl_performance.jmx
│   ├── Results/
│   │   ├── smoke_results.jtl
│   │   ├── baseline_results.jtl
│   │   └── stress_results.jtl
│   └── HtmlReports/
│       ├── smoke/
│       ├── baseline/
│       └── stress/
├── Screenshots/
└── Notes/
    └── endpoints.md
```

## Installation (macOS)

### Option 1: Homebrew (Recommended)

```bash
brew install jmeter
```

After installation, verify:
```bash
jmeter --version
```

### Option 2: Manual Download

1. Download Apache JMeter from: https://jmeter.apache.org/download_jmeter.cgi
2. Extract the archive:
   ```bash
   tar -xzf apache-jmeter-*.tgz
   ```
3. Move to a permanent location (optional):
   ```bash
   sudo mv apache-jmeter-* /opt/jmeter
   ```
4. Add to PATH (add to `~/.zshrc`):
   ```bash
   export PATH=$PATH:/opt/jmeter/bin
   ```
5. Reload shell:
   ```bash
   source ~/.zshrc
   ```

### Verify Installation

```bash
jmeter --version
```

Expected output: `Apache JMeter (version X.X.X)`

## Test Plan Overview

The test plan (`JMeter/TestPlans/Lab4_ffl_performance.jmx`) contains:

- **HTTP Request Defaults**: Protocol=https, Domain=ffl.org.ua
- **HTTP Cookie Manager**: Manages cookies automatically
- **HTTP Cache Manager**: Simulates browser caching
- **HTTP Header Manager**: Sets User-Agent header
- **Three Thread Groups**:
  1. `01_SMOKE`: 1 user, 1 loop (verification)
  2. `02_BASELINE`: 10 users, 25s ramp-up, 90s duration
  3. `03_STRESS_LIGHT`: 25 users, 60s ramp-up, 120s duration
- **Response Assertions**: Validates HTTP 200 status codes
- **Listeners**: View Results Tree (SMOKE only), Summary Report, Aggregate Report

## Running Tests

### Prerequisites

Navigate to the Lab4 directory:
```bash
cd /Users/sonyakoldun/Downloads/LabTesting5_backup/Lab4.JMeter.Performance
```

### Quick Start (Using Scripts)

The easiest way to run tests is using the provided scripts:

**SMOKE test:**
```bash
./run_smoke.sh
```

**BASELINE test** (enable 02_BASELINE in GUI first):
```bash
./run_baseline.sh
```

**STRESS_LIGHT test** (enable 03_STRESS_LIGHT in GUI first):
```bash
./run_stress.sh
```

### Manual Execution

### 1. SMOKE Test (GUI Mode - for debugging)

Open JMeter GUI:
```bash
jmeter
```

1. File → Open → Select `JMeter/TestPlans/Lab4_ffl_performance.jmx`
2. Disable `02_BASELINE` and `03_STRESS_LIGHT` thread groups (right-click → Disable)
3. Enable only `01_SMOKE`
4. Click Run → Start (or press Ctrl+R / Cmd+R)
5. Check View Results Tree for any errors
6. Verify all requests return status 200

### 2. SMOKE Test (Non-GUI Mode)

**Note**: By default, `02_BASELINE` and `03_STRESS_LIGHT` are disabled in the test plan. Only `01_SMOKE` is enabled.

```bash
# Clean previous results (IMPORTANT: JMeter requires empty folder and file)
rm -rf JMeter/HtmlReports/smoke/*
rm -f JMeter/Results/smoke_results.jtl

# Run SMOKE test
jmeter -n -t JMeter/TestPlans/Lab4_ffl_performance.jmx \
  -l JMeter/Results/smoke_results.jtl \
  -e -o JMeter/HtmlReports/smoke/
```

**Important**: JMeter requires the output directory to be empty when generating HTML reports. Always clean the folder before running.

**Expected output**: 6 requests, 0 errors, average response time ~400-500ms

### 3. BASELINE Test (Non-GUI Mode)

**Before running**: Enable `02_BASELINE` and disable other thread groups in GUI, then save.

1. Open test plan in GUI: `jmeter` → File → Open → `JMeter/TestPlans/Lab4_ffl_performance.jmx`
2. Disable `01_SMOKE` and `03_STRESS_LIGHT` (right-click → Disable)
3. Enable `02_BASELINE` (right-click → Enable)
4. Save the test plan (File → Save)
5. Run:
   ```bash
   # Clean previous results (IMPORTANT: JMeter requires empty folder and file)
   rm -rf JMeter/HtmlReports/baseline/*
   rm -f JMeter/Results/baseline_results.jtl
   
   # Run BASELINE test
   jmeter -n -t JMeter/TestPlans/Lab4_ffl_performance.jmx \
     -l JMeter/Results/baseline_results.jtl \
     -e -o JMeter/HtmlReports/baseline/
   ```

### 4. STRESS_LIGHT Test (Non-GUI Mode)

**Before running**: Enable `03_STRESS_LIGHT` and disable other thread groups in GUI, then save.

1. Open test plan in GUI: `jmeter` → File → Open → `JMeter/TestPlans/Lab4_ffl_performance.jmx`
2. Disable `01_SMOKE` and `02_BASELINE` (right-click → Disable)
3. Enable `03_STRESS_LIGHT` (right-click → Enable)
4. Save the test plan (File → Save)
5. Run:
   ```bash
   # Clean previous results (IMPORTANT: JMeter requires empty folder and file)
   rm -rf JMeter/HtmlReports/stress/*
   rm -f JMeter/Results/stress_results.jtl
   
   # Run STRESS_LIGHT test
   jmeter -n -t JMeter/TestPlans/Lab4_ffl_performance.jmx \
     -l JMeter/Results/stress_results.jtl \
     -e -o JMeter/HtmlReports/stress/
   ```

## Generating HTML Reports

HTML reports are automatically generated when using the `-e -o` flags:

```bash
jmeter -n -t <test_plan.jmx> -l <results.jtl> -e -o <html_report_dir>
```

Where:
- `-n`: Non-GUI mode
- `-t`: Test plan file
- `-l`: Results file (.jtl)
- `-e`: Generate HTML report
- `-o`: Output directory for HTML report

### Viewing HTML Reports

Open the generated HTML report in a browser:

```bash
# macOS
open JMeter/HtmlReports/baseline/index.html
open JMeter/HtmlReports/stress/index.html
```

## Test Scenarios

### 01_SMOKE
- **Purpose**: Verify test plan works correctly
- **Users**: 1
- **Ramp-up**: 1 second
- **Loops**: 1
- **Listeners**: View Results Tree, Summary Report

### 02_BASELINE
- **Purpose**: Establish baseline performance metrics
- **Users**: 10
- **Ramp-up**: 25 seconds
- **Duration**: 90 seconds
- **Listeners**: Summary Report, Aggregate Report

### 03_STRESS_LIGHT
- **Purpose**: Light stress test to identify performance degradation
- **Users**: 25
- **Ramp-up**: 60 seconds
- **Duration**: 120 seconds
- **Listeners**: Summary Report, Aggregate Report

## Endpoints Tested

See `Notes/endpoints.md` for the complete list of endpoints. The test plan includes:
- Home Page (/)
- News Page (/news)
- FAQ Page (/questions/faq)
- Questions Page (/questions)
- Contacts Page (/pages/66)
- Video Gallery (/media/video)

## Key Metrics

The HTML dashboard and Aggregate Report provide:
- **Response Time**: Average, Min, Max, 90th/95th percentile
- **Throughput**: Requests per second
- **Error Rate**: Percentage of failed requests
- **Response Codes**: Distribution of HTTP status codes

## Troubleshooting

### JMeter not found
- Ensure JMeter is in your PATH
- Try using full path: `/opt/jmeter/bin/jmeter` or `/usr/local/bin/jmeter`

### Out of Memory errors
- Increase heap size: `export HEAP="-Xms1g -Xmx4g -XX:MaxMetaspaceSize=512m"`
- Run: `jmeter -n -t ...` with the HEAP variable set

### Test plan won't open
- Ensure JMeter version is 5.0 or higher
- Check XML syntax in the .jmx file

### Connection errors
- Verify internet connection
- Check if https://ffl.org.ua/ is accessible
- Review firewall settings

## Ethical Considerations

⚠️ **Important**: This test plan uses moderate load profiles to avoid overloading the target server. Do NOT:
- Increase thread counts beyond 30
- Run tests for extended durations
- Generate excessive traffic that could be considered a DDoS attack

## Additional Resources

- [Apache JMeter Documentation](https://jmeter.apache.org/usermanual/index.html)
- [JMeter Best Practices](https://jmeter.apache.org/usermanual/best-practices.html)
- [HTML Dashboard Report](https://jmeter.apache.org/usermanual/generating-dashboard.html)

