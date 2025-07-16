import React, { useEffect, useState } from "react";
import "./AssistantProjectContext.scss";
import t from "../../../locales/en.json";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { assistantConfigurationAction } from "../../pages/AssistantConfigurations/AssistantConfigurationsSlice";
import { CONTEXT_KEY, IconSizes, MAX_PROJECT_CONTEXT_LENGTH } from "../../../common/utility/constants";
import { containsXSS, startsWithSpecialChar } from "../../../common/utility/utility";
import { checkMark } from "../../../common/components/AppIcons/Icons";
import FaIcons from "../../../common/components/AppIcons/AppIcons";

const AssistantProjectContext: React.FC = () => {
  const dispatch = useAppDispatch();
  const [focused, setFocused] = useState<boolean>(false);
  const [descriptionText, updateDescriptionText] = useState<string>("");
  const [validationError, setValidationError] = useState<string>("");
  const assistantConfigDetails = useAppSelector(
    (state) => state.AssistantConfigurationsSlice.assistantContextDetail
  );
  const isProjectContextDirty: boolean = useAppSelector((state) => {
    return state.AssistantConfigurationsSlice.isProjectContextDirty;
  });
  const handleFocus = (value: boolean) => {
    setFocused(value);
  };
  const validateTextArea = (text: string) => {
    if (startsWithSpecialChar(text)) {
      const firstChar = text.trim().split("")[0];
      setValidationError(`${t.special_character_error} ${firstChar}`);
      return false;
    } 
    setValidationError("");
    return true;
  };
  const saveProjectContext = () => {
    dispatch(
      assistantConfigurationAction.saveProjectContext({
        id: assistantConfigDetails.id,
        Key: CONTEXT_KEY,
        Value: descriptionText,
        Title: "",
      })
    );
  }
  const checkIfValueUpdated = (value:string) => {
    let contextUpdated=false;
    if (value === assistantConfigDetails.value) {
      contextUpdated=false;
    }else{
      contextUpdated=true;
    }
   dispatch(assistantConfigurationAction.setDirtyFlag({key:'isProjectContextDirty',value:contextUpdated}));
  };
  useEffect(()=>{
    checkIfValueUpdated(descriptionText)
  },[descriptionText]);

  useEffect(() => {
    dispatch(
      assistantConfigurationAction.getProjectContext({ Key: CONTEXT_KEY })
    );
  }, [dispatch]);

  useEffect(() => {
    updateDescriptionText(assistantConfigDetails.value);
  }, [assistantConfigDetails]);

  return (
    <section className="add-project-modal">
      <div className="add-project-modal__title"> {t.project_config_title}</div>
      <div
        className="add-project-modal__description"
        dangerouslySetInnerHTML={{
          __html: t.project_config_description,
        }}
      ></div>

      <div
        className={`add-project-modal__text-area-wrapper ${focused || descriptionText ? "focused" : ""}`}
      >
        <textarea
          className="add-project-modal__custom-text-area"
          value={descriptionText}
          maxLength={MAX_PROJECT_CONTEXT_LENGTH}
          onChange={(e) => {
            validateTextArea(e.target.value);
            updateDescriptionText(e.target.value);
          }}
          onFocus={() => handleFocus(true)}
          onBlur={() => handleFocus(false)}
          placeholder={t.assistant_projectContext_placeHolder}
        />
      </div>
      {validationError && (
        <p className="add-project-modal__error">{validationError}</p>
      )}
      <div className='add-project-modal__reset_btn'>
        <span className ={`${isProjectContextDirty?'':' disabled'}`} onClick={saveProjectContext}>
              <FaIcons
                  icon={checkMark}
                  size={IconSizes.SMALL}
                /> {t.Update}
        </span>
        </div>

    </section>
  );
};

export default AssistantProjectContext;
