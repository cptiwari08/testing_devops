import React, { useState } from "react";
import "./Feedback.scss";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import { xMark } from "../../../common/components/AppIcons/Icons";
import { useAppDispatch } from "../../hooks/hooks";
import { chatActions } from "../../redux/slices/chatSlice";
import { IResponseFeedback } from "../../model";
import {
  EventKeys,
  FEEDBACK_CLOSE_IN_MILLISECONDS,
  SessionKeys,
} from "../../../common/utility/constants";
import t from "../../../locales/en.json";
import {
  containsXSS,
  startsWithSpecialChar,
} from "../../../common/utility/utility";
import DOMPurify from "dompurify";
import ToggleSwitch from "../../../common/components/ToggleSwitch/ToggleSwitch";

interface FeedbackProps {
  messageId: number;
  IsLiked: boolean | null | undefined;
  InstanceId: string;
  onFeedbackClose: (messageId: number) => void;
}

const Feedback = (props: FeedbackProps) => {
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [feedbackMessage, setFeedbackMessage] = useState("");
  const [feedbackConsent, setFeedbackConsent] = useState<boolean>(true);
  const [validationError, setValidationError] = useState<string>("");
  const chatStateDispatch = useAppDispatch();

  const handleKeyDown = (event: any) => {
    if (event.key === EventKeys.ENTER) {
      handleSubmit();
    }
  };

  const handleChange = (event: any) => {
    setFeedbackMessage(event.target.value);
    validateFeedbackText(event.target.value);
  };

  const handleClose = () => {
    props.onFeedbackClose(props.messageId);
  };

  const validateFeedbackText = (text: string) => {
    if (startsWithSpecialChar(text)) {
      const firstChar = text.trim().split("")[0];
      setValidationError(`${t.special_character_error} ${firstChar}`);
      return false;
    } else {
      setValidationError("");
      return true;
    }
  };

  const handleSubmit = () => {
    if (feedbackMessage.trim()) {
      if (containsXSS(feedbackMessage)) return;
      else if (validationError) return;

      const FeedbackText = DOMPurify.sanitize(feedbackMessage);

      setIsSubmitted(true);
      const ChatId = sessionStorage.getItem(SessionKeys.CHAT_ID) as string;
      const feedbackRequest: IResponseFeedback = {
        MessageID: props.messageId,
        FeedbackText,
        IsLiked: props.IsLiked,
        ChatId,
        Consent: feedbackConsent,
        InstanceId: props.InstanceId,
      };
      chatStateDispatch(chatActions.feedbackMessage(feedbackRequest));

      setTimeout(() => {
        handleClose();
      }, FEEDBACK_CLOSE_IN_MILLISECONDS);
    }
  };

  const onConsent = (ev: any) => {
    setFeedbackConsent(!feedbackConsent);
  };

  return isSubmitted ? (
    <span className="submit-thanks-message">{t.feedbackThanks}</span>
  ) : (
    <section className="feedback-section">
      <div className="feedback-section__header">
        <span className="feedback-section__header__heading">
          {t.feedbackRecorded}
        </span>
        <span className="feedback-section__header__close" onClick={handleClose}>
          <AppIcon
            icon={xMark}
            className="feedback-section__header__close_icon"
          ></AppIcon>
        </span>
      </div>
      <div className="feedback-section__body">
        <span className="feedback-section__body__input">
          <input
            placeholder={t.tellUsMore}
            onKeyDown={handleKeyDown}
            onChange={handleChange}
          ></input>
        </span>
      </div>
      {validationError && (
        <p className="feedback-section__error">{validationError}</p>
      )}

      <div className="feedback-section__consent-part">
        <p>
          {t.feedbackConsentText}
        </p>
        <div className="feedback-section__consent-part__toggle">
          <ToggleSwitch
            checked={feedbackConsent}
            key={props.InstanceId}
            type={2}
            onChange={onConsent}
          />
          <span className="feedback-section__consent-part__toggle_label">
            {t.Confirm}
          </span>
        </div>

        <div className="feedback-section__consent-part__buttons">
          <span
            className="feedback-section__consent-part__buttons__cancel_button"
            onClick={handleClose}
          >
            {t.cancel_text}
          </span>
          <span
            className="feedback-section__consent-part__buttons__submit_button"
            onClick={handleSubmit}
          >
            {t.submit}
          </span>
        </div>
      </div>
    </section>
  );
};

export default Feedback;
