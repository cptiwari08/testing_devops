import React from 'react';
import { FontAwesomeIcon, FontAwesomeIconProps } from '@fortawesome/react-fontawesome';
import "./AppIcons.scss"
const FaIcons: React.FC<FontAwesomeIconProps> = ({ icon, className, ...rest }) => {
    return (
        <FontAwesomeIcon
            icon={icon}
            className={`${className || ''} app-fontAwsome-icon ${rest.onClick ? 'icon-link' : ''}`}  
            {...rest}
        />
    )
}
export default FaIcons