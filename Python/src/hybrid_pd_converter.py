import os
from pathlib import Path
import pdfplumber
from pdf2image import convert_from_path
import pytesseract

# === CONFIG ===
INPUT_FOLDER = r"../docs/pdfs"
OUTPUT_FOLDER = r"../docs/texts/"
TESSERACT_PATH = r"C:\Program Files\Tesseract-OCR\tesseract.exe"
POPPLER_PATH = r"C:\\Core\\Tools\\poppler-24.08.0\\Library\\bin"
DPI = 300

# Setup
pytesseract.pytesseract.tesseract_cmd = TESSERACT_PATH
os.makedirs(OUTPUT_FOLDER, exist_ok=True)

# Process all PDFs
for pdf_path in Path(INPUT_FOLDER).glob("*.pdf"):
    print(f"\nProcessing: {pdf_path.name}")
    output_path = Path(OUTPUT_FOLDER) / (pdf_path.stem + ".txt")

    try:
        full_text = ""
        with pdfplumber.open(pdf_path) as pdf:
            extracted = [page.extract_text() for page in pdf.pages]
            if any(text and text.strip() for text in extracted):
                print("  → Text extracted using pdfplumber.")
                for i, text in enumerate(extracted, start=1):
                    full_text += f"\n\n--- Page {i} ---\n\n{text or ''}"
            else:
                raise ValueError("No extractable text found. Switching to OCR...")

    except Exception as e:
        print(f"  ⚠ {e}")
        print("  → Falling back to OCR...")
        try:
            images = convert_from_path(str(pdf_path), dpi=DPI, poppler_path=POPPLER_PATH)
            for i, image in enumerate(images, start=1):
                text = pytesseract.image_to_string(image, lang="eng")
                full_text += f"\n\n--- Page {i} ---\n\n{text}"
        except Exception as ocr_error:
            print(f"  ❌ OCR failed: {ocr_error}")
            continue

    with open(output_path, "w", encoding="utf-8") as f:
        f.write(full_text)
    print(f"  ✔ Saved to: {output_path}")
