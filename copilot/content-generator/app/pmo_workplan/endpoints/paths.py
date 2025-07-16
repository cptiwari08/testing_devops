from enum import Enum


class WorkplanPaths(str, Enum):
    start = "/pmo-workplan-generator/start"
    terminate = "/pmo-workplan-generator/terminate/{instance_id}"
