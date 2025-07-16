from fastapi import FastAPI, HTTPException, status
from business_logic.multi_index_processor import MultiIndexProcessor
from business_logic.general_index_processor import GeneralIndexProcessor
from typing import Dict, Any


class IndexProcessingError(Exception):
    def __init__(self, process_type: str, detail: str, status_code: int = 500):
        self.process_type = process_type
        self.detail = detail
        self.status_code = status_code
        super().__init__(self.detail)


app = FastAPI(
    title="AI Search Ingest API",
    description="API for managing AI Search ingestion processes"
)


@app.post(
    "/project/ingest/multi-index",
    tags=["Project Specific"],
    status_code=status.HTTP_202_ACCEPTED
)
async def ingest_project_data_multi_index() -> Dict[str, Any]:
    """
    Trigger the project-specific data ingestion process using the legacy
    multi-index approach (cronjob3). Returns 202 Accepted as this is a
    long-running process.
    Note: This endpoint uses the deprecated multi-index approach for
    backward compatibility.
    """
    try:
        processor = MultiIndexProcessor()

        # This should ideally be moved to a background task
        try:
            processor.run()  # result not needed for response
        except Exception as e:
            raise IndexProcessingError(
                process_type="multi_index",
                detail=f"Multi-index ingestion process failed: {str(e)}"
            )

        return {
            "status": "accepted",
            "message": (
                "Project data ingestion process started using "
                "multi-index approach"
            ),
            "details": {
                "process_type": "multi_index",
                "successful": True
            }
        }
    except IndexProcessingError as e:
        # Handle our custom processing errors
        return {
            "status": "error",
            "message": f"Error in {e.process_type} processing",
            "error_detail": e.detail,
            "details": {
                "process_type": e.process_type,
                "successful": False
            }
        }
    except Exception as e:
        # Handle any unexpected errors
        raise HTTPException(
            status_code=500,
            detail={
                "status": "error",
                "message": "Unexpected error during multi-index processing",
                "error_detail": str(e),
                "details": {
                    "process_type": "multi_index",
                    "successful": False
                }
            }
        )


@app.post(
    "/project/ingest/single-index",
    tags=["Project Specific"],
    status_code=status.HTTP_202_ACCEPTED
)
async def ingest_project_data_single_index() -> Dict[str, Any]:
    """
    Trigger the project-specific data ingestion process using the current
    single-index approach (cronjob4). Returns 202 Accepted as this is a
    long-running process.
    This is the recommended approach for new projects.
    """
    try:
        processor = GeneralIndexProcessor()

        # This should ideally be moved to a background task
        try:
            processor.run()  # result not needed for response
        except Exception as e:
            raise IndexProcessingError(
                process_type="single_index",
                detail=f"Single-index ingestion process failed: {str(e)}"
            )

        return {
            "status": "accepted",
            "message": (
                "Project data ingestion process started using "
                "single-index approach"
            ),
            "error_detail": None,
            "details": {
                "process_type": "single_index",
                "successful": True
            }
        }
    except IndexProcessingError as e:
        # Handle our custom processing errors
        return {
            "status": "error",
            "message": f"Error in {e.process_type} processing",
            "error_detail": e.detail,
            "details": {
                "process_type": e.process_type,
                "successful": False
            }
        }
    except Exception as e:
        # Handle any unexpected errors
        raise HTTPException(
            status_code=500,
            detail={
                "status": "error",
                "message": "Unexpected error during single-index processing",
                "error_detail": str(e),
                "details": {
                    "process_type": "single_index",
                    "successful": False
                }
            }
        )
