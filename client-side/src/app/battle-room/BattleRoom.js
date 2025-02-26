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
import GameHubConnector from '../../context/GameHubConnector';
import { typeColors } from "../../utils/TypeColors";

const twoColors = {
  '0%': '#108ee9',
  '100%': '#87d068',
};

const getTypeColor = (type) => {
  const typeColors = {
    fire: "from-red-600 to-orange-500",
    water: "from-blue-500 to-cyan-400",
    grass: "from-green-500 to-lime-400",
    electric: "from-yellow-400 to-yellow-600",
    ice: "from-blue-300 to-blue-500",
    psychic: "from-purple-500 to-pink-400",
    normal: "from-gray-500 to-gray-700",
    fighting: "from-red-700 to-orange-600",
    ground: "from-yellow-700 to-brown-600",
    flying: "from-indigo-500 to-blue-400",
    bug: "from-green-600 to-yellow-400",
    rock: "from-gray-600 to-gray-800",
    ghost: "from-purple-700 to-black",
    dark: "from-black to-gray-700",
    dragon: "from-indigo-700 to-purple-500",
    steel: "from-gray-500 to-gray-300",
    fairy: "from-pink-400 to-pink-600"
  };
  return typeColors[type] || "from-gray-500 to-gray-700";
};

const DescriptionItem = ({ title, content }) => (
  <div className="site-description-item-profile-wrapper mt-2">
    <p className="site-description-item-profile-p-label">{title}:</p>
    {content}
  </div>
);

export default function BattleRoom() {
  const { roomId } = useParams();

  const { roomBattle, reload } = useGetRoomBattle(roomId);
  const { user } = useUserInfo();

  const [attack, setAttack] = useState(null);
  const [animationClass, setAnimationClass] = useState('');
  const [enemyAnimationClass, setEnemyAnimationClass] = useState('');
  const [missed, setMissed] = useState('');
  const [player, setPlayer] = useState(null);
  const [opponent, setOpponent] = useState(null);
  const [currentPokemon, setCurrentPokemon] = useState(null);
  const [isPlayer, setIsPlayer] = useState(false);
  const [damgePlayer, setDamagePlayer] = useState(null);
  const [damgeOpponent, setDamageOpponent] = useState(null);
  const [pendingBattleResult, setPendingBattleResult] = useState(null);

  const handleSwitch = (newPokemon) => {
    const connector = GameHubConnector;
    const executeTurn = {
      roomId: roomId,
      moveId: 0,
      newPokemon: newPokemon,
      type: "Switch",
      usernamePlayer: user?.data?.UserName
    }
    connector.ExecuteTurn(executeTurn)
  }

  const handleAttack = (move) => {
    const connector = GameHubConnector;
    const executeTurn = {
      roomId: roomId,
      moveId: move?.Id,
      newPokemon: 0,
      type: "Attack",
      usernamePlayer: user?.data?.UserName
    }
    connector.ExecuteTurn(executeTurn)

    setAttack(move?.Name);
    setEnemyAnimationClass("enemy-missing");

    var animateClass = "";
    if (player?.UserName === "caokay") {
      animateClass = ClassAnimationPlayer[move?.Name];
    }
    else {
      animateClass = ClassAnimationOpponent[move?.Name];
    }

    if (animateClass) {
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

  const handleReceiveBattleResult = (battleResult) => {
    console.log("Received battle result")
    if (!roomBattle || !user) {
      setPendingBattleResult(battleResult);
      return;
    }

    reload();

    console.log(battleResult)
    console.log("name" + roomBattle)
    console.log("username: " + user.data.UserName)
    var currentPlayer = roomBattle.Participants[0].UserName == user.data.UserName
      ? roomBattle.Participants[0]
      : roomBattle.Participants[1];

    console.log(currentPlayer)

    if (battleResult.attacker == currentPlayer.CurrentPokemon.Name)
      setDamagePlayer(battleResult.damageDealt)
    else
      setDamageOpponent(battleResult.damageDealt)

    setTimeout(() => {
      setDamageOpponent(null)
      setDamagePlayer(null)
      reload();
    }, 2000)
  }

  const handleMissedAttack = (username, moveName) => {
    // alert(`${username} missed ${moveName}`)
    setMissed(username)

    setTimeout(() => {
      setMissed(null);
      reload();
    }, 2000);
  }

  const handleSwitchPokemon = (username) => {
    console.log(username + " switch pokemon")
    setTimeout(reload(), 4000)
  }

  const handleFinished = (roomId, username) => {
    setTimeout(alert(`${username} win the game`), 2000)
  }

  useEffect(() => {
    if (roomBattle && user && pendingBattleResult) {
      handleReceiveBattleResult(pendingBattleResult);
      setPendingBattleResult(null);
    }
  }, [roomBattle, user, pendingBattleResult]);

  useEffect(() => {
    if (missed || damgeOpponent) {
      setTimeout(() => {
        setMissed(null);
        setDamageOpponent(null);
      }, 2000)
    }
  }, [missed, damgeOpponent])

  useEffect(() => {
    if (roomBattle?.Participants[0]?.UserName === user?.data?.UserName) {
      setPlayer(roomBattle?.Participants[0]);
      setOpponent(roomBattle?.Participants[1]);
    } else {
      setPlayer(roomBattle?.Participants[1]);
      setOpponent(roomBattle?.Participants[0]);
    }
  }, [roomBattle]);

  useEffect(() => {
    const connector = GameHubConnector;
    connector.connection.on("Finished", handleFinished)
    connector.connection.on("ReceiveBattleResult", handleReceiveBattleResult)
    connector.connection.on("MissedAttack", handleMissedAttack)
    connector.connection.on("SwitchPokemon", handleSwitchPokemon)

    return () => {
      const connector = GameHubConnector;
      connector.connection.off("ReceiveBattleResult", handleReceiveBattleResult);
      connector.connection.off("MissedAttack", handleMissedAttack)
      connector.connection.off("SwitchPokemon", handleSwitchPokemon)
    };
  }, [true])

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
              style={{ width: "190px", marginLeft: 40 }}
              src="https://tcrf.net/images/2/28/PokeBW_Development_Trainer_Hilbert_r6250.png"
            />
            <div className="grid gap-4 grid-cols-2">
              {player?.pokemons.map(() => (
                <div>
                  <img
                    style={{ width: "70%" }}
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
                  {opponent?.CurrentPokemon?.Name}
                </h3>
                <Progress
                  style={{
                    border: "2px solid white",
                    borderRadius: "11px"
                  }}
                  percent={Math.round((opponent?.CurrentPokemon?.Stat?.Hp * 100) / opponent?.CurrentPokemon?.OriginalStat?.Hp)}
                  percentPosition={{
                    align: 'center',
                    type: 'inner',
                  }}
                  size={[200, 17]}
                />
                <Popover content={() => (
                  <div style={{
                    borderTop: "1px solid #333"
                  }}>
                    <div>
                      <DescriptionItem title="Type" content={opponent?.CurrentPokemon?.Type?.map((type, idx) => {
                        return (
                          <span
                            key={idx}
                            style={{
                              backgroundColor: typeColors[type] || typeColors.Default,
                              color: 'white',
                              padding: '4px 8px',
                              borderRadius: '8px',
                              fontSize: '12px',
                            }}
                          >
                            {type}
                          </span>
                        );
                      })} />
                    </div>
                    <div className='flex '>
                      <div className='pr-3'>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>hp: </p>
                          <p>{opponent?.CurrentPokemon?.Stat?.Hp}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>atk: </p>
                          <p>{opponent?.CurrentPokemon?.Stat?.Atk}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>defense: </p>
                          <p>{opponent?.CurrentPokemon?.Stat?.Defense}</p>
                        </div>
                      </div>

                      <div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>sp atk: </p>
                          <p>{opponent?.CurrentPokemon?.Stat?.SpAtk}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>sp def: </p>
                          <p>{opponent?.CurrentPokemon?.Stat?.SpDef}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>speed: </p>
                          <p>{opponent?.CurrentPokemon?.Stat?.Speed}</p>
                        </div>
                      </div>
                    </div>
                  </div>
                )} title={opponent?.CurrentPokemon?.Name} trigger="hover">
                  <img
                    src={opponent?.CurrentPokemon?.Sprites?.Front}
                    alt="Charizard"
                    className={`w-auto h-35 mr-4 ml-4 ${animationClass}`}
                  />
                  {missed == player?.UserName && <div className="miss-text-opponent">Miss</div>}
                  {damgePlayer && <div className="damge-text-opponent">{damgePlayer}</div>}
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
                  {player?.CurrentPokemon?.Name}
                </h3>
                <Progress
                  style={{
                    border: "2px solid white",
                    borderRadius: "11px"
                  }}
                  percent={Math.round((player?.CurrentPokemon?.Stat?.Hp * 100) / player?.CurrentPokemon?.OriginalStat?.Hp)}
                  percentPosition={{
                    align: 'center',
                    type: 'inner',
                  }}
                  size={[200, 17]}
                />
                <Popover content={() => (
                  <div style={{
                    borderTop: "1px solid #333"
                  }}>
                    <DescriptionItem title="Type" content={player?.CurrentPokemon?.Type?.map((type, idx) => {
                      return (
                        <span
                          key={idx}
                          style={{
                            backgroundColor: typeColors[type] || typeColors.Default,
                            color: 'white',
                            padding: '4px 8px',
                            borderRadius: '8px',
                            fontSize: '12px',
                          }}
                        >
                          {type}
                        </span>
                      );
                    })} />
                    <div className='flex '>
                      <div className='pr-3'>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>hp: </p>
                          <p>{player?.CurrentPokemon?.Stat?.Hp}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>atk: </p>
                          <p>{player?.CurrentPokemon?.Stat?.Atk}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>defense: </p>
                          <p>{player?.CurrentPokemon?.Stat?.Defense}</p>
                        </div>
                      </div>

                      <div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>sp atk: </p>
                          <p>{player?.CurrentPokemon?.Stat?.SpAtk}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>sp def: </p>
                          <p>{player?.CurrentPokemon?.Stat?.SpDef}</p>
                        </div>
                        <div className='flex'>
                          <p className='pr-2 font-bold'>speed: </p>
                          <p>{player?.CurrentPokemon?.Stat?.Speed}</p>
                        </div>
                      </div>
                    </div>
                  </div>
                )} title={player?.CurrentPokemon?.Name} trigger="hover">
                  <img
                    src={player?.CurrentPokemon?.Sprites?.Back}
                    alt={player?.CurrentPokemon?.Name}
                    className={`w-auto h-35 mr-4 ml-4 ${enemyAnimationClass}`}
                  />
                  {missed == opponent?.UserName && <div className="miss-text-player">Miss</div>}
                  {damgeOpponent && <div className="damge-text-player">{damgeOpponent}</div>}
                </Popover>
              </div>
            </div>
          </div>
          <div className="md:col-span-1 col-span-12 p-4 w-64 bg-[rgba(0,0,0,0.3)] flex flex-col items-center">
            <p className="text-white font-bold">
              {opponent?.UserName}
            </p>
            <img
              style={{ width: "190px", marginLeft: 40 }}
              src="https://tcrf.net/images/2/28/PokeBW_Development_Trainer_Hilbert_r6250.png"
            />
            <div className="grid gap-4 grid-cols-2 items-center">
              {[1, 2, 3, 4].map(() => (
                <div className="flex justify-center items-center">
                  <img
                    style={{ width: "70%" }}
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
          {roomBattle?.Participants?.find(p => p.UserName === user?.data?.UserName)?.pokemons.map((item) => (
            <button
              disabled={item?.Stat?.Hp > 0 ? false : true}
              className={`${item?.Stat?.Hp > 0 ? "bg-blue-500" : "bg-gray-500"} text-white px-4 py-2 rounded`}
              onClick={() => handleSwitch(item?.Id)} >
              <div className='flex items-center'>
                <img
                  src={item?.Sprites?.Image}
                  alt={item?.Name}
                  className={`w-10 h-35 mr-4 ml-4`}
                />
                <p>{item?.Name}</p>
              </div>
            </button>
          ))}
        </div>

        <h3 className="text-lg font-semibold mb-4">Choose Attack</h3>
        <div className="grid grid-cols-2 gap-4 w-full bg-gray-900 p-4 rounded-lg border-2 border-gray-700">
          {roomBattle?.Participants?.find(p => p.UserName === user?.data?.UserName)?.CurrentPokemon?.Moves?.map((item) => (
            <button
              key={item.Name}
              disabled={item?.OriginalPP <= 0}
              className={`relative flex flex-col justify-between items-center h-16 p-3 rounded-md shadow-md text-white font-semibold transition-all duration-300
        ${item?.OriginalPP <= 0 ? "bg-gray-500 cursor-not-allowed opacity-50" : `bg-gradient-to-b ${getTypeColor(item.Type)} hover:scale-105 hover:ring-2 hover:ring-white`}`}
              onClick={() => handleAttack(item)}
            >
              <p className="text-md">{item.Name}</p>
              <div className="flex justify-between w-full text-sm">
                <span className="bg-white/30 px-2 py-1 rounded">{item.Type}</span>
                <p>{item.PP}/{item.OriginalPP}</p>
              </div>
            </button>
          ))}
        </div>
        {/* <button 
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
          <button className="bg-purple-500 text-white px-4 py-2 rounded">Electro Ball</button> */}
      </div>
    </div>
  )
}
