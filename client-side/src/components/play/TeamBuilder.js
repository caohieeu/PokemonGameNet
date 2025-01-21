import React, { useState } from 'react';
import {
    Button,
    Drawer,
    Input,
    Radio,
    Space,
    Tabs,
    Spin
} from 'antd';
import "./TeamBuilder.css"
import TeamBuilderBody from './TeamBuilderBody';
import { axiosInstance } from '../../api/axiosClient';
import { usePostTeam } from '../../hooks/usePostTeam';

const TeamBuilder = () => {
    const [open, setOpen] = useState(false);
    const [placement, setPlacement] = useState('left');
    const [tabPosition, setTabPosition] = useState('left');
    // const [tabLabels, setTabLabels] = useState(["Team 1", "Team 2", "Team 3"]);
    const [teams, setTeams] = useState([
        {name: "Team 1", pokemons: []}
    ])
    const [activeTeamIndex, setActiveTeamIndex] = useState(0);
    const [loadingSubmit, setLoadingSubmit] = useState(false);

    const postTeam = usePostTeam();

    const handleTeamNameChange = (newName, index) => {
        setTeams((prevTeams) =>
            prevTeams.map((team, i) =>
                i === index ? { ...team, name: newName } : team
            )
        );
    };    

    const handlePokemonChange = (newPokemon, index) => {
        setTeams((prevTeams) => {
          return prevTeams.map((team, i) => 
            i === index 
              ? { ...team, pokemons: [...team.pokemons, newPokemon] } 
              : team
          );
        });
      };
      
    const handleAddNewTeam = () => {
        setTeams((prevTeams) => [
          ...prevTeams,
          { name: `Team ${prevTeams.length + 1}`, pokemons: [] },
        ]);
        setActiveTeamIndex(teams.length);
    }

    const handleSubmitTeam = async () => {
        const team = teams[activeTeamIndex];
        
        setLoadingSubmit(true);

        try {
            await postTeam(team);

            setLoadingSubmit(false);
        } catch {
            setLoadingSubmit(false);
        }
    }

    // const handleLabelChange = (value, index) => {
    //     const newLabels = [...tabLabels];
    //     newLabels[index] = value;
    //     setTabLabels(newLabels);
    // };

    const showDrawer = () => {
        setOpen(true);
    };

    const onClose = () => {
        setOpen(false);
    };
    return (
        <>
            <Button
                className="w-full bg-yellow-500 text-black font-bold py-2 px-4 rounded-lg shadow-xl transform transition duration-300 hover:bg-yellow-600 hover:scale-105"
                type="primary"
                onClick={showDrawer}>
                Build Your Team
            </Button>
            <Drawer
                style={{ padding: 0 }}
                title="Build your team pokemon"
                placement={placement}
                width={800}
                onClose={onClose}
                open={open}
                extra={
                    <Space>
                        <Button onClick={handleAddNewTeam}>New Team</Button>
                        <Button onClick={onClose}>Cancel</Button>
                        <Button type="primary" onClick={onClose}>
                            OK
                        </Button>
                    </Space>
                }
            >
                <Tabs
                    style={{ padding: 0 }}
                    tabPosition={tabPosition}
                    activeKey={String(activeTeamIndex)}
                    onChange={(key) => setActiveTeamIndex(Number(key))}
                    items={teams?.map((team, index) => ({
                        key: String(index),
                        label: (
                          <div style={{ display: "flex", alignItems: "center" }}>
                            <Input
                              value={team.name}
                              onChange={(e) => handleTeamNameChange(e.target.value, index)}
                              style={{
                                width: "55px",
                                border: "none",
                                background: "transparent",
                                padding: "0",
                                fontWeight: "bold",
                              }}
                            />
                          </div>
                        ),
                        children: (
                            <div>
                                <TeamBuilderBody 
                                    // nameTeam={label} 
                                    onPokemonsChange={(newPokemons) => 
                                        handlePokemonChange(newPokemons, index)
                                    }
                                />
                                {false ? (
                                    <Button
                                        style={{ marginTop: "10px", marginLeft: "10px" }}
                                >
                                    <Spin 
                                        color="white"
                                        style={{ color: 'white' }} />
                                </Button>
                                ) : (
                                    <Button
                                        type="primary"
                                        onClick={async() => await handleSubmitTeam()}
                                        disabled={teams?.pokemons?.length < 4}
                                        style={{ marginTop: "10px", marginLeft: "10px" }}
                                    >
                                        Submit Team
                                    </Button>
                                )}
                            </div>
                        ),
                      }))}
                />
            </Drawer>
        </>
    );
};
export default TeamBuilder;