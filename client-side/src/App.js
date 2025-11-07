//import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { NextUIProvider } from "@nextui-org/react";
import Login from './app/auth/Login';
import Register from './app/auth/Register';
import Index from './app/index';
import Pokedex from './app/pokedex/Pokedex';
import Home from './app/home/Home';
import Play from './app/play/Play';
import ChatRooms from './app/chat-rooms/ChatRooms';
import Leaderboard from './app/leader-board/LeaderBoard';
import BattleRoom from './app/battle-room/BattleRoom';
import Unauthorized from './app/unauthorized/Unauthorized';
import Admin from './app/admin/index';
import ProtectRoute from './components/protect-route/ProtectRoute';
import Profile from './app/profile/Profile';
import { useUser } from './hooks/useUser';

function App() {
  const user = useUser();

  return (
    //<SocketContext.Provider value={socket}>
    <NextUIProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Index />} />
          <Route path="/unauthorized" element={<Unauthorized />} />
          <Route index path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
          <Route path="pokedex" element={<Pokedex />} />
          <Route path="home" element={<Home />} />
          <Route path="leader-board" element={<Leaderboard />} />
          <Route path="play" element={<Play />} />
          <Route path="chat-rooms" element={<ChatRooms />} />
          <Route path="battle-room/:roomId" element={<BattleRoom />} />
          <Route path="profile" element={<Profile />} />
          <Route path="/admin" element={
            <ProtectRoute isAdmin={true}>
              <Admin />
            </ProtectRoute>
          } />
        </Routes>
      </BrowserRouter>
    </NextUIProvider>
    //</SocketContext.Provider>
  );
}

export default App;
