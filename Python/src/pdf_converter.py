import pdfplumber
from pathlib import Path

# === CONFIG ===
INPUT_FOLDER = r"C:\path\to\your\pdfs"       # <-- change this
OUTPUT_FOLDER = r"C:\path\to\your\output"    # <-- and this

Path(OUTPUT_FOLDER).mkdir(parents=True, exist_ok=True)

for pdf_path in Path(INPUT_FOLDER).glob("*.pdf"):
    print(f"Reading: {pdf_path.name}")
    full_text = ""

    try:
        with pdfplumber.open(pdf_path) as pdf:
            for i, page in enumerate(pdf.pages, start=1):
                text = page.extract_text() or ""
                full_text += f"\n\n--- Page {i} ---\n\n{text}"
    except Exception as e:
        print(f"Failed to process {pdf_path.name}: {e}")
        continue

    output_path = Path(OUTPUT_FOLDER) / (pdf_path.stem + ".txt")
    with open(output_path, "w", encoding="utf-8") as f:
        f.write(full_text)

    print(f"Saved to: {output_path}")
