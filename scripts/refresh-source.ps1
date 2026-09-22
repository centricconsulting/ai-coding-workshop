#!/usr/bin/env pwsh
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$Branch
)

$ErrorActionPreference = "Stop"

git checkout main
git pull origin main
git checkout $Branch
git merge main
