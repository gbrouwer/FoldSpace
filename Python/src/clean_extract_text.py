import re
from pathlib import Path

# === CONFIG ===
INPUT_FOLDER = r"../docs/texts/"
OUTPUT_FOLDER = r"../docs/markdown/"

Path(OUTPUT_FOLDER).mkdir(parents=True, exist_ok=True)

def is_line_independent(line):
    return (
        line.startswith("•")
        or re.match(r"^\s*[\-\*\d]+\.", line)
        or line.isupper()
        or line.strip() == "---"
    )

def clean_and_convert(text):
    lines = text.strip().splitlines()
    paragraphs = []
    current_para = []

    for line in lines:
        stripped = line.rstrip()

        if "--- Page" in stripped:
            if current_para:
                paragraphs.append(" ".join(current_para))
                current_para = []
            paragraphs.append("---")
            continue

        if not stripped.strip():
            if current_para:
                paragraphs.append(" ".join(current_para))
                current_para = []
            continue

        if is_line_independent(stripped):
            if current_para:
                paragraphs.append(" ".join(current_para))
                current_para = []
            paragraphs.append(stripped)
            continue

        current_para.append(stripped)

    if current_para:
        paragraphs.append(" ".join(current_para))

    return "\n\n".join(paragraphs)

# Process all .txt files
for txt_path in Path(INPUT_FOLDER).glob("*.txt"):
    with open(txt_path, "r", encoding="utf-8") as f:
        raw_text = f.read()

    cleaned_md = clean_and_convert(raw_text)

    md_path = Path(OUTPUT_FOLDER) / (txt_path.stem + ".md")
    with open(md_path, "w", encoding="utf-8") as f:
        f.write(cleaned_md)

    print(f"Converted: {txt_path.name} → {md_path.name}")
