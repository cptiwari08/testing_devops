import t from "../../../locales/en.json";
import "./Unauthorized.scss";
import { isIframe } from "../../utility/constants";
import accessRemoved from "../../../assets/img/AccessRemoved.svg";
import accessDenied from "../../../assets/img/AccessDenied.svg";

const Unauthorized = () => {
  return (
    <div className="unauthorized-container">
      <div className="unauthorized-content">
        <img src={!isIframe ? accessDenied : accessRemoved} alt={t.unauthorized_access}/>
        <h1 className="unauthorized-title">
          {!isIframe ? t.access_denied_title : t.access_removed_title}
        </h1>
        <p className="unauthorized-description">
          {!isIframe ? t.accessDenied : t.permissionRemoved}
        </p>
      </div>
    </div>
  );
};

export default Unauthorized;
