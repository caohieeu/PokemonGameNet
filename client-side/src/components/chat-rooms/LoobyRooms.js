import React, { useEffect, useRef, useState } from 'react';
import { Input, Button, List, Avatar } from 'antd';
import ChatConnector from '../../context/ChatHubConnector';
import useUserInfo from '../../hooks/useUserInfo';
import { SERVER_URI } from '../../utils/Uri';
import useGetParticipants from '../../hooks/useGetParticipants';

export default function LoobyRooms({ roomId }) {
    const messagesEndRef = useRef(null);
    const { user } = useUserInfo();
    const { participants, reload } = useGetParticipants(roomId);
    const [messages, setMessages] = useState([]);
    const [inputMessage, setInputMessage] = useState('');
    const [users, setUsers] = useState([]);
    const [userColors, setUserColors] = useState({});

    const getRandomColor = () => {
        const letters = "0123456789ABCDEF";
        let color = "#";
        for (let i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    };

    const getColorForUser = (username) => {
        if (!userColors[username]) {
            setUserColors((prevColors) => ({
                ...prevColors,
                [username]: getRandomColor(),
            }));
        }
        return userColors[username];
    };

    const handleReload = () => {
        reload();
    }

    const handleSendMessage = () => {
        if (inputMessage.trim()) {
            const connector = ChatConnector;
            connector.SendMessageToAllUser(user?.data?.UserName, inputMessage);
            setInputMessage('');
        }
    };

    useEffect(() => {
        const handleMessageReceived = (username, message) => {
            console.log("Lobby room: " + username + " " + message)
            setMessages((prevMessages) => [
                ...prevMessages,
                { id: prevMessages.length + 1, username, message },
            ]);
        };

        const handleConnected = (userResponse) => {
            if(userResponse) {
                setUsers((prev) => [
                    ...prev,
                    {
                        id: users.length + 1,
                        name: userResponse.userName,
                        avatar: userResponse.avatar,
                    },
                ]);   
            }
            handleReload();
        };

        const handleDisConnected = (username) => {
            setUsers((prev) => prev.filter(user => user.username !== username));
            handleReload();
        };

        const connector = ChatConnector;
        connector.connection.off("ReceiveMessageGroup", handleMessageReceived);
        connector.connection.off("ReceiveMessage", handleMessageReceived);

        connector.connection.on("UserConnected", handleConnected);
        connector.connection.on("ReceiveMessage", handleMessageReceived);
        connector.connection.on("UserDisconnected", handleDisConnected);

        return () => {
            const connector = ChatConnector;
            connector.connection.off("ReceiveMessage", handleMessageReceived);
            connector.connection.off("UserConnected", handleConnected);
            connector.connection.off("UserDisconnected", handleDisConnected);
        };
    }, [roomId]);

    useEffect(() => {
        if (messagesEndRef.current) {
            messagesEndRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    }, [messages]);

    return (
        <div className="flex flex-col" style={{height: '67vh'}}>
            <div className="flex flex-col md:flex-row h-full">
                <div className="md:w-1/3 w-full p-4 border-b md:border-r md:border-b-0 border-gray-200">
                    <h3 className="text-lg font-semibold">Users in Room</h3>
                    <List
                        className="overflow-y-auto"
                        style={{ maxHeight: '60vh' }}
                        itemLayout="horizontal"
                        dataSource={participants?.data}
                        renderItem={(participant) => (
                            <List.Item>
                                <List.Item.Meta
                                    key={participant?.UserId}
                                    avatar={<Avatar src={`${SERVER_URI}${participant?.Avatar}`} />}
                                    title={participant?.UserName}
                                />
                            </List.Item>
                        )}
                    />
                </div>

                <div className="flex flex-col w-full p-4">
                    <div className="flex-1 overflow-y-auto mb-4 p-4 border rounded-lg border-gray-200" style={{ maxHeight: "60vh" }}>
                        {messages.length > 0 ? (
                            messages.map((message) => (
                                <div key={message.id} className="mb-2">
                                    <strong
                                        style={{
                                            color: getColorForUser(message.username),
                                        }}
                                    >
                                        {message.username}:
                                    </strong>{" "}
                                    {message.message}
                                </div>
                            ))
                        ) : (
                            <div className="text-gray-400">No messages yet.</div>
                        )}
                        <div ref={messagesEndRef} />
                    </div>

                    {user ? (
                        <div className="flex items-center">
                            <Input
                                placeholder="Type a message..."
                                value={inputMessage}
                                onChange={(e) => setInputMessage(e.target.value)}
                                onPressEnter={handleSendMessage}
                                className="mr-2"
                            />
                            <Button
                                type="primary"
                                onClick={handleSendMessage}
                            >
                                Send
                            </Button>
                        </div>
                    ) : (
                        <p>
                            Please signin to chat this room{" "}
                            <a href="/login">Login</a>
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
}

