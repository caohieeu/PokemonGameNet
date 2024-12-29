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

function App() {
  return (
    //<SocketContext.Provider value={socket}>
      <NextUIProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Index />} />
            <Route index path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
            <Route path="pokedex" element={<Pokedex />} />
            <Route path="home" element={<Home />} />
            <Route path="play" element={<Play />} />
            <Route path="chat-rooms" element={<ChatRooms />} />
          </Routes>
        </BrowserRouter>
      </NextUIProvider>
    //</SocketContext.Provider>
  );
}

export default App;
