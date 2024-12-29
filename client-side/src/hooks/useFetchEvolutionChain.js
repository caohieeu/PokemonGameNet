import axios from "axios";
import { POKEAPI_URI } from "../utils/Uri";

const useFetchEvolutionChain = () => {
    const getPokemonImage = (id) => 
        `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`;

    const chainEvolution = async (chainId) => {
        try {
            const response = await axios.get(`https://pokeapi.co/api/v2/evolution-chain/${chainId}`);
            const data = response.data;
            
            const evolutionList = [];

            let current = data.chain;
            while(current) {
                const pokemonName = current.species.name;
                const pokemonId = current.species.url.split('/').filter(Boolean).pop();

                evolutionList.push({
                    name: pokemonName,
                    image: getPokemonImage(pokemonId)
                })

                current = current.evolves_to[0];
            }

            return evolutionList;
        } catch (err) {
            console.log(err.message || 'fetch evolution chain failed')
            return [];
        }
    };

    return chainEvolution;
};

export { useFetchEvolutionChain };
