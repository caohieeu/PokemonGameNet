import { useEffect, useState } from "react";
import { axiosInstance } from "../api/axiosClient";

const useGetRooms = (isReload = false) => {
    const [roomChats, setRoomChats] = useState([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchRooms = async () => {
            try {
                setLoading(true);

                const response = await axiosInstance.get("RoomChat");
                setRoomChats(response.data);

                setLoading(false);
            } catch (err) {
                console.log(err.response?.data?.message || "Get room chat failed");
                setLoading(false);
            }
        };

        if (isReload) {
            fetchRooms();
        }
    }, [isReload]);

    return { roomChats, loading };
};

export default useGetRooms;
