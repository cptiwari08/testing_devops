from httpx import Response


class PromptValidationError(Exception):
    pass


class ProgramOfficeResponseError(Exception):
    def __init__(self, response: Response) -> None:
        super().__init__()
        self.response = response

    def __str__(self) -> str:
        return f"Program Office SQL endpoint has returned HTTP {self.response.status_code} error"


class ProgramOfficeTimeoutError(Exception):
    def __init__(self, url: str) -> None:
        super().__init__()
        self.url = url

    def __str__(self) -> str:
        return "Program Office SQL endpoint has returned HTTP 408 error"


class NotSupportedQueryTypeError(Exception):
    pass


class NonIntegerChatHistoryError(Exception):
    pass


class NonNegativeChatHistoryError(Exception):
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

class MissingTokenClaimError(Exception):
    def __init__(self, claim_name) -> None:
        super().__init__()
        self.claim_name = claim_name

    def __str__(self) -> str:
        return f"{self.claim_name} claim not present in token"
