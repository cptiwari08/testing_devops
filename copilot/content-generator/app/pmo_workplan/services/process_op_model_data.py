from app.core.prompt_manager import create_prompt_manager
from app.core.program_office_api import ProgramOfficeResponse
import pandas as pd
from typing import List, Dict, Any, Union
import textwrap
import asyncio

async def process_op_model_data(
    program_office,
    eyip_ids_list: List[int],
    max_level: int = 2,
) -> List[Dict[str, Any]]:
    """
    For all ID in `eyip_ids_list` a query is executed    
    """
    if not eyip_ids_list:        # back
        return []

    # --- SQL for `target_gp_id` -----------------
    def build_query(target_gp_id: int) -> str:
        return textwrap.dedent(
            f"""
            /* =========  Recursive tree walk ========= */
            WITH RecursiveCTE AS (
                /* -------- Level 0 : root PROCESS nodes -------- */
                SELECT
                    n.ID          AS ProcessID,
                    n.Title       AS ProcessName,
                    n.ID          AS CurrentID,
                    n.Title       AS CurrentTitle,
                    n.NodeTypeId  AS CurrentNodeType,
                    n.NodeParentID,
                    0             AS Level
                FROM Nodes n
                WHERE n.NodeTypeId = 3

                UNION ALL

                /* -------- Level 1 … {max_level} -------- */
                SELECT
                    r.ProcessID,
                    r.ProcessName,
                    p.ID          AS CurrentID,
                    p.Title       AS CurrentTitle,
                    p.NodeTypeId  AS CurrentNodeType,
                    p.NodeParentID,
                    r.Level + 1
                FROM RecursiveCTE r
                JOIN Nodes p ON r.NodeParentID = p.ID
                WHERE r.Level < {max_level}
            )

            /* =========  Final projection ========= */
            SELECT
                ProcessID,
                ProcessName,
                MAX(CASE WHEN Level = 1 THEN CurrentID    END) AS ParentID,
                MAX(CASE WHEN Level = 1 THEN CurrentTitle END) AS ParentName,
                MAX(CASE WHEN Level = 2 THEN CurrentID    END) AS GrandParentID,
                MAX(CASE WHEN Level = 2 THEN CurrentTitle END) AS GrandParentName,
                d.Title        AS Disposition,
                d.Description  AS Disposition_Desc
            FROM RecursiveCTE
            /*  Join with Dispositions  */
            LEFT JOIN NodesToDispositionsForDispositionNew nd
                   ON ProcessID = nd.NodeID
            LEFT JOIN Dispositions d
                   ON nd.DispositionID = d.ID
            GROUP BY
                ProcessID,
                ProcessName,
                d.Title,
                d.Description
            /*  Keep only the branch whose grand-parent == `target_gp_id`.  */
            HAVING MAX(CASE WHEN Level = 2 THEN CurrentID END) = {target_gp_id}
            ORDER BY ProcessID
            OPTION (MAXRECURSION 100);
            """
        )
    
    # --- helper async  ---------------------------
    async def fetch_for_gp_id(gp_id: int) -> List[Dict[str, Any]]:
        response = await program_office.run_sql(
            sql_query=build_query(gp_id),
            tables=["Nodes", 
                    "NodesToDispositionsForDispositionNew", 
                    "Dispositions"],
        )
        return response.data or []  

    # --- excecute all --------------------------------------
    tasks = [fetch_for_gp_id(gpid) for gpid in eyip_ids_list]
    results_per_id: List[List[Dict[str, Any]]] = await asyncio.gather(*tasks)

    # --- flatened ------------------------------------------
    flat_results: List[Dict[str, Any]] = [
        row for sublist in results_per_id for row in sublist
    ]

    # Do we need DataFrame? Use the following --------------------
    # df_all = pd.DataFrame(flat_results)
    # return df_all.to_dict(orient="records")
    return flat_results


async def process_op_model_data_v2(
    program_office,
    eyip_ids_list: List[int],
    max_level: int = 2,
) -> List[Dict[str, Any]]:
    """
    Fetch the hierarchy (Process → Parent → Grand-parent) plus Disposition
    for every node that has *any* relationship (self/parent/grand-parent)
    with an ID in `eyip_ids_list`.

    The function is API-compatible with the old version but issues *one*
    query that relies on a single IN () predicate instead of looping.
    """
    if not eyip_ids_list:                      # nothing to do
        return []

    ids_csv = ", ".join(str(x) for x in eyip_ids_list)

    # 1) Build the SQL query
    sql = textwrap.dedent(f"""
        SELECT 
            child.ID AS ProcessID,
            child.Title AS ProcessName,
            NT.Title AS ProcessType, 
            parent.ID AS ParentID,
            CASE 
                WHEN parent.ID IN ({ids_csv}) THEN 
                    CASE WHEN parent_nt.[Key] = 'BUSINESS_ENTITY' THEN NULL ELSE parent.Title END
                ELSE NULL
            END AS ParentName,
            grandparent.ID AS GrandParentID,
            CASE 
                WHEN grandparent.ID IN ({ids_csv}) THEN 
                    CASE WHEN grandparent_nt.[Key] = 'BUSINESS_ENTITY' THEN NULL ELSE grandparent.Title END
                ELSE NULL
            END AS GrandParentName,
            d.Title AS Disposition,
            d.[Description] AS Disposition_Desc
        FROM 
            Nodes child
        INNER JOIN 
            NodeTypes NT ON child.NodeTypeId = NT.ID
        LEFT JOIN 
            Nodes parent ON child.NodeParentId = parent.ID
        LEFT JOIN 
            NodeTypes parent_nt ON parent.NodeTypeId = parent_nt.ID
        LEFT JOIN 
            Nodes grandparent ON parent.NodeParentId = grandparent.ID
        LEFT JOIN 
            NodeTypes grandparent_nt ON grandparent.NodeTypeId = grandparent_nt.ID
        LEFT JOIN 
            NodesToDispositionsForDispositionNew nd ON child.ID = nd.NodeID
        LEFT JOIN 
            Dispositions d ON nd.DispositionID = d.ID
        WHERE 
            ISNULL(grandparent.ID,-1) = grandparent.ID
            AND
            (
                grandparent.ID IN ({ids_csv})
                OR 
                parent.ID IN ({ids_csv})
                OR
                child.ID IN ({ids_csv})
            )
        ORDER BY 
            ProcessID
       
    """)

    
    # 2) Single round-trip to the DB
    
    response = await program_office.run_sql(
        sql_query=sql,
        tables=[
            "Nodes",
            "NodesToDispositionsForDispositionNew",
            "Dispositions",
            "NodeTypes"
        ],
    )

    return response.data or []
