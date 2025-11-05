import axios from "axios";
import { POKEAPI_URI } from "../constants/Uri";
const useFetchDetailMove = () => {
    const moveDetail = async (moveId) => {
        try {
            const response = await axios.get(`${POKEAPI_URI}move/${moveId}`)
            return response.data;
        } catch (err) {
            console.log(err.response.data.message || 'Get move failed')
        }
    };

    return moveDetail;
};

export { useFetchDetailMove };
