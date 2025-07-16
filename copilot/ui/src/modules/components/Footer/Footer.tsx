import React, { useEffect, useState } from "react";
import "./Footer.scss";
import t from "../../../locales/en.json";
import config from "../../../common/configs/env.config";
import { xMark } from "../../../common/components/AppIcons/Icons";
import FaIcons from "../../../common/components/AppIcons/AppIcons";
import { IconSizes, MAX_FEEDBACK_TEXT_LENGTH } from "../../../common/utility/constants";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { footerSliceAction } from "./FooterSlice";
import Loader from "../../../common/components/Loader/Loader";
import { startsWithSpecialChar } from "../../../common/utility/utility";

function Footer() {
  const engageManagementGuidance = config.REACT_APP_ENGAGE_MGMT_GUIDANCE_URL;
  const eyAiPrinciplesURL = config.EY_AI_PRINCIPLES;
  const microsoftAcceptableUsePolicyURL =
    config.MICROSOFT_ACCEPTABLE_USE_POLICY_URL;
  const microsoftCodeOfConductOpenAIServicesURL =
    config.MICROSOFT_CODE_OF_CONDUCT_OPENAI_SERVICES_URL;
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [rating, setRating] = useState<number>(0);
  const [feedbackText, setFeedbackText] = useState<string>("");
  const [showLoader, setShowLoader] = useState<boolean>(false);
  const [validationError, setValidationError] = useState<string>("");
  const isLoading = useAppSelector((state) => state.FooterSlice.isLoading);
  const [isExpanded, setIsExpanded] = useState<boolean>(false);

  const shortText = t.footerMessage.substring(0, 170) + "...";
  const textToDisplay = isExpanded ? t.footerMessage : shortText;

  const dispatch = useAppDispatch();

  useEffect(() => {
    if (isLoading) {
      setShowLoader(true);
    } else {
      setShowLoader(false);
      closeModal();
    }
  }, [isLoading]);

  const openFeedbackModal = () => {
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setRating(0);
    setFeedbackText("");
    setValidationError("");
  };

  const submitRating = () => {
    if (validationError) {
      return;
    }
    const payload = {
      Rating: rating,
      FeedbackText: feedbackText,
    };
    dispatch(footerSliceAction.saveFeedback(payload));
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

  const formatText = (text: string) => {
    return text
      .replace(
        "{1}",
        `<a href=${eyAiPrinciplesURL} target="_blank" rel="noreferrer">EY AI Principles</a>`
      )
      .replace(
        "{2}",
        `<a href=${microsoftAcceptableUsePolicyURL} target="_blank" rel="noreferrer">Microsoft Acceptable Use Policy</a>`
      )
      .replace(
        "{3}",
        `<a href=${microsoftCodeOfConductOpenAIServicesURL} target="_blank" rel="noreferrer">Microsoft Code of Conduct for OpenAI Services</a>`
      )
      .replaceAll(
        "{4}",
        `<a href=${engageManagementGuidance} target="_blank" rel="noreferrer">Engagement Management Guidance</a>`
      );
  };

  const formattedText = formatText(textToDisplay);

  const toggleExpanded = (e: any) => {
    e.preventDefault();
    setIsExpanded(!isExpanded);
  };

  return (
    <section className="footer">
      <span dangerouslySetInnerHTML={{ __html: formattedText }}></span>

      <span onClick={toggleExpanded} className="footer__show-more-less">
        {!isExpanded ? t.footerShowMore : t.footerShowLess}
      </span>

      <span className="footer__message--sub-text">
        {t.disclaimerForAiMistake}
      </span>
      <div className="footer__message--links">
        <button className="footer__feedback" onClick={openFeedbackModal}>
          {t.feedback}
        </button>{" "}
        |{" "}
        <a href={engageManagementGuidance} target="_blank" rel="noreferrer">
          {t.engageManagementGuidance}
        </a>
      </div>

      {/* Modal */}
      {isModalOpen && (
        <div className="modal-backdrop">
          <div className="modal">
            <div className="modal__header">
              <h2 className="modal__title">{t.assistant_feedback_title}</h2>
              <button className="modal__close-btn" onClick={closeModal}>
                <FaIcons
                  icon={xMark}
                  size={IconSizes.SMALL}
                  title={t.tooltipCopy}
                  className="modal__close-btn-icon"
                />
              </button>
            </div>
            <div className="modal__body">
              {showLoader && <Loader isLoading={showLoader} />}
              <p>{t.assistant_feedback_description}</p>
              <div className="modal__body--rating">
                {[1, 2, 3, 4, 5].map((star, index) => {
                  return (
                    <span
                      key={index}
                      className={`modal__body--rating-star ${rating >= star ? "selected" : ""}`}
                      onClick={() => {
                        setRating(star);
                      }}
                    >
                      {" "}
                      ★{" "}
                    </span>
                  );
                })}
              </div>
              <div>
                <textarea
                  className="modal__textarea"
                  placeholder={t.assistant_feedback_placeholder}
                  maxLength={MAX_FEEDBACK_TEXT_LENGTH}
                  value={feedbackText}
                  onChange={(e) => {
                    setFeedbackText(e.target.value);
                    validateFeedbackText(e.target.value);
                  }}
                ></textarea>
                {validationError && (
                  <p className="modal__error">{validationError}</p>
                )}
              </div>
            </div>

            <div className="modal__footer">
              <button className="modal__cancel-btn" onClick={closeModal}>
                {t.assistant_feedback_cancel}
              </button>
              <button
                className={`modal__submit-btn ${rating === 0 || validationError !== "" ? "disabled" : ""}`}
                disabled={rating === 0 || validationError !== ""}
                onClick={submitRating}
              >
                {t.submit}
              </button>
            </div>
          </div>
        </div>
      )}
    </section>
  );
}

export default Footer;
