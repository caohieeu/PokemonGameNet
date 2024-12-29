import React from 'react'
import { Progress, Row, Col } from 'antd';

export default function BaseStat({ stat }) {
    // CSS inline styles
    const statItemStyle = {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        marginBottom: '8px',
    };

    const textStyle = {
        fontSize: '16px',
        marginRight: '8px',
        flex: '0 1 20%',
    };

    const boldTextStyle = {
        fontSize: '18px',
        fontWeight: 'bold',
        flex: '0 1 20%', 
        textAlign: 'center',
    };

    const progressStyle = {
        flex: '1',
        marginLeft: '10px',
    };

    const maxStatValue = Math.max(
        stat?.Hp || 0,
        stat?.Atk || 0,
        stat?.Defense || 0,
        stat?.SpAtk || 0,
        stat?.SpDef || 0,
        stat?.Speed || 0
    );

    const getNormalizedPercent = (value) => (value / maxStatValue) * 100;

    return (
        <div style={{ padding: '16px' }}>
            <Row gutter={[16, 16]}>
                {[
                    { label: 'HP', value: stat?.Hp },
                    { label: 'Attack', value: stat?.Atk },
                    { label: 'Defense', value: stat?.Defense },
                    { label: 'Sp. Atk', value: stat?.SpAtk },
                    { label: 'Sp. Def', value: stat?.SpDef },
                    { label: 'Speed', value: stat?.Speed },
                ].map(({ label, value }) => (
                    <Col span={15} key={label}>
                        <div style={statItemStyle}>
                            <text style={textStyle}>{label}:</text>
                            <b style={boldTextStyle}>{value}</b>
                            <Progress
                                percent={getNormalizedPercent(value)}
                                status="active"
                                strokeColor={{
                                    from: '#108ee9',
                                    to: '#87d068',
                                }}
                                format={() => null}
                                style={progressStyle}
                            />
                        </div>
                    </Col>
                ))}
            </Row>
        </div>
    );
}
