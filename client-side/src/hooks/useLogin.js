import { axiosInstance } from "../api/axiosClient";
import { notification } from "antd";
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';

const useLogin = () => {
    const navigate = useNavigate();
    const login = async (userData) => {
        try {
            const response = await axiosInstance.post("User/SignIn", userData);

            notification.open({
                message: "Login success",
                type: "success",
                showProgress: true,
                pauseOnHover: false,
            });
            navigate("/");
            return response.data;
        } catch (err) {
            notification.open({
                message: err.response?.data?.message || 'Login failed',
                type: "error",
                showProgress: true,
                pauseOnHover: false,
            });
            console.log(err);
        }
    };

    return login;
};

export { useLogin };
