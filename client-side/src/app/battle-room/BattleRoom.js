import React, { useEffect } from 'react'
import { Flex, Progress, Popover } from 'antd';
import { useState } from 'react';
import "./BattleRoom.css"
import "./AnimationPlayer.css"
import "./AnimationOpponent.css"
import { ClassAnimationPlayer } from '../../utils/ClassAnimationPlayer';
import { ClassAnimationOpponent } from '../../utils/ClassAnimationOpponent';
import pokeball from "../../assets/img/pokeball.png"
import useGetRoomBattle from '../../hooks/useGetRoomBattle';
import { useParams } from "react-router-dom";
import useUserInfo from '../../hooks/useUserInfo';

const twoColors = {
  '0%': '#108ee9',
  '100%': '#87d068',
};

export default function BattleRoom() {
  const { roomId } = useParams();

  const { roomBattle } = useGetRoomBattle(roomId);
  const { user } = useUserInfo();

  const [attack, setAttack] = useState(null);
  const [animationClass, setAnimationClass] = useState('');
  const [enemyAnimationClass, setEnemyAnimationClass] = useState('');
  const [player, setPlayer] = useState(null);
  const [opponent, setOpponent] = useState(null);
  const [isPlayer, setIsPlayer] = useState(false);

  const handleAttack = (move) => {
    setAttack(move);
    setEnemyAnimationClass("enemy-missing");

    var animateClass = "";
    if(player?.UserName === "caokay") {
      animateClass = ClassAnimationPlayer[move];
    }
    else {
      animateClass = ClassAnimationOpponent[move];
    }

    if(animateClass) {
      setAnimationClass(animateClass)
    }
    
    setTimeout(() => {
      setEnemyAnimationClass("enemy-damage-player");

      setTimeout(() => {
        setEnemyAnimationClass("")
      }, 500)
    }, 500)

    setTimeout(() => {
      setAnimationClass('')
      setAttack(null)
    }, 1000);
  };

  useEffect(() => {
    if (roomBattle?.Participants[0]?.UserName === user?.data?.UserName) {
      setPlayer(roomBattle?.Participants[0]);
      setOpponent(roomBattle?.Participants[1]);
    } else {
      setPlayer(roomBattle?.Participants[1]);
      setOpponent(roomBattle?.Participants[0]);
    }
  }, [roomBattle]);

  return (
    <div className="grid grid-cols-12 gap-4">

      <div 
        className="md:col-span-8 col-span-12 p-4 border border-gray-300 rounded-lg relative flex flex-col justify-between"
         >
        <div 
          className="flex w-full"
          style={{
            height: "60vh",
            backgroundImage: `url("https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/2fb2821a-1406-4a1d-9b04-6668f278e944/d843okx-eb13e8e4-0fa4-4fa9-968a-e0f36ff168de.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzJmYjI4MjFhLTE0MDYtNGExZC05YjA0LTY2NjhmMjc4ZTk0NFwvZDg0M29reC1lYjEzZThlNC0wZmE0LTRmYTktOTY4YS1lMGYzNmZmMTY4ZGUucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.TIK_E5L8dTyBUk_dADA5WkLP8jSJMR7YGJG54KNAido")`,
            backgroundSize: 'cover',
            backgroundPosition: 'center',
          }} >
          <div className="md:col-span-1 col-span-12 p-4 w-64 bg-[rgba(0,0,0,0.3)] flex flex-col items-center">
            <p className="text-white font-bold">{player?.UserName}</p>
            <img
              style={{width: "190px", marginLeft: 40}}
              src="https://tcrf.net/images/2/28/PokeBW_Development_Trainer_Hilbert_r6250.png"
            />
            <div className="grid gap-4 grid-cols-2">
              {player?.pokemons.map(() => (
                <div>
                  <img
                    style={{width: "70%"}}
                    src={pokeball}
                  />
                </div>
              ))}
            </div>
          </div>
          <div className="md:col-span-10 col-span-12 p-4 w-full">
            <div className="flex flex-col items-end pr-4 h-40">
              <div className="flex flex-col items-center">
              <h3 className="text-lg font-semibold text-border font-extrabold">
                {opponent?.pokemons[0]?.Name}
              </h3>
                <Progress
                  style={{
                    border: "2px solid white",
                    borderRadius: "11px"
                  }}
                  percent={100}
                  percentPosition={{
                    align: 'center',
                    type: 'inner',
                  }}
                  size={[200, 17]}
                />
                <Popover content={() => (
                  <div>
                  <p>Content</p>
                  <p>Content</p>
                </div>
                )} title="Title" trigger="hover">
                    <img
                      src={opponent?.pokemons[0]?.Sprites?.Front}
                      alt="Charizard"
                      className={`w-auto h-35 mr-4 ml-4 ${animationClass}`}
                    />
                </Popover>
                {(attack == "Flamethrower") && (
                  <div>
                    <div className="flame-opponent"></div>
                    <div className="flame-opponent mt-5 ml-2"></div>
                    <div className="flame-opponent mt-10 ml-4"></div>
                  </div>
                )}
              </div>
            </div>

            <div className="flex flex-col items-start pl-4">
              <div className="flex flex-col items-center">
                <h3 className="text-lg font-semibold text-border font-extrabold">
                  {player?.pokemons[0]?.Name}
                </h3>
                <Progress
                  style={{
                    border: "2px solid white",
                    borderRadius: "11px"
                  }}
                  percent={100}
                  percentPosition={{
                    align: 'center',
                    type: 'inner',
                  }}
                  size={[200, 17]}
                />
                <img
                  src={player?.pokemons[0]?.Sprites?.Back}
                  alt={player?.pokemons[0]?.Name}
                  className={`w-auto h-35 mr-4 ml-4 ${enemyAnimationClass}`}
                />
              </div>
            </div>
          </div>
          <div className="md:col-span-1 col-span-12 p-4 w-64 bg-[rgba(0,0,0,0.3)] flex flex-col items-center">
          <p className="text-white font-bold">
            {opponent?.UserName}
          </p>
            <img
              style={{width: "190px", marginLeft: 40}}
              src="https://tcrf.net/images/2/28/PokeBW_Development_Trainer_Hilbert_r6250.png"
            />
            <div className="grid gap-4 grid-cols-2 items-center">
              {[1, 2, 3, 4].map(() => (
                <div className="flex justify-center items-center">
                  <img
                    style={{width: "70%"}}
                    src={pokeball}
                  />
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className="flex-grow bg-gray-100 p-2 rounded mt-auto">
            <h3 className="text-lg font-semibold">Battle Log</h3>
            <div className="space-y-2">
              <div className="bg-gray-200 p-2 rounded">Charizard used Flamethrower!</div>
              <div className="bg-gray-200 p-2 rounded">Pikachu's HP is now 75/100.</div>
            </div>
          </div>
      </div>

      <div className="md:col-span-4 col-span-12 p-4 border border-gray-300 rounded-lg">
        <h3 className="text-lg font-semibold mb-4">Choose Your Pokémon</h3>
        <div className="grid grid-cols-2 gap-4 mb-6">
          {roomBattle?.Participants[0]?.pokemons.map((item) => (
            <button className="bg-blue-500 text-white px-4 py-2 rounded">{item?.Name}</button>
          ))}
        </div>

        <h3 className="text-lg font-semibold mb-4">Choose Attack</h3>
        <div className="grid grid-cols-2 gap-4">
          <button 
            className="bg-green-500 text-white px-4 py-2 rounded"
            onClick={() => handleAttack('normal')}
            >
              <p className="text-black font-bold">Attack Normal</p>
              <div className="flex justify-between">
                <p>Rock</p>
                <p>20/20</p>
              </div>
          </button>
          <button 
            className="bg-green-500 text-white px-4 py-2 rounded"
            onClick={() => handleAttack('flash')}
            >
              <p className="text-black font-bold">Attack Flash</p>
              <div className="flex justify-between">
                <p>Normal</p>
                <p>8/8</p>
              </div>
          </button>
          <button 
            className="bg-green-500 text-white px-4 py-2 rounded"
            onClick={() => handleAttack('Flamethrower')}
            >
              <p className="text-black font-bold">Flamethrower</p>
              <div className="flex justify-between">
                <p>Normal</p>
                <p>8/8</p>
              </div>
          </button>
          <button className="bg-purple-500 text-white px-4 py-2 rounded">Electro Ball</button>
        </div>
      </div>
    </div>
  )
}
