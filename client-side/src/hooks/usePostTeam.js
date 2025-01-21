import { axiosInstance } from "../api/axiosClient";
import { notification } from "antd";
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import useUserInfo from "./useUserInfo";

const usePostTeam = () => {
    const { user } = useUserInfo();

    const postTeam = async (team) => {
        try {
            if(!user) {
                notification.open({
                    message: "You are not logged in yet",
                    type: "error",
                    showProgress: true,
                    pauseOnHover: false,
                });
            }

            const response = await axiosInstance.put(`User/AddNewTeam/${user?.data?.Id}`, team);

            notification.open({
                message: "Add new team success",
                type: "success",
                showProgress: true,
                pauseOnHover: false,
            });

            return response.data;
        } catch (err) {
            notification.open({
                message: err.response?.data?.message || 'Add new team failed',
                type: "error",
                showProgress: true,
                pauseOnHover: false,
            });
            console.log(err);
        }
    };

    return postTeam;
};

export { usePostTeam };
