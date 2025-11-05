import * as signalR from "@microsoft/signalr";
import { GAME_HUB } from "../constants/Uri";
import { notification } from "antd";

const URL = GAME_HUB;

const hubName = "GameHub";

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
                console.log(`[${hubName}]: Connection started successfully.`);
            })
            .catch((err) => console.error("Connection error:", err));
    }

    checkConnected = () => {
        if(this.connection.state === signalR.HubConnectionState.Connected) {
            return true;
        }else {
            console.error(`[${hubName}]: Connection is not established yet.`);
            return false;
        }
    }

    events(
        onMatchFound = () => {},
    ) {
        if (!this.listenersAdded) {
            this.connection.on("MatchFound", (response) => {
                console.log(response);
                onMatchFound();
            });
    
            this.connection.on()
            
            this.listenersAdded = true;
        }
    }
    
    FindMatch = async (setLoadingFindMatch) => {
        const isConnected = this.checkConnected();

        isConnected && await this.connection
                .invoke("FindMatch")
                .then(() => console.log("Find match success"))
                .catch((err) => {
                    console.error("Error find match:", err)
                    
                    const errorMessage = err?.message?.split("HubException:")[1]?.trim() || 
                        "An unexpected error occurred";

                    notification.open({
                        message: errorMessage,
                        type: "error",
                        showProgress: true,
                        pauseOnHover: false,
                    });

                    setLoadingFindMatch(false)
                });
    };

    Attack = async (roomId, moveId) => {
        const isConnected = this.checkConnected();

        isConnected && await this.connection
                .invoke("Attack", roomId, moveId)
                .then(() => console.log("Attack success"))
                .catch((err) => {
                    console.error("Error when attack:", err)
                });
    };

    ExecuteTurn = (executeTurn) => {
        const isConnected = this.checkConnected();

        isConnected && this.connection
                .invoke("ExecuteTurn", executeTurn)
                .then(() => console.log("Send execute success"))
                .catch((err) => {
                    console.error("Error when excute:", err)
                });
    };
    ExecuteTurn2 = (executeTurn) => {
        const isConnected = this.checkConnected();

        isConnected && this.connection
                .invoke("ExecuteTurn2", executeTurn)
                .then(() => console.log("Send execute success"))
                .catch((err) => {
                    console.error("Error when excute:", err)
                });
    };

    static getInstance() {
        if (!Connector.instance) {
            Connector.instance = new Connector();
        }
        return Connector.instance;
    }
}

export default Connector.getInstance();

