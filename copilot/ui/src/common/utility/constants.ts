export const ASK_YOUR_QUESTION_MAXLENGTH = 255;
export const DEFAULT_TOKEN_RENEWAL_OFFSET_SECONDS = "300";
export const EXTERNAL_APP_SETTINGS = "COPILOT_EXTERNAL_APP_SETTINGS";
export const FEEDBACK_CLOSE_IN_MILLISECONDS = 3000;
export const FILE_SIZES = [
  "Bytes",
  "KB",
  "MB",
  "GB",
  "TB",
  "PB",
  "EB",
  "ZB",
  "YB",
];
export const isIframe = window !== window.parent && !window.opener;

export enum ChatRole {
  USER = "user",
  ASSISTANT = "assistant",
}
export enum ResponseStatuses {
  IN_PROGRESS = "In Progress",
  SUCCESS = "Success",
  ERROR = "Error",
}
export enum SourceType {
  ProjectDoc = "project-docs",
  ProjectData = "project-data",
  EyGuidance = "ey-guidance",
  Internet = "internet",
}
export enum ReferenceSourceType {
  ProjectDoc = "project-docs",
  ProjectData = "project-data",
  EyGuidance = "ey-guidance-help-copilot",
  EyGuidanceIp = "ey-guidance-ey-ip",
  Internet = "internet",
}
export enum SessionKeys {
  CHAT_ID = "chatId",
  MESSAGE_ID = "messageId",
  EXCLUDED_PROJECT_DOCS = "excludedProjectDocs"
}

export enum EventKeys {
  ENTER = "Enter",
}

export enum IconSizes {
  SMALL = "1x",
  MEDIUM = "2x",
  LARGE = "3x",
  EXTRA_LARGE = "5x",
}

export enum ReferenceType {
  FILENAME = "Filename",
  DOCUMENTS = "Documents",
  PAGEKEY = "Page-key",
}
export enum ReferenceMode{
  COLLAPSE="collapse",
  EXPAND="expand"
}
export const ASSISTANT_KEY = "PORTAL:IS_ASSISTANT_ENABLED";
export const CONTEXT_KEY = "PROJECT_CONTEXT";
export const DEFAULT_SUGGESTION_COUNT = 4;
export const MAX_FEEDBACK_TEXT_LENGTH = 1000;
export const MAX_SUGGESTION_COUNT = 8;
export const COLLAPSE_ASSISTANT_MAX_WIDTH = 900;
export const SOURCE_CONFIGS_KEY = "SOURCE_CONFIGS";
export const CHAT_SCRIPT_FILE_NAME = "AssistantHistory";
export const MAX_SQL_TEXT_LENGTH = 5000;
export const MAX_SUGGESTION_TEXT_LENGTH = 250;
export const MAX_PROJECT_CONTEXT_LENGTH = 5000;

export enum DownloadHistoryFileType {
  PDF = "pdf",
  WORD = "docx",
  PPT = "pptx",
}
