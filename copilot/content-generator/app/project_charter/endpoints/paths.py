from enum import Enum


class ProjectCharterPaths(str, Enum):
    start = "/project-charter-generator/start"
    terminate = "/project-charter-generator/terminate/{instance_id}"
