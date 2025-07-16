import React, { useEffect, useMemo, useRef, useState } from "react";
import "./SuggestionConfigurations.scss";
import ToggleSwitch from "../../../common/components/ToggleSwitch/ToggleSwitch";
import t from "../../../locales/en.json";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { AgGridReact } from "ag-grid-react";
import "@ag-grid-community/styles/ag-grid.css";
import "@ag-grid-community/styles/ag-theme-quartz.css";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import {
  pencil,
  trash,
  magnifyingGlass,
  plus,
  trinagleWarning,
} from "../../../common/components/AppIcons/Icons";

import { getAllSuggestions } from "../Suggestions/SuggestionsSlice";
import {
  IState,
  assistantConfigurationAction
} from "../../pages/AssistantConfigurations/AssistantConfigurationsSlice";
import { ISuggestionsData } from "../../model";
import SuggestionAlterModal from "../SuggestionAlterModal/SuggestionAlterModal";
import AppModal from "../../../common/components/AppModal/AppModal";
interface IAlterSuggestion {
  data?: Partial<ISuggestionRecord>;
  mode: string;
  isVisible: boolean;
}

export interface ISuggestionRecord extends Partial<ISuggestionsData> {
  appAffinityDisplayName?: string;
  sourceDisplayName?:string;
}

const SuggestionConfigurations = () => {
  enum GridColumnKeys {
    suggestionText = "suggestionText",
    appAffinity = "appAffinity",
    source = "source",
    isIncluded = "isIncluded",
    id = "id",
  }
  const dispatch = useAppDispatch();
  const [suggestionFilterText, setSuggestionFilterText] = useState<string>("");
  const [isDeleteConfirmVisible, setIsDeleteConfirmVisible] = useState(false);
  const [rowIdToDelete, setRowIdToDelete] = useState<number>(0);
  const [alterModalState, setAlterModalState] = useState<IAlterSuggestion>({
    mode: "",
    isVisible: false,
  });
  const suggestions: Partial<ISuggestionRecord>[] = useAppSelector(
    (state) => state.suggestions.suggestions
  );
  const assistantConfigSlice: IState = useAppSelector((state) => {
    return state.AssistantConfigurationsSlice;
  });
  const sourceConfigs = useAppSelector(
    (state) => state.projectConfig.sourceConfigs
  );
  const userApps = useAppSelector(
    (state) => state.projectConfig.userApps
  );
  const gridRef = useRef<AgGridReact>(null);
  const [suggestionRows, setSuggestionRows] = useState<any[]>([]);

  useEffect(() => {
    if (
      assistantConfigSlice.isSuggestionAddSuccess ||
      assistantConfigSlice.isSuggestionDeleteSuccess ||
      assistantConfigSlice.isSuggestionSaveSuccess
    ) {
      if(alterModalState.isVisible){
        setAlterModalState({ mode: "", isVisible: false });
      }
      dispatch(getAllSuggestions());
    }
  }, [assistantConfigSlice.isSuggestionAddSuccess, assistantConfigSlice.isSuggestionDeleteSuccess, assistantConfigSlice.isSuggestionSaveSuccess]);

  useEffect(() => {
    transformDataForGrid();
  }, [suggestions]);

  const transformDataForGrid = () => {
    const mappedRows: any[] = suggestions.map((item) => {
      //Mapping Apps
      const apps = (item?.appAffinity || "").split(",").map((app) => {
        const appKV = userApps.find((uApp) => uApp.key === app.trim());
        return appKV?.name;
      });

      //Mapping Source
      const sources = (item?.source || "").split(",").map((source) => {
        const sourceKV = sourceConfigs.find((s) => s.key === source.trim());
        return sourceKV?.displayName;
      });

      const _sources = sources.join(", ").trim() || item.source;
      const _apps = apps.join(", ").trim() || item.appAffinity;
      return {
        ...item,
        sourceDisplayName: _sources,
        appAffinityDisplayName: _apps,
      };
    });
    setSuggestionRows(mappedRows);
  };

  const actionIconRenderer = (rowObject: any) => (
    <div className="manage-assistant-page__action-button">
      <span>
        <AppIcon icon={pencil} onClick={() => onSuggestionEdit(rowObject)} />
      </span>
      <span>
        <AppIcon icon={trash} onClick={() => onSuggestionDelete(rowObject)} />
      </span>
    </div>
  );

  const onSuggestionSearch = (input: any) => {
    setSuggestionFilterText(input.target.value);
  };

  const onSuggestionAdd = () => {
    dispatch(assistantConfigurationAction.clearQuestionSaveResponse());
    setAlterModalState({ mode: "add", isVisible: true });
  };

  const onSuggestionDelete = (node: any) => {
    setIsDeleteConfirmVisible(true);
    setRowIdToDelete(node.data.id);
  };
  const deleteRow = () => {
    dispatch(assistantConfigurationAction.deleteSuggestion(rowIdToDelete));
    setIsDeleteConfirmVisible(false);
  };

  const onSuggestionEdit = (node: any) => {
    dispatch(assistantConfigurationAction.clearQuestionSaveResponse());
    setAlterModalState({
      mode: "edit",
      isVisible: true,
      data: { ...node.data },
    });
  };

  const onToggleSwitchChange = (node: any) => {
    const payload = {
      ...node.data,
      [GridColumnKeys.isIncluded]: !node.data["isIncluded"],
    };
    node.setData(payload);
    dispatch(assistantConfigurationAction.saveSuggestion([{id:payload.id,isIncluded:payload.isIncluded}]));
  };

  const onAlterModalSave = () => {
    //Action on save click
  };

  const onAlterModalCancel = () => {
    setAlterModalState({ mode: "", isVisible: false });
  };

  const toggleSwitchRenderer = (rowObject: any) => {
    return (
      <span className="manage-assistant-page__toggle-switch">
        {rowObject.data.visibleToAssistant ? (
          <ToggleSwitch
            onChange={() => onToggleSwitchChange(rowObject.node)}
            type={2}
            checked={rowObject.value}
          />
        ) : (
          <ToggleSwitch
            onChange={() => {}}
            type={2}
            checked={rowObject.value}
            disabled={true}
          />
        )}
      </span>
    );
  };

  const colDefs = useMemo<any[]>(
    () => [
      { headerName: t.Questions, field: "suggestionText", flex: 3, filter: true },
      { headerName: t.apps, field: "appAffinityDisplayName", flex: 1, filter: true },
      { headerName: t.source, field: "sourceDisplayName", flex: 1, filter: true },
      {
        headerName: t.Show_Suggestion,
        field: "isIncluded",
        flex: 1,
        filter: true,
        cellRenderer: toggleSwitchRenderer,
      },
      { headerName: "", field: "", flex: 1, cellRenderer: actionIconRenderer },
    ],
    []
  );

  return (
    <>
      <section className="manage-assistant-page__detail_right__head">
        <div className="manage-assistant-page__detail_right__head__left">
          <div>
            <span className="manage-assistant-page__detail_right__head__left__title">
              {t.Question_Lib}
            </span>
            <span className="manage-assistant-page__detail_right__head__left__count">
              {suggestions.length}
            </span>
          </div>
        </div>
        <div className="manage-assistant-page__detail_right__head__right">
          <span
            className="manage-assistant-page__detail_right__head__right__add"
            onClick={onSuggestionAdd}
          >
            <AppIcon icon={plus} /> {t.Add_Question}
          </span>
        </div>
      </section>
      <section className="manage-assistant-page__detail_right__body">
        <div className="manage-assistant-page__detail_right__body__search">
          <span className="manage-assistant-page__detail_right__body__search__input_wrapper">
            <AppIcon icon={magnifyingGlass} />
            <input
              type="text"
              placeholder={t.search}
              onInput={onSuggestionSearch}
            />
          </span>
        </div>
        <div className="manage-assistant-page__detail_right__body__grid">
          <div className="ag-theme-quartz manage-assistant-page__detail_right__body__grid__theme">
            <AgGridReact
              ref={gridRef}
              rowData={suggestionRows}
              columnDefs={colDefs}
              quickFilterText={suggestionFilterText}
            />
          </div>
        </div>
      </section>

      {alterModalState.isVisible && (
        <SuggestionAlterModal
          mode={alterModalState.mode}
          data={alterModalState.data}
          onSave={onAlterModalSave}
          onClose={onAlterModalCancel}
          onCancel={onAlterModalCancel}
        />
      )}

      <AppModal
        showModal={isDeleteConfirmVisible}
        heading={t.AreYouSure}
        description={t.DeleteQuestionConfirmText}
        confirmBtnText={t.YesDelete}
        cancelButtonText={t.cancel_text}
        onClose={() => setIsDeleteConfirmVisible(false)}
        onCancel={() => setIsDeleteConfirmVisible(false)}
        onConfirm={deleteRow}
        iconName={trinagleWarning}
      />
    </>
  );
};

export default SuggestionConfigurations;
