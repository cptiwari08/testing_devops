import React, {
  useState,
  ChangeEvent,
  MouseEvent,
  useEffect,
  useCallback,
  useRef,
} from "react";
import "./AskQuestion.scss";
import t from "../../../locales/en.json";
import {
  ASK_YOUR_QUESTION_MAXLENGTH,
  ChatRole,
  ResponseStatuses,
  SessionKeys,
  SourceType,
} from "../../../common/utility/constants";
import privacyIcon from "../../../assets/img/privacy_icon.svg";
import newTopic from "../../../assets/img/new_topic.svg";
import sendIcon from "../../../assets/img/send_icon.svg";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { chatActions } from "../../redux/slices/chatSlice";
import {
  IChatOutput,
  IChatQuestion,
  IResetChatSession,
  ISourceConfig,
  ISuggestionsData,
} from "../../model";
import DOMPurify from "dompurify";
import { resetSelectedSuggestion } from "../Suggestions/SuggestionsSlice";
import { channel, startsWithSpecialChar } from "../../../common/utility/utility";
const moment = require("moment-timezone");
const dateTimeFormat = "YYYY-MM-DD HH:mm:ss";

const AskQuestion: React.FC = () => {
  const messageId = sessionStorage.getItem(SessionKeys.MESSAGE_ID);
  const [sequenceId, setSequenceId] = useState<number>(
    messageId ? parseInt(messageId) : 1
  );
  const [question, setQuestion] = useState<string>("");
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [errorInSource, setErrorInSource] = useState<string>("");
  const [chatId, setChatId] = useState<string>("");
  const [isChatStarted, setIsChatStarted] = useState<boolean>(false);
  const [isChatDisabled, setIsChatDisabled] = useState<boolean>(false);
  const [selectedSourcesForQuestion, setSelectedSourcesForQuestion] = useState<
    string[]
  >([]);

  const chatStateDispatch = useAppDispatch();
  const initialChatId = useAppSelector((state) => state.chat.chatId);
  const chatOutput = useAppSelector((state) => state.chat.output);
  const selectedSources = useAppSelector(
    (state) => state.chooseSource.selectedSources
  );
  const selectedSuggestion = useAppSelector(
    (state) => state.suggestions.selectedSuggestion
  );
  const sourceConfigs = useAppSelector(
    (state) => state.projectConfig.sourceConfigs
  );
  const { appInfo, externalAppKey } = useAppSelector(
    (state) => state.userAuth.externalAppSettings
  );

  const inputRef = useRef<HTMLInputElement>(null);

  const removeMessageId = () => {
    sessionStorage.removeItem(SessionKeys.MESSAGE_ID);
  };
  const resetSequenceId = () => {
    setSequenceId(1);
  };
  const generateNewChatId = useCallback(async () => {
    chatStateDispatch(chatActions.startChat());
    removeMessageId();
    resetSequenceId();
  }, [chatStateDispatch]);
 
  useEffect(() => {
    const handleBroadcast = (event: any) => {
      const { receivedChatId, key, value } = event.data;
      if (chatId == receivedChatId) {
        sessionStorage.setItem(key, value);
        setSequenceId(parseInt(value));
      }
    };

    channel.addEventListener("message", handleBroadcast);
    return () => {
      channel.removeEventListener("message", handleBroadcast);
    };
  }, [chatId]);

  useEffect(() => {
    const getChatIdFromStorage = () => {
      const sessionId = sessionStorage.getItem(SessionKeys.CHAT_ID);
      if (sessionId) {
        setChatId(sessionId);
      } else {
        generateNewChatId();
      }
    };

    getChatIdFromStorage();
  }, [generateNewChatId]);

  useEffect(() => {
    if (initialChatId) {
      setChatId(initialChatId);
    }
  }, [initialChatId]);

  useEffect(() => {
    if (
      chatOutput.status &&
      (chatOutput.status === ResponseStatuses.SUCCESS ||
        chatOutput.status === ResponseStatuses.ERROR)
    ) {
      setIsChatStarted(false);
      chatStateDispatch(resetSelectedSuggestion());
    }
  }, [chatOutput, chatStateDispatch]);

  useEffect(() => {
    if (selectedSources?.length > 0 && sourceConfigs) {
      const disabledSources = selectedSources.filter(
        (selectedSource) =>
          sourceConfigs.find((source:ISourceConfig) => source.key === selectedSource)?.isActive &&
          !sourceConfigs.find((source:ISourceConfig) => source.key === selectedSource)?.isQuestionTextBoxEnabled
      );
      const enabledSources = selectedSources.filter(
        (source) => !disabledSources.includes(source)
      );
      setSelectedSourcesForQuestion(enabledSources);
      setIsChatDisabled(disabledSources.length === selectedSources.length);
      const isAnySourceDisabled = disabledSources.length > 0;
      if (isAnySourceDisabled) {
        const disabledSourceTitles = disabledSources.map((sourceKey) => {
          const source = sourceConfigs.find((desc) => desc.key === sourceKey);
          return source ? source.displayName : sourceKey;
        });
        const disabledSourceNames = disabledSourceTitles.join(", ");
        setErrorInSource(
          t.only_suggested_questions_allowed.replace("{1}", disabledSourceNames)
        );
      } else {
        setErrorInSource("");
        setSelectedSourcesForQuestion(selectedSources);
      }
    }
  }, [selectedSources, sourceConfigs]);

  useEffect(() => {
    if (selectedSuggestion) {
      handleSendMessage(undefined, selectedSuggestion);
    }
  }, [selectedSuggestion]);

  const handleQuestionChange = (event: ChangeEvent<HTMLInputElement>) => {
    const text: string = event.target.value;
    if (text.length <= ASK_YOUR_QUESTION_MAXLENGTH) setQuestion(text);
    validateSpecialChar(text);
  };

  const handleNewTopicClick = () => {
    const resetChatSession: IResetChatSession = {
      reset: true,
      chatId,
    };
    setQuestion("");
    generateNewChatId();
    chatStateDispatch(chatActions.chatbotResetChatMessages(resetChatSession));
    setErrorMessage("");
  };

  const handleSendMessage = (
    event?: MouseEvent<HTMLImageElement | HTMLDivElement>,
    suggestion?: ISuggestionsData
  ) => {
    if (isChatStarted || (isChatDisabled && !suggestion?.id)) return;
    event?.preventDefault();
    const askedQuestion: string =
      suggestion?.suggestionText.trim() ?? question.trim();

    if (askedQuestion.length > 0) {
      if (containsXSS(askedQuestion)) {
        setErrorMessage(t.xssError);
        return;
      } else if (!validateSpecialChar(askedQuestion)) {
        return;
      }

      const sanitizedInput = DOMPurify.sanitize(askedQuestion);
      const sources = suggestion?.id
        ? selectedSources
        : selectedSourcesForQuestion;
      const excludeDocuments:string[] = sources.includes(SourceType.ProjectDoc)? JSON.parse(sessionStorage.getItem(SessionKeys.EXCLUDED_PROJECT_DOCS) || "[]"): [];
      const request: IChatQuestion = {
        messageId: sequenceId,
        chatId,
        question: sanitizedInput,
        sources,
        context: {
          excludeDocuments,
          suggestion: suggestion?.id
        ? {
            id: suggestion.id.toString(),
            source: suggestion?.source,
          }
        : undefined,
          appInfo: selectedSources.includes(SourceType.ProjectData)
        ? { ...appInfo, key: externalAppKey ?? appInfo?.key }
        : undefined,
        },
      };
      const questionObj: IChatOutput = {
        messageId: sequenceId,
        role: ChatRole.USER,
        content: sanitizedInput,
        sources,
        context: {},
        response: [],
        createdTime: moment().format(dateTimeFormat),
        lastUpdatedTime: moment().format(dateTimeFormat),
        inputSources: [],
        InstanceId: ""
      };

      const resetChatSession: IResetChatSession = {
        reset: false,
      };

      setSequenceId(sequenceId + 2);
      setIsChatStarted(true);
      sessionStorage.setItem(
        SessionKeys.MESSAGE_ID,
        (sequenceId + 2).toString()
      );
      const message = {
        receivedChatId:chatId,
        key: 'messageId',
        value: (sequenceId + 2).toString()
      };
      channel.postMessage(message);
      chatStateDispatch(chatActions.chatbotUpdateQuestion(questionObj));
      chatStateDispatch(chatActions.chatbotQuestion(request));
      chatStateDispatch(chatActions.chatbotResetChatMessages(resetChatSession));
      setQuestion("");
      restFocusForIntput();
    }
  };

  const restFocusForIntput = () => {
    setTimeout(() => inputRef.current?.focus(), 100);
  };

  const containsXSS = (input: string) => {
    const xssPatterns = [
      /<script/i,
      /<\/script/i,
      /javascript:/i,
      /on\w+\s*=\s*["']\s*.*?\s*["']/i,
      /<img.*?src=["']\s*javascript:\s*.*?\s*["']/i,
    ];
    return xssPatterns.some((pattern) => pattern.test(input));
  };

  const validateSpecialChar = (text: string) => {
    if (startsWithSpecialChar(text)) {
      const firstChar = text.trim().split("")[0];
      setErrorMessage(`${t.special_character_error} ${firstChar}`);
      return false;
    } else {
      setErrorMessage("");
      return true;
    }
  };

  const handleKeyDown = (event: any) => {
    if (event.key === "Enter") {
      handleSendMessage(event);
    }
  };

  return (
    <>
      <section className="ask-question">
        <section className="ask-question__new_topic">
          <span
            className={`ask-question__new_topic_btn ${isChatStarted ? "disabled" : ""}`}
            onClick={isChatStarted ? undefined : handleNewTopicClick}
          >
            <img src={newTopic} alt={t.newTopic} />
          </span>
          <span className="ask-question__new_topic_text">{t.newTopic}</span>
        </section>

        <section className="ask-question__section">
          <div className="ask-question__section__data-privacy">
            <img src={privacyIcon} alt={t.privacyIcon} />
            <span className="ask-question__section__data-privacy_message">
              {t.privacyText}
            </span>
          </div>
          <div className="ask-question__section_input_wrapper">
            <span
              className={`ask-question__section_input_wrapper_input ${isChatStarted || isChatDisabled ? "disabled" : ""}`}
            >
              <input
                ref={inputRef}
                autoFocus={true}
                maxLength={ASK_YOUR_QUESTION_MAXLENGTH}
                placeholder={t.typeYourQuestion}
                spellCheck="true"
                value={question}
                onChange={handleQuestionChange}
                onKeyDown={handleKeyDown}
                disabled={isChatStarted || isChatDisabled}
              />
            </span>
            <span
              className={`ask-question__section_input_wrapper_svg-icon ${isChatStarted || isChatDisabled ? "disabled" : ""}`}
              onClick={
                handleSendMessage as React.MouseEventHandler<HTMLSpanElement>
              }
            >
              <img src={sendIcon} alt={t.sendIcon} />
            </span>
          </div>
          <span className="ask-question__section_error-wrapper">
            {errorMessage && (
              <p className="ask-question__section_error-wrapper_error">
                {errorMessage}
              </p>
            )}
            {!errorMessage && errorInSource && (
              <p className="ask-question__section_error-wrapper_error">
                {errorInSource}
              </p>
            )}
          </span>
        </section>
      </section>
    </>
  );
};

export default AskQuestion;
