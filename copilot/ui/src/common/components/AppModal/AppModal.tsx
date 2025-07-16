import React, { useRef, useEffect } from "react";
import "./AppModal.scss";
import FaIcons from "../AppIcons/AppIcons";
import { trinagleWarning, xMark } from "../AppIcons/Icons";
import { IconSizes } from "../../utility/constants";

interface ModalProps {
  showModal:boolean
  heading: string;
  description: string;
  confirmBtnText:string;
  cancelButtonText:string;
  iconName: any;
  onClose: () => void;
  onCancel: () => void;
  onConfirm: () =>void;
}

const AppModal: React.FC<ModalProps> = ({
  showModal,
  heading,
  iconName,
  description,
  confirmBtnText,
  cancelButtonText,
  onClose,
  onCancel,
  onConfirm
}) => {


  return (
    <>
    {showModal && (
    <div className="app-modal">
      <div className="modal-overlay">
        <div className="modal-container">

          <div className="modal-header">
            <span className="modal-header__icon warning">
          <FaIcons
                  icon={iconName}
                  size={IconSizes.MEDIUM}
                  className=".modal-close-btn"
                  onClick={onClose}
                />
              </span>
              <span className="modal-header__close-icon">
            <FaIcons
                  icon={xMark}
                  size={IconSizes.SMALL}
                  className=".modal-close-btn"
                  onClick={onClose}
                />
                </span>
          </div>
          <div className="modal-title">{heading}</div>

          <div className="modal-body">
            <p>{description}</p>
          </div>
          <div className="modal-footer">
            <button
              className="modal-action-btn modal-action-btn_confirm-btn"
              onClick={onConfirm}
            >
             {confirmBtnText}
            </button>
            <button className="modal-action-btn" onClick={onCancel}>
              {cancelButtonText}
            </button>
          </div>
        </div>
      </div>
    </div>
    )}
    </>
  );
};

export default AppModal;
