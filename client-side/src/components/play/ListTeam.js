import React from 'react';
import { Divider, List, Typography } from 'antd';
import useUserInfo from '../../hooks/useUserInfo';
const data = [
    'Racing car sprays burning fuel into crowd.',
    'Japanese princess to wed commoner.',
    'Australian walks 100km after outback crash.',
    'Man charged over missing wedding girl.',
    'Los Angeles battles huge wildfires.',
];
const ListTeam = () => {
    const { user } = useUserInfo();

    if(!user)
        return <p>You are not logged yet</p>

    return (
        <>
            <Divider orientation="left">List Team Pokemon</Divider>
            <List
                bordered
                dataSource={user?.data?.Teams}
                renderItem={(item) => (
                    <List.Item
                        className='cursor-pointer hover:bg-[rgb(234,179,8)]'
                        onClick={() => console.log("clicked")}
                    >
                        <Typography.Text mark></Typography.Text> {item?.Name}
                        <div className='flex items-center gap-3'>
                            {item?.Pokemons?.map((pokemon) => (
                                <div className='flex items-center'>
                                <img 
                                    src={`${pokemon?.Sprites?.Image}`}
                                    className='w-12 h-12' />
                                <p>{pokemon?.Name}</p>
                            </div>
                            ))}
                        </div>
                    </List.Item>
                )}
            />
        </>
    )
}

export default ListTeam;