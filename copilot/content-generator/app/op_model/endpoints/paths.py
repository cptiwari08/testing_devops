from enum import Enum


class OPModelPaths(str, Enum):
    start = "/op-model-generator/start"
    terminate = "/op-model-generator/terminate/{instance_id}"
