import DOMPurify from "dompurify";
import config from "../configs/env.config";
import { FILE_SIZES } from "./constants";

const moment = require("moment-timezone");

// Get the current date and time in the user's time zone
type DateTimeFormat = "dddd h:mmA" | "YYYY-MM-DD HH:mm:ss";
export const getCurrentDateTimeFormatted = (format: DateTimeFormat) => {
  return moment().format(format);
};

// Format a given date string into 'dddd h:mmA' format
export const formatDateToDayTime = (dateString: string) => {
  return moment(dateString).format("dddd h:mmA");
};

// Format a given date string into 'YYYY-MM-DD HH:mm:ss' format
export const formatDateToDateTime = (dateString: string) => {
  return moment(dateString).format("YYYY-MM-DD HH:mm:ss");
};

export const getExternalAppKey = (appKey: string | null) => {
  switch (appKey) {
    case "PROJECT_MANAGEMENT":
      return "CE4-PMO";
    case "VALUE_CAPTURE":
      return "CE4-VC";
    case "OPERATING_MODEL":
      return "CE4-OM";
    case "TSA":
      return "CE4-TSA";
    case "LEGAL_ENTITY":
      return "CE4-LE";
    case "CONTRACT_MANAGEMENT":
      return "CE4-CM";
    case "PERFORMANCE_AND_TURNAROUND_CENTER":
      return "CE4-PTC";
    case "APP_INVENTORY_TRACKER":
      return "CE4-AIT";
    case "PIPELINE_MANAGEMENT":
      return "CE4-TP";
    default:
      return "PROJECT_LEVEL";
  }
};

export const getMimeTypeByFileName = (fileName: string) => {
  const extensionToMIME: any = {
    png: "image/png",
    jpg: "image/jpeg",
    jpeg: "image/jpeg",
    gif: "image/gif",
    svg: "image/svg+xml",
    webp: "image/webp",
    pdf: "application/pdf",
    txt: "text/plain",
    xml: "text/xml",
    html: "text/html",
    csv: "text/csv",
    css: "text/css",
    js: "application/javascript",
    doc: "application/msword",
    docm: "application/vnd.ms-word.document.macroEnabled.12",
    docx: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    ppt: "application/vnd.ms-powerpoint",
    pptm: "application/vnd.ms-powerpoint.presentation.macroEnabled.12",
    pptx: "application/vnd.openxmlformats-officedocument.presentationml.presentation",
    xls: "application/vnd.ms-excel",
    xlsm: "application/vnd.ms-excel.sheet.macroEnabled.12",
    xlsx: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    // Add more mappings as needed
  };

  const extension = extractFileExtention(fileName);
  return extensionToMIME[extension] || "application/octet-stream";
};

const extractDomain = (url: string): string => {
  const hostname = new URL(url).hostname;
  const match = hostname.match(/(?<=\.)[^.]+\.[^.]+$/);
  return match ? match[0] : "";
};

export const formatBytes = (_bytes: string, decimals: number = 1) => {
  const bytes = parseInt(_bytes);
  if (isNaN(bytes)) return "";
  if (bytes === 0) return `${bytes} ${FILE_SIZES[bytes]}`;
  const divisor = 1024,
    i = Math.floor(Math.log(bytes) / Math.log(divisor));
  return `${parseFloat((bytes / Math.pow(divisor, i)).toFixed(decimals))} ${FILE_SIZES[i]}`;
};

export const extractFileExtention = (fileName: string = ""): string => {
  return fileName.split(".").pop() || "";
};

export const downloadBlobFile = (file:Blob, fileName:string)=>{
  const link = document.createElement("a");
  const url = URL.createObjectURL(file);
  link.href = url;
  link.download = fileName
  link.click();
  URL.revokeObjectURL(url);
}

export const sanitizeContent = (content: string): string => {
  const formattedContent = content
    .replace(/\n/g, "<br />")
    .replace(/(<br\s*\/?>)+\s*$/gi, "")
    .trim();

  return DOMPurify.sanitize(formattedContent);
};

export const containsXSS = (input: string) => {
  const xssPatterns = [
    /<script/i,
    /<\/script/i,
    /javascript:/i,
    /on\w+\s*=\s*["']\s*.*?\s*["']/i,
    /<img.*?src=["']\s*javascript:\s*.*?\s*["']/i,
  ];
  return xssPatterns.some((pattern) => pattern.test(input));
};

export const validateDomain = (targetDomain: string): boolean => {
  try {
    const appDomain = extractDomain(config.REACT_APP_URL);
    const expectedDomain = extractDomain(targetDomain);
    return appDomain === expectedDomain;
  } catch (e) {
    return false;
  }
};

export const startsWithSpecialChar = (val: string) => {
  if (typeof val === "string" && val.length > 0) {
    const regex = /^[^a-zA-Z0-9]/;
    return regex.test(val.trim());
  }
  return false;
};

export const getCurrentFormattedDate = (): string => {
  const date = new Date();
  const pad = (num: number): string => String(num).padStart(2, "0");

  const year: number = date.getFullYear();
  const month: string = pad(date.getMonth() + 1); // Months are zero-indexed
  const day: string = pad(date.getDate());
  const hours: string = pad(date.getHours());
  const minutes: string = pad(date.getMinutes());
  const seconds: string = pad(date.getSeconds());

  return `${year}-${month}-${day}_${hours}-${minutes}-${seconds}`;
};
export const channel = new BroadcastChannel('message-id-broadcast-channel');

export const compareTwoArraysProperties=(arr1:Object[], arr2: Object[]): boolean =>{
  if (arr1.length !== arr2.length) return true;

  for (let i = 0; i < arr1.length; i++) {
    const obj1 = arr1[i];
    const obj2 = arr2[i];

    // Compare each property
    for (const key in obj1) {
      if (obj1[key as keyof Object] !== obj2[key as keyof Object]) {
        return true; // Exit early if a difference is found
      }
    }
  }

  return false; // Return true if all properties match
}