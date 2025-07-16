import React, { useState } from "react";
import PropTypes from "prop-types";
import "./Tooltip.scss";

interface ITooltipProps {
  position?: string;
  tooltipTitle?: string;
  content?: string;
  children?: React.ReactNode;
  customStyle?: {[key:string]: string};
}
enum TooltipPosition {
  top = "top",
  right = "right",
  bottom = "bottom",
  left = "left",
}

const Tooltip: React.FC<ITooltipProps> = ({
  children,
  content = "",
  tooltipTitle = "",
  customStyle = {},
  position = TooltipPosition.top,
}) => {
  const [isVisible, setIsVisible] = useState(false);

  const showTooltip = () => setIsVisible(true);
  const hideTooltip = () => setIsVisible(false);

  return (
    <div
      className="tooltip-container"
      onMouseEnter={showTooltip}
      onMouseLeave={hideTooltip}
    >
      {children}
      {isVisible && (
        <div
        style={customStyle}
          className={`tooltip-container__tooltip-box tooltip-container__tooltip-${position}`}
        >
          {tooltipTitle && (
            <span className="tooltip-container__tooltip-box__heading">
            {tooltipTitle}
          </span>
         
          )}
          {content && (
            <span className="tooltip-container__tooltip-box__description">
              {content}
            </span>
          )}
        </div>
       )} 
    </div>
  );
};

Tooltip.propTypes = {
  children: (props: any, propName: string, componentName: string) => {
    if (typeof props[propName] === 'bigint') {
      props[propName] = props[propName].toString();
    }
    return PropTypes.node.isRequired(props, propName, componentName, 'prop', propName);
  },
  position: PropTypes.oneOf([
    TooltipPosition.top,
    TooltipPosition.right,
    TooltipPosition.bottom,
    TooltipPosition.left,
  ]),
};

export default Tooltip;
