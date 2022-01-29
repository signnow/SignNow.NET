#!/usr/bin/env bash

# -e  Exit immediately if a command exits with a non-zero status.
# -u  Treat unset variables as an error when substituting.

set -eu
set -o pipefail

# Gets Release notes for the latest release from CHANGELOG.md
#
# This script takes description in `markdown` format from CHANGELOG.md file
# You must to use the `markdown` H2 tag for the release version
# and `markdown` H3 tag for the description
#
# Script will return the content between the first and second string occurences that start with `## [`
#
# How to use:
#   release-notes.sh CHANGELOG.md

# Searching for line which starts from markdowd H3 tag: ###
startLine=$(cat < "$1" | grep -nE "^### " | head -n 1 | cut -d ":" -f 1)
# Searching for line (second line) which ends on markdowd H2 tag with leading square bracket: ## [
finishLine=$(($(cat < "$1" | grep -nE "^## \[" | head -n 2 | tail -n 1 | cut -d ":" -f 1) - 1))
changelog=$(sed -n "${startLine},${finishLine}p" "$1");

echo "${changelog}"
