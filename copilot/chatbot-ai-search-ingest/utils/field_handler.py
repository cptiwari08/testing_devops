from azure.search.documents.indexes.models import (
    ExhaustiveKnnAlgorithmConfiguration,
    ExhaustiveKnnParameters,
    HnswAlgorithmConfiguration,
    HnswParameters,
    SearchIndex,
    SemanticConfiguration,
    SemanticField,
    SemanticPrioritizedFields,
    SemanticSearch,
    SearchField,
    SearchableField,
    SearchFieldDataType,
    SimpleField,
    ComplexField,
    VectorSearch,
    VectorSearchAlgorithmKind,
    VectorSearchAlgorithmMetric,
    VectorSearchProfile,
)

class FieldHandler:
    """
    Class to handle the Field creation.
    This class initialize a basic list of fields and then lets you create new ones with specific configurations.
    """

    def __init__(self):
        # Constants
        self.__DATA_TYPE_STR = "Edm.String"
        self.__DATA_TYPE_INT = "Edm.Int32"
        self.__DATA_TYPE_BOL = "Edm.Boolean"
        self.__DATA_TYPE_COM = "Edm.ComplexType"
        self.__ANALYZER_NAME = "en.microsoft"

        self.__fields = [
            SimpleField(name="id", type=self.__DATA_TYPE_STR, key=True, retrievable=True),
            SearchField(
                name="embedding",
                type=SearchFieldDataType.Collection(SearchFieldDataType.Single),
                searchable=True,
                vector_search_dimensions=1536,
                vector_search_profile_name="myHnswProfile",
            ),
        ]
        self.add_str_field({"name": "chunk", "retrievable": True, "searchable": True})

    def add_str_field(self, kwargs):
        """
        Fields addition.
        This method adds to the list a nre field with the given configuration.
        SimpleField and SearchableField are the field types allowed.
        Just string fields can be added.

        Args:
            kwargs (dict): Fields configuration.
        """
        if kwargs.get('searchable'):
            self.__fields.append(SearchableField(type=self.__DATA_TYPE_STR, analyzer_name=self.__ANALYZER_NAME, **kwargs))
        else:
            self.__fields.append(SimpleField(type=self.__DATA_TYPE_STR, **kwargs))
    
    def add_bool_field(self, kwargs):
        """
        Fields addition.
        This method adds to the list a nre field with the given configuration.
        SimpleField and SearchableField are the field types allowed.
        Just int fields can be added.

        Args:
            kwargs (dict): Fields configuration.
        """
        if kwargs.get('searchable'):
            self.__fields.append(SearchableField(type=self.__DATA_TYPE_BOL, analyzer_name=self.__ANALYZER_NAME, **kwargs))
        else:
            self.__fields.append(SimpleField(type=self.__DATA_TYPE_BOL, **kwargs))
    
    def add_int_field(self, kwargs):
        """
        Fields addition.
        This method adds to the list a nre field with the given configuration.
        SimpleField and SearchableField are the field types allowed.
        Just int fields can be added.

        Args:
            kwargs (dict): Fields configuration.
        """
        if kwargs.get('searchable'):
            self.__fields.append(SearchableField(type=self.__DATA_TYPE_INT, analyzer_name=self.__ANALYZER_NAME, **kwargs))
        else:
            self.__fields.append(SimpleField(type=self.__DATA_TYPE_INT, **kwargs))

    def add_complex_field(self, kwargs):
        """
        Fields addition.
        This method adds to the list a nre field with the given configuration.
        SimpleField and SearchableField are the field types allowed.
        Just string fields can be added.

        Args:
            kwargs (dict): Fields configuration.
        """
        self.__fields.append(ComplexField(type=self.__DATA_TYPE_COM, 
                                              fields=[ComplexField(name="prjdatacitingsources", type=self.__DATA_TYPE_COM,fields=[
                                                SimpleField(name="AppId", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SearchableField(name="AppName", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True, searchable=True,analyzer_name="en.microsoft"),
                                                SimpleField(name="AppKey", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SimpleField(name="PageHeader", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SimpleField(name="HREF", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SimpleField(name="PageKey", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SimpleField(name="TeamType", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True)]),
                                                ComplexField(name="prjdata", type=self.__DATA_TYPE_COM,fields=[
                                                SearchableField(name="tableName", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True, searchable=True,analyzer_name="en.microsoft"),
                                                SimpleField(name="isMainTable", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                ]),
                                                ComplexField(name="prjgloss", type=self.__DATA_TYPE_COM,fields=[
                                                SearchableField(name="tableName", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True, searchable=True,analyzer_name="en.microsoft"),
                                                SearchableField(name="clarification_sentence", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True, searchable=True,analyzer_name="en.microsoft"),
                                                SimpleField(name="tag", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SimpleField(name="isSchema", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SimpleField(name="dynamicFields", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                ]),
                                                ComplexField(name="prjsuggestions", type=self.__DATA_TYPE_COM,fields=[
                                                SimpleField(name="sqlQuery", type=self.__DATA_TYPE_STR, retrievable=True, filterable=True),
                                                SearchableField(name="source", type=self.__DATA_TYPE_STR, searchable=True, retrievable=True,analyzer_name="en.microsoft"),
                                                SearchableField(name="appAffinity", type=self.__DATA_TYPE_STR, searchable=True, retrievable=True,analyzer_name="en.microsoft"),
                                                SimpleField(name="idSuggestion", type=self.__DATA_TYPE_INT, retrievable=True, filterable=True),
                                                SimpleField(name="visibleToAssistant", type=self.__DATA_TYPE_BOL, retrievable=True, filterable=True),
                                                SimpleField(name="isIncluded", type=self.__DATA_TYPE_BOL, retrievable=True, filterable=True),
                                                ])], **kwargs))
        
        
    @property
    def fields(self):
        """
        Index fields retriever.
        This method is the getter method for the list of Fields for a specific AI Search index.

        Returns:
            Retriever: Return a list of index fields.
        """
        return self.__fields