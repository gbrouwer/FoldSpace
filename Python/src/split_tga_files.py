import os
from PIL import Image

# Path to your directory with .tga files
input_dir = r"C:\Core\FoldSpace\WorldCreator\Textures\tga"
output_dir = r"C:\Core\FoldSpace\WorldCreator\Textures\png"

# Create output directory if it doesn't exist
os.makedirs(output_dir, exist_ok=True)

# Iterate over all .tga files in the directory
for filename in os.listdir(input_dir):
    if filename.lower().endswith('.tga'):
        path = os.path.join(input_dir, filename)
        img = Image.open(path).convert('RGBA')  # Ensure 4 channels

        r, g, b, a = img.split()

        base = os.path.splitext(filename)[0]

        # Save each channel as a grayscale PNG
        r.save(os.path.join(output_dir, f"{base}_metallic.png"))
        g.save(os.path.join(output_dir, f"{base}_roughness.png"))
        a.save(os.path.join(output_dir, f"{base}_ao.png"))

        print(f"Processed {filename}")
