import React, { useEffect, useState } from "react";
import {
  Input,
  List,
  Button,
  Card,
  Select,
  Avatar,
  Modal,
} from "antd";
import { SERVER_URI } from "../../constants/Uri";
import useGetUsersByUserName from "../../hooks/useGetUsersByUserName";
import "./Play.css"
import { useGetPokemons } from "../../hooks/useGetPokemons";
import { DownOutlined } from '@ant-design/icons';
import TeamBuilder from "../../components/play/TeamBuilder";
import Video from "../../assets/videos/video_fighting_2.mp4"
import ListTeam from "../../components/play/ListTeam";
import { typeColors } from "../../constants/TypeColors";
import GameHubConnector from '../../context/GameHubConnector';
import { useNavigate } from 'react-router-dom';

const { Option } = Select;
const { Search } = Input;
let timeout;
let currentValue;

export default function Play() {
  const navigate = useNavigate();

  const [searchTerm, setSearchTerm] = useState("");
  const [selectedPokemon, setSelectedPokemon] = useState([]);
  const [currentPokemon, setCurrentPokemon] = useState(null);
  const [pokemons, setPokemons] = useState(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [value, setValue] = useState(['Ava Swift']);
  const [loadingFindMatch, setLoadingFindMatch] = useState(false);
  const [customStats, setCustomStats] = useState({
    hp: 50,
    attack: 50,
    defense: 50,
    specialAttack: 50,
    specialDefense: 50,
    speed: 50,
  });
  const [objectQuery, setObjectQuery] = useState({
    page: 1,
    pageSize: 10,
    pokemonName: ""
  })

  const rooms = [
    { id: 1, name: "Room 1", players: 5 },
    { id: 2, name: "Room 2", players: 3 },
    { id: 3, name: "Room 3", players: 10 },
  ];

  const getPokemons = useGetPokemons();

  const suffix = (
    <>
      <span>
        {value.length} / {4}
      </span>
      <DownOutlined />
    </>
  );

  useEffect(() => {
    const fetchPokemons = async () => {
      const pokemonData = await getPokemons(objectQuery);
      setPokemons(pokemonData.data.data);
    }
    fetchPokemons();
  }, [objectQuery])

  useEffect(() => {
    const connector = GameHubConnector;
    connector.connection.on("MatchFound", handleMatchFound)

    return () => {
      const connector = GameHubConnector;
      connector.connection.off("MatchFound", handleMatchFound);
    };
  }, [true])

  const onSearch = (value, _e) => {
    setObjectQuery({ ...objectQuery, pokemonName: value, page: 1 })
  }

  const handleMatchFound = (response) => {
    setLoadingFindMatch(false)

    console.log("match found !")
    console.log(response)

    navigate(`/battle-room/${response.roomId}`);
  }

  const handleFindRandommatch = () => {
    setLoadingFindMatch(true);

    const connector = GameHubConnector;
    connector.FindMatch(setLoadingFindMatch);
  }

  const BodyCollapse = () => {
    return (
      <div>
        <div className="flex justify-end">
          <Search
            onSearch={onSearch}
            className="mb-8"
            placeholder="Search pokemon"
            style={{
              width: 200,
            }}
          />
        </div>

        <div
          style={{
            height: "400px",
            overflowY: "auto",
            border: "1px solid #ccc",
            borderRadius: "8px",
            padding: "8px"
          }}>
          <List
            itemLayout="vertical"
            dataSource={pokemons || []}
            renderItem={(pokemon, index) => (
              <List.Item
                style={{ cursor: "pointer" }}
                onClick={() => {
                  handleOpenModal()
                  setCurrentPokemon(pokemon)
                }}
              >
                <div
                  className="flex items-center gap-3 p-2 border-b border-gray-300"
                  style={{ alignItems: "center", flexWrap: "nowrap" }}
                >
                  <Avatar
                    src={pokemon?.Sprites?.Image}
                    size={36}
                    className="flex-shrink-0"
                  />
                  <h4 style={{ fontSize: "10px" }} className="font-bold flex-shrink-0">{pokemon?.Name}</h4>
                  {pokemon?.Type?.map((type, idx) => (
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
                  ))}
                  <p style={{ fontSize: "10px" }} className="text-gray-500 truncate flex-shrink-0">
                    {pokemon?.Abilities?.map((ability) => ability.Ability.Name).join(" | ")}
                  </p>

                  <div style={{ fontSize: "10px" }} className="flex gap-2 text-xxs text-center ml-auto">
                    {Object.entries(pokemon?.Stat).map(([key, value]) => (
                      <div
                        key={key}
                        className="p-1 rounded-md shadow-sm"
                        style={{ minWidth: "40px" }}
                      >
                        <p className="font-semibold text-gray-500">{key}</p>
                        <p className="font-bold text-gray-800">{value}</p>
                      </div>
                    ))}
                  </div>
                </div>
              </List.Item>
            )}
          />
        </div>

      </div>
    )
  }
  const items = [
    {
      key: '1',
      label: 'Build your team',
      children: <BodyCollapse />,
      showArrow: false,
    }
  ];

  const SearchInput = (props) => {
    const [value, setValue] = useState("");
    const { users } = useGetUsersByUserName(value);
    const handleSearch = (searchValue) => {
      setValue(searchValue);
    };
    const handleChange = (selectedValue) => {
      setValue(selectedValue);
      if (props.onSelect) {
        props.onSelect(selectedValue);
      }
    };
    return (
      <Select
        showSearch
        value={value}
        placeholder={props.placeholder}
        style={{ ...props.style, width: "100%" }}
        defaultActiveFirstOption={false}
        suffixIcon={null}
        filterOption={false}
        onSearch={handleSearch}
        onChange={handleChange}
        notFoundContent={null}
        options={(users || []).map((user) => ({
          value: user?.Id,
          label: (
            <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
              <img
                src={`${SERVER_URI}${user?.ImagePath}`}
                alt={user?.UserName}
                style={{
                  width: "32px",
                  height: "32px",
                  borderRadius: "50%",
                  objectFit: "cover"
                }}
              />
              <span>{user?.UserName}</span>
            </div>
          )
        }))}
      />
    );
  };

  const handleOpenModal = () => {
    setCurrentPokemon(null);
    setIsModalVisible(true);
  };

  const handleCloseModal = () => {
    setIsModalVisible(false);
    setCurrentPokemon(null);
  };

  const handleAddToTeam = () => {
    if (selectedPokemon.length < 4 && !selectedPokemon.includes(currentPokemon)) {
      setSelectedPokemon([...selectedPokemon, currentPokemon]);
      setIsModalVisible(false);
    }
  };

  return (
    <div
      className="min-h-screen bg-gradient-to-b from-green-100 to-blue-300 p-4 grid grid-cols-12 gap-4"
      style={{
        backgroundImage: `url("${SERVER_URI}/Images/Backgrounds/Battle_Lobby_Bg.png")`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
      }}
    >
      <div className="md:col-span-4 col-span-12">
        <h1 className="text-3xl font-bold text-center text-gray-800 mb-6">
          Play Pokémon Battle Simulator
        </h1>

        <div className="mb-8">
          <Card title="Search Users" className="shadow-lg">
            <SearchInput
              style={{ with: "100%" }}
              placeholder="input search text"
            />
            <div className="mt-4">
              <p className="text-gray-600">
                You searched for: <strong>{searchTerm || "None"}</strong>
              </p>
            </div>
          </Card>
        </div>

        <div className="mb-8">
          <Card title="Team Builder" className="shadow-lg">
            <TeamBuilder />
          </Card>
        </div>

        <div className="mb-8">
          <Card title="Battle" className="shadow-lg">
            {!loadingFindMatch ? (
              <Button
                type="primary"
                onClick={handleFindRandommatch}
                className="w-full bg-yellow-500 text-black font-bold py-2 px-4 rounded-lg shadow-xl transform transition duration-300 hover:bg-yellow-600 hover:scale-105"
              >
                Find Random Battle
              </Button>
            ) : (
              <Button
                type="primary"
                className="w-full bg-yellow-500 text-black font-bold py-2 px-4 rounded-lg shadow-xl matching-btn"
              >
                <p style={{ marginRight: "-7px" }}>Matching</p>
                <span className="matching-text">
                  <span className="letter">.</span>
                  <span className="letter">.</span>
                  <span className="letter">.</span>
                </span>
              </Button>
            )}

            <Button
              type="primary"
              onClick={handleOpenModal}
              className="w-full mt-6 bg-yellow-500 text-black font-bold py-2 px-4 rounded-lg shadow-xl transform transition duration-300 hover:bg-yellow-600 hover:scale-105"
            >
              Find Battle With Team
            </Button>
          </Card>
        </div>

        <div className="mb-8">
          <Card title="Available Rooms" className="shadow-lg">
            <List
              itemLayout="horizontal"
              dataSource={rooms}
              renderItem={(room) => (
                <List.Item
                  actions={[<Button type="primary" key="join">Join</Button>]}
                >
                  <List.Item.Meta
                    title={room.name}
                    description={`Players: ${room.players}`}
                  />
                </List.Item>
              )}
            />
          </Card>

          <Card title="Instructions" className="shadow-lg mt-8">
            <p className="text-gray-600">
              Select Pokémon from the list to build your team. You can adjust their stats
              and select moves in the modal that appears.
            </p>
          </Card>
        </div>
      </div>

      <div style={{ marginTop: '3.8rem' }} className="md:col-span-8 col-span-12">
        <div style={{ position: "relative", height: "500px", overflow: "hidden", borderRadius: "8px" }}>
          <video
            autoPlay
            loop
            muted
            style={{
              position: "absolute",
              top: "50%",
              left: "50%",
              transform: "translate(-50%, -50%)",
              minWidth: "100%",
              minHeight: "100%",
            }}
          >
            <source src={Video} type="video/mp4" />
          </video>

          <div
            style={{
              position: "absolute",
              top: 0,
              left: 0,
              width: "100%",
              height: "100%",
              backgroundColor: "rgba(0, 0, 0, 0.3)",
            }}
          ></div>

          <div
            style={{
              position: "absolute",
              top: "50%",
              left: "50%",
              transform: "translate(-50%, -50%)",
              color: "white",
              textAlign: "center",
            }}
          >
            <h1 className="text-3xl font-bold">Welcome to Pokémon Game Battle</h1>
            <p className="mt-2 text-lg">Choose your Pokémon and battle like a champion!</p>
          </div>
        </div>
        <Card title="How to Play" className="shadow-lg mb-8 mt-8">
          <div className="flex flex-col gap-4">
            <p className="text-gray-500">
              Welcome to Pokémon Battle! Follow these steps to get started:
            </p>
            <ol className="list-decimal list-inside text-gray-700">
              <li>Build your Pokémon team by catching and training them.</li>
              <li>Select the best moves and strategies for your team.</li>
              <li>Challenge other players in battles to earn rewards and climb the leaderboard.</li>
              <li>Participate in events to unlock rare Pokémon and exclusive items.</li>
            </ol>
            <button className="bg-blue-500 text-white py-2 px-6 mt-4 rounded hover:bg-blue-600">
              Learn More
            </button>
          </div>
        </Card>
      </div>

      <Modal
        // title={`Build Your Team`}
        visible={isModalVisible}
        onCancel={handleCloseModal}
        footer={[
          <Button key="cancel" onClick={handleCloseModal}>
            Cancel
          </Button>,
          <Button type="primary" key="add" onClick={handleAddToTeam}>
            Choose Team
          </Button>,
        ]}
      >
        <ListTeam />
      </Modal>
    </div>
  );
}
