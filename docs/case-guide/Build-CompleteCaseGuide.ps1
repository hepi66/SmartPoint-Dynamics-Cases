# Regenerate the disposable combined guide; individual case files remain authoritative.
$ErrorActionPreference = 'Stop'
$files = @(
    '01-room-planning.md',
    '02-booking-confirmation.md',
    '03-javascript-account-notification.md',
    '04-account-numbering-console.md',
    '05-account-numbering-function.md',
    '06-account-creator.md',
    '07-opportunity-notification.md'
)
$parts = foreach ($file in $files) {
    $text = [IO.File]::ReadAllText((Join-Path $PSScriptRoot $file))
    if ($text -match 'What I Should Be Able to Explain|interview-preparation/|Case_Questions_and_Answers|data:image/') {
        throw "Excluded content found in $file. Review the source before exporting."
    }
    foreach ($image in [regex]::Matches($text, '<img\b[^>]*\bsrc="([^"]+)"')) {
        $relativePath = $image.Groups[1].Value
        if (-not $relativePath.StartsWith('evidence/') -or
            -not (Test-Path -LiteralPath (Join-Path $PSScriptRoot $relativePath) -PathType Leaf)) {
            throw "Missing or unexpected evidence image in ${file}: $relativePath"
        }
    }
    # Keep the content and relative paths intact, including repository reference links.
    $text.TrimEnd()
}
$output = Join-Path $PSScriptRoot '00-complete-case-guide.md'
$introduction = @"
# SmartPoint Dynamics Case

## Complete Case Guide

Formal Case Documentation & Evidence

Environment: ``CRM816895``<br>
Solution: ``Achim Beispiel``

This document combines the formal SmartPoint practical case documentation for Cases 01$([char]0x2013)07.

Personal interview-preparation material is intentionally excluded.

## Contents

1. Room Planning
2. Booking Confirmation
3. JavaScript Account Form Notification
4. C# Account Numbering Console
5. Timer-triggered Account Numbering Function
6. Copilot Studio Account Creator
7. Optional Opportunity Notification
"@
# Markdown PDF's documented page-break class; keep default styles enabled.
$pageBreak = "`n`n<div class=""page""></div>`n`n"
[IO.File]::WriteAllText($output, $introduction + $pageBreak + ($parts -join $pageBreak) + "`n", [Text.UTF8Encoding]::new($false))
Write-Host "Generated $output from $($files.Count) cases. Open it in VS Code, preview, then export with Markdown PDF."
