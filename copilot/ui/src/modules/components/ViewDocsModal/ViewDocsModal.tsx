import React, { useEffect, useState } from "react";
import "./ViewDocsModal.scss";
import t from "../../../locales/en.json";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import {
  file,
  magnifyingGlass,
  arrowDown,
  fileExcel,
  filePdf,
  filePowerpoint,
  fileWord,
  refresh,
} from "../../../common/components/AppIcons/Icons";
import { getProjectDocs, refreshProjectDocs } from "./ViewDocsModalSlice";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { SessionKeys } from "../../../common/utility/constants";
import { IProjectDocs } from "../../model";
import { formatBytes } from "../../../common/utility/utility";
import SquareLoader from "../../../common/components/SquareLoader/SquareLoader";
import Tooltip from "../../../common/components/Tooltip/Tooltip";
import ToggleSwitch from "../../../common/components/ToggleSwitch/ToggleSwitch";

interface IVievDocs {
  closeModal: () => void;
}

const ViewDocsModal: React.FC<IVievDocs> = (props: IVievDocs) => {
  const dispatch = useAppDispatch();
  const [fileCount, setFileCount] = useState<number>(0);
  const [searchQuery, setSearchQuery] = useState<string>("");
  const [fileList, setFileList] = useState<IProjectDocs[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [excludedProjectDocs, setExcludedProjectDocs] = useState(
    JSON.parse(
      sessionStorage.getItem(SessionKeys.EXCLUDED_PROJECT_DOCS) || "[]"
    )
  );
  const userAuthDetails = useAppSelector(
    (state) => state.userAuth.userAuthDetails
  );
  const mainFileList = useAppSelector((state) => {
    if (isLoading !== state.projectDocs.isLoading)
      setIsLoading(state.projectDocs.isLoading);
    return state.projectDocs.list;
  });

  const fileExtIcons: any = {
    file,
    xls: fileExcel,
    xlsx: fileExcel,
    pdf: filePdf,
    doc: fileWord,
    docx: fileWord,
    ppt: filePowerpoint,
    pptx: filePowerpoint,
    pps: filePowerpoint,
    ppsx: filePowerpoint,
  };

  const getFilteredData = () => {
    if (!searchQuery) {
      return mainFileList;
    }
    return mainFileList.filter((item) => {
      return item.name.toLowerCase().includes(searchQuery.toLowerCase());
    });
  };

  useEffect(() => {
    const docsList: string[] = JSON.parse(
      sessionStorage.getItem(SessionKeys.EXCLUDED_PROJECT_DOCS) || "[]"
    );
    setExcludedProjectDocs(docsList);
  }, []);

  useEffect(() => {
    const filteredItem: IProjectDocs[] = getFilteredData();
    setFileList(filteredItem);
    setFileCount((filteredItem || []).length);
  }, [searchQuery, mainFileList]);

  const handleOnInput = (event: React.FormEvent<HTMLInputElement>) => {
    const value = event.currentTarget.value;
    setSearchQuery(value || "");
  };

  const handleFileOpen = (file: IProjectDocs) => {
    const url =
      file.linkingUri ||
      `${new URL(userAuthDetails.sp_url).origin}${file.path}`;
    window.open(url, "_blank");
  };
  const refreshViewDocs = () => {
    dispatch(refreshProjectDocs());
  };

  const handleToggleChange = (id: string) => {
    let docsList: string[] = JSON.parse(
      sessionStorage.getItem(SessionKeys.EXCLUDED_PROJECT_DOCS) || "[]"
    );
    const existIndex = docsList.indexOf(id);
    if (existIndex !== -1) docsList.splice(existIndex, 1);
    else docsList.push(id);
    if (docsList.length) {
      sessionStorage.setItem(
        SessionKeys.EXCLUDED_PROJECT_DOCS,
        JSON.stringify(docsList)
      );
    } else {
      sessionStorage.removeItem(SessionKeys.EXCLUDED_PROJECT_DOCS);
    }
    setExcludedProjectDocs(docsList);
  };

  return (
    <article className="viewdocs-modal">
      <section className="viewdocs-modal__header">
        <div className="viewdocs-modal__header__left-side">
          <span className="viewdocs-modal__header__left-side__title">
            {t.viewDocuments}
            <small>
              {fileCount - excludedProjectDocs.length}/{fileCount}
            </small>
          </span>
        </div>
        <div className="viewdocs-modal__header__right-btns">
          <div
            className="viewdocs-modal__header__right-side"
            onClick={refreshViewDocs}
          >
            <Tooltip tooltipTitle="Refresh" position="bottom">
              <span className="viewdocs-modal__header__right-side__back">
                <AppIcon
                  icon={refresh}
                  className="viewdocs-modal__header__right-side__back_icon"
                ></AppIcon>
              </span>
            </Tooltip>
          </div>
          <div
            className="viewdocs-modal__header__right-side"
            onClick={props.closeModal}
          >
            <Tooltip tooltipTitle="Back" position="bottom">
              <span className="viewdocs-modal__header__right-side__back">
                <AppIcon
                  icon={arrowDown}
                  className="viewdocs-modal__header__right-side__back_icon"
                ></AppIcon>
              </span>
            </Tooltip>
          </div>
        </div>
      </section>
      {mainFileList.length > 0 ? (
        <section className="viewdocs-modal__body">
          <div className="viewdocs-modal__body__search">
            <span className="viewdocs-modal__body__search__input">
              <AppIcon icon={magnifyingGlass}></AppIcon>
              <input
                type="text"
                placeholder={t.search}
                onInput={handleOnInput}
              />
            </span>
          </div>
          <div className="viewdocs-modal__body__table-head">
            <span>{t.fileName}</span>
            <span></span>
          </div>
          <section className="scrollable">
            {fileList.map((_file) => (
              <div className="viewdocs-modal__body__item" key={_file.id}>
                <div className="item-wrapper">
                  <div className="viewdocs-modal__body__item__icon">
                    <span onClick={() => handleFileOpen(_file)}>
                      <AppIcon
                        icon={fileExtIcons["file"]}
                        className="feedback-section__header__close_icon"
                      ></AppIcon>
                    </span>
                  </div>
                  <span className="viewdocs-modal__body__item__label">
                    <span
                      className="viewdocs-modal__body__item__label__filename"
                      onClick={() => handleFileOpen(_file)}
                    >
                      {_file.name}
                    </span>
                    <small className="viewdocs-modal__body__item__label__filesize">
                      {formatBytes(_file.size)}
                    </small>
                  </span>
                </div>
                <span>
                  {(excludedProjectDocs.length + 1 === mainFileList.length && !(
                          excludedProjectDocs.includes(_file.id) ||
                          _file.visibleToAssistant === "No"
                        )) ? (<Tooltip
                      tooltipTitle={t.excludedDocsTooltipTitle}
                      content={t.excludedDocsTooltipDescription}
                      position="left"
                      customStyle={{"maxWidth": "400px"}}
                    >
                      <ToggleSwitch
                        type={2}
                        checked={
                          !(
                            excludedProjectDocs.includes(_file.id) ||
                            _file.visibleToAssistant === "No"
                          )
                        }
                        onChange={() => {}}
                      />
                    </Tooltip>) : (<ToggleSwitch
                      type={2}
                      checked={
                        !(
                          excludedProjectDocs.includes(_file.id) ||
                          _file.visibleToAssistant === "No"
                        )
                      }
                      onChange={() => handleToggleChange(_file.id)}
                    />
                  )}
                </span>
              </div>
            ))}
          </section>
        </section>
      ) : (
        <section className="viewdocs-modal__errorsection">
          <div className="viewdocs-modal__errorsection__body">
            {isLoading ? (
              <SquareLoader />
            ) : (
              <>
                <span className="viewdocs-modal__errorsection__body_icon">
                  <AppIcon icon={magnifyingGlass}></AppIcon>
                </span>
                <span className="viewdocs-modal__errorsection__body_title">
                  {t.no_document_found}
                </span>
                <span
                  className="viewdocs-modal__errorsection__body_message"
                  dangerouslySetInnerHTML={{
                    __html: t.no_document_found_message,
                  }}
                ></span>
              </>
            )}
          </div>
        </section>
      )}
    </article>
  );
};

export default ViewDocsModal;
