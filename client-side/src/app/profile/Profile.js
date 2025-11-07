import {
  Card,
  Avatar,
  Descriptions,
  List,
  Tag,
  Typography,
  Divider,
  Row,
  Col,
  Spin,
  Skeleton,
  Space
} from "antd";

import { FunctionOutlined, TrophyOutlined, UserOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import useUserInfo from "../../hooks/useUserInfo";
import UploadAvaatar from "../../components/upload-avatar/UploadAvatar";

const { Title, Text } = Typography;

export default function Profile() {
  const { loading, user } = useUserInfo();

  if (loading) {
    return (
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "60vh",
        }}
      >
        <Spin size="large" tip="Đang tải thông tin người chơi..." color="#eab308" />
      </div>
    )
  }

  const player = {
    username: "AshKetchum",
    avatar: "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/trainers/ash.png",
    level: 42,
    rank: "Elite Trainer",
    mainPokemon: "Pikachu",
    coins: 25000,
    battlesWon: 320,
    battlesLost: 200,
    joinedDate: "2023-08-10",
  };

  const battleHistory = [
    { id: 1, opponent: "Misty", result: "Win", date: "2025-10-25", pokemonUsed: "Pikachu" },
    { id: 2, opponent: "Brock", result: "Lose", date: "2025-10-23", pokemonUsed: "Bulbasaur" },
    { id: 3, opponent: "Gary", result: "Win", date: "2025-10-20", pokemonUsed: "Charizard" },
  ];

  return (
    <div style={{ maxWidth: 900, margin: "40px auto", padding: 20 }}>
      <Card bordered>
        <Row gutter={[16, 16]} align="middle">
          <Col xs={24} md={8} style={{ textAlign: "center" }}>
            <Avatar
              size={120}
              src={player.avatar}
              icon={<UserOutlined />}
              style={{ border: "2px solid #91d5ff" }}
            />
            <UploadAvaatar />
            <Title level={3} style={{ marginTop: 10 }}>
              {user?.data?.UserName}
            </Title>
            <Space direction="vertical" size={4} align="center">
              <Text type="secondary">{user?.data?.Email}</Text>
              <Tag color="gold">{player.rank}</Tag>
            </Space>
          </Col>

          <Col xs={24} md={16}>
            <Descriptions title="Basic information" bordered size="small" column={1}>
              <Descriptions.Item label="Level">{player.level}</Descriptions.Item>
              <Descriptions.Item label="Main pokémon">{player.mainPokemon}</Descriptions.Item>
              <Descriptions.Item label="Quantity of coin">{player.coins.toLocaleString()}</Descriptions.Item>
              <Descriptions.Item label="Win / Lose">
                {player.battlesWon} / {player.battlesLost}
              </Descriptions.Item>
              <Descriptions.Item label="Join date">
                {dayjs(player.joinedDate).format("DD/MM/YYYY")}
              </Descriptions.Item>
            </Descriptions>
          </Col>
        </Row>
      </Card>

      <Divider />

      <Card title={<><TrophyOutlined /> Match History</>}>
        <List
          dataSource={battleHistory}
          renderItem={(battle) => (
            <List.Item>
              <List.Item.Meta
                title={
                  <>
                    fight with <b>{battle.opponent}</b> ({battle.pokemonUsed})
                  </>
                }
                description={dayjs(battle.date).format("DD/MM/YYYY")}
              />
              <Tag color={battle.result === "Win" ? "green" : "red"}>
                {battle.result}
              </Tag>
            </List.Item>
          )}
        />
      </Card>
    </div>
  );
};
