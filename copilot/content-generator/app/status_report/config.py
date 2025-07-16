import os

from app.core.key_vault import KeyVaultManager
from dotenv import load_dotenv

load_dotenv()


class ProgramOfficeConfig:
    def __init__(self, key_vault: KeyVaultManager) -> None:
        self.host_program_office: str = str(os.getenv("HOST_PROGRAM_OFFICE"))
        self.ce_po_api_key = key_vault.get_secret("Ce-API-Key-Po")
        self.timeout: int = int(os.getenv("PROGRAM_OFFICE_TIMEOUT", 50))
