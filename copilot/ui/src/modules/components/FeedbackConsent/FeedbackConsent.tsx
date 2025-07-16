import React, { useState } from "react";
import "./FeedbackConsent.scss";
import { useAppDispatch } from "../../hooks/hooks";
import { chatActions } from "../../redux/slices/chatSlice";
import t from "../../../locales/en.json";
import { IResponseFeedback } from "../../model";
import {
  FEEDBACK_CLOSE_IN_MILLISECONDS,
  SessionKeys,
} from "../../../common/utility/constants";

interface FeedbackProps {
  messageId: number;
  InstanceId: string;
  onFeedbackClose: (messageId: number) => void;
}

const FeedbackConsent = (props: FeedbackProps) => {
  const chatStateDispatch = useAppDispatch();
  const [consentFeedbackSubmitted, setConsentFeedbackSubmitted] =
    useState<boolean>(false);
  const handleSubmit = (consentValue: boolean) => {
    setConsentFeedbackSubmitted(true);
    const ChatId = sessionStorage.getItem(SessionKeys.CHAT_ID) as string;
    const feedbackRequest: Partial<IResponseFeedback> = {
      MessageID: props.messageId,
      ChatId,
      InstanceId: props.InstanceId,
      Consent: consentValue,
    };

    setTimeout(
      () => chatStateDispatch(chatActions.feedbackMessage(feedbackRequest)),
      FEEDBACK_CLOSE_IN_MILLISECONDS
    );
  };

  return (
    <section className="feedback-consent-section">
      {consentFeedbackSubmitted ? (
        <div className="feedback-consent-section__submitted">
          {t.feedbackThanks}
        </div>
      ) : (
        <div className="feedback-consent-section__wrapper">
          <p>
            {t.saveQuestionConsentText}
          </p>
          <div className="feedback-consent-section__wrapper__action">
            <button
              onClick={() => handleSubmit(false)}
              className="feedback-consent-section__wrapper__action_deny"
            >
              {t.Deny}
            </button>
            <button
              onClick={() => handleSubmit(true)}
              className="feedback-consent-section__wrapper__action_confirm"
            >
              {t.Confirm}
            </button>
          </div>
        </div>
      )}
    </section>
  );
};

export default FeedbackConsent;
