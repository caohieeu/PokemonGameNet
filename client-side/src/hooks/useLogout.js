import { axiosInstance } from "../api/axiosClient";
import { notification } from "antd";
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';

const useLogout = () => {
    const navigate = useNavigate();
    const logout = async () => {
        try {
            const response = await axiosInstance.post("User/Logout", null);

            notification.open({
                message: "Logout success",
                type: "success",
                showProgress: true,
                pauseOnHover: false,
            });
            navigate("/");
            window.location.reload();

            return response.data;
        } catch (err) {
            notification.open({
                message: err.response?.data?.message || 'Logout failed',
                type: "error",
                showProgress: true,
                pauseOnHover: false,
            });
            console.log(err);
        }
    };

    return logout;
};

export { useLogout };
