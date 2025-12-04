/**
 * Post-install script for Locale CLI
 * Downloads the platform-specific binary from GitHub Releases
 *
 * Note: This script may not run in all package managers (e.g., bun blocks postinstall by default).
 * The main CLI script (bin/locale.js) will attempt to download the binary if this fails.
 */

import { existsSync } from "node:fs";
import { join, dirname, resolve } from "node:path";
import { fileURLToPath } from "node:url";
import { createRequire } from "node:module";
import { getBinaryDir, getBinaryPath, getDownloadUrl, getPlatformInfo } from "./platform.js";
import { downloadFile, extractZip, makeExecutable, cleanupFile } from "./download.js";

const __dirname = dirname(fileURLToPath(import.meta.url));
const rootDir = resolve(__dirname, "..");

const require = createRequire(import.meta.url);
const packageJson = require("../package.json");

/**
 * Main installation function
 */
async function install() {
  const version = packageJson.version;
  const { rid } = getPlatformInfo();

  console.log(`Installing Locale CLI v${version} for ${rid}...`);

  const binaryPath = getBinaryPath(rootDir);
  const binaryDir = getBinaryDir(rootDir);

  // Check if already installed
  if (existsSync(binaryPath)) {
    console.log("Locale CLI binary already exists, skipping download.");
    return;
  }

  const downloadUrl = getDownloadUrl(version);
  const zipPath = join(rootDir, `locale-${rid}.zip`);

  try {
    console.log(`Downloading from ${downloadUrl}...`);
    await downloadFile(downloadUrl, zipPath);

    console.log("Extracting...");
    await extractZip(zipPath, binaryDir);

    // Make binary executable
    makeExecutable(binaryPath);

    console.log(`Locale CLI v${version} installed successfully!`);
  } catch (error) {
    // Don't exit with error - the CLI wrapper will attempt to download on first run
    console.warn(`Note: Could not download Locale CLI during installation: ${error.message}`);
    console.warn("The binary will be downloaded automatically when you first run 'locale'.");
    console.warn("");
    console.warn("Alternative installation methods:");
    console.warn("  1. Install .NET SDK and use: dotnet tool install -g Locale.CLI");
    console.warn("  2. Download manually from: https://github.com/Taiizor/Locale/releases/latest");
  } finally {
    // Clean up zip file
    cleanupFile(zipPath);
  }
}

// Run installation
install().catch((error) => {
  // Don't exit with error code - allow installation to continue
  // The CLI wrapper will handle the download on first run
  console.warn("Installation notice:", error.message);
  console.warn("The binary will be downloaded automatically when you first run 'locale'.");
});