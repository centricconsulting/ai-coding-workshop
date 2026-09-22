#!/usr/bin/env bash
set -euo pipefail

if [ $# -ne 1 ]; then
  echo "Usage: $0 <branch-name>" >&2
  exit 1
fi

branch="$1"

git checkout main
git pull origin main
git checkout "$branch"
git merge main
