import { axiosInstance } from "../api/axiosClient";
import { notification } from "antd";
import Cookies from 'js-cookie';
import { useEffect, useState } from "react";

export default function useGetUsersByUserName(username) {
    const [loading, setLoading] = useState(true);
    const [users, setUsers] = useState(null);

    useEffect(() => {
        const sub = async () => {
            try {
                if(!username) {
                    setUsers(null);
                    return;
                }

                const token = Cookies.get('auth_token');
                const response = await axiosInstance.get(`User/${username}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setLoading(false);
                setUsers(response.data.data);
                return response.data.data;
            } catch (err) {
                // notification.open({
                //     message: err.response?.data?.message || 'Get user failed',
                //     type: "error",
                //     showProgress: true,
                //     pauseOnHover: false,
                // });
                console.log(err.response.data.message || 'Get user failed')
            }
            finally {
                setLoading(false)
            }
        };
        sub();
    }, [username])

    return { loading, users }
};
