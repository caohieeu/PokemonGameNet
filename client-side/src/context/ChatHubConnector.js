import * as signalR from "@microsoft/signalr";
import { CHAT_HUB } from "../utils/Uri";

const URL = CHAT_HUB;

class Connector {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(URL, {
                accessTokenFactory: () => {
                    const token = localStorage.getItem("my_token");
                    return token;
                }
            })
            .withAutomaticReconnect()
            .build();

        this.connection
            .start()
            .then(() => {
                console.log('Connection started successfully.');
            })
            .catch((err) => console.error("Connection error:", err));
    }

    

    checkConnected = () => {
        if(this.connection.state === signalR.HubConnectionState.Connected) {
            return true;
        }else {
            console.error("Connection is not established yet.");
            return false;
        }
    }

    events(
        onMessageReceived = () => {},
        onMessageGroupReceived = () => {},
        onConnected = () => {},
        onDisConnected = () => {},
        onConnectedGroup = () => {}
    ) {
        if (!this.listenersAdded) {
            this.connection.on("ReceiveMessage", (username, message) => {
                console.log(`${username}: ${message}`);
                onMessageReceived(username, message);
            });

            this.connection.on("ReceiveMessageGroup", (username, message, roomReceiveId) => {
                onMessageGroupReceived(username, message, roomReceiveId);
            });
    
            this.connection.on("UserConnected", (userResponse) => {
                onConnected(userResponse);
            });

            this.connection.on("UserDisconnected", (username) => {
                onDisConnected(username);
            });

            this.connection.on("UserJoined", (userResponse) => {
                console.log("join group: " + userResponse?.userName)
                onConnectedGroup(userResponse);
            });
    
            this.connection.on()
            
            this.listenersAdded = true;
        }
    }
    
    SendMessageToAllUser = (username, message) => {
        const isConnected = this.checkConnected();

        isConnected && this.connection
                .invoke("SendMessageToAllUser", username, message)
                .then(() => console.log("Message sent to all users"))
                .catch((err) => console.error("Error sending message:", err));
    };

    JoinGroup = (roomId) => {
        const isConnected = this.checkConnected();

        isConnected && this.connection.invoke("JoinGroup", roomId)
    }

    ExitGroup = (roomId) => {
        const isConnected = this.checkConnected();

        isConnected && this.connection.invoke("ExitGroup", roomId)
    }

    SendMessageToGroup = (roomId, username, message) => {
        if(this.connection.state === signalR.HubConnectionState.Connected) {
            this.connection.invoke("SendMessageToGroup", roomId, username, message)
        } else {
            console.error("Connection is not established yet.");
        }
    }

    static getInstance() {
        if (!Connector.instance) {
            Connector.instance = new Connector();
        }
        return Connector.instance;
    }
}

export default Connector.getInstance();

