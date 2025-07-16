class PromptValidationError(Exception):
    pass


class SecretNotFoundError(Exception):
    def __init__(self, secret_name) -> None:
        super().__init__()
        self.secret_name = secret_name

    def __str__(self) -> str:
        return f"{self.secret_name} key not present in key vault instance"


class KeyVaultHTTPConnectionError(Exception):
    def __init__(self, secret_name, exc) -> None:
        super().__init__()
        self.secret_name = secret_name
        self.exc = exc

    def __str__(self) -> str:
        return f"HTTP Response error getting {self.secret_name} key from key vault \n {self.exc}"
