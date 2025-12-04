#!/usr/bin/env node

/**
 * Locale CLI - Cross-platform localization management tool
 * This wrapper script executes the .NET-based Locale CLI tool.
 * If the binary is not found, it will attempt to download it automatically.
 */

import { spawn } from "node:child_process";
import { existsSync } from "node:fs";
import { dirname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";
import { createRequire } from "node:module";
import { getBinaryPath, getBinaryDir, getDownloadUrl, getPlatformInfo } from "../lib/platform.js";
import { downloadFile, extractZip, makeExecutable, cleanupFile } from "../lib/download.js";

const __dirname = dirname(fileURLToPath(import.meta.url));
const rootDir = resolve(__dirname, "..");

const require = createRequire(import.meta.url);
const packageJson = require("../package.json");

/**
 * Attempt to download and install the binary
 * @returns {Promise<boolean>} - Whether the installation was successful
 */
async function installBinary() {
  const version = packageJson.version;
  const { rid } = getPlatformInfo();
  const binaryPath = getBinaryPath(rootDir);
  const binaryDir = getBinaryDir(rootDir);
  const downloadUrl = getDownloadUrl(version);
  const zipPath = join(rootDir, `locale-${rid}.zip`);

  console.log(`Locale CLI binary not found. Attempting to download...`);
  console.log(`Downloading Locale CLI v${version} for ${rid}...`);

  try {
    console.log(`Downloading from ${downloadUrl}...`);
    await downloadFile(downloadUrl, zipPath);

    console.log("Extracting...");
    await extractZip(zipPath, binaryDir);

    makeExecutable(binaryPath);

    console.log(`Locale CLI v${version} installed successfully!`);
    console.log("");
    return true;
  } catch (error) {
    console.error(`Failed to download Locale CLI: ${error.message}`);
    return false;
  } finally {
    cleanupFile(zipPath);
  }
}

/**
 * Show installation help
 */
function showInstallHelp() {
  const { rid } = getPlatformInfo();
  const version = packageJson.version;

  console.error("");
  console.error("The Locale CLI binary could not be found or downloaded.");
  console.error("");
  console.error("This can happen when:");
  console.error("  - The postinstall script was blocked (common with bun, pnpm, or --ignore-scripts)");
  console.error("  - The download failed due to network issues");
  console.error("  - The release is not yet available");
  console.error("");
  console.error("Alternative installation methods:");
  console.error("");
  console.error("  1. Enable postinstall scripts and reinstall:");
  console.error("     npm install @taiizor/locale-cli");
  console.error("     # or for bun:");
  console.error("     bun pm trust @taiizor/locale-cli && bun install");
  console.error("");
  console.error("  2. Install the .NET CLI tool (requires .NET SDK):");
  console.error("     dotnet tool install -g Locale.CLI");
  console.error("");
  console.error("  3. Download manually from GitHub Releases:");
  console.error(`     https://github.com/Taiizor/Locale/releases/download/v${version}/locale-${rid}.zip`);
  console.error(`     Extract to: ${getBinaryDir(rootDir)}`);
  console.error("");
}

/**
 * Main entry point
 */
async function main() {
  let binaryPath = getBinaryPath(rootDir);

  // If binary doesn't exist, try to download it
  if (!existsSync(binaryPath)) {
    const success = await installBinary();
    if (!success) {
      showInstallHelp();
      process.exit(1);
    }
  }

  // Double-check that binary now exists
  if (!existsSync(binaryPath)) {
    showInstallHelp();
    process.exit(1);
  }

  // Pass all arguments to the binary
  const args = process.argv.slice(2);

  const child = spawn(binaryPath, args, {
    stdio: "inherit",
    cwd: process.cwd(),
  });

  child.on("error", (err) => {
    console.error(`Failed to start Locale CLI: ${err.message}`);
    process.exit(1);
  });

  child.on("close", (code) => {
    process.exit(code ?? 0);
  });
}

main();