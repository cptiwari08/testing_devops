import nltk
from app.core.singleton_meta import SingletonMeta
from nltk import WordNetLemmatizer, wordpunct_tokenize
from nltk.corpus import stopwords


class NLTKManager(metaclass=SingletonMeta):
    def __init__(self) -> None:
        nltk.download("wordnet")

    def clean_and_tokenize_text(self, text: str):
        """
        Processes and cleans a text.
        This method applies a set of preprocessing steps for a user's text.
        Args:
            text (str): User's text.
        Returns:
            str: Returns the cleaned and tokenized text.
        """
        tokens = wordpunct_tokenize(text)
        words = [w.lower() for w in tokens]
        words = [word for word in words if word.isalpha()]
        stop_words = set(stopwords.words("english"))
        words = [w for w in words if w not in stop_words]
        lemmatizer = WordNetLemmatizer()
        words = [lemmatizer.lemmatize(word) for word in words]
        clean_text = " ".join(words)
        return clean_text
