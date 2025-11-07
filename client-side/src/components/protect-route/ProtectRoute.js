import { Navigate } from "react-router-dom";
import useUserInfo from "../../hooks/useUserInfo";

export default function ProtectRoute({ children, isAdmin }) {
    const { loading, user } = useUserInfo();

    if(loading) {
        return <div>Loading...</div>;
    }

    if(!user) {
        return <Navigate to="/login" />;
    }

    if(isAdmin && user?.data.Roles.includes("Admin") === false) {
        return <Navigate to="/unauthorized" />;
    }

    return children;
}