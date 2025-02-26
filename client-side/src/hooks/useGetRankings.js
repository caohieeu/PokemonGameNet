import { useEffect, useState } from "react";
import { axiosInstance } from "../api/axiosClient";

const useGetRankings = () => {
    const [rankings, setRankings] = useState(null);
    const [loading, setLoading] = useState(false);
    const [reloadTrigger, setReloadTrigger] = useState(0);

    const reload = () => setReloadTrigger((prev) => prev + 1);

    useEffect(() => {
        setLoading(true);

        axiosInstance
            .get(`Ranking`)
            .then((response) => {
                setRankings(response.data.data);
                setLoading(false);
            })
            .catch((err) => {
                console.log(err.response?.data?.message || "Get rankings failed");
                setLoading(false);
            });
    }, [reloadTrigger]);

    return { rankings, loading, reload };
};

export default useGetRankings;
