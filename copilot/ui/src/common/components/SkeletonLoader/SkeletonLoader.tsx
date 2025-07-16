import "./SkeletonLoader.scss";

const SkeletonLoader: React.FC = ({}) => {
  return (
    <article className="skeleton-loader">
      <section className="skeleton-loader__banner has-loading"></section>
      <section className="skeleton-loader__choose-sources">
        <div className="skeleton-loader__choose-sources__heading has-loading">
          <span></span>
        </div>
        <div className="skeleton-loader__choose-sources__items has-loading">
          <span></span>
        </div>
        <div className="skeleton-loader__choose-sources__description-heading has-loading">
          <span></span>
        </div>
      </section>

      <section className="skeleton-loader__suggestion">
        <div className="skeleton-loader__suggestion_head-row">
          <span className="skeleton-loader__suggestion_head-row_icon has-loading"></span>
          <span className="skeleton-loader__suggestion_head-row_labels ">
            <span className="row-1 has-loading"></span>
            <span className="row-2 has-loading"></span>
          </span>
        </div>

        <div className="skeleton-loader__suggestion__wrapper">
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
          <span className="skeleton-loader__suggestion__wrapper_item has-loading"></span>
        </div>
      </section>

      <section className="skeleton-loader__footer">
        <section className="skeleton-loader__footer__privacy has-loading"></section>
        <section className="skeleton-loader__footer__ask-question">
          <span className="skeleton-loader__footer__ask-question_icon has-loading"></span>
          <span className="skeleton-loader__footer__ask-question_input has-loading"></span>
          <span className="skeleton-loader__footer__ask-question_send has-loading"></span>
        </section>
        <section className="skeleton-loader__footer__footer-message has-loading"></section>
      </section>
    </article>
  );
};

export default SkeletonLoader;
