#!/usr/bin/env bash

# -e  Exit immediately if a command exits with a non-zero status.
# -u  Treat unset variables as an error when substituting.

set -eu
set -o pipefail


# Get Release notes for the latest release from CHANGELOG.md
# How to use:
#   release-notes.sh CHANGELOG.md

startLine=$(cat $1 | grep -nE "^### " | head -n 1 | cut -d ":" -f 1)
finishLine=$(($(cat $1 | grep -nE "^## " | head -n 2 | tail -n 1 | cut -d ":" -f 1) - 1))
changelog=`sed -n "${startLine},${finishLine}p" $1`;

echo "${changelog}"
