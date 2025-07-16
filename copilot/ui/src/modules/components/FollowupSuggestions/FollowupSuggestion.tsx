import React from "react";
import "./FollowupSuggestions.scss";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import { messageQuestion } from "../../../common/components/AppIcons/Icons";
import {
  sendSuggestionToBot,
} from "../Suggestions/SuggestionsSlice";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { IFollowUpSuggestion, ISuggestionsData } from "../../model";

interface FollowupSuggestionProp {
  suggestions: IFollowUpSuggestion[] | undefined;
}

const FollowupSuggestion = (prop: FollowupSuggestionProp) => {
  const dispatch = useAppDispatch();

  const suggestions = prop.suggestions || [];
  //for project docs we are generating suggestions using chunk so having id = 0 for them
  const projectDocsSuggestions = suggestions.filter(({ id }) => id === 0).slice(0, 2);
  //for other sources we are generating suggestions using existing questions from suggestions so having id != 0 for them
  const otherSourcesSuggestions = suggestions.filter(({ id }) => id !== 0).slice(0, 2);
  const conbinedSuggestions = [...projectDocsSuggestions, ...otherSourcesSuggestions];
  const suggestionsToShow = suggestions.length === 3 ? suggestions : conbinedSuggestions.slice(0, 4);
  
  const handleSuggestionClick = (suggestion: IFollowUpSuggestion) => {
    const payload: ISuggestionsData = {
      id: suggestion.id,
      source: "",
      suggestionText: suggestion.suggestionText,
      appAffinity: "",
      createdAt: "",
      updatedAt: "",
      createdBy: "",
      updatedBy: "",
    };
    dispatch(sendSuggestionToBot(payload));
  };

  return (
    <section className="followup-suggestion">
      <section className="followup-suggestion__left-container">
        <span>
          <AppIcon
            icon={messageQuestion}
            className="followup-suggestion__icon"
          />
        </span>
      </section>
      <section className="followup-suggestion__right-container">
        {suggestionsToShow &&
          suggestionsToShow.map(
            (
              suggestion: IFollowUpSuggestion,
              index: React.Key | null | undefined
            ) => (
              <div
                className="followup-suggestion__right-container__boxes"
                key={index}
                onClick={() => handleSuggestionClick(suggestion)}
              >
                <div className="followup-suggestion__right-container__text">
                  {suggestion?.suggestionText}
                </div>
              </div>
            )
          )}
      </section>
    </section>
  );
};

export default FollowupSuggestion;
