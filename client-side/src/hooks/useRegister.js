import { axiosInstance } from "../api/axiosClient";
import { notification } from "antd";
import { useNavigate } from 'react-router-dom';

const useRegister = () => {
    const navigate = useNavigate();
    const register = async (userData) => {
        console.log(userData);
        try {
            const response = await axiosInstance.post("auth/register", userData);
            notification.open({
                message: "Register success",
                type: "success",
                showProgress: true,
                pauseOnHover: false,
            });
            navigate("/login");
            return response.data;
        } catch (err) {
            notification.open({
                message: err.response?.data?.message || 'Register failed',
                type: "error",
                showProgress: true,
                pauseOnHover: false,
            });
            console.log(err);
        }
    };

    return register;
};

export { useRegister };
