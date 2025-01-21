import { Select, Slider } from "antd";
import React, { useRef, useState } from "react";
import { DownOutlined } from "@ant-design/icons";
import { useGetDetailPokemon } from "../../hooks/useGetDetailPokemon";
import { notification } from "antd";
import MoveItems from "../moves/MoveItems";

export default function DetailPokemon({ pokemonId, onPokemonsChange }) {
  const maxTotalStat = useRef(0);
  const [value, setValue] = useState([]);
  const [currentPokemon, setCurrentPokemon] = useState(null)
  const [customStats, setCustomStats] = useState({
    hp: 50,
    attack: 50, 
    defense: 50,
    specialAttack: 50,
    specialDefense: 50,
    speed: 50,
  });
  // const [newPokemon, setNewPokemon] = useState(
  //   {
  //     pokemonId: "",
  //     stat: {
  //       hp: 0,
  //       atk: 0,
  //       defense: 0,
  //       spAtk: 0,
  //       spDef: 0,
  //       speed: 0
  //     },
  //     moveIds: []
  //   }
  // )

  const suffix = (
    <>
      <span>
        {value.length} / {4}
      </span>
      <DownOutlined />
    </>
  );

  const getPokemon = useGetDetailPokemon(pokemonId);

  const handleStatChange = (stat, value) => {
    setCustomStats({ ...customStats, [stat]: value });
  };

  const handleSavePokemon = () => {
    const newPokemon = {
      pokemonId: pokemonId,
      stat: {
        hp: customStats.Hp - currentPokemon?.Stat?.Hp,
        atk: customStats.Atk - currentPokemon?.Stat?.Atk,
        defense: customStats.Defense - currentPokemon?.Stat?.Defense,
        spAtk: customStats.SpAtk - currentPokemon?.Stat?.SpAtk,
        spDef: customStats.SpDef - currentPokemon?.Stat?.SpDef,
        speed: customStats.Speed - currentPokemon?.Stat?.Speed
      },
      moveIds: value
    }
    onPokemonsChange(newPokemon)

    notification.open({
      message: "Save pokemon success",
      type: "success",
      showProgress: true,
      pauseOnHover: false,
  });
  }

  const getTotalStat = () => {
    return Object.keys(customStats).reduce((sum, stat) => {
      return sum + customStats[stat];
    }, 0)
  }

  useState(() => {
    const fetch = async () => {
        const pokemonDetailData = await getPokemon(pokemonId);
        const stats = pokemonDetailData?.data?.Stat

        setCurrentPokemon(pokemonDetailData.data)
        setCustomStats(stats)
        maxTotalStat.current = Object.keys(stats).reduce((sum, stat) => {
          return sum + stats[stat];
        }, 0) + 508;
    }
    fetch();
  }, [pokemonId])

  return (
    <>
      {currentPokemon && (
        <div className="p-4 rounded-lg bg-gray-100 shadow-md">
          <h2 className="text-xl font-bold capitalize mb-4">
            {currentPokemon?.Name}
          </h2>

          <div className="mb-6">
            <h3 className="font-bold text-lg mb-2">Abilities:</h3>
            <ul className="list-disc list-inside">
              {currentPokemon?.Abilities?.map((ability, index) => (
                <li key={index} className="text-sm capitalize">
                  {ability?.Ability?.Name}
                </li>
              ))}
            </ul>
          </div>

          <div className="mb-6">
            <h3 className="font-bold text-lg mb-2">Stats:</h3>
            {Object.keys(customStats).map((stat) => (
              <div key={stat} className="mb-4 flex items-center">
                <p className="capitalize w-32">{stat}:</p>
                <Slider
                  min={0}
                  max={252}
                  value={customStats[stat]}
                  onChange={(value) => {
                    const diff = value - customStats[stat];

                    if(diff <= 0 || getTotalStat() + diff <= maxTotalStat.current)
                      handleStatChange(stat, value)
                  }}
                  className="flex-1 mx-4"
                />
                <span className="text-sm w-12 text-right">
                  {customStats[stat]}
                </span>
              </div>
            ))}
          </div>
          
          <div className="mb-6">
            <h3 className="font-bold text-lg mb-2">Moves (Max 4):</h3>
            <Select
              mode="multiple"
              maxCount={4}
              value={value}
              onChange={setValue}
              style={{ width: "100%" }}
              suffixIcon={suffix}
              placeholder="Select up to 4 moves"
              tagRender={(props) => {
                const { label, value, closable, onClose } = props;
                return (
                  <div style={{
                    display: "flex",
                    alignItems: "center",
                    backgroundColor: "#e6f7ff",
                    borderRadius: "16px",
                    padding: "4px 8px",
                    margin: "2px",
                    maxHeight: "32px",
                    fontSize: "12px",
                    lineHeight: "1.2",
                  }}>
                    <span style={{ whiteSpace: "nowrap", overflow: "hidden", textOverflow: "ellipsis" }}>
                      {label}
                    </span>
                    {closable && (
                      <span
                      onClick={onClose}
                      style={{
                        marginLeft: "5px",
                        cursor: "pointer",
                        color: "#ff4d4f",
                        fontSize: "14px",
                      }}
                    >
                      ✖
                    </span>
                    )} 
                  </div>
                );
              }}
              options={currentPokemon?.Moves?.map((move, idx) => ({
                value: move?.Id,
                label: (
                    <MoveItems move={move} />
                )
              }))}
            />
          </div>

          <button 
            onClick={() => handleSavePokemon()}
            className="w-full py-2 text-white bg-blue-500 rounded-md hover:bg-blue-600">
              Save Pokémon
          </button>
        </div>
      )}
    </>
  );
}
