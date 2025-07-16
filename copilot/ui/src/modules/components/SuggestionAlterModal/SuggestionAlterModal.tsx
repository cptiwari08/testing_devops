import React, { useEffect, useState } from "react";
import "./SuggestionAlterModal.scss";
import { xMark } from "../../../common/components/AppIcons/Icons";
import {
  IconSizes,
  MAX_SQL_TEXT_LENGTH,
  MAX_SUGGESTION_TEXT_LENGTH,
} from "../../../common/utility/constants";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import {
  IQuestionResponse,
  assistantConfigurationAction,
} from "../../pages/AssistantConfigurations/AssistantConfigurationsSlice";
import MultiSelectDropdown, {
  Option,
} from "../../../common/components/MultiSelectDropDown/MultiSelectDropDown";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import t from "../../../locales/en.json";
import { ISuggestionRecord } from "../SuggestionConfigurations/SuggestionConfigurations";
import {
  containsXSS,
  startsWithSpecialChar,
} from "../../../common/utility/utility";

interface ModalProps {
  mode: string;
  data?: ISuggestionRecord;
  onClose?: () => void;
  onCancel?: () => void;
  onSave?: () => void;
}

const SuggestionAlterModal: React.FC<ModalProps> = ({
  mode,
  data,
  onClose,
  onCancel,
  onSave,
}) => {
  const PROJECT_DATA_KEY = "project-data";
  const dispatch = useAppDispatch();
  const sourceConfigs = useAppSelector(
    (state) => state.projectConfig.sourceConfigs
  );
  const userApps = useAppSelector((state) => state.projectConfig.userApps);

  const saveResponses: IQuestionResponse[] = useAppSelector(
    (state) => state.AssistantConfigurationsSlice.suggestionSaveResponseList
  );

  const [selectedApps, setSelectedApps] = useState<string[]>([]);
  const [selectedSources, setSelectedSources] = useState<string[]>([]);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [record, setRecord] = useState<Partial<ISuggestionRecord>>({
    source: "",
    suggestionText: "",
    appAffinity: "",
    isIncluded: true,
    visibleToAssistant: true,
    answerSQL: "",
  });

  const [sources, setSources] = useState<Option[]>([]);
  const [apps, setApps] = useState<Option[]>([]);

  useEffect(() => {
    const sourceList: Option[] = sourceConfigs.map((item) => ({
      value: item.key,
      label: item.displayName,
    }));
    setSources(sourceList);

    const userAppList = userApps.map((user) => ({
      value: user.key,
      label: user.name,
    }));
    setApps([...userAppList]);
  }, [sourceConfigs]);

  useEffect(() => {
    if (data) setRecord({ ...data });

    if (data?.appAffinity) {
      const apps = data.appAffinity.split(",").map((app) => app.trim());
      setSelectedApps(apps);
    }
    if (data?.source) {
      const sources = data.source.split(",").map((source) => source.trim());
      setSelectedSources(sources);
    }
  }, [data]);

  const save = () => {
    const {
      id,
      source,
      suggestionText,
      appAffinity,
      isIncluded,
      visibleToAssistant,
      answerSQL,
    } = record;
    let payload = {
      source,
      suggestionText,
      appAffinity,
      isIncluded,
      visibleToAssistant,
      answerSQL,
    };
    
    if (!source?.includes("project-data")) delete payload.answerSQL;

    if (mode === "edit") {
      dispatch(
        assistantConfigurationAction.saveSuggestion([{ id, ...payload }])
      );
    } else {
      dispatch(assistantConfigurationAction.addSuggestion([{ ...payload }]));
    }
    if (onSave) onSave();
  };

  const isQuestionValid = (input: string) => {
    if (startsWithSpecialChar(input)) {
      const firstChar = input.trim().split("")[0];
      setErrorMessage(`${t.Question} ${t.special_character_error} ${firstChar}`);
      return false;
    }
    if (containsXSS(input)) {
      setErrorMessage(`${t.Question} ${t.xssError}`);
      return false;
    }
    setErrorMessage("");
    return true;
  };

  const onDescriptionChange = (event: any) => {
    const value = event.target.value;
    isQuestionValid(value);
    setRecord({ ...record, suggestionText: value });
  };

  const onSqlChange = (event: any) => {
    setRecord({ ...record, answerSQL: event.target.value });
  };

  const onAppDropdownChange = (selectedApps: string[]) => {
    setSelectedApps([...selectedApps]);
    setRecord({ ...record, appAffinity: selectedApps.join(", ") });
  };

  const onSourceDropdownChange = (selectedSources: string[]) => {
    setSelectedSources([...selectedSources]);
    setRecord({ ...record, source: selectedSources.join(", ") });
  };

  return (
    <div className="alter-suggestion-modal">
      <div className="modal-overlay">
        <div className="modal-container">
          <div className="modal-header">
            <span>{mode === "add" ? t.Add_Question : t.Edit_Question}</span>
            <AppIcon
              icon={xMark}
              size={IconSizes.SMALL}
              className=".modal-close-btn"
              onClick={onClose}
            />
          </div>
          <div className="modal-body">
            <article className="alter-suggestion-modal__main">
              <section className="alter-suggestion-modal__main__left">
                <label>
                  {t.Question} ({t.required})
                </label>
                <span>
                  <textarea
                  className={
                    errorMessage.length
                      ? "has-error"
                      : ""
                  }
                    onChange={onDescriptionChange}
                    placeholder={t.EnterQuestionHere}
                    value={record?.suggestionText}
                    maxLength={MAX_SUGGESTION_TEXT_LENGTH}
                  ></textarea>
                  <span className="description-hint">
                    <span>
                      {record?.suggestionText?.length}/
                      {MAX_SUGGESTION_TEXT_LENGTH}
                    </span>
                  </span>
                </span>
              </section>
              <section className="alter-suggestion-modal__main__right">
                <div className="alter-suggestion-modal__main__right_app">
                  <label>
                    {t.AppsLabel} ({t.required})
                  </label>

                  <MultiSelectDropdown
                    options={apps}
                    placeholder={t.SelectApps}
                    selectedValues={selectedApps}
                    onChange={onAppDropdownChange}
                  />
                </div>
                <div className="alter-suggestion-modal__main__right_source">
                  <label>
                    {t.SourceLabel} ({t.required})
                  </label>
                  <MultiSelectDropdown
                    options={sources}
                    selectedValues={selectedSources}
                    onChange={onSourceDropdownChange}
                    placeholder={t.SelectSource}
                  />
                </div>
              </section>
            </article>
            {selectedSources.includes(PROJECT_DATA_KEY) && (
              <section className="alter-suggestion-modal__sql">
                <label>
                  {t.SqlStatement} ({t.required})
                </label>
                <textarea
                  className={
                    saveResponses.some((item) => !item.isValid)
                      ? "has-error"
                      : ""
                  }
                  onChange={onSqlChange}
                  placeholder={t.EnterSqlStatement}
                  value={record?.answerSQL}
                  maxLength={MAX_SQL_TEXT_LENGTH}
                ></textarea>
                <span className="description-hint">
                  <span>
                    {record?.answerSQL?.length}/{MAX_SQL_TEXT_LENGTH}
                  </span>
                </span>
              </section>
            )}
          </div>
          <div>
            <span className="error-message">
              <span>{errorMessage}</span>
              <span>
                {saveResponses.map((item) =>
                  item.isValid ? "" : item.errorString + " "
                )}
              </span>
            </span>
          </div>
          <div className="modal-footer">
            <button
              className={
                record?.suggestionText?.length &&
                !errorMessage.length &&
                selectedApps.length &&
                selectedSources.length &&
                (!selectedSources.includes(PROJECT_DATA_KEY) ||
                  (selectedSources.includes(PROJECT_DATA_KEY) &&
                    record.answerSQL))
                  ? "modal-action-btn modal-action-btn_confirm-btn"
                  : "modal-action-btn modal-action-btn_confirm-btn disabled"
              }
              onClick={save}
            >
              {mode === "add" ? t.Add : t.Save}
            </button>
            <button className="modal-action-btn" onClick={onCancel}>
              {t.cancel_text}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SuggestionAlterModal;
