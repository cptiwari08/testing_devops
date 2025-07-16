import React, { useEffect, useState } from "react";
import "./AssistantSwitch.scss";
import ToggleSwitch from "../../../common/components/ToggleSwitch/ToggleSwitch";
import t from "../../../locales/en.json";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { assistantConfigurationAction } from "../../pages/AssistantConfigurations/AssistantConfigurationsSlice";
import { ASSISTANT_KEY } from "../../../common/utility/constants";
import toggleBannerImg from "../../../assets/img/Vector_Local.svg";

const AssistantSwitch: React.FC = () => {
  const dispatch = useAppDispatch();
  const [isChecked, setIsChecked] = useState<boolean>(false);
  const assistantEnableDetail = useAppSelector(
    (state) => state.AssistantConfigurationsSlice.assistantEnableDetail
  );
  const handleToggleChange = (
    checked: boolean | ((prevState: boolean) => boolean)
  ) => {
    setIsChecked(checked);
    dispatch(
      assistantConfigurationAction.saveAssistantToggle({
        id: assistantEnableDetail.id,
        Key: ASSISTANT_KEY,
        value: checked ? "true" : "false",
        Title: "",
      })
    );
  };

  useEffect(() => {
    dispatch(
      assistantConfigurationAction.getProjectAssistantFlag({
        Key: ASSISTANT_KEY,
      })
    );
  }, [dispatch]);

  useEffect(() => {
    const isAssistantChecked =
      assistantEnableDetail?.value.toLowerCase() === "true" ? true : false;
    setIsChecked(isAssistantChecked);
  }, [assistantEnableDetail]);
  
  return (
    <div className="manage-assistant-page__detail_left__enable_assistant">
      <img className="banner__image" src={toggleBannerImg} alt="banner" />

      <div className="app-toggle">
        <label>{t.toggle_assistant_text}</label>
        <span className="app-toggle__switch">
          <ToggleSwitch
            checked={isChecked}
            onChange={() => handleToggleChange(!isChecked)}
            type={2}
          />
        </span>
      </div>
    </div>
  );
};

export default AssistantSwitch;
