import React, { useEffect, useState } from 'react';
import { Tabs, Card, notification } from 'antd';
import { Button, Modal, Space } from 'antd';
import { MessageOutlined } from '@ant-design/icons';
import Header from '../Header';
import ChatRoomContent from '../../components/ChatRoomContent';
import LoobyRooms from '../../components/chat-rooms/LoobyRooms';
import BattleRoom from '../../components/chat-rooms/BattleRoom';
import useGetRooms from '../../hooks/useGetRooms';
import ChatConnector from '../../context/ChatHubConnector';
import useUserInfo from '../../hooks/useUserInfo';
import ChatHubConnector from '../../context/ChatHubConnector';

export default function ChatRooms() {
  const [activeKey, setActiveKey] = useState('roomList');
  const [openedTabs, setOpenedTabs] = useState([
    {
      label: 'List room chat',
      key: 'roomList',
      children: (
        <RoomList
          onRoomSelect={(room) => addRoomTab(room)}
        />
      ),
      closable: false,
    },
  ]);

  const addRoomTab = (room) => {
    setOpenedTabs((prevTabs) => {
      const tabExists = prevTabs.some((tab) => tab.key === room?.Id);
      if (!tabExists) {
        return [
          ...prevTabs,
          { label: room?.Name, key: room?.Id, children: room?.Description },
        ];
      }
      return prevTabs;
    });
    setActiveKey(room?.Id); 
  };

  const onEdit = (targetKey, action) => {
    if (action === 'remove') {
      if(targetKey !== 'roomList') {
        const connector = ChatHubConnector;
        connector.ExitGroup(targetKey);
      }

      const updatedTabs = openedTabs.filter((tab) => tab.key !== targetKey);
      setOpenedTabs(updatedTabs);
      setActiveKey(updatedTabs.length > 0 ? updatedTabs[0].key : 'roomList');
    }
  };

  return (
    <div className='mx-1 md:mx-32'>
      <Header />
      <h1 className="text-center text-4xl font-bold my-4 text-[#555555]">
        Chat Rooms
      </h1>

      <div style={{ padding: '16px' }}>
        <Tabs
          type="editable-card"
          activeKey={activeKey}
          onChange={setActiveKey}
          onEdit={onEdit}
        >
          {openedTabs.map((tab) => (
            <Tabs.TabPane 
              tab={tab.label}
              key={tab.key} 
              closable={tab.closable ?? true}>
              {tab.key !== "roomList" ? (
                tab.key === '676f9a07849fb9191106065f' ? (
                  <LoobyRooms roomId={tab.key} />
                ) : <BattleRoom roomId={tab.key} />
              ) : tab.children }
            </Tabs.TabPane>
          ))}
        </Tabs>
      </div>
    </div>
  );
}

const RoomList = ({ onRoomSelect }) => {
  const { user } = useUserInfo();
  const { roomChats } = useGetRooms(true);

  const handleJoinRoom = (room) => {
    if(room?.Name !== "Lobby") {
      if(!user) {
        notification.open({
          message: "Login required to join this chat room",
          type: "error",
          showProgress: true,
          pauseOnHover: false,
      });
      return;
    }

      const connector = ChatConnector;
      connector.JoinGroup(room?.Id)
    }
    onRoomSelect(room)
  }

  const onModalJoinRoom = (room) => {
    Modal.confirm({
      title: 'Confirm',
      content: 'Join this room ?',
      onOk: () => {handleJoinRoom(room)},
      footer: (_, { OkBtn, CancelBtn }) => (
        <>
          <CancelBtn />
          <OkBtn />
        </>
      ),
    });
  }

  return (
    <div>
      <h3>List room chat: </h3>
      {roomChats?.data?.map((room) => (
        <Card
          key={room?.Id}
          bordered
          style={{ marginBottom: 16 }}
          onClick={() => {
            onModalJoinRoom(room)
          }}
        >
          <div style={{ display: 'flex', alignItems: 'center' }}>
            <MessageOutlined style={{ fontSize: 24, color: '#1890ff', marginRight: 12 }} />
            <div>
              <div style={{ fontWeight: 'bold', fontSize: 16 }}>{room?.Name}</div>
              <div style={{ color: '#888' }}>{room?.Description}</div>
            </div>
          </div>
        </Card>
      ))}
    </div>
  );
};
