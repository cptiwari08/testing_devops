import React, { useRef, useState, useEffect } from "react";
import "./SourceDescription.scss";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import { squarePlus, squareMinus } from "../../../common/components/AppIcons/Icons";
import t from "../../../locales/en.json";
import { useAppSelector } from "../../hooks/hooks";
import { ISourceConfig } from "../../model";

const SourceDescription: React.FC = () => {
  const [isSourceDescriptionExpanded, setIsSourceDescriptionExpanded] = useState(false);
  const [descriptionDetailHeight, setDescriptionDetailHeight] = useState(0);
  const descriptionDivRef = useRef<HTMLDivElement>(null);
  const userAuthDetails = useAppSelector((state) => state.userAuth.userAuthDetails);
  const sourceConfigs = useAppSelector((state) => state.projectConfig.sourceConfigs);
  const selectedSources: string[] = userAuthDetails?.scope || [];
  const selectedDataSources: ISourceConfig[] = sourceConfigs
    ?.filter((source) => selectedSources.includes(source.key) && !!source.isActive)
    .sort((a, b) => a.ordinal - b.ordinal) || [];

  //Get height of expandable panel after mounting
  useEffect(() => {
    if (descriptionDivRef.current) setDescriptionDetailHeight(descriptionDivRef.current.offsetHeight);
  }, [(descriptionDivRef.current || {}).offsetHeight]);

  const handleOnClick = () => {
    setIsSourceDescriptionExpanded(!isSourceDescriptionExpanded);
  };

  return (
    <article className="source_description">
      <div className="source_description__heading" onClick={handleOnClick}>
        <span className={isSourceDescriptionExpanded ? "open" : ""}>{t.descriptionToSources}</span>
        <AppIcon icon={isSourceDescriptionExpanded ? squareMinus : squarePlus} size="xs" className="source_description__heading_icon" />
      </div>

      <section
        style={{ height: isSourceDescriptionExpanded ? descriptionDetailHeight + "px" : 0 }}
        className={isSourceDescriptionExpanded ? "source_description__detail expanded" : "source_description__detail"}
      >
        <div ref={descriptionDivRef}>
          {selectedDataSources.map((item) => (
            <div key={item?.key} className="source_description__detail_item">
              <span>{item?.displayName}</span>
              <p>{item.description}</p>
            </div>
          ))}
        </div>
      </section>
        <hr/>
    </article>
  );
};

export default SourceDescription;
