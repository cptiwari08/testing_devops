import time
from inspect import signature
from utils.logger import Logger

log = Logger()

def measure_index_execution_time(func):
    """
    Decorador para medir el tiempo de ejecución de una función.
    """
    def wrapper(*args, **kwargs):
        start_time = time.time()
        result = func(*args, **kwargs)
        end_time = time.time()
        execution_time = end_time - start_time
        arguments = signature(func).bind(*args, **kwargs)
        arguments.apply_defaults()
        index_name = arguments.arguments.get("index_name")
        log.info(f"Index {index_name} successfully loaded after {execution_time:.0f} seconds")
        return result
    return wrapper