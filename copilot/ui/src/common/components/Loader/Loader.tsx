
import t from "../../../locales/en.json";
import { spinner } from "../AppIcons/Icons";
import FaIcons from "../AppIcons/AppIcons";
import "./Loader.scss";

interface LoaderProps {
  isLoading?: boolean;
  loaderLabel?: string;
  isLabelVisible?: boolean;
  fullScreen?: boolean;
}

const Loader: React.FC<LoaderProps> = ({
  isLoading = false,
  loaderLabel = t.loading,
  fullScreen = true,
  isLabelVisible = true,
}) => {
  return isLoading ? (
    <div className={`spinner ${fullScreen ? "spinner--full-screen" : ""}`}>
      <FaIcons
        icon={spinner}
        size={'3x'}
        spin
        style={{
          '--fa-primary-color': 'var(--loader-primary-color)',
          '--fa-secondary-color': 'var(--loader-secondary-color)',
        } as React.CSSProperties}
      />
      {isLabelVisible && <div className="spinner__label">{loaderLabel}</div>}
    </div>
  ) : null;
};

export default Loader;
