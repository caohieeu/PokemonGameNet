import { axiosInstance } from "../api/axiosClient";
import { notification } from "antd";
import Cookies from 'js-cookie';
import { useEffect, useState } from "react";

export default function useUserInfo() {
    const [loading, setLoading] = useState(true);
    const [user, setUser] = useState();

    useEffect(() => {
        const sub = async () => {
            try {
                const token = Cookies.get('auth_token');
                const response = await axiosInstance.get("User/GetInfoUser", {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setLoading(false);
                setUser(response.data);
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
        sub();
    }, [])

    return { loading, user }
};
