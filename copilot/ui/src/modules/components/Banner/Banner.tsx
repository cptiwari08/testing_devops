import React from "react";
import "./Banner.scss";
import bannerImage from "../../../assets/img/banner.svg";

function Banner() {
  return (
    <section className="banner">
      <img className="banner__image" src={bannerImage} alt="banner" />
    </section>
  );
}

export default Banner;
