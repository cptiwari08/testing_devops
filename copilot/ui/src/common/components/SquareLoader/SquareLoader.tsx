import React from "react";
import "./SquareLoader.scss";

const SquareLoader: React.FC<{ text?: string }> = ({ text }) => {
  return (
    <>
      <div className="loader-square">
        <div className="loader-square__block"></div>
        <div className="loader-square__block"></div>
        <div className="loader-square__block"></div>
      </div>
      {text && <div className="loader-square__message">{text}</div>}
    </>
  );
};

export default SquareLoader;