import React from "react";
import "./ToggleSwitch.scss";

interface ToggleSwitchProps {
  checked?: boolean;
  onChange: Function;
  id?: string;
  type?: number;
  disabled?:boolean
}

const ToggleSwitch: React.FC<ToggleSwitchProps> = ({
  checked = false,
  onChange,
  type = 1,
  id = "app-toggle-switch",
  disabled=false
}) => {
  return (
    <label className={`switch type-${type} ${disabled ? 'toggle_disabled' : ''}`}>
      <input
        type="checkbox"
        checked={checked}
        onChange={() => onChange()}
        id={id}
      />
      <span className="slider round"></span>
    </label>
  );
};

export default ToggleSwitch;
