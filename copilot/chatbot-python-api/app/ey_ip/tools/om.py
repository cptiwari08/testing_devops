from app.core.ms_server_manager import MSSQLServerManager
from app.core.prompt_manager import create_prompt_manager
from app.ey_ip.config import config
from app.ey_ip.tools.base import EYIPBaseTool
from llama_index.core import SQLDatabase
from llama_index.core.query_engine import NLSQLTableQueryEngine
from llama_index.core.tools import QueryEngineTool
from llama_index.core.prompts.base import PromptTemplate
from llama_index.core.prompts.prompt_type import PromptType

class OMTool(EYIPBaseTool):
    """
    A class used to handle the Operating Model (OM) tool.
    This class provides methods to initialize the OM tool and generate a
    QueryEngineTool. It inherits from the EYIPBaseTool base class.
    """

    def __init__(self, logger, llm_models) -> None:
        super().__init__(logger, llm_models)
        self._logger.info("Initializing OMTool")
        self._engine = MSSQLServerManager(config, self._logger).get_engine()
        self._tables_names = ["Nodes"]
        self._prompt_manager = create_prompt_manager(self._logger)

    def generate(self) -> QueryEngineTool:
        """
        Generates a QueryEngineTool for the OM tool.
        Returns:
            A QueryEngineTool for the OM tool.
        """
        self._logger.info("OMTool Getting SQL Connection")
        sql_database = SQLDatabase(self._engine, include_tables=self._tables_names)
        
        # Get the text_to_sql_prompt template using the prompt manager (synchronously)
        text_to_sql_prompt_template = self._prompt_manager.get_prompt_sync(
            agent="ey_ip", 
            key="text_to_sql_prompt"
        )
        
        text_to_to_sql_prompt = PromptTemplate(
            text_to_sql_prompt_template,
            prompt_type=PromptType.TEXT_TO_SQL,
        )
        
        sql_query_engine = NLSQLTableQueryEngine(
            sql_database=sql_database,
            tables=self._tables_names,
            llm=self.llm_models.llm,
            embed_model=self.llm_models.embed_model,
            text_to_sql_prompt=text_to_to_sql_prompt,
        )
        
        tables_names = " ".join(self._tables_names)
        
        # Get the tool description using the prompt manager (synchronously)
        description = self._prompt_manager.get_prompt_sync(
            agent="ey_ip", 
            key="om_tool_description",
            prompt_parameters={"tables_names": tables_names}
        )

        sql_tool = QueryEngineTool.from_defaults(
            name="om_tool",
            query_engine=sql_query_engine,
            description=description,
        )
        self._logger.info("OMTool generated")
        return sql_tool
