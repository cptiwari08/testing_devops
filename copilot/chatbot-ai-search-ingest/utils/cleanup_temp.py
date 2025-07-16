import os
import glob

def clear_temp_files(temp_dir="temp"):
    """
    Clear temporary files in the temp directory.
    
    Args:
        temp_dir (str): The directory containing temporary files to be cleared.
    """
    pattern = os.path.join(temp_dir, '*.pkl')    
    temp_files = glob.glob(pattern)        
    for file_path in temp_files:
        try:
            os.remove(file_path)
            print(f"Removed temporary file: {file_path}")
        except Exception as e:
            print(f"Error removing file {file_path}: {e}")

