import { useState, useEffect } from "react";
import FaIcons from "../../../common/components/AppIcons/AppIcons";
import {
  clipboard,
  thumbsDown,
  thumbsUp,
  arrowDownToLine,
  checkMark,
  sThumbsUp,
  sThumbsDown,
  spinner,
} from "../../../common/components/AppIcons/Icons";
import "./MessageHelperIcons.scss";
import useCopyToClipboard from "../../hooks/copyToClipboard";
import { DownloadHistoryFileType, IconSizes } from "../../../common/utility/constants";
import t from "../../../locales/en.json";
import Tooltip from "../../../common/components/Tooltip/Tooltip";
import wordIcon from "../../../assets/img/word.svg";
import pdfIcon from "../../../assets/img/pdf.png";

interface MessageHelperIconsProps {
  message: string;
  messageId: number;
  isLiked: boolean | undefined | null;
  InstanceId: string;
  likeDislike: (action: string, messageId: number, InstanceId: string) => void;
  OnDownload: (messageId: number, fileType: string) => void;
}

interface DownloadDropDownState {
  isVisible?: boolean;
  top?: number;
  left?: number;
}

enum LikeDislikeEnum {
  LIKED = "liked",
  DISLIKED = "disliked",
}

const MessageHelperIcons = (props: MessageHelperIconsProps) => {
  const copyToClipboard = useCopyToClipboard();
  const [loading, setLoading] = useState(false);
  const [downloading, setDownloading] = useState(false);
  const [likeDislikeState, setLikeDislikeState] = useState("");
  const [downloadDropDownState, setDownloadDropDownState] = useState<DownloadDropDownState>();

  const copySuccess = () => {
    setLoading(true);
  };

  useEffect(() => {
    let timeoutId: NodeJS.Timeout | undefined;
    if (loading) {
      timeoutId = setTimeout(() => {
        setLoading(false);
      }, 3000) as NodeJS.Timeout;
    }

    return () => {
      if (timeoutId) {
        clearTimeout(timeoutId);
      }
    };
  }, [loading]);

  useEffect(() => {
    setLikeDislikeState(
      props.isLiked === true
        ? LikeDislikeEnum.LIKED
        : props.isLiked === false
          ? LikeDislikeEnum.DISLIKED
          : ""
    );
  }, [props.isLiked]);

  const onLikeDislike = (action: string) => {
    setLikeDislikeState(action);
    props.likeDislike(action, props.messageId, props.InstanceId);
  };

  const isLiked = likeDislikeState === LikeDislikeEnum.LIKED;
  const isDisliked = likeDislikeState === LikeDislikeEnum.DISLIKED;

  const handleOptionClick = async (option: string) => {
    setDownloadDropDownState({...downloadDropDownState, isVisible: false});
    setDownloading(true);
    await props.OnDownload(props.messageId, option);
    setDownloading(false);
  };

  const handleDownloadClick = (event:any) => {
    setDownloadDropDownState({left: event.clientX, top: (event.clientY+7), isVisible: true});
  };

  const handleClickOutside = (event: MouseEvent) => {
    const dropdown = document.querySelector('.ai-helper__dropdown');
    if (dropdown && !dropdown.contains(event.target as Node)) {
      setDownloadDropDownState({...downloadDropDownState, isVisible: false});
    }
  };

  const handleScroll = ()=>{
    setDownloadDropDownState({...downloadDropDownState, isVisible: false});
  }

  useEffect(() => {
    document.addEventListener('mousedown', handleClickOutside);
    document.getElementById('scrollableSection')?.addEventListener('scroll', handleScroll);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
      document.getElementById('scrollableSection')?.removeEventListener('scroll', handleScroll);
    };
  }, []);

  return (
    <div className="ai-helper">
      <span className="ai-helper__label">{t.wasThisHelpful}</span>
      <Tooltip tooltipTitle={t.tooltipLike}>
        <FaIcons
          icon={isLiked ? sThumbsUp : thumbsUp}
          size={IconSizes.SMALL}
          style={{
            color: isLiked ? "var(--like-primary-color)" : "",
          }}
          className="ai-helper__icons fa-solid"
          onClick={() => onLikeDislike(isLiked ? "" : LikeDislikeEnum.LIKED)}
        />
      </Tooltip>

      <Tooltip tooltipTitle={t.tooltipDislike}>
        <FaIcons
          icon={isDisliked ? sThumbsDown : thumbsDown}
          size={IconSizes.SMALL}
          style={{
            color: isDisliked ? "var(--dislike-primary-color)" : "",
          }}
          className="ai-helper__icons"
          onClick={() =>
            onLikeDislike(isDisliked ? "" : LikeDislikeEnum.DISLIKED)
          }
        />
      </Tooltip>
      <span className="ai-helper__icons_seperator">|</span>
      {!loading ? (
        <Tooltip tooltipTitle={t.tooltipCopy}>
          <FaIcons
            icon={clipboard}
            size={IconSizes.SMALL}
            onClick={() => copyToClipboard(props.message, copySuccess)}
            className="ai-helper__icons"
          />
        </Tooltip>
      ) : (
        <FaIcons
          icon={checkMark}
          size={IconSizes.SMALL}
          className="ai-helper__icons"
        />
      )}
      {!downloading ? (
        <>
        <div className="ai-helper__dropdown-container">
          <Tooltip tooltipTitle={t.tooltipDownload}>
            <FaIcons
              icon={arrowDownToLine}
              size={IconSizes.SMALL}
              className="ai-helper__icons"
              onClick={handleDownloadClick}
            />
          </Tooltip>
        </div>
        {downloadDropDownState?.isVisible && (
          <div className="ai-helper__dropdown" style={{'top': downloadDropDownState.top, 'left': downloadDropDownState.left}}>
            <div onClick={() => handleOptionClick(DownloadHistoryFileType.PDF)}>
              <img className="ai-helper__dropdown--icon" src={pdfIcon} alt="PDF" />
              <span>PDF</span>
            </div>
            <div onClick={() => handleOptionClick(DownloadHistoryFileType.WORD)}>
              <img className="ai-helper__dropdown--icon" src={wordIcon} alt="Word" />
              <span>Word</span>
            </div>
          </div>
        )}
        </>
      ) : (
        <FaIcons
          className="ai-helper__icons"
          icon={spinner}
          size={IconSizes.SMALL}
          spin
        />
      )}
    </div>
  );
};

export default MessageHelperIcons;
