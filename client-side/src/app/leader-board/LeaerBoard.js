import React, { useEffect, useState } from "react";
import { Table, Input, Card, Typography, Row, Col, Divider } from "antd";
import { SearchOutlined, GoldOutlined, StarOutlined, TrophyOutlined } from '@ant-design/icons';
import Header from "../Header";
import useGetRankings from "../../hooks/useGetRankings";

const { Title } = Typography;
const { Search } = Input;

const Leaderboard = () => {
  const { rankings, reload } = useGetRankings();
  
  const [searchTerm, setSearchTerm] = useState("");
  const [players, setPlayers] = useState(null);

  const handleSearch = (value) => {
    setSearchTerm(value);
  };

  const filteredPlayers = players?.filter((player) =>
    player?.UserName?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getRankIcon = (rank) => {
    switch (rank) {
      case 0:
        return <GoldOutlined style={{ color: 'gold', fontSize: '20px' }} />;
      case 1:
        return <StarOutlined style={{ color: 'silver', fontSize: '20px' }} />;
      case 2:
        return <TrophyOutlined style={{ color: '#cd7f32', fontSize: '20px' }} />;
      default:
        return null;
    }
  };

  useEffect(() => {
    console.log(rankings)
    setPlayers(rankings);
  }, [rankings])

  return (
    <div className="mx-1 md:mx-32">
      <Header />
      <div style={{ padding: '30px', backgroundColor: '#f7f7f7' }}>
        <Row justify="center">
          <Col span={12}>
            <Card
              title={<Title level={2} style={{ textAlign: 'center' }}>Leaderboard</Title>}
              bordered={false}
              style={{
                borderRadius: '10px',
                boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
                padding: '20px'
              }}
            >
              <Search
                placeholder="Search for a player"
                enterButton={<SearchOutlined />}
                size="large"
                onSearch={handleSearch}
                style={{ marginBottom: '20px', borderRadius: '5px' }}
              />
              <Divider />
              <Table
                dataSource={filteredPlayers}
                columns={[
                  {
                    title: "Rank",
                    render: (text, record, index) => (
                      <div style={{ display: 'flex', alignItems: 'center' }}>
                        {getRankIcon(index)}
                        <span style={{ marginLeft: '8px' }}>{index + 1}</span>
                      </div>
                    ),
                    width: '15%',
                  },
                  {
                    title: "Player Name",
                    dataIndex: "UserName",
                    key: "UserName",
                    sorter: (a, b) => a.UserName.localeCompare(b.UserName),
                  },
                  {
                    title: "Point",
                    dataIndex: "Point",
                    key: "Point",
                    sorter: (a, b) => a.Point - b.Point,
                    render: (text) => <strong>{text}</strong>,
                  },
                ]}
                rowKey="name"
                pagination={false}
                style={{
                  boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
                }}
              />
            </Card>
          </Col>
        </Row>
      </div>
    </div>
  );
};

export default Leaderboard;
