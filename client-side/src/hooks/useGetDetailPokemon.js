import { axiosInstance } from "../api/axiosClient";
const useGetDetailPokemon = () => {
    const pokemon = async (pokemonId = 1) => {
        try {
            const response = await axiosInstance.get(`Pokemon/${pokemonId}`);
            console.log(response.data)
            return response.data;
        } catch (err) {
            console.log(err.response.data.message || 'Get pokemons failed')
        }
    };

    return pokemon;
};

export { useGetDetailPokemon };
