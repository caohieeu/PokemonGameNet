import React from "react";
import Header from "../Header";
import { Carousel, Card, List } from "antd";
import ImageBattle from "../../assets/img/game_battle.png"

export default function Home() {
  const announcements = [
    { title: "January Event", description: "Join the Winter Pokémon Tournament!" },
    { title: "New Updates", description: "New generation Pokémon added to the system!" },
    { title: "New Features", description: "Explore the special PvP mode for players!" },
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-200 via-yellow-200 to-pink-200">
      <Header />

      <div className="relative w-full h-[400px] bg-yellow-400 flex items-center justify-center">
        <img
          src="https://raw.githubusercontent.com/PokeAPI/media/master/logo/pokeapi_256.png"
          alt="Pokemon Logo"
          className="absolute top-6 left-6 w-40"
        />
        <div className="text-center">
          <h1 className="text-4xl md:text-6xl font-bold text-white drop-shadow-lg">
            Pokémon Battle Simulator
          </h1>
          <p className="text-lg md:text-xl text-white mt-4">
            Experience epic Pokémon battles and conquer tournaments!
          </p>
        </div>
      </div>

      <div className="mt-8 px-4">
        <Carousel autoplay>
          <div 
            className="h-100 bg-gradient-to-r from-yellow-400 to-red-500 flex items-center justify-center"
            style={{
                position: "absolute"
            }} >
            <h2 className="text-white text-3xl font-bold" style={{
              position: "relative",
              top: 0,
              left: 100
            }}>Choose your favorite Pokémon!</h2>
            <img
              style={{
                width: "100%",
              }} 
              src={ImageBattle}
            />
          </div>
          <div className="h-64 bg-gradient-to-r from-green-400 to-blue-500 flex items-center justify-center">
            <h2 className="text-white text-3xl font-bold">Join exciting PvP tournaments!</h2>
          </div>
          <div className="h-64 bg-gradient-to-r from-purple-400 to-pink-500 flex items-center justify-center">
            <h2 className="text-white text-3xl font-bold">Test new strategies today!</h2>
          </div>
        </Carousel>
      </div>

      <div className="mt-8 px-4">
        <Card
          className="shadow-lg rounded-lg"
          title={
            <h2 className="text-xl md:text-2xl font-bold text-yellow-500">
              About Pokémon Game Battle Simulator
            </h2>
          }
          bordered={false}
          style={{ backgroundColor: "#fff" }}
        >
          <p>
            Pokémon Game Battle Simulator is where you can experience thrilling Pokémon battles with top-notch strategies. 
            With a rich Pokémon system and engaging PvP modes, we promise to deliver the best experience for you!
          </p>
        </Card>
      </div>

      <div className="mt-8 px-4">
        <Card
          className="shadow-lg rounded-lg"
          title={
            <h2 className="text-xl md:text-2xl font-bold text-red-500">
              Important Announcements
            </h2>
          }
          bordered={false}
          style={{ backgroundColor: "#fff" }}
        >
          <List
            itemLayout="vertical"
            dataSource={announcements}
            renderItem={(item) => (
              <List.Item>
                <List.Item.Meta
                  title={
                    <a href="#" className="text-blue-500 font-bold hover:underline">
                      {item.title}
                    </a>
                  }
                  description={<p className="text-gray-600">{item.description}</p>}
                />
              </List.Item>
            )}
          />
        </Card>
      </div>

      <footer className="mt-16 bg-gray-800 text-center text-white py-4">
        <p>© 2025 Pokémon Game Battle Simulator. All rights reserved.</p>
      </footer>
    </div>
  );
}
