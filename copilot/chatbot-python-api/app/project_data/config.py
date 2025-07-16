import os
from dataclasses import dataclass

from dotenv import load_dotenv

load_dotenv()


@dataclass
class ProgramOfficeConfig:
    timeout: int = int(os.getenv("PROGRAM_OFFICE_TIMEOUT", 50))
