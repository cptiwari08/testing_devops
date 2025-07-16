import React, { useEffect, useState } from "react";
import "./AssistantConfigurations.scss";
import t from "../../../locales/en.json";
import SuggestionConfigurations from "../../components/SuggestionConfigurations/SuggestionConfigurations";
import AssistantSwitch from "../../components/AssistantSwitch/AssistantSwitch";
import AssistantProjectContext from "../../components/AssistantProjectContext/AssistantProjectContext";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import {
  checkMark,
  refresh,
  trinagleWarning,
  xMark,
} from "../../../common/components/AppIcons/Icons";
import { useAppSelector } from "../../hooks/hooks";
import {
  IState,
  assistantConfigurationAction,
} from "./AssistantConfigurationsSlice";
import Loader from "../../../common/components/Loader/Loader";
import {
  IconSizes,
  SOURCE_CONFIGS_KEY,
} from "../../../common/utility/constants";
import AppModal from "../../../common/components/AppModal/AppModal";
import { ISourceConfig } from "../../model";
import { useDispatch } from "react-redux";
import { compareTwoArraysProperties, containsXSS, startsWithSpecialChar } from "../../../common/utility/utility";
import { getAllSuggestions } from "../../components/Suggestions/SuggestionsSlice";

const AssistantConfigurations: React.FC = () => {
  const [errorMessage, setErrorMessage] = useState<string>("");

  const dispatch = useDispatch();
  const assistantConfigSlice: IState = useAppSelector((state) => {
    return state.AssistantConfigurationsSlice;
  });
  const isSuggestionLoading: boolean = useAppSelector((state) => {
    return state.suggestions.isLoading;
  });
  const isSourceNameUpdateInProgress: boolean = useAppSelector((state) => {
    return state.AssistantConfigurationsSlice.isSourceNameSaveSuccess;
  });
  const sourceNameFailureMessage:string =  useAppSelector((state) => {
    return state.AssistantConfigurationsSlice.sourceNameFailureMessage;
  });
  const [showCancelDialogue, setShowCancelDialogue] = useState(false);
  const [sourceUpdated,setSourceUpdated]=useState(false);
  const sourceConfigs: ISourceConfig[] = useAppSelector(
    (state) => state.projectConfig.sourceConfigs
  );
  const [editSourceConfiguration, setEditSourceConfiguration] = useState<
    ISourceConfig[]
  >([]);
  const sourceConfigurationData = useAppSelector(
    (state) => state.projectConfig.sourceConfigurationData
  );
  const [sourceOriginalNameUpdated, setSourceOriginalNameUpdated] = useState(false);

  const resetSourceConfig = () => {
    const updatedSourceconfig = editSourceConfiguration.map(obj => ({
      ...obj,
      displayName: obj.originalDisplayName
    }));
     updateSourceConfig(updatedSourceconfig);
     setErrorMessage("");
  };

  const updateSourceConfig = (sourceConfigToUpdate:any) => {
    let jsonString = JSON.stringify(sourceConfigToUpdate);
    const sourceConfigPayload = {
      id: sourceConfigurationData.id,
      key: SOURCE_CONFIGS_KEY,
      title: sourceConfigurationData.title,
      isEnabled: sourceConfigurationData.isEnabled,
      value: jsonString,
    };
    dispatch(
      assistantConfigurationAction.updateProjectConfigByKey(sourceConfigPayload)
    );
  };

  const detectChanges = () => {
    if (sourceUpdated || assistantConfigSlice.isProjectContextDirty) {
      setShowCancelDialogue(true);
    } else {
      returnToPrevPage();
    }
  };
  const returnToPrevPage = () => {
    window.parent.postMessage(t.CANCEL_WITHOUT_SAVING, "*"); //  `*` on any domain
  };

  const isValidText = (input: string) => {
    if (startsWithSpecialChar(input)) {
      const firstChar = input.trim().split("")[0];
      setErrorMessage(`${t.special_character_error} ${firstChar}`);
      return false;
    }
    if (containsXSS(input)) {
      setErrorMessage(`${t.xssError}`);
      return false;
    }
    setErrorMessage("");
    return true;
  };

  const editSourceName = (event: any, key: string) => {
    isValidText(event.target.value);
    setEditSourceConfiguration((prevConfigs) =>
      prevConfigs.map((source) =>
        source.key === key
          ? { ...source, displayName: event.target.value }
          : source
      )
    );
  };

  //Loading Questions on - First load / Source name update
  useEffect(()=> {
    if(!isSourceNameUpdateInProgress) dispatch(getAllSuggestions());
  }, [isSourceNameUpdateInProgress]);

  useEffect(() => {
    if (sourceConfigs && sourceConfigs.length) {
      setEditSourceConfiguration(sourceConfigs);
    }
  }, [sourceConfigs]);

  useEffect(()=>{
    const hasMismatch = editSourceConfiguration.some(item => item.displayName !== item.originalDisplayName);
    setSourceOriginalNameUpdated(hasMismatch);
    const hasEmpty = editSourceConfiguration.some(item => item.displayName.trim() === "");
    hasEmpty ? setSourceUpdated(false) :
    setSourceUpdated(compareTwoArraysProperties(sourceConfigs, editSourceConfiguration));
  },[editSourceConfiguration]);

  return (
    <>
      <Loader isLoading={assistantConfigSlice.isLoading || isSuggestionLoading} />
      <article className="manage-assistant-page">
        <section className="manage-assistant-page__detail">
          <div className="manage-assistant-page__detail_left">
            <div className="manage-assistant-page__detail_top_left">
              <AssistantSwitch />
              <div className="manage-assistant-page__detail_left__edit_source_name">
                <div className="manage-assistant-page__detail_left__edit_source_name__title">
                  {t.Edit_source_names}
                </div>
                {editSourceConfiguration &&
                  editSourceConfiguration.map((source) => (
                    <span
                      key={source.key}
                      className="manage-assistant-page__detail_left__edit_source_name__input"
                    >
                      <input
                        key={source.key}
                        type="text"
                        placeholder={source.displayName}
                        value={source.displayName}
                        onChange={(e) => editSourceName(e, source.key)}
                        maxLength={15}
                      />
                    </span>
                  ))}
                  <span className="manage-assistant-page__error">
                        {errorMessage || sourceNameFailureMessage? t.invalidInputErrorMessage : ""}
                  </span>
                <span className="manage-assistant-page__detail_left__edit_source_name__reset_btn">
                  <span onClick={resetSourceConfig} className={`${sourceOriginalNameUpdated?'':' disabled'}`}>
                    <AppIcon icon={refresh} size={IconSizes.SMALL} /> {t.Reset}
                  </span>
                  <span className={`${sourceUpdated && errorMessage === "" ? '': ' disabled'}`} onClick={()=>updateSourceConfig(editSourceConfiguration)}>
                    <AppIcon icon={checkMark} size={IconSizes.SMALL} />{" "}
                    {t.Update}
                  </span>
                </span>
              </div>
            </div>
            <div className="manage-assistant-page__detail_left__add_context">
              <AssistantProjectContext />
            </div>
          </div>
          <div className="manage-assistant-page__detail_right">
            <SuggestionConfigurations />
          </div>
        </section>
        <section className="manage-assistant-page__buttons">
          <span
            className="manage-assistant-page__buttons_cancel"
            onClick={detectChanges}
          >
            <AppIcon icon={xMark} /> {t.cancel_text}
          </span>
          
        </section>
      </article>
      <AppModal
        showModal={showCancelDialogue}
        iconName={trinagleWarning}
        heading={t.changes_not_saved}
        description={t.cancel_dialogue_description}
        confirmBtnText={t.continue_without_saving}
        cancelButtonText={t.cancel_text}
        onClose={() => setShowCancelDialogue(false)}
        onCancel={() => setShowCancelDialogue(false)}
        onConfirm={returnToPrevPage}
      />
    </>
  );
};

export default AssistantConfigurations;
