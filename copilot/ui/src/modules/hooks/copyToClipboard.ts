import { useCallback } from 'react';

const useCopyToClipboard = (): ((str: string, copySuccessCallback: (success: boolean) => void) => void) => {
  const copyToClipboard = useCallback(
    async (str: string, copySuccessCallback: (success: boolean) => void) => {
      const copyToClipboardDeprecated = (e: ClipboardEvent) => {
        e.clipboardData?.setData("text/plain", str);
        copySuccessCallback(true);
        e.preventDefault();
      };

      try {
        await navigator.clipboard.writeText(str.trim());
        copySuccessCallback(true);
      } catch (e) {
        try {
          document.addEventListener("copy", copyToClipboardDeprecated);
          document.execCommand("copy");
          document.removeEventListener("copy", copyToClipboardDeprecated);
        } catch (e2) {
          console.error(e, e2);
          copySuccessCallback(false);
        }
      }
    },
    []
  );

  return copyToClipboard;
};

export default useCopyToClipboard;
