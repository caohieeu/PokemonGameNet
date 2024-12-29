import { useEffect, useState } from "react";
import { axiosInstance } from "../api/axiosClient";

const useGetParticipants = (roomId = "") => {
    const [participants, setParticipants] = useState([]);
    const [loading, setLoading] = useState(false);
    const [reloadTrigger, setReloadTrigger] = useState(0);

    const reload = () => setReloadTrigger((prev) => prev + 1);

    useEffect(() => {
        const fetchParticipants = async () => {
            try {
                setLoading(true);

                const response = await axiosInstance.get(`RoomChat/GetParticipants/${roomId}`);
                setParticipants(response.data);

                setLoading(false);
            } catch (err) {
                console.log(err.response?.data?.message || "Get participants failed");
                setLoading(false);
            }
        };

        if (roomId) {
            fetchParticipants();
        }
    }, [reloadTrigger, roomId]);

    return { participants, loading, reload };
};

export default useGetParticipants;
