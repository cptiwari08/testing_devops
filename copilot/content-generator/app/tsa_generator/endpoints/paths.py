from enum import Enum


class TSAPaths(str, Enum):
    start = "/tsa-generator/start"
    terminate = "/tsa-generator/terminate/{instance_id}"
