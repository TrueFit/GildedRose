import * as React from "react";
type Props = {
    imgLogo: string,
};

export const Footer = (props: Props) => {
    const footerContainerStyle: React.CSSProperties = {
        display: "flex",
        backgroundColor: "#FFF",
        // border: "1px solid #ccc",
        alignItems: "center",
        padding: "10px",
    };

    const footerItemStyle: React.CSSProperties = {
        flex: 1,
    };

    const footerWideItem: React.CSSProperties = {
        flex: 3,
    };

    const imgPremier = require("./headerResources/header.png");

    // TODO: USE SETTINGS HERE FOR YEAR!
    return (
        <div style={footerContainerStyle}>
            <div style={footerWideItem}>
                <div>
                    <div> Copyright 2018 Gilded Rose Inc - All rights reserved. </div>
                    <div>
                        <a href="http://localhost/content/PrivacyPolicy.pdf">Privacy Policy &amp; Terms of Use</a>
                    </div>
                </div>
            </div>
            <div style={footerItemStyle}>
                <img src={props.imgLogo} />
            </div>
            <div style={footerItemStyle}>
                <img src={imgPremier} />
            </div>
        </div>
    );
};
