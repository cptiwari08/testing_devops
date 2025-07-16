import { SourceType } from "../common/utility/constants";

export interface ISuggestionsData {
  id: number;
  source: string;
  suggestionText: string;
  appAffinity: string;
  isIncluded?: boolean;
  createdAt: string;
  updatedAt: string;
  createdBy: string;
  updatedBy: string;
  answerSQL?: string;
  visibleToAssistant?: boolean;
}
export interface FetchCETokenPayload {
  projectId: string;
  scopes: object;
  isStrictAudience: boolean;
}

export interface UserAuthDetails {
  unique_name: string;
  email: string;
  family_name: string;
  given_name: string;
  oid: string;
  upn: string;
  sp_url: string;
  po_app_url: string;
  po_api_url: string;
  copilot_app_url: string;
  copilot_api_url: string;
  project_id: string;
  project_friendly_id: string;
  scope: SourceType[];
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}

export interface IProjectDocs {
  id: string;
  name: string;
  path: string;
  visibletoAI: boolean;
  aiProcessingStatus: null;
  createdBy: string;
  modifiedBy: string;
  size: string;
  linkingUri: string;
  embedUri?: string;
  visibleToAssistant?: string;
}

export interface IChatState {
  isLoading: boolean;
  chatId: string;
  question: IChatQuestion;
  output: IChatOutput;
  intermediaryResponse: IChatIntermediaryResponse;
  resetChatMessages?: boolean;
  messageList: IChatOutput[];
  feedbackResponse: IAssistantFeedbackResponse;
}

export interface IChatQuestion {
  messageId: number;
  question: string;
  chatId: string;
  sources: string[];
  context?: IChatContext | null
}

export interface IChatContext {
  suggestion?: {
    id: string;
    source: string;
  };
  isMessageLiked?: boolean;
  appInfo?: IAppInfo;
  excludeDocuments?: string[];
  consent?: boolean;
}

export interface IChatIntermediaryResponse {
  id: string;
  purgeHistoryDeleteUri: string;
  sendEventPostUri: string;
  statusQueryGetUri: string;
  terminatePostUri: string;
}

export interface ICitingSource {
  sourceName: string;
  sourceType: string | null;
  sourceValue: any[] | null;
}

export interface IFollowUpSuggestion {
  id: number;
  suggestionText: string;
}

export interface IChatOutput {
  messageId: number;
  InstanceId: string;
  role: string;
  content: string;
  sources: string[];
  inputSources:string[];
  context: IChatContext;
  response: IChatSourceResponse[];
  createdTime: string;
  lastUpdatedTime: string;
  status?: string;
  StatusCode?: number;
  followUpSuggestions?: IFollowUpSuggestion[];
}

export interface IchatResponseHistory {
  key: string;
  content: IChatOutput[];
}

export interface IChatStatusQuery {
  uri: string;
  messageId: string;
  instanceId: string;
}

export interface IResponseFeedback {
  MessageID: number;
  ChatId?: string;
  FeedbackText: string;
  IsLiked: boolean | null | undefined;
  InstanceId: string;
  Consent? : boolean;
}
export interface IAssistantFeedbackResponse {
  [id: string]: Partial<IResponseFeedback>;
}

export interface IExternalAppSettings {
  appInfo: IAppInfo;
  externalAppKey: string | null;
}

export interface IAppInfo {
  id: number;
  name: string;
  key: string;
  teamTypeIds: number[];
}

export interface IResetChatSession {
  reset: boolean;
  chatId?: string;
}

export interface IChatSourceResponse {
  sourceName: string;
  content: string;
  status: string | null;
  sqlQuery: string | null;
  citingSources: ICitingSource[] | null;
}
export interface IAssistantContextDetail {
  id: number;
  isEnabled: string;
  key: string;
  title: string;
  value: string;
}

export interface IAssistantEnabledDetails{
  id: number;
  isEnabled: string;
  key: string;
  title: string;
  value: string;
}

export interface IUserDetails{
  givenName: string;
  surname: string;
  mail: string;
  userType: string;
  accountType: string;
  photo: string | null;
}

export interface ISourceConfig {
  key: string,
  displayName: string,
  originalDisplayName: string,
  isQuestionTextBoxEnabled: boolean,
  description: string,
  isDefault: boolean,
  ordinal: number,
  isActive: boolean
}

export interface IsourceConfiguration{
  id:string
  isEnabled:boolean
  key:string
  title:string
  value:ISourceConfig
}