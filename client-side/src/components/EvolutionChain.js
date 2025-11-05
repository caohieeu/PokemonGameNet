import React, { useEffect, useState } from 'react'
import { useFetchEvolutionChain } from '../hooks/useFetchEvolutionChain';
import { Flex, Spin } from 'antd';
import axios from 'axios';
import { POKEAPI_URI } from '../constants/Uri';

const contentStyle = {
    padding: 50,
    background: 'rgba(0, 0, 0, 0.05)',
    borderRadius: 4,
};

export default function EvolutionChain({ pokemonId }) {
    const [evolutionChain, setEvolutionChain] = useState([]);
    const [loading, setLoading] = useState(true);
    const fetchEvolution = useFetchEvolutionChain();

    const getChainId = async (pokemonId) => {
        try {
            const response = await axios.get(
                `${POKEAPI_URI}pokemon-species/${pokemonId}`
            )
            const data = response.data; console.log(data.evolution_chain.url);

            const evolutionChainUrl = data.evolution_chain.url;

            const evolutionChainId = evolutionChainUrl.split("/").filter(Boolean).pop();

            return evolutionChainId;
        } catch (err) {
            console.error("Fetch evolution chain id failed: ", err.message);
            return null;
        }
    }

    useEffect(() => {
        async function FetchData() {
            setLoading(true);

            const chainId = await getChainId(pokemonId);

            if (chainId) {
                const chain = await fetchEvolution(chainId);
                setEvolutionChain(chain);
            }
            else {
                console.error("Unable to fetch evolution chain ID");
            }
            setLoading(false);
        }
        FetchData();
    }, [pokemonId])

    return (
        <>
            {!loading ? (
                <div style={{ display: 'flex', gap: '20px' }}>
                    {evolutionChain?.map((pokemon, index) => (
                        <>
                            <div key={index} style={{ textAlign: 'center' }}>
                                <img src={pokemon.image} alt={pokemon.name} width="100" height="100" />
                                <p>{pokemon.name}</p>
                            </div>
                            {index < evolutionChain.length - 1 && (
                                <div style={{ fontSize: '24px', fontWeight: 'bold', lineHeight: 5 }}>→</div>
                            )}
                        </>
                    ))}
                </div>
            ) : (
                <Flex gap="middle" vertical>
                    <Flex gap="middle">
                        <Spin tip="Loading" size="large"></Spin>
                    </Flex>
                </Flex>
            )}
        </>
    )
}
