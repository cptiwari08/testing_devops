import json


class PromptStore:
    """
    This is the first implementation, just a placeholder to start
    using the get_prompt method and then replace in the code base
    and replace the logic inside it with the final store mechanism.

    I JSON file is only temporal to start wiring out all the workflows
    steps with the get_prompt method, is not intended to be used in
    production.
    """

    def get_prompt(self, prompt_name: str):
        db = json.loads("app/project_charter/templates/prompts.json")
        return db[prompt_name]
