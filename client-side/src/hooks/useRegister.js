import { axiosInstance } from "../api/axiosClient";
import { notification } from "antd";
import { useNavigate } from 'react-router-dom';

const useRegister = () => {
    const navigate = useNavigate();
    const register = async (userData) => {
        console.log(userData);
        try {
            userData.Role = "Player"
            const response = await axiosInstance.post("User/SignUp", userData, {
                headers: {
                    "Content-Type": "multipart/form-data",
                }
            });
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
