from app.core.pydantic_models import QueryPipelineContext


def citing_sources_output_parser(citing_sources_output, context: QueryPipelineContext):
    context.logger.info("I executed the citing_sources output parser function component")
    if citing_sources_output:
        citing_sources_output_response = {"sourceName": "project-data", "sourceType": "page-key"}
        source_value = []
        for node in citing_sources_output:
            current_node = {
                "appId": node.metadata.get("appId"), 
                "appName": node.metadata.get("appName"),
                "key": node.metadata.get("key"),
                "name": node.metadata.get("name"),
                "href": node.metadata.get("href"),
                "securityKey": node.metadata.get("securityKey") or "",
                "score": "{:.0f}".format(node.score),
            }
            source_value.append(current_node)

        citing_sources_output_response |= {"sourceValue": source_value}

        return citing_sources_output_response
    else:
        return {}