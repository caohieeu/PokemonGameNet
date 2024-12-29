import React, { useEffect, useState } from 'react';
import { Input, Button, List, Avatar } from 'antd';
import ChatConnector from '../context/ChatConnector';
import useUserInfo from '../hooks/useUserInfo';

export default function ChatRoomContent() {
    const { user } = useUserInfo();
    const [messages, setMessages] = useState([]);
    const [inputMessage, setInputMessage] = useState('');
    const users = [
        { id: 1, name: 'John Doe', avatar: 'https://i.pravatar.cc/50?img=1' },
        { id: 2, name: 'Jane Smith', avatar: 'https://i.pravatar.cc/50?img=2' },
    ];

    const handleSendMessage = () => {
        if (inputMessage.trim()) {
            const connector = ChatConnector;
            connector.SendMessageToAllUser(user?.data?.UserName, inputMessage);
            // setMessages((prev) => [
            //     ...prev,
            //     { id: prev.length + 1, text: inputMessage, username: user?.data?.UserName }
            // ]);
            setInputMessage('');
        }
    };

    useEffect(() => {
        const handleMessageReceived = (username, message) => {
            console.log(`${username} : ${message}`);
            setMessages((prevMessages) => [
                ...prevMessages,
                { username, message }
            ]);
        };
    
        const connector = ChatConnector;
        connector.connection.on("ReceiveMessage", handleMessageReceived);
    
        return () => {
            const connector = ChatConnector;
            connector.connection.off("ReceiveMessage", handleMessageReceived);
        };
    }, []);
    

    return (
        <div className="flex flex-col h-screen">
            <div className="flex flex-col md:flex-row h-full">
                <div className="md:w-1/3 w-full p-4 border-b md:border-r md:border-b-0 border-gray-200">
                    <h3 className="text-lg font-semibold">Users in Room</h3>
                    <List
                        className='overflow-y-auto'
                        style={{ maxHeight: '60vh' }}
                        itemLayout="horizontal"
                        dataSource={users}
                        renderItem={(user) => (
                            <List.Item>
                                <List.Item.Meta
                                    avatar={<Avatar src={user.avatar} />}
                                    title={user.name}
                                />
                            </List.Item>
                        )}
                    />
                </div>

                <div className="flex flex-col w-full p-4">
                    <div
                        className="flex-1 overflow-y-auto mb-4 p-4 border rounded-lg border-gray-200"
                        style={{ maxHeight: '60vh' }}
                    >
                        {messages.length > 0 ? (
                            messages.map((message) => (
                                <div key={message.id} className="mb-2">
                                    <strong>{message.username}:</strong> {message.message}
                                </div>
                            ))
                        ) : (
                            <div className="text-gray-400">No messages yet.</div>
                        )}
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
                        <p>Please signin to chat this room <a href="/login">Login</a></p>
                    )}
                </div>
            </div>
        </div>
    );
}
