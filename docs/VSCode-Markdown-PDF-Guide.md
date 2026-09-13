# VS Code Markdown-to-PDF Guide

## Goal

Markdown source files → optional PowerShell merge into one generated Markdown file → VS Code Markdown Preview → Markdown PDF extension → final PDF.

Markdown remains the source of truth. Edit the source, regenerate any combined document, and export again. The SmartPoint workflow and settings below were successfully tested by the user.

## Extension and Single-File Export

Install **Markdown PDF**, publisher **yzane**, extension ID **yzane.markdown-pdf**. In VS Code Extensions, search:

```text
@id:yzane.markdown-pdf
```

1. Open the Markdown file in VS Code and save it.
2. Press **Ctrl+Shift+V** to inspect Markdown Preview.
3. Return to the Markdown editor and press **Ctrl+Shift+P**.
4. Run **Markdown PDF: Export (pdf)** (some versions display `markdown-pdf: Export (pdf)`).
5. Open the resulting PDF and visually inspect it before sharing.

Use the PDF command, not Export (all), when only a PDF is needed. See the [extension documentation](https://github.com/yzane/vscode-markdown-pdf) for browser requirements and export troubleshooting; complete an initial export before relying on offline use.

## Recommended Settings

In VS Code Settings, filter by `@ext:yzane.markdown-pdf`:

| Setting | Value | Purpose |
| --- | --- | --- |
| `markdown-pdf.format` | `A4` | Standard page size |
| `markdown-pdf.includeDefaultStyles` | `true` | Use the extension's default Markdown styles |
| `markdown-pdf.displayHeaderFooter` | `false` | Avoid technical filename, date and page-number output in headers/footers |
| `markdown-pdf.outputDirectory` | Empty | Create the PDF beside the Markdown file |

These are the principles used successfully in SmartPoint. Preview and PDF rendering can still differ slightly in fonts, syntax highlighting and pagination.

## Images

Use normal relative Markdown or HTML image references, for example:

```markdown
![Runtime result](evidence/runtime.png)

<img src="evidence/runtime.png" alt="Runtime result" width="80%">
```

Keep the original images in a repository folder such as `evidence/`. Do not use Base64 embedding or duplicate images for export. Place the generated Markdown beside the source documents when they share the same relative paths; otherwise adapt paths during the merge. Check image references from the generated file's location.

## Combining Documents

Individual Markdown documents remain authoritative. A small PowerShell script can read an explicit ordered list, add a document title and contents, and write one disposable combined Markdown file. Do not manually maintain a second copy of the content.

The SmartPoint example is [Build-CompleteCaseGuide.ps1](case-guide/Build-CompleteCaseGuide.ps1). It combines Cases 01–07, preserves relative evidence paths and excludes personal interview-preparation material. Its outputs are:

- Generated Markdown: `docs/case-guide/00-complete-case-guide.md`
- Exported PDF: `docs/case-guide/00-complete-case-guide.pdf`

Both are ignored by Git. Regenerate the Markdown before every export after source changes.

Between major documents, the script inserts Markdown PDF's [documented page-break element](https://github.com/yzane/vscode-markdown-pdf#page-break):

```html
<div class="page"></div>
```

Keep default styles enabled. Page breaks affect the PDF; the continuous VS Code Preview is not a page-layout preview.

Do not assume `${include ...}$` is supported by Markdown PDF itself. A lightweight merge script avoids an additional include extension and gives built-in Preview ordinary Markdown to render.

## SmartPoint Quick Command

From the repository root in VS Code's integrated terminal:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\docs\case-guide\Build-CompleteCaseGuide.ps1
```

`-ExecutionPolicy Bypass` applies only to this launched PowerShell process. It does not permanently change the user's system execution policy.

Open the generated Markdown, preview it with **Ctrl+Shift+V**, then export with **Markdown PDF: Export (pdf)**. The formal SmartPoint PDF must not include `docs/interview-preparation/`.

## Checklist for a Future Project

- Install or verify the VS Code extension above.
- Organize the authoritative Markdown source documents.
- Store images with relative paths.
- Copy and adapt the PowerShell merge pattern if multiple documents are needed; update the ordered file list, title, contents and output name.
- Generate the combined Markdown.
- Inspect it with **Ctrl+Shift+V**.
- Export with **Markdown PDF: Export (pdf)**.
- Visually inspect screenshots, page breaks and links in the PDF before sharing. Relative repository links are not portable attachments in a shared PDF.
- Add exact generated Markdown/PDF output paths to `.gitignore` when they are build outputs; keep source Markdown and original evidence tracked.

PDF text extraction may show encoding artifacts even when the visible PDF and hyperlinks are correct. Check the actual rendered PDF before changing source content based on extracted text alone.
