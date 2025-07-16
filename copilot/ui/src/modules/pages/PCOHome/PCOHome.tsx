import { useEffect, useRef, useState } from "react";
import AskQuestion from "../../components/AskQuestion/AskQuestion";
import Banner from "../../components/Banner/Banner";
import ChooseSources from "../../components/ChooseSources/ChooseSources";
import SourceDescription from "../../components/SourceDescription/SourceDescription";
import Footer from "../../components/Footer/Footer";
import Suggestion from "../../components/Suggestions/Suggestion";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import "./PCOHome.scss";
import MessagesList from "../../components/MessagesList/MessagesList";
import { arrowDown, arrowUp } from "../../../common/components/AppIcons/Icons";
import ViewDocsModal from "../../components/ViewDocsModal/ViewDocsModal";

const PCOHome = () => {
  const scrollPadding: number = 20;

  enum Direction {
    UP = "up",
    DOWN = "down",
  }
  // State to hold the data to be shared between ChooseSources and Suggestion
  const [hasScrollButton, setHasScrollButton] = useState<string>("");
  const [isViewDocsVisible, setIsViewDocsVisible] = useState<boolean>(false);
  const scrollableDivRef = useRef<HTMLElement>(null);

  const handleScrollButtonClick = (action: string) => {
    const scrollPosition: any =
      action === Direction.DOWN ? scrollableDivRef.current?.scrollHeight : 0;
    scrollableDivRef.current?.scrollTo({
      top: scrollPosition,
      left: 0,
      behavior: "smooth",
    });
  };

  const openViewDocsModal = (isOpen: boolean) => {
    setIsViewDocsVisible(isOpen);
  };

  // Function to handle button visibility based on scrollbar presence
  const handleScrollButtonVisibility = () => {
    if (scrollableDivRef.current) {
      const button: string = whichScrollButton();
      setHasScrollButton(button);
    }
  };

  const whichScrollButton = () => {
    let button: string = "";
    if (scrollableDivRef.current) {
      const { scrollTop, scrollHeight, clientHeight } =
        scrollableDivRef.current;
      const bottomPosition = Math.floor(
        scrollTop + clientHeight + scrollPadding
      );
      button = !(scrollHeight > clientHeight)
        ? ""
        : scrollTop < scrollPadding
          ? Direction.DOWN
          : bottomPosition > scrollHeight
            ? Direction.UP
            : "";
    }
    return button;
  };

  useEffect(() => {
    const divElement = scrollableDivRef.current;
    const handleScroll = () => {
      handleScrollButtonVisibility();
    };

    if (divElement) {
      divElement.addEventListener("scroll", handleScroll);
    }
    handleScrollButtonVisibility();

    //cleanup
    return () => {
      if (divElement) {
        divElement.removeEventListener("scroll", handleScroll);
      }
    };
  });

  return (
    <section className="copilot-container">
      {isViewDocsVisible && (
        <ViewDocsModal
          closeModal={() => openViewDocsModal(false)}
        ></ViewDocsModal>
      )}
      <section
        className="copilot-container__scrollable-content"
        id="scrollableSection"
        ref={scrollableDivRef}
      >
        <div className="banner-wrapper">
          <Banner />
          <section
            className={
              hasScrollButton ? "scroll-buttons" : "scroll-buttons hide-me"
            }
          >
            <div
              className={
                hasScrollButton === Direction.DOWN
                  ? "scroll-button scroll-buttons__down"
                  : "scroll-button scroll-buttons__down hide-me"
              }
              onClick={() => handleScrollButtonClick(Direction.DOWN)}
            >
              <AppIcon icon={arrowDown} className="scroll-buttons_icon" />
            </div>
          </section>
        </div>

        <ChooseSources
          openViewDocsModal={() => openViewDocsModal(true)}
        ></ChooseSources>
        <SourceDescription></SourceDescription>
        <section className="copilot-container__middle-section">
          <Suggestion />
        </section>
        <section>
          <MessagesList />
        </section>
        <section
          className={
            hasScrollButton
              ? "scroll-buttons position-up"
              : "scroll-buttons hide-me position-up"
          }
        >
          <div
            className={
              hasScrollButton === Direction.UP
                ? "scroll-button scroll-buttons__up"
                : "scroll-button scroll-buttons__up hide-me"
            }
            onClick={() => handleScrollButtonClick(Direction.UP)}
          >
            <AppIcon icon={arrowUp} className="scroll-buttons_icon" />
          </div>
        </section>
      </section>
      <section className="copilot-container__footer-section">
        <AskQuestion />
        <Footer />
      </section>
    </section>
  );
};

export default PCOHome;
