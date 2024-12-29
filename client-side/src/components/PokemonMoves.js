import axios from 'axios';
import React, { useEffect, useState } from 'react'
import { SERVER_URI } from '../utils/Uri';
import { useGetDetailPokemon } from '../hooks/useGetDetailPokemon';
import { useFetchDetailMove } from '../hooks/useFetchDetailMove';
import { Avatar, List } from 'antd';

export default function PokemonMoves({ pokemonId }) {
    const getPokemon = useGetDetailPokemon();
    const getMove = useFetchDetailMove();
    const [moves, setMoves] = useState([]);

    const getCategoryImage = (category) => {
        const moveImage = {
            physical: 'https://play.pokemonshowdown.com/sprites/categories/Physical.png',
            special: 'https://play.pokemonshowdown.com/sprites/categories/Special.png',
            status: 'https://play.pokemonshowdown.com/sprites/categories/Status.png',
        }

        return moveImage[category]
    }

    const typeColors = {
        normal: "#A8A878",
        fighting: "#C03028",
        flying: "#A890F0",
        poison: "#A040A0",
        ground: "#E0C068",
        rock: "#B8A038",
        bug: "#A8B820",
        ghost: "#705898",
        steel: "#B8B8D0",
        fire: "#F08030",
        water: "#6890F0",
        grass: "#78C850",
        electric: "#F8D030",
        psychic: "#F85888",
        ice: "#98D8D8",
        dragon: "#7038F8",
        dark: "#705848",
        fairy: "#EE99AC",
      };

    useEffect(() => {
        const fetchPokemonMoves = async () => {
            try {
                const pokemon = await getPokemon(pokemonId);
                const movesData = await Promise.all(
                    pokemon?.data?.Moves?.map(async (move) => {
                        const moveDetail = await getMove(move?.Id);
                        const categoryMove = moveDetail?.damage_class?.name;
    
                        return {
                            name: move?.Name,
                            category: categoryMove,
                            type: move?.Type?.Name,
                            typeImage: getCategoryImage(categoryMove),
                            power: move?.Power,
                            accuracy: move?.Accuracy,
                            pp: move?.PP,
                            effect: moveDetail?.effect_entries[0]?.short_effect
                        };
                    })
                );
                console.log(movesData);
                setMoves(movesData);
            }
            catch (ex) {
                console.log("fetch pokemon moves failed: " + ex.message)
            }
        }
        fetchPokemonMoves();
    }, [pokemonId])

    return (
        // <div style={{ display: 'flex', gap: '20px', flexWrap: 'wrap' }}>
        //     {moves.map((move, index) => (
        //         <div key={index} style={{ textAlign: 'center' }}>
        //             <img src={move.typeImage} alt={move.type} width="40" height="40" />
        //             <p>{move.name}</p>
        //         </div>
        //     ))}
        // </div>
        <List
            itemLayout="horizontal"
            dataSource={moves}
            renderItem={(move, index) => (
                <List.Item>
                    <List.Item.Meta
                        key={index}
                        title={<a href="https://ant.design">{move.name}</a>}
                        avatar={
                            <div style={{ 
                                    display: 'flex', 
                                    alignItems: 'center', 
                                    gap: '8px', 
                                    justifyContent: 'space-between',
                                    width: '120px',
                                }}>
                                <span
                                    style={{
                                        backgroundColor: typeColors[move.type] || typeColors.Default,
                                        color: 'white',
                                        padding: '4px 10px',
                                        borderRadius: '8px',
                                        fontSize: '12px',
                                    }}
                                    >
                                    {move.type}
                                </span>
                                <Avatar src={move.typeImage || 'https://via.placeholder.com/50'} />
                            </div>
                        }
                        description={
                            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
                              <span style={{backgroundColor: '#f5f5f5', padding: '4px 8px', borderRadius: '5px', display: 'flex' }}>
                                PP: <p className='font-semibold ml-2'>{move.pp}</p>
                              </span>
                              <span style={{ backgroundColor: '#f5f5f5', padding: '4px 8px', borderRadius: '5px', display: 'flex' }}>
                                Power: <p className='font-semibold ml-2'>{move.power}</p>
                              </span>
                              <span style={{ backgroundColor: '#f5f5f5', padding: '4px 8px', borderRadius: '5px', display: 'flex' }}>
                                Accuracy: <p className='font-semibold ml-2'>{move.accuracy}</p>
                              </span>
                              <span style={{ backgroundColor: '#f5f5f5', padding: '4px 8px', borderRadius: '5px', display: 'inline-flex' }}>
                                Effect: <p className='font-semibold ml-2'>{move.effect || 'N/A'}</p>
                              </span>
                            </div>
                          }
                    />
                </List.Item>
            )}
        />
    )
}
