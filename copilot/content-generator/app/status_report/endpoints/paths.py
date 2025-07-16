from enum import Enum


class StatusReportPaths(str, Enum):
    start = "/pmo-status-report-generator/start"
    terminate = "/pmo-status-report-generator/terminate/{instance_id}"
