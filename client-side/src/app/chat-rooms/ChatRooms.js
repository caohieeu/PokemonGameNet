import React, { useEffect, useState } from 'react';
import { Tabs, Card } from 'antd';
import { MessageOutlined } from '@ant-design/icons';
import Header from '../Header';
import ChatRoomContent from '../../components/ChatRoomContent';
import LoobyRooms from '../../components/chat-rooms/LoobyRooms';
import BattleRoom from '../../components/chat-rooms/BattleRoom';
import useGetRooms from '../../hooks/useGetRooms';

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
            <Tabs.TabPane tab={tab.label} key={tab.key} closable={tab.closable ?? true}>
              {tab.key !== "roomList" ? (
                tab.key === '676f9a07849fb9191106065f' ? (
                  <LoobyRooms roomId={tab.key} />
                ) : (tab.key === '6770dba4b120d48bc4900c53' ? (
                  <BattleRoom />
                ) : (tab.key === '3' ? (
                  <LoobyRooms />
                ) : (<LoobyRooms />)))
              ) : tab.children }
            </Tabs.TabPane>
          ))}
        </Tabs>
      </div>
    </div>
  );
}

const RoomList = ({ onRoomSelect }) => {
  // const rooms = [
  //   { id: '1', name: 'Lobby', description: 'Room chat to all user.', content: 'Welcome to Room 1' },
  //   { id: '2', name: 'Room 2', description: 'Phòng chat về thể thao.', content: 'Welcome to Room 2' },
  //   { id: '3', name: 'Room 3', description: 'Phòng chat về âm nhạc.', content: 'Welcome to Room 3' },
  // ];

  const { roomChats } = useGetRooms(true);

  return (
    <div>
      <h3>List room chat: </h3>
      {roomChats?.data?.map((room) => (
        <Card
          key={room?.Id}
          bordered
          style={{ marginBottom: 16 }}
          onClick={() => onRoomSelect(room)}
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
