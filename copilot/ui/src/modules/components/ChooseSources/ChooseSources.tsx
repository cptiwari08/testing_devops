import React, { useEffect, useMemo, useState } from "react";
import ToggleButton from "../../../common/components/ToggleButton/ToggleButton";
import t from "../../../locales/en.json";
import "./ChooseSources.scss";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import {
  SessionKeys,
  SourceType
} from "../../../common/utility/constants";
import { updateSelectedSources } from "./ChooseSourceSlice";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import { eye } from "../../../common/components/AppIcons/Icons";
import SkeletonLoader from "../../../common/components/SkeletonLoader/SkeletonLoader";
import { ISourceConfig } from "../../model";

interface IViewDocs {
  openViewDocsModal: () => void;
}

const ChooseSources: React.FC<IViewDocs> = (props: IViewDocs) => {
  const dispatch = useAppDispatch();
  const userAuthDetails = useAppSelector(
    (state) => state.userAuth.userAuthDetails
  );
  const userDataSources = useMemo(
    () => userAuthDetails?.scope || [],
    [userAuthDetails]
  );
  const sourceConfigs: ISourceConfig[] = useAppSelector(
    (state) => state.projectConfig.sourceConfigs
  );
  const isSourceConfigLoading = useAppSelector(
    (state) => state.projectConfig.isLoading
  );
  const [selectedSources, setSelectedSources] = useState<string[]>();
  const [includedProjectDocumentCount, setIncludedProjectDocumentCount] =
    useState<number>(0);
  const [excludedProjectDocs, setExcludedProjectDocs] = useState<string[]>([]);

  const projectDocumentList = useAppSelector((state) => {
    const _excludedDocs = JSON.parse(
      sessionStorage.getItem(SessionKeys.EXCLUDED_PROJECT_DOCS) || "[]"
    );
    if (excludedProjectDocs.length !== _excludedDocs.length)
      setExcludedProjectDocs(_excludedDocs);
    return state.projectDocs.list;
  });

  const isViewDocsLoading = useAppSelector(
    (state) => state.projectDocs.isLoading
  );

  useEffect(() => {
    let count = projectDocumentList.length;
    if (excludedProjectDocs.length && projectDocumentList.length) {
      const includedDocs = projectDocumentList.filter(
        (item) => !excludedProjectDocs.includes(item.id)
      );
      count = includedDocs.length;
    }
    setIncludedProjectDocumentCount(count);
  }, [projectDocumentList, excludedProjectDocs]);

  useEffect(() => {
    if (!isSourceConfigLoading && sourceConfigs) {
      const storedSelectedSources = sessionStorage.getItem("selectedSources");
      const parsedSelectedSources = storedSelectedSources
        ? JSON.parse(storedSelectedSources)
        :  sourceConfigs?.filter((config) => config.isActive && config.isDefault)?.sort((a, b) => a.ordinal - b.ordinal)?.map((config) => config.key)
      setSelectedSources(parsedSelectedSources.slice(0, 2));
    }
  }, [isSourceConfigLoading, sourceConfigs]);

  const filteredSources: ISourceConfig[] = useMemo(() => {
    if (isSourceConfigLoading || !sourceConfigs) {
      return [];
    }
    return sourceConfigs
    ?.filter((config) => config.isActive)
    .filter((config) => config.key !==  SourceType.ProjectDoc || !!userAuthDetails.sp_url)
    .sort((a, b) => a.ordinal - b.ordinal) || [];
 
  }, [isSourceConfigLoading, sourceConfigs, userDataSources, userAuthDetails.sp_url]);

  useEffect(() => {
    dispatch(updateSelectedSources(selectedSources));
  }, [dispatch, selectedSources]);

  const handleSelect = (selectedSource: ISourceConfig): void => {
    setSelectedSources((prevSelectedSources) => {
      const isSelected = prevSelectedSources?.includes(selectedSource.key);
      if (prevSelectedSources?.length === 1 && isSelected) {
        return prevSelectedSources;
      }
  
      let updatedSources: string[] = [];
      if (isSelected) {
        updatedSources = prevSelectedSources?.filter((source) => source !== selectedSource.key) || [];
      } else {
        if (prevSelectedSources?.length! < 2) {
          updatedSources = [...(prevSelectedSources || []), selectedSource.key];
        } else {
          updatedSources = [prevSelectedSources![1], selectedSource.key];
        }
      }
      sessionStorage.setItem("selectedSources", JSON.stringify(updatedSources));
      return updatedSources;
    });
  };

  const handleViewDocs = () => {
    props.openViewDocsModal();
  };

  const getSharePointMessageTooltipText = (spUrl?: string): JSX.Element => {
    if (!spUrl) {
      return <>{t.noSharePointDocumentFoundContent}</>;
    }
    const parts = t.noSharePointDocumentFoundContent.split("SharePoint");
    return (
      <>
        {parts[0]}
        <a href={spUrl} target="_blank" rel="noopener noreferrer">SharePoint</a>
        {parts[1]}
      </>
    );
  };
  return isSourceConfigLoading || isViewDocsLoading ? (
    <SkeletonLoader/>
  ) : (
    <>
   
      <div className="choose-sources">
        <div className="choose-sources__title">{t.chooseUpto2Sources}</div>
        <div className="choose-sources__container">
          {filteredSources.length > 0 && (
            <div className="choose-sources__row">
              {filteredSources.map(({ key, displayName }) => (
                <ToggleButton
                  name={key}
                  projectDocsCount={projectDocumentList.length}
                  projectDocsIncludedCount={includedProjectDocumentCount}
                  key={key}
                  label={displayName}
                  selected={selectedSources !== undefined && selectedSources.includes(key)}
                  onSelect={() => handleSelect(filteredSources.find(source => source.key === key)!)}
                  disabled={key === SourceType.ProjectDoc && includedProjectDocumentCount === 0}
                  tooltipTextTitle={t.noSharePointDocumentFoundTitle}
                  tooltipTextContent={getSharePointMessageTooltipText(userAuthDetails?.sp_url)}
                />
              ))}
            </div>
          )}
        </div>
      </div>

      {selectedSources?.includes(SourceType.ProjectDoc) ? (
        <section className="view_docs_link">
          <span className="view_docs_link__title" onClick={handleViewDocs}>
            {t.viewDocs}
          </span>{" "}
          <span className="view_docs_link__icon" onClick={handleViewDocs}>
            <AppIcon
              icon={eye}
              style={{ color: "var(--link-primary-color)" }}
              className="scroll-buttons_icon"
            />
          </span>
        </section>
      ) : null}
    </>
  );
};

export default ChooseSources;
