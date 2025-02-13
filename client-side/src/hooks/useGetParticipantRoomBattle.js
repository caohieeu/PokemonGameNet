import { useEffect, useState } from "react";
import { axiosInstance } from "../api/axiosClient";

const useGetParticipantRoomBattle = (roomId = "", userName = "") => {
    const [participant, setParticipant] = useState([]);
    const [loading, setLoading] = useState(false);
    const [reloadTrigger, setReloadTrigger] = useState(0);

    const reload = () => setReloadTrigger((prev) => prev + 1);

    useEffect(() => {
        const fetchParticipant = async () => {
            try {
                setLoading(true);
                
                const response = await axiosInstance.get(`RoomBattle/GetParicipant?RoomId=${roomId}&UserName=${userName}`);
                setParticipant(response.data);

                setLoading(false);
            } catch (err) {
                console.log(err.response?.data?.message || "Get participant failed");
                setLoading(false);
            }
        };

        if (roomId) {
            fetchParticipant();
        }
    }, [reloadTrigger, roomId]);

    return { participant, loading, reload };
};

export default useGetParticipantRoomBattle;
