import React, { useCallback, useEffect, useState } from "react";
import { IAppInfo, IChatSourceResponse, ICitingSource } from "../../model";
import "./References.scss";
import t from "../../../locales/en.json";
import {
  ReferenceMode,
  ReferenceSourceType,
  ReferenceType,
  SourceType,
} from "../../../common/utility/constants";
import { ChatService } from "../../redux/apis/chatApis";
import {
  downloadBlobFile,
  getMimeTypeByFileName,
} from "../../../common/utility/utility";
import { getProjectDocs } from "../../redux/apis/projectApis";
import { useAppSelector } from "../../hooks/hooks";
import AppModal from "../../../common/components/AppModal/AppModal";
import { trinagleWarning } from "../../../common/components/AppIcons/Icons";

interface IReferenceProps {
  appInfo: IAppInfo | undefined;
  sourceResponses: IChatSourceResponse[];
  uniqueId: number;
}

enum ReferenceTypeKeys {
  FileNameReferences = "file_name_reference",
  PdfReferences = "pdf_reference",
  ProjectDataReferences = "project_data_reference"
}

const TOOLTIP_LEFT_PADDING = 55;
const TOOLTIP_TOP_PADDING = 8;

export const ReferenceComponent: React.FC<IReferenceProps> = ({
  appInfo,
  sourceResponses,
  uniqueId,
}) => {
  const [loadingItems, setLoadingItems] = useState<string[]>([]);
  const [refrenceCount, setRefrenceCount] = useState(0);
  const [isCollapse, setIsCollapse] = useState(true);
  const [moreThantwoRows,setMoreThanTwoRows]=useState(false);
  const [redirectUser, setRedirectUser] = useState(false);
  const [redirectURL, setRedirectURL] = useState("");
  const [confirmRedirectMessage, setConfirmRedirectMessage] = useState(t.redirectCitingSourceMessage);
  
  const userAuthDetails = useAppSelector((state) => state.userAuth.userAuthDetails);

  const hasCitingSources = sourceResponses.some((sourceResponse) => {
    const hasCiting = (citingSource: any) =>
      citingSource.sourceValue && citingSource.sourceValue.length > 0;
  
    return sourceResponse.citingSources?.some((citingSource) => {
      return sourceResponse.sourceName === SourceType.ProjectData
        ? hasCiting(citingSource) &&
            citingSource.sourceType === ReferenceType.PAGEKEY.toLowerCase()
        : hasCiting(citingSource);
    });
  });

  const getReferenceType = useCallback((sourceType: string) => {
    switch (sourceType?.toLowerCase()) {
      case ReferenceType.FILENAME.toLowerCase():
        return ReferenceTypeKeys.FileNameReferences;
      case ReferenceType.DOCUMENTS.toLowerCase():
        return ReferenceTypeKeys.PdfReferences;
      case ReferenceType.PAGEKEY.toLowerCase():
        return ReferenceTypeKeys.ProjectDataReferences;
      default:
        return null;
    }
  }, []);

  const showLoading = useCallback((key: string, isLoading: boolean) => {
    setLoadingItems((prev) =>
      isLoading
        ? prev.includes(key)
          ? prev
          : [...prev, key]
        : prev.filter((item) => item !== key)
    );
  }, []);

  const handleProjectDoc = useCallback(async (sourceItem: any, loadingKey: string) => {
    if (sourceItem?.documentId) {
      showLoading(loadingKey, true);
      try {
        const response: any = await getProjectDocs([sourceItem.documentId]);
        if (response && response.length) {
          const [item] = response;
          const spUrl = new URL(userAuthDetails.sp_url);
          window.open(item.linkingUri || item.embedUri || `${spUrl.origin}${item.path}`, "_blank");
        }
      } catch {
        // Handle error if required.
      } finally {
        showLoading(loadingKey, false);
      }
    }
  }, [showLoading]);
    
  const handleUserRedirect = useCallback(() => {
    window.open(redirectURL, "_blank");
    setRedirectUser(false);
  }, [redirectURL]);
  
  const handleProjectData = useCallback((sourceItem: any) => {
    const url = sourceItem.href === "page"
      ? `${userAuthDetails.po_app_url}/${sourceItem.href}/${sourceItem.key}/${appInfo?.id ? `?appId=${appInfo.id}` : ''}`
      : `${userAuthDetails.po_app_url}/${sourceItem.href}/${appInfo?.id ? `?appId=${appInfo.id}` : ''}`;
      
    setRedirectURL(url);
    const message = appInfo
      ? t.redirectCitingSourceMessage.replace("${source}", `${appInfo?.name} | ${sourceItem?.name}`)
      : t.redirectCitingSourceMessage.replace("${source}", sourceItem?.name);
    setConfirmRedirectMessage(message);
    setRedirectUser(true);
  }, [appInfo, userAuthDetails.po_app_url]);
  

  const handleDocumentDownload = useCallback(async (
    sourceItem: any, sourceName: string, referenceType: string, loadingKey: string
  ) => {
    if (referenceType.toLowerCase() === ReferenceType.DOCUMENTS.toLowerCase() && sourceItem?.documentName) {
      const mimeType = getMimeTypeByFileName(sourceItem.documentName);
      showLoading(loadingKey, true);
      try {
        const response: any = await ChatService.getDocumentByGuid(sourceItem.documentGuid || sourceItem.documentId, sourceName);
        downloadBlobFile(new Blob([response], { type: mimeType }), sourceItem.documentName);
      } catch {
        // Handle error if required.
      } finally {
        showLoading(loadingKey, false);
      }
    }
  }, [showLoading]);

  const onReferenceOpen = useCallback((sourceItem: any, sourceName: string, referenceType: string, key: string) => {
    switch (sourceName) {
      case ReferenceSourceType.ProjectDoc:
        handleProjectDoc(sourceItem, key);
        break;
      case ReferenceSourceType.ProjectData:
        handleProjectData(sourceItem);
        break;
      default:
        handleDocumentDownload(sourceItem, sourceName, referenceType, key);
        break;
    }
  }, [handleProjectDoc, handleProjectData, handleDocumentDownload]);

  const getSourceValue = (sourceValue: any, referenceType: string | null) => {
    if (!sourceValue) return "";
    switch (referenceType) {
      case ReferenceType.FILENAME.toLowerCase():
        if (sourceValue?.documentName?.length > 0) {
          return sourceValue?.documentName?.length > 0
            ? sourceValue?.documentName.replace(
                `_${sourceValue?.documentId}`,
                ""
              )
            : "";
        }
        return sourceValue;
      case ReferenceType.DOCUMENTS.toLowerCase():
        return sourceValue?.documentName?.length > 0
          ? sourceValue?.documentName.replace(`_${sourceValue?.documentId}`, "")
          : "";
      case ReferenceType.PAGEKEY.toLowerCase():
          return sourceValue["name"];
      default:
        return "";
    }
  };

  const findHiddenElements = () => {
    const container = document.querySelector(`[data-uniqueid="${uniqueId}"]`);
    const containerRect = container?.getBoundingClientRect();
    const children = container?.children || [];
    let tempCount = 0;
    for (let i = 0; i < children.length; i++) {
      const child = children[i];
      const childRect = child.getBoundingClientRect();
      if (
        (containerRect?.top || 0) + (containerRect?.height || 0) <=
        childRect.top
      ) {
        tempCount++;
      }
    }
    setRefrenceCount(tempCount);
    if(containerRect && containerRect?.height>60){
      setMoreThanTwoRows(true)
    }else{
      setMoreThanTwoRows(false)
    }
  };

  const expandCollapseRef = (mode: string) => {
    if (mode === ReferenceMode.COLLAPSE) {
      setIsCollapse(false);
    } else {
      setIsCollapse(true);
    }
  };

  useEffect(() => {
    calculateHiddenElements();
  }, [isCollapse]);

  const showTooltip = (event: any, item: any) => {
    if (typeof item === "string") return;
    const tooltip = document.getElementById("ref-tooltip");
    if (tooltip) {
      const nodes = tooltip.getElementsByClassName(
        "tooltip-container__tooltip-box__heading"
      );
      const pageNumberString: string = item?.pages
        ?.map((page: { pageNumber: number }) =>
          typeof page === "number" ? page : page.pageNumber
        )
        .sort((a: number, b: number) => a - b)
        .join(", ");
      if (!pageNumberString) return;
      nodes[0].innerHTML = `${t.Page} ${pageNumberString}`;
      const targetRect = event.target.getBoundingClientRect();
      const top =
        targetRect.top +
        window.scrollY -
        tooltip.offsetHeight -
        TOOLTIP_TOP_PADDING;
      tooltip.style.top = top + "px";
      tooltip.style.left =
        event.target.offsetLeft +
        -TOOLTIP_LEFT_PADDING +
        event.target.clientWidth / 2 +
        "px";
    }
    tooltip?.classList.remove("hide-tooltip");
  };

  const hideTooltip = () => {
    document.getElementById("ref-tooltip")?.classList.add("hide-tooltip");
  };
 const calculateHiddenElements= ()=>{
 return setTimeout(() => {
    findHiddenElements();
  }, 100);
 }
  useEffect(() => {
    findHiddenElements();
    let debounceTimer: any = 0;
    window.addEventListener("resize", () => {
      clearTimeout(debounceTimer);
      debounceTimer=calculateHiddenElements();
    });
    return () => {
      window.removeEventListener("resize", ()=>{});
    };
  }, []);

  return (
    <>
      <div
        id="ref-tooltip"
        className="tooltip-container__tooltip-box hide-tooltip"
      >
        <span className="tooltip-container__tooltip-box__heading"></span>
      </div>
      <div
        data-uniqueid={uniqueId}
        className={`references__container ${!isCollapse ? "references__refrenceHeight" : ""}`}
      >
        {hasCitingSources && (
          <span className="references__container__text">{t.references}</span>
        )}
        {sourceResponses.map((sourceResponse, index) => {
          return sourceResponse.citingSources?.map(
            (citingSource: ICitingSource, citingIndex: number) => {
              const { sourceValue, sourceType, sourceName } = citingSource;
              const referenceType = getReferenceType(sourceType as string);
              return (
                referenceType && (
                  <React.Fragment key={`${index}-${citingIndex}`}>
                    {sourceValue?.map((item, docIndex) => {
                      const value = getSourceValue(item, sourceType);
                      return value ? (
                        <span
                          key={`${index}-${citingIndex}-${docIndex}`}
                          className={`references__container__${referenceType} ${loadingItems.includes(`${index}-${citingIndex}-${docIndex}`) ? " loading" : ""}`}
                          onClick={() =>
                            onReferenceOpen(
                              item,
                              sourceName,
                              sourceType as string,
                              `${index}-${citingIndex}-${docIndex}`
                            )
                          }
                          onMouseEnter={(event) => showTooltip(event, item)}
                          onMouseLeave={hideTooltip}
                        >
                          {value}
                        </span>
                      ) : null;
                    })}
                  </React.Fragment>
                )
              );
            }
          );
        })}
        {!isCollapse && moreThantwoRows &&(
          <span
            className="expand"
            onClick={() => expandCollapseRef(ReferenceMode.EXPAND)}
          >
            {t.ViewLess}
          </span>
        )}
      </div>
     
      {refrenceCount > 0 && (
        <div className="references__showlessShowMore">
          {isCollapse ? (
            <span
              className="collapse"
              onClick={() => expandCollapseRef(ReferenceMode.COLLAPSE)}
            >
              +{refrenceCount}
            </span>
          ) : null}
        </div>
      )}

      <AppModal
        showModal={redirectUser}
        heading={t.confirmRedirectCitingSourceTitle}
        description={confirmRedirectMessage}
        confirmBtnText={t.confirmRedirectCitingSourceButton}
        cancelButtonText={t.cancel_text}
        onClose={() => setRedirectUser(false)}
        onCancel={() => setRedirectUser(false)}
        onConfirm={handleUserRedirect}
        iconName={trinagleWarning}
      />
    </>
  );
};

export default ReferenceComponent;
