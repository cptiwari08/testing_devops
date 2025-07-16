import React, { useRef, useEffect, ReactNode, FC } from "react";
import { useLocation } from "react-router-dom";
import t from "../../../locales/en.json";
import "./CrashBoundary.scss";

interface ErrorBoundaryState {
  hasError: boolean;
}

type ErrorBoundaryProps = {
  children: ReactNode;
};

const Crash: React.FC<{ onRetry: () => void }> = ({ onRetry }) => (
  <div className="crash-boundary-info">
    <p>{t.crashBoundaryTitle}</p>
    <p>{t.crashBoundarySubTitle}</p>
  </div>
);

class ErrorBoundary extends React.Component<
  ErrorBoundaryProps,
  ErrorBoundaryState
> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError() {
    return { hasError: true };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error("ErrorBoundary caught an error:", error, errorInfo);
  }

  resetErrorBoundary = () => {
    this.setState({ hasError: false });
  };

  render() {
    if (this.state.hasError) {
      return <Crash onRetry={this.resetErrorBoundary} />;
    }
    return this.props.children;
  }
}

const CrashBoundary: FC<ErrorBoundaryProps> = ({ children }) => {
  const location = useLocation();
  const errorBoundaryRef = useRef<ErrorBoundary>(null);

  useEffect(() => {
    if (errorBoundaryRef.current) {
      errorBoundaryRef.current.resetErrorBoundary();
    }
  }, [location]);

  return <ErrorBoundary ref={errorBoundaryRef}>{children}</ErrorBoundary>;
};

export default CrashBoundary;
