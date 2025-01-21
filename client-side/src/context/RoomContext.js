import { createContext, useContext } from 'react';

const RoomContext = createContext();

export const useRoom = () => useContext(RoomContext);

export const RoomProvider = ({ children, roomId }) => {
    return (
        <RoomContext.Provider value={roomId}>
            {children}
        </RoomContext.Provider>
    );
};
