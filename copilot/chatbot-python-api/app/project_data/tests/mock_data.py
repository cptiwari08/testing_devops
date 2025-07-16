from uuid import uuid4

# Datos de ejemplo para test_query_pipeline
SAMPLE_SEARCH_RESULTS = [
    {
        "chunk": "sugestion_1",
        "metadata": {
            "prjsuggestions": {
                "sqlQuery": "SELECT SUM(Revenue) FROM Department WHERE DepartmentName = '{Username}'"
            }
        },
        "@search.reranker_score": 0.95
    }
]

# Payload de ejemplo para la solicitud
def get_query_payload():
    return {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "How many tasks does each team have?",
        "context": {
            "user": {
                "email": "test@test.com",
            },
            "appInfo":{
                "name": "Project Management",
                "teamTypeIds": [1]
            },
        },
    }

COMPLEX_QUESTION_SEARCH_RESULTS = [
    {
        "chunk": "No match",
        "metadata": {
            "prjsuggestions": {
                "sqlQuery": "SELECT SUM(Revenue) FROM Department WHERE DepartmentName = '{Username}'"
            }
        },
        "@search.reranker_score": 0.95
    }
]

# Add this new function to mock_data.py
def get_complex_query_data():
    """Specific test data for complex question test scenario"""
    return {
        "chatId": str(uuid4()),
        "instanceId": str(uuid4()),
        "question": "What's our total revenue and how is it broken down by team?",
        "context": {
            "user": {
                "email": "test@test.com",
            },
            "appInfo": {
                "name": "Project Management",
                "teamTypeIds": [1]
            },
        },
    }

# Clase auxiliar para crear iteradores asincrónicos
class AsyncIterator:
    def __init__(self, items):
        self.items = items
        self.index = 0
        
    async def __anext__(self):
        if self.index < len(self.items):
            item = self.items[self.index]
            self.index += 1
            return item
        raise StopAsyncIteration
        
    def __aiter__(self):
        return self
