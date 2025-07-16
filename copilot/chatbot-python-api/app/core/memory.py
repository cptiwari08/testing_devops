from app.core.errors import NonIntegerChatHistoryError, NonNegativeChatHistoryError
from app.core.pydantic_models import ChatHistory
from llama_index.core.llms import ChatMessage, MessageRole


class MemoryManager:
    """
    A class used to manage the memory of a Llama chatbot.
    This class provides a method to generate a list of ChatMessage objects from a chat history.
    Each ChatMessage object represents a message in the chat history and contains the role of the sender
    (user, assistant, or system) and the content of the message.
    """

    @staticmethod
    def generate_chat_messages(
        chat_history: ChatHistory, size: int | str = -6
    ) -> list | None:
        """
        Generates a list of ChatMessage objects from a chat history.
        Args:
            chat_history: A list of message objects. Each message object should have a 'role' attribute that
            indicates the role of the sender (user, assistant, or system) and a 'content' attribute that
            contains the content of the message.
        Returns:
            A list of ChatMessage objects. Each ChatMessage object represents a message in the chat history.
        """
        try:
            size = int(size)
        except ValueError:
            raise NonIntegerChatHistoryError()

        if size >= 0:
            raise NonNegativeChatHistoryError()

        if not chat_history:
            return []

        roles = {
            "user": MessageRole.USER,
            "assistant": MessageRole.ASSISTANT,
            "system": MessageRole.SYSTEM,
        }
        messages = []
        for message in chat_history:
            messages.append(
                ChatMessage(role=roles[message.role], content=message.content)
            )

        return messages[size:] if messages[size:] else None
