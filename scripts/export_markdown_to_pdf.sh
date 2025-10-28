#!/bin/bash
# Recursively convert Markdown files to PDF while mirroring folder structure
# Requires: pandoc + wkhtmltopdf (brew install pandoc wkhtmltopdf)

set -e  # stop on error

INPUT_DIR="."             # where to start searching
OUTPUT_DIR="./pdfs"       # where to store all PDFs

# Create output directory if not exists
mkdir -p "$OUTPUT_DIR"

# Find all markdown files recursively
find "$INPUT_DIR" -type f -name "*.md" -print0 | while IFS= read -r -d '' md; do
  # Derive relative path (remove leading ./)
  rel_path="${md#./}"
  # Replace .md with .pdf
  rel_pdf="${rel_path%.md}.pdf"
  # Build full destination path inside OUTPUT_DIR
  out="$OUTPUT_DIR/$rel_pdf"

  # Create destination directory
  mkdir -p "$(dirname "$out")"

  echo "Converting: $md → $out"
  pandoc "$md" -o "$out" \
  --pdf-engine=xelatex \
  --from=gfm \
  --metadata=title:"$(basename "${md%.*}")" \
  --toc \
  --syntax-highlighting=kate
done

echo "✅ Conversion complete. PDFs are in $OUTPUT_DIR"