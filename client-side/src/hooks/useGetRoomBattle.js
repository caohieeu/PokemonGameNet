import { useEffect, useState } from "react";
import { axiosInstance } from "../api/axiosClient";

const useGetRoomBattle = (roomId = "") => {
    const [roomBattle, setRoomBattle] = useState(null);
    const [loading, setLoading] = useState(false);
    const [reloadTrigger, setReloadTrigger] = useState(0);

    const reload = () => setReloadTrigger((prev) => prev + 1);

    useEffect(() => {
        setLoading(true);

        axiosInstance
            .get(`RoomBattle/${roomId}`)
            .then((response) => {
                setRoomBattle(response.data.data);
                setLoading(false);
            })
            .catch((err) => {
                console.log(err.response?.data?.message || "Get room battle failed");
                setLoading(false);
            });
    }, [roomId, reloadTrigger]);

    return { roomBattle, loading, reload };
};

export default useGetRoomBattle;
