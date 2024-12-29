import { axiosInstance } from "../api/axiosClient";
import { useNavigate } from 'react-router-dom';
import { notification } from "antd";
import Cookies from 'js-cookie';

const useUser = () => {
    const user = async () => {
        try {
            const token = Cookies.get('auth_token');
            localStorage.setItem("my_token", token)
            const response = await axiosInstance.get("User/GetInfoUser", {
                headers: { Authorization: `Bearer ${token}` }
            });
            return response.data;
        } catch (err) {
            // notification.open({
            //     message: err.response?.data?.message || 'Get user failed',
            //     type: "error",
            //     showProgress: true,
            //     pauseOnHover: false,
            // });
            console.log(err.response.data.message || 'Get user failed')
        }
    };

    return user;
};

export { useUser };
