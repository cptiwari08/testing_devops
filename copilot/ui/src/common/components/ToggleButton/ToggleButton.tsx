import { SourceType } from "../../utility/constants";
import "./ToggleButton.scss";
import Tooltip from "../../../common/components/Tooltip/Tooltip";

interface IToggleButtonProps {
  label: string;
  name: string;
  selected: boolean;
  onSelect: (label: string) => void;
  disabled?: boolean;
  projectDocsCount: number;
  projectDocsIncludedCount: number;
  children?: React.ReactNode; // Optional children prop
  tooltipTextTitle?: string;
  tooltipTextContent?: any;
}

const ToggleButton: React.FC<IToggleButtonProps> = ({ label, name, projectDocsIncludedCount, projectDocsCount, selected, onSelect, disabled, children, tooltipTextTitle, tooltipTextContent }) => {
  const handleClick = () => {
    if (onSelect) {
      onSelect(label);
    }
  };

  return (
    <>
      {name === SourceType.ProjectDoc && projectDocsCount === 0 ?
        <Tooltip tooltipTitle={tooltipTextTitle} content={tooltipTextContent}>
          <button
            className={`toggle-button ${selected ? 'selected' : ''}`}
            onClick={handleClick}
            disabled={disabled}
          >
            {label}
            <span className="toggle-button__docs-count"> {projectDocsIncludedCount}/{projectDocsCount}</span>
          </button>
        </Tooltip> :
        <button
          className={`toggle-button ${selected ? 'selected' : ''}`}
          onClick={handleClick}
          disabled={disabled}
        >
          {label}
          {name === SourceType.ProjectDoc ? <span className="toggle-button__docs-count">{projectDocsIncludedCount}/{projectDocsCount}</span> : ""}
        </button>
      }
    </>
  );
};

export default ToggleButton;
