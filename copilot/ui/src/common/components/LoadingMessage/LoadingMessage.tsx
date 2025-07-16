import React from "react";
import "./LoadingMessage.scss";

interface LoadingMessageProps {
  message: string;
}

const LoadingMessage: React.FC<LoadingMessageProps> = ({
    message
}) => {
  return (
    <div>
      <div className="loading-message">
        <div>{message}</div>
        <div className="dots">
          <span>.</span>
          <span>.</span>
          <span>.</span>
        </div>
      </div>
    </div>
  );
};

export default LoadingMessage;
