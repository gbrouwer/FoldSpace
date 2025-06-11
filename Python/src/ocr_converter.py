import os
from pathlib import Path
from pdf2image import convert_from_path
import pytesseract

# === CONFIG ===
INPUT_FOLDER = r"../docs/"       
OUTPUT_FOLDER = r"../texts/"
TESSERACT_PATH = r"C:\Program Files\Tesseract-OCR\tesseract.exe"
POPPLER_PATH = r"C:\\Core\\Tools\\poppler-24.08.0\\Library\\bin"
DPI = 300

# Setup
pytesseract.pytesseract.tesseract_cmd = TESSERACT_PATH
os.makedirs(OUTPUT_FOLDER, exist_ok=True)

# Process all PDFs
for pdf_file in Path(INPUT_FOLDER).glob("*.pdf"):
    print(f"Processing: {pdf_file.name}")
    
    try:
        # Convert PDF pages to images
        images = convert_from_path(str(pdf_file), dpi=DPI, poppler_path=POPPLER_PATH)
        
        full_text = ""
        for i, image in enumerate(images, start=1):
            text = pytesseract.image_to_string(image, lang="eng")
            full_text += f"\n\n--- Page {i} ---\n\n{text}"
        
        # Save text
        output_path = Path(OUTPUT_FOLDER) / (pdf_file.stem + ".txt")
        with open(output_path, "w", encoding="utf-8") as f:
            f.write(full_text)
        
        print(f"Saved to {output_path}")
    
    except Exception as e:
        print(f"Error processing {pdf_file.name}: {e}")
