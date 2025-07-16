import React, { useEffect, useRef, useState } from "react";
import "./MultiSelectDropDown.scss";
import AppIcon from "../../../common/components/AppIcons/AppIcons";
import { chevronDown, chevronUp } from "../AppIcons/Icons";
import { IconSizes } from "../../utility/constants";
import t from "../../../locales/en.json";
export interface Option {
  label: string;
  value: string;
}

export interface MultiSelectProps {
  options: Option[];
  selectedValues: string[];
  onChange: (selected: string[]) => void;
  placeholder?: string;
}
const MultiSelectDropdown: React.FC<MultiSelectProps> = ({
  options,
  selectedValues,
  onChange,
  placeholder = t.SelectOption,
}) => {
  const [isOpen, setIsOpen] = useState(false);
  const toggleDropdown = () => setIsOpen(!isOpen);
  const dropdownRef = useRef<any>(null);

  const handleOptionClick = (value: string) => {
    let updatedSelectedValues: string[];
    if (selectedValues.includes(value)) {
      updatedSelectedValues = selectedValues.filter((item) => item !== value);
    } else {
      updatedSelectedValues = [...selectedValues, value];
    }
    onChange(updatedSelectedValues);
  };

  const isSelected = (value: string) => selectedValues.includes(value);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        dropdownRef &&
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target)
      )
        setIsOpen(false);
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <div className="multi-select" ref={dropdownRef}>
      <div className="multi-select__control" onClick={toggleDropdown}>
        <div className="multi-select__value">
          {selectedValues.length > 0
            ? selectedValues
                .map(
                  (val) => options.find((option) => option.value === val)?.label
                )
                .join(", ")
            : placeholder}
        </div>
        <div className="multi-select__arrow">
          {isOpen ? (
            <AppIcon icon={chevronUp} size={IconSizes.SMALL} />
          ) : (
            <AppIcon icon={chevronDown} size={IconSizes.SMALL} />
          )}
        </div>
      </div>
      {isOpen && (
        <div className="multi-select__options">
          {options.map((option) => (
            <div
              key={option.value}
              className={`multi-select__option ${isSelected(option.value) ? "selected" : ""}`}
              onClick={() => handleOptionClick(option.value)}
            >
              <div
                className={`multi-select__options__custom-checkbox ${isSelected(option.value) ? "checked" : ""}`}
              >
                {isSelected(option.value) && (
                  <span className="multi-select__options__checkmark">✓</span>
                )}
              </div>
              <span>{option.label}</span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MultiSelectDropdown;
