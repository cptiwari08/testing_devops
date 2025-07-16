import React, { useEffect, useState } from "react";
import "./MessagesList.scss";
import { useAppSelector } from "../../hooks/hooks";
import {
  IResponseFeedback,
  IAssistantFeedbackResponse,
  IChatOutput,
} from "../../model";
import MessageHelperIcons from "../MessageHelperIcons/MessageHelperIcons";
import Feedback from "../Feedback/Feedback";
import Featured_icon from "../../../assets/img/ce_assistant_icon.svg";
import t from "../../../locales/en.json";
import {
  ChatRole,
  ResponseStatuses,
  SessionKeys,
  CHAT_SCRIPT_FILE_NAME,
  SourceType,
} from "../../../common/utility/constants";
import {
  formatDateToDayTime,
  getMimeTypeByFileName,
  sanitizeContent,
  getCurrentFormattedDate,
} from "../../../common/utility/utility";
import { chatActions } from "../../redux/slices/chatSlice";
import { useDispatch } from "react-redux";
import ReferenceComponent from "../References/References";
import SquareLoader from "../../../common/components/SquareLoader/SquareLoader";
import { ChatService } from "../../redux/apis/chatApis";
import FollowupSuggestion from "../FollowupSuggestions/FollowupSuggestion";
import ReactMarkdown from "react-markdown";
import rehypeRaw from "rehype-raw";
import remarkBreaks from "remark-breaks";
import remarkGfm from "remark-gfm";
import FeedbackConsent from "../FeedbackConsent/FeedbackConsent";

enum LikeDislikeEnum {
  LIKED = "liked",
  DISLIKED = "disliked",
}
const MessagesList = () => {
  const [chatMessagesList, setChatMessagesList] = useState<IChatOutput[]>([]);
  const [openFeedBackList, setOpenFeedbackList] =
    useState<IAssistantFeedbackResponse>({});
  const sourceConfigs = useAppSelector(
    (state) => state.projectConfig.sourceConfigs
  );
  const [isLoading, setIsLoading] = useState(false);
  const userDetails = useAppSelector((state) => state.user.userDetails);

  const chatQuestion = useAppSelector((state) => state.chat.question);
  const chatResponse = useAppSelector((state) => state.chat.output);
  const resetChatMessages = useAppSelector(
    (state) => state.chat.resetChatMessages
  );
  const messageList = useAppSelector((state) => state.chat.messageList);
  const dispatch = useDispatch();
  const scrollDiv: any = document.getElementById("scrollableSection");
  useEffect(() => {
    const sessionId = sessionStorage.getItem(SessionKeys.CHAT_ID);
    if (sessionId) {
      dispatch(chatActions.getChatHistory());
    }
  }, []);

  useEffect(() => {
    const _messageList:IChatOutput[] = [];
    (messageList || []).forEach(message=> _messageList.push({...message, content: applySourceName(message.content)}));
    
    if (_messageList?.length) setChatMessagesList(_messageList);
  }, [messageList]);

  useEffect(() => {
    if (chatResponse.status && chatResponse.content) {
      setIsLoading(chatResponse.status === ResponseStatuses.IN_PROGRESS);
      if(typeof chatResponse.context?.consent !== "undefined"){
        setChatMessagesList((prevMessagesList) => {
          const index = prevMessagesList.findIndex(msg=> msg.messageId === chatResponse.messageId && msg.role === ChatRole.ASSISTANT);
          const _prevMessagesList = [...prevMessagesList];

          if(index !== -1){
            const currentItem:Partial<IChatOutput> = {..._prevMessagesList[index]};
            const context = {...currentItem.context, consent: chatResponse.context?.consent}
            delete currentItem.context; // need to delete as it is not extensible
            currentItem.context = context;
            _prevMessagesList.splice(index, 1, currentItem as IChatOutput);
          }
          return [..._prevMessagesList];
        });
      }
      else {
        setChatMessagesList((prevMessagesList) => [
          ...prevMessagesList,
          {...chatResponse, content: applySourceName(chatResponse.content)},
        ]);
      }
    }
    if(typeof chatResponse.context?.consent !== "undefined"){
      applyConsent(chatResponse);
    }
    scrollToResponse();
  }, [chatResponse, chatQuestion]);

  useEffect(() => {
    if (resetChatMessages) {
      setChatMessagesList([]);
    }
  }, [resetChatMessages]);

  const applyConsent = (chatResponse:IChatOutput)=>{
    setChatMessagesList((prevMessagesList) => {
      const index = prevMessagesList.findIndex(msg=> msg.messageId === chatResponse.messageId && msg.role === ChatRole.ASSISTANT);
      const _prevMessagesList = [...prevMessagesList];

      if(index !== -1){
        const currentItem:Partial<IChatOutput> = {..._prevMessagesList[index]};
        const context = {...currentItem.context, consent: chatResponse.context?.consent}
        delete currentItem.context; // need to delete as it is not extensible
        currentItem.context = context;
        _prevMessagesList.splice(index, 1, currentItem as IChatOutput);
      }
      return [..._prevMessagesList];
    });
  }

  const applySourceName = (message:string) =>{
    let _message:string = message;
    sourceConfigs.forEach(source => {
      _message = _message.replaceAll(`SourceKey:${source.key}`, source.displayName);
    });
    return _message;
  }

  const scrollToResponse = () => {
    if (scrollDiv) {
      setTimeout(
        () =>
          scrollDiv.scrollTo({
            top: scrollDiv.scrollHeight,
            behavior: "smooth",
          }),
        0
      );
    }
  };

  const onFeedbackClose = (messageId: number) => {
    let list = { ...openFeedBackList };
    delete list[messageId];
    setOpenFeedbackList({ ...list });
  };

  const likeDislike = (action: string, MessageID: number, InstanceId: string) => {
    const ChatId = sessionStorage.getItem(SessionKeys.CHAT_ID) as string;
    const IsLiked = action ? action === LikeDislikeEnum.LIKED : null;
    const request: IResponseFeedback = {
      IsLiked: action ? action === LikeDislikeEnum.LIKED : null,
      ChatId,
      MessageID,
      FeedbackText: "",
      InstanceId
    };
    dispatch(chatActions.feedbackMessage(request));

    setOpenFeedbackList({ ...openFeedBackList, [MessageID]: { IsLiked } });
    setTimeout(() => {
      document
        .getElementById(`id_${MessageID}`)
        ?.scrollIntoView({ behavior: "smooth", block: "nearest" });
    }, 0);
  };

  /**
   * This function downloads the chat history file for the given message id and file type,
   * @param messageId  = 1
   * @param fileType  = "pdf", "ppt", "docx"
   */

  const OnDownload = async (messageId: number, fileType: string) => {
    try {
      const fileName: string = `${CHAT_SCRIPT_FILE_NAME}_${getCurrentFormattedDate()}.${fileType}`;
      const conversationId = sessionStorage.getItem(
        SessionKeys.CHAT_ID
      ) as string;
      const downloadResponse: any = await ChatService.downloadChatHistory(
        conversationId,
        String(messageId),
        fileType
      );
      if (downloadResponse) {
        const file = new Blob([downloadResponse], {
          type: getMimeTypeByFileName(fileName),
        });
        const fileURL = window.URL.createObjectURL(file);
        let aLink = document.createElement("a");
        aLink.href = fileURL;
        aLink.download = fileName;
        aLink.click();
      }
    } catch (error) {
      //Action on error if required
    }
  };

  /**
   * This function will print the sources in the message for assistant,
   * @param messageSources  = ['source 1', 'source 2']
   * @returns sources in the message joined by ' | '
   */
  const renderSelectedSources = (messageSources: string[] = []) => {
    const selectedSourceList: string[] = [];
    const sortedSourceConfigs = sourceConfigs
      .filter((config) => messageSources.includes(config.key))
      .sort((a, b) => a.ordinal - b.ordinal);

    sortedSourceConfigs.forEach((source) => {
      selectedSourceList.push(source.displayName || "");
    });
    return selectedSourceList.join(" | ");
  };

  const renderUserPhotoOrInitials = () => {
    const title = `${userDetails?.givenName || ""} ${userDetails?.surname || ""}`;
    if (userDetails?.photo) {
      return (
        <img
          src={`data:image/jpeg;base64,${userDetails?.photo}`}
          title={title}
          alt={t.user_profile_icon}
          className="user-profile-image"
        />
      );
    } else {
      const initials = `${userDetails?.givenName?.charAt(0) || ""}${userDetails?.surname?.charAt(0) || ""}`;
      return (
        <div className="messages-list__user-message__initials" title={title}>
          {initials}
        </div>
      );
    }
  };

  return (
    <section className="messages-list">
      {chatMessagesList.map((message: IChatOutput, index: number) => (
        <React.Fragment key={index}>
          {(message.role || "").toLowerCase() === ChatRole.USER && (
            <div className="messages-list__user-message">
              <div className="messages-list__user-message__header">
                <div className="messages-wrapper">
                  <span className="messages-list__user-message__heading">
                    {t.userRefForQuestion}
                  </span>
                  <span className="messages-list__user-message__dateTime">
                    {formatDateToDayTime(message?.lastUpdatedTime)}
                  </span>
                  <div className="messages-list__user-message__content">
                    <span
                      className="messages-list__user-message__content__text"
                      dangerouslySetInnerHTML={{
                        __html: sanitizeContent(message.content),
                      }}
                    />
                  </div>
                </div>
                {renderUserPhotoOrInitials()}
              </div>
            </div>
          )}
          {(message.role || "").toLowerCase() === ChatRole.ASSISTANT && (
            <>
              <div className="messages-list__assistant-message">
                <div className="messages-list__assistant-message__header">
                  <div>
                    <img src={Featured_icon} alt={t.featureIcon} />
                  </div>
                  <div className="messages-wrapper">
                    <div className="messages-list__assistant-message__heading-wrapper">
                      <span className="messages-list__assistant-message__heading">
                        {t.Project_title}
                      </span>
                      <span className="messages-list__assistant-message__dateTime">
                        {formatDateToDayTime(message?.lastUpdatedTime)}
                      </span>
                      <span className="messages-list__assistant-message__sources">
                        {message.sources
                          ? renderSelectedSources(message.inputSources)
                          : ""}
                      </span>
                    </div>
                    <div className="messages-list__assistant-message__content">
                      <span className="messages-list__assistant-message__content__text">
                        <ReactMarkdown
                          className="markdown-content"
                          rehypePlugins={[rehypeRaw]}
                          remarkPlugins={[remarkBreaks, remarkGfm]}
                          remarkRehypeOptions={{ allowDangerousHtml: false }}
                          >
                          {message.content}
                        </ReactMarkdown>
                      </span>
                      {message?.response?.length > 0 && (
                        <ReferenceComponent
                          appInfo = {message?.context?.appInfo}
                          sourceResponses={message?.response}
                          uniqueId={index}
                        />
                      )}
                      {
                        message.StatusCode && message.StatusCode != 200 && typeof message.context?.consent === 'undefined' ?<FeedbackConsent 
                        InstanceId={message.InstanceId}
                        messageId={message.messageId}
                        onFeedbackClose={onFeedbackClose}
                        />:<MessageHelperIcons
                        messageId={message.messageId}
                        message={message.content}
                        isLiked={message.context?.isMessageLiked}
                        InstanceId = {message.InstanceId}
                        likeDislike={likeDislike}
                        OnDownload={OnDownload}
                      />
                      }
                    </div>
                  </div>
                </div>
              </div>

              {openFeedBackList[message.messageId] &&
                openFeedBackList[message.messageId].IsLiked !== null && (
                  <section
                    id={"id_" + message.messageId}
                    className="messages-list__feedback-part"
                  >
                    <Feedback
                      InstanceId={message.InstanceId}
                      messageId={message.messageId}
                      IsLiked={openFeedBackList[message.messageId].IsLiked}
                      onFeedbackClose={onFeedbackClose}
                    ></Feedback>
                  </section>
                )}
            </>
          )}
        </React.Fragment>
      ))}
      {(chatMessagesList.length && chatMessagesList[chatMessagesList.length-1]?.followUpSuggestions?.length) ? <FollowupSuggestion  suggestions={chatMessagesList[chatMessagesList.length-1].followUpSuggestions} />: null }
      {isLoading && <SquareLoader text={t.disclaimerForProjectDataResponses}/>}
    </section>
  );
};

export default MessagesList;
