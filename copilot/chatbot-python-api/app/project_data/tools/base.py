from abc import abstractmethod

from app.core.interfaces import IBaseLogger, ILlamaBaseTool
from app.core.singleton_meta import SingletonMeta
from llama_index.core.tools import FunctionTool, QueryEngineTool, ToolMetadata

import numpy as np

def add(x: list) -> float:
    """Function to sum a list of numbers. Parameters: x (list)."""
    if not isinstance(x, list):
        raise ValueError("x must be a list.")
    return np.sum(x)

def multiply(x: float, y: float) -> float:
    """Function to multiply two numbers. Parameters: x (float), y (float)."""
    if not isinstance(x, (int, float)) or not isinstance(y, (int, float)):
        raise ValueError("Both x and y must be numbers.")
    return x * y

def divide(x: float, y: float) -> float:
    """Function to divide x by y. Parameters: x (float), y (float)."""
    if not isinstance(x, (int, float)) or not isinstance(y, (int, float)):
        raise ValueError("Both x and y must be numbers.")
    if y == 0:
        raise ValueError("y must not be zero.")
    return x / y

def subtract(x: float, y: float) -> float:
    """Function to subtract y from x. Parameters: x (float), y (float)."""
    if not isinstance(x, (int, float)) or not isinstance(y, (int, float)):
        raise ValueError("Both x and y must be numbers.")
    return x - y

def average(numbers: list[float]) -> float:
    """Function to calculate the average of all given numbers.    
    Parameters: 
    numbers (list of float): List of numbers to calculate the average of.    
    Returns:
    float: The average of the numbers.
    """
    if not all(isinstance(num, (int, float)) for num in numbers):
        raise ValueError("All elements in the list must be numbers.")
    if len(numbers) == 0:
        raise ValueError("The list must contain at least one number.")
    return sum(numbers) / len(numbers)

def max_value(numbers: list[float]) -> float:
    """Function to find the maximum of all given numbers.
    
    Parameters: 
    numbers (list of float): List of numbers to find the maximum of.
    
    Returns:
    float: The maximum of the numbers.
    """
    if not all(isinstance(num, (int, float)) for num in numbers):
        raise ValueError("All elements in the list must be numbers.")
    if len(numbers) == 0:
        raise ValueError("The list must contain at least one number.")
    return max(numbers)

def min_value(numbers: list[float]) -> float:
    """Function to find the minimum of all given numbers.
    
    Parameters: 
    numbers (list of float): List of numbers to find the minimum of.
    
    Returns:
    float: The minimum of the numbers.
    """
    if not all(isinstance(num, (int, float)) for num in numbers):
        raise ValueError("All elements in the list must be numbers.")
    if len(numbers) == 0:
        raise ValueError("The list must contain at least one number.")
    return min(numbers)

def ratio(x: float, y: float) -> float:
    """Function to calculate the ratio of x to y. Parameters: x (float), y (float)."""
    if not isinstance(x, (int, float)) or not isinstance(y, (int, float)):
        raise ValueError("Both x and y must be numbers.")
    if y == 0:
        raise ValueError("y must not be zero.")
    return x / y

def percent(x: float, y: float) -> float:
    """Function to calculate the percentage of x over y. Parameters: x (float), y (float)."""
    if not isinstance(x, (int, float)) or not isinstance(y, (int, float)):
        raise ValueError("Both x and y must be numbers.")
    if y == 0:
        raise ValueError("y must not be zero.")
    return (x / y) * 100


# Creating FunctionTool instances for each function
add_tool = FunctionTool.from_defaults(add)
multiply_tool = FunctionTool.from_defaults(multiply)
divide_tool = FunctionTool.from_defaults(divide)
subtract_tool = FunctionTool.from_defaults(subtract)
average_tool = FunctionTool.from_defaults(average)
max_tool = FunctionTool.from_defaults(max_value)
min_tool = FunctionTool.from_defaults(min_value)
ratio_tool = FunctionTool.from_defaults(ratio)
percent_tool = FunctionTool.from_defaults(percent)

