import React, { useCallback, useEffect, useState } from "react";
import "./Suggestion.scss";
import Featured_icon from "../../../assets/img/ce_assistant_icon.svg";
import t from "../../../locales/en.json";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import { lightBulbOn } from "../../../common/components/AppIcons/Icons";
import { getCurrentDateTimeFormatted } from "../../../common/utility/utility";
import { getAllSuggestionsVissibleToAssistant, sendSuggestionToBot } from "./SuggestionsSlice";
import { useAppDispatch, useAppSelector } from "../../hooks/hooks";
import { ISuggestionsData } from "../../model";
import {
  COLLAPSE_ASSISTANT_MAX_WIDTH,
  DEFAULT_SUGGESTION_COUNT,
  MAX_SUGGESTION_COUNT,
} from "../../../common/utility/constants";
import SkeletonLoader from "../../../common/components/SkeletonLoader/SkeletonLoader";

const Suggestion: React.FC = () => {
  const dispatch = useAppDispatch();
  const { externalAppKey } = useAppSelector(
    (state) => state.userAuth.externalAppSettings
  );
  const suggestions = useAppSelector((state) => state.suggestions.suggestionsVisibleToAssistant);
  const selectedSources = useAppSelector(
    (state) => state.chooseSource.selectedSources
  );
  const isLoading = useAppSelector((state) => state.suggestions.isLoading);

  const [suggestionCount, setSuggestionCount] = useState(DEFAULT_SUGGESTION_COUNT);
  const [filteredSuggestions, setFilteredSuggestions] = useState<ISuggestionsData[]>([]);
  const [suggestionsToDisplay, setSuggestionsToDisplay] = useState<ISuggestionsData[]>([]);
  const [loadMoreSuggestions, setLoadMoreSuggestions] = useState(false);
  const [currentIndex, setCurrentIndex] = useState(0);

  useEffect(() => {
    dispatch(getAllSuggestionsVissibleToAssistant());
  }, [dispatch]);

  useEffect(() => {
    filterSuggestions();
  }, [suggestions, selectedSources, externalAppKey]);

  useEffect(() => {
    updateRows();
    window.addEventListener("resize", updateRows);
  
    return () => {
      window.removeEventListener("resize", updateRows);
    };
  }, []);

  useEffect(() => {
    setCurrentIndex(0);
  }, [selectedSources]);

  useEffect(() => {
    generateSuggestionsList();
  }, [filteredSuggestions, currentIndex, suggestionCount]);

  const handleSuggestionClick = (suggestion: ISuggestionsData) => {
    dispatch(sendSuggestionToBot(suggestion));
  };

  const updateRows = useCallback(() => {
    const container = document.querySelector<HTMLElement>(".copilot-container__middle-section");
    const width = container?.offsetWidth || 0;
    setSuggestionCount(width > COLLAPSE_ASSISTANT_MAX_WIDTH ? MAX_SUGGESTION_COUNT : DEFAULT_SUGGESTION_COUNT);
  }, []);

  const filterSuggestions = useCallback(() => {
    if (!suggestions || !selectedSources || suggestions?.length === 0 || selectedSources?.length === 0) return;

    const filteredSuggestions = suggestions.filter((suggestion: ISuggestionsData) => {
      const appAffinity = suggestion.appAffinity.split(",").map(item => item.trim());
      const sources = suggestion.source.split(",").map(item => item.trim());
      return appAffinity.includes(externalAppKey || "PROJECT_LEVEL") &&
             selectedSources.some((source) => sources.includes(source)) &&
             suggestion.isIncluded;
    });

    shuffleSuggestions(filteredSuggestions);
    setFilteredSuggestions(filteredSuggestions);
  }, [suggestions, selectedSources, externalAppKey]);

  const shuffleSuggestions = (filteredSuggestions: ISuggestionsData[]) => {
    for (let i = filteredSuggestions.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [filteredSuggestions[i], filteredSuggestions[j]] = [filteredSuggestions[j], filteredSuggestions[i]];
    }
  };

  const generateSuggestionsList = useCallback(() => {
    if (!filteredSuggestions || filteredSuggestions.length === 0) {
      setSuggestionsToDisplay([]);
      setLoadMoreSuggestions(false);
      return;
    }

    const suggestionCountToLoad = filteredSuggestions.slice(currentIndex, currentIndex + MAX_SUGGESTION_COUNT);

    if (suggestionCountToLoad.length < MAX_SUGGESTION_COUNT && filteredSuggestions.length > MAX_SUGGESTION_COUNT) {
      const remainingCount = MAX_SUGGESTION_COUNT - suggestionCountToLoad.length;
      const remainingSuggestions = filteredSuggestions.slice(0, remainingCount);
      suggestionCountToLoad.push(...remainingSuggestions);
    }

    setSuggestionsToDisplay(suggestionCountToLoad);
    setLoadMoreSuggestions(filteredSuggestions.length > suggestionCountToLoad.length || ((suggestionCount === DEFAULT_SUGGESTION_COUNT) && filteredSuggestions.length > DEFAULT_SUGGESTION_COUNT));
  }, [filteredSuggestions, currentIndex, suggestionCount]);

  const getNextElements = () => {
    const nextIndex = currentIndex + suggestionCount;
    setCurrentIndex(nextIndex >= filteredSuggestions.length ? 0 : nextIndex);
  };

  return isLoading ? (
    <SkeletonLoader />
  ) : (
    <div className="suggestion">
      <div>
        <img src={Featured_icon} alt={t.featureIcon} />
      </div>
      <div>
        <div className="suggestion__title">
          <span className="suggestion__heading">{t.Project_title}</span>
          <span className="suggestion__dateTime">
            {getCurrentDateTimeFormatted("dddd h:mmA")}
          </span>
        </div>
        <div className="suggestion__sub-heading">
          <AppIcon icon={lightBulbOn} className="suggestion__lightbubl_icon" />
          <span className="suggestion__sub-heading__content">
            {t.suggestions}
          </span>
        </div>
        <section className="suggestion__suggestionBox-container">
          {suggestionsToDisplay &&
            suggestionsToDisplay.slice(0, suggestionCount).map(
              (
                suggestion: ISuggestionsData,
                index: React.Key | null | undefined
              ) => (
                <div
                  className="suggestion__boxes"
                  key={index}
                  onClick={() => handleSuggestionClick(suggestion)}
                >
                  <div className="suggestion__text">
                    {suggestion?.suggestionText}
                  </div>
                </div>
              )
            )}
        </section>
        {loadMoreSuggestions && (
          <section className="suggestion__refresh_icon">
            <span>{t.different_suggestion} </span>
            <a onClick={getNextElements}>{t.click_to_refresh}</a>
          </section>
        )}
      </div>
    </div>
  );
};

export default Suggestion;
