import json
from typing import Generator, List

from app.project_data.services.program_office_api import ProgramOffice
from pypika import Query, Table


async def get_widgets(token: str):
    pc = Table("PageConfigs", alias="PC")
    mo = Table("MenuOptions", alias="MO")

    query = (
        Query.from_(pc)
        .select(mo.Key.as_("KeyItem"), pc.JSON.as_("JSONItem"))
        .join(mo)
        .on(mo.Key == pc.Key)
        .where(pc.IsFormConfig == 0)
        .orderby(pc.Key)
    )

    final_query = str(query).replace("JOIN", "INNER JOIN")

    result = dict()

    program_office = ProgramOffice()

    response = await program_office.run_query(
        {"SqlQuery": final_query, "tables": []}, token
    )

    for json_data in response:
        key = json_data["KeyItem"]
        json_page_conf = (
            json.loads(json_data["JSONItem"])
            if isinstance(json_data["JSONItem"], str)
            else json_data["JSONItem"]
        )

        set_page_conf = set()

        for x in find_keys(json_page_conf, ["tableName", "listTitle"]):
            set_page_conf.add(x)

            if result.get(x, None) is None:
                result[x] = set()
            result[x].add(key)

    for k in result:
        result[k] = list(result[k])
    return result




def find_keys_in_list(node: list, kv: List[str]) -> Generator[str, str, None]:
    for item in node:
        yield from find_keys(item, kv)

def find_keys_in_dict(node: dict, kv: List[str]) -> Generator[str, str, None]:
    for elem in kv:
        if elem in node:
            yield node[elem].replace(" ", "")
    for value in node.values():
        yield from find_keys(value, kv)

def find_keys(node: dict | list, kv: List[str]) -> Generator[str, str, None]:
    if isinstance(node, list):
        yield from find_keys_in_list(node, kv)
    elif isinstance(node, dict):
        yield from find_keys_in_dict(node, kv)

async def get_table_keys(tables: list, token: str) -> list:
    get_keys = await get_widgets(token)
    key_list = []
    if len(tables) > 0:
        for x in tables:
            keys = get_keys.get(x, None)
            if keys is not None:
                for i in keys:
                    key_list.append(i)
    return key_list
