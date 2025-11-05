import React from "react";
import { Carousel } from "antd";
import{ bannerData } from "../../../data/bannerData";

export default function BannerSlider() {
    return (
        <Carousel autoplay>
            {bannerData.map((item, index) => (
                <div
                    key={index} 
                    className={
                        `relative h-[500px] md:h-[600px] bg-gradient-to-r flex items-center 
                        justify-center ${item?.gradient} `
                    }>
                    <img
                        style={{
                            width: "100%",
                        }}
                        src={item?.image}
                        alt={item?.alt || `banner-${index}`}
                        className="absolute top-0 left-0 w-full h-full object-cover"
                    />
                </div>
            ))}
        </Carousel>
    );
}