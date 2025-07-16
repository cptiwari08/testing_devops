from llama_index.core.workflow import Event

class NoDataEvent(Event):
    """Event for the case where there is no data in project_doc and eyip_ids."""
    pass
