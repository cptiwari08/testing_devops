from fastapi import FastAPI, HTTPException, status
from business_logic.ingest_processor import IngestProcessor
from business_logic.metadata_processor import MetadataProcessor
from typing import Dict, Any

class ProcessingError(Exception):
    def __init__(self, process_name: str, detail: str, status_code: int = 500):
        self.process_name = process_name
        self.detail = detail
        self.status_code = status_code
        super().__init__(self.detail)

app = FastAPI(
    title="EY.IP Ingest API",
    description="API for managing EY.IP document ingestion processes"
)

@app.post("/processes/eyip-ingestion", tags=["EY.IP"], status_code=status.HTTP_202_ACCEPTED)
async def ingest_eyip_data() -> Dict[str, Any]:
    """
    Trigger the EY.IP template ingestion process (cronjob1 + cronjob2 sequential execution).
    This endpoint handles both the document ingestion and metadata processing for EY.IP templates.
    Returns 202 Accepted as this is a long-running process.
    """
    try:
        # Execute sequentially as required
        ingest_processor = IngestProcessor()
        metadata_processor = MetadataProcessor()

        # Run the processes in sequence - this should ideally be moved to a background task
        try:
            ingest_result = ingest_processor.run()
        except Exception as e:
            raise ProcessingError(
                process_name="document_ingestion",
                detail=f"Document ingestion process failed: {str(e)}"
            )

        try:
            metadata_result = metadata_processor.run()
        except Exception as e:
            raise ProcessingError(
                process_name="metadata_processing",
                detail=f"Metadata processing failed: {str(e)}"
            )

        return {
            "status": "accepted",
            "message": "EY.IP ingestion process succeeded",
            "error_detail": None,
            "details": {
                "ingest_successful": True,
                "metadata_successful": True
            }
        }
    except ProcessingError as e:
        # Handle our custom processing errors
        return {
            "status": "error",
            "message": f"Error in {e.process_name}",
            "error_detail": e.detail,
            "details": {
                "ingest_successful": e.process_name != "document_ingestion",
                "metadata_successful": e.process_name != "metadata_processing"
            }
        }
    except Exception as e:
        # Handle any unexpected errors
        raise HTTPException(
            status_code=500,
            detail={
                "status": "error",
                "message": "Unexpected error during processing",
                "error_detail": str(e),
                "details": {
                    "ingest_successful": False,
                    "metadata_successful": False
                }
            }
        )
