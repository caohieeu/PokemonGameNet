import { axiosInstance } from "../api/axiosClient";
const useGetPokemons = () => {
    const pokemons = async (params = {}) => {
        try {
            const queryString = new URLSearchParams(params).toString();

            const response = await axiosInstance.get(`Pokemon?${queryString}`);
            return response.data;
        } catch (err) {
            console.log(err.response.data.message || 'Get pokemons failed')
        }
    };

    return pokemons;
};

export { useGetPokemons };
