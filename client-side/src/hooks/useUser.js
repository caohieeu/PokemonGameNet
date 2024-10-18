import { axiosInstance } from "../api/axiosClient";
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';

const useUser = () => {
    const navigate = useNavigate();
    const user = async () => {
        try {
            const token = Cookies.get('auth_token');
            const response = await axiosInstance.get("user", {
                headers: { Authorization: `Bearer ${token}` }
            });
            console.log(response.data);
            return response.data;
        } catch (err) {
            console.log(err);
        }
    };

    return user;
};

export { useUser };
