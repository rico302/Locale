/**
 * Download and extraction utilities for Locale CLI
 */

import { createWriteStream, existsSync, mkdirSync, chmodSync, rmSync } from "node:fs";
import { dirname, join } from "node:path";
import { pipeline } from "node:stream/promises";
import { spawn } from "node:child_process";

/**
 * Download a file from URL with retry logic
 * @param {string} url - URL to download
 * @param {string} destPath - Destination file path
 * @param {number} retries - Number of retry attempts (default: 3)
 * @throws {Error} If all retry attempts fail, throws an error with the last failure message
 */
export async function downloadFile(url, destPath, retries = 3) {
  let lastError;

  for (let attempt = 1; attempt <= retries; attempt++) {
    try {
      const response = await fetch(url, {
        redirect: "follow",
        headers: {
          "User-Agent": "locale-cli-npm-installer",
        },
      });

      if (!response.ok) {
        throw new Error(`HTTP ${response.status} ${response.statusText}`);
      }

      const destDir = dirname(destPath);
      if (!existsSync(destDir)) {
        mkdirSync(destDir, { recursive: true });
      }

      const fileStream = createWriteStream(destPath);
      await pipeline(response.body, fileStream);
      return; // Success
    } catch (error) {
      lastError = error;
      if (attempt < retries) {
        console.log(`Download attempt ${attempt} failed, retrying...`);
        await new Promise(resolve => setTimeout(resolve, 1000 * attempt));
      }
    }
  }

  throw new Error(`Failed to download after ${retries} attempts: ${lastError.message}`);
}

/**
 * Extract a zip file
 * @param {string} zipPath - Path to zip file
 * @param {string} destDir - Destination directory
 * @throws {Error} If extraction fails
 */
export async function extractZip(zipPath, destDir) {
  if (!existsSync(destDir)) {
    mkdirSync(destDir, { recursive: true });
  }

  const isWindows = process.platform === "win32";

  return new Promise((resolve, reject) => {
    let child;

    if (isWindows) {
      child = spawn("powershell", [
        "-NoProfile",
        "-Command",
        `Expand-Archive -Path "${zipPath}" -DestinationPath "${destDir}" -Force`,
      ]);
    } else {
      child = spawn("unzip", ["-o", zipPath, "-d", destDir]);
    }

    child.on("error", (err) => {
      reject(new Error(`Failed to extract zip: ${err.message}`));
    });

    child.on("close", (code) => {
      if (code === 0) {
        resolve();
      } else {
        reject(new Error(`Extraction failed with code ${code}`));
      }
    });
  });
}

/**
 * Make a file executable (Unix only)
 * @param {string} filePath - Path to file
 */
export function makeExecutable(filePath) {
  if (process.platform !== "win32") {
    chmodSync(filePath, 0o755);
  }
}

/**
 * Clean up a file if it exists
 * @param {string} filePath - Path to file to remove
 */
export function cleanupFile(filePath) {
  if (existsSync(filePath)) {
    rmSync(filePath, { force: true });
  }
}