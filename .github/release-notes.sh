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
# Script will return the content between the first and second string occurrences
# that start with H2 markdown tag with release version: example: `## [1.0.0]`
#
# How to use:
#   release-notes.sh CHANGELOG.md


# Searching for line which starts from markdown H2 tag: ## [1.0.0]
VERSION_PATTERN="^## \[[0-9]+\\.[0-9]+\\.[0-9]+]"

startLine=$(($(cat < "$1" | grep -nE "$VERSION_PATTERN" | head -n 1 | tail -n 1 | cut -d ":" -f 1) + 1))
finishLine=$(($(cat < "$1" | grep -nE "$VERSION_PATTERN" | head -n 2 | tail -n 1 | cut -d ":" -f 1) - 1))
changelog=$(sed -n "${startLine},${finishLine}p" "$1");

echo "${changelog}"
