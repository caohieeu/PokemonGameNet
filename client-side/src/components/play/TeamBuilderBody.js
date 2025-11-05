import React, { useEffect, useState } from 'react'
import { AntDesignOutlined } from '@ant-design/icons';
import {
    Avatar,
    Button,
    ConfigProvider,
    Input,
    List,
    Space,
    Tabs
} from 'antd';
import { createStyles } from 'antd-style';
import { useGetPokemons } from '../../hooks/useGetPokemons';
import { typeColors } from '../../constants/TypeColors';
import DetailPokemon from './DetailPokemon';

const useStyle = createStyles(({ prefixCls, css }) => ({
    linearGradientButton: css`
      &.${prefixCls}-btn-primary:not([disabled]):not(.${prefixCls}-btn-dangerous) {
        > span {
          position: relative;
        }
  
        &::before {
          content: '';
          background: linear-gradient(135deg,rgb(194, 149, 12),rgb(244, 189, 24));
          position: absolute;
          inset: -1px;
          opacity: 1;
          transition: all 0.3s;
          border-radius: inherit;
        }
  
        &:hover::before {
          opacity: 0;
        }
      }
    `,
}));

const { Search } = Input;

export default function TeamBuilderBody({ onPokemonsChange }) {
    const { styles } = useStyle();
    const getPokemons = useGetPokemons();

    const [pokemons, setPokemons] = useState(null);
    const [currentPokemon, setCurrentPokemon] = useState(null);
    const [openPokemons, setOpenPokemons] = useState(false);
    const [pokemonsTeam, setPokemonsTeam] = useState([]);
    const [objectQuery, setObjectQuery] = useState({
        page: 1,
        pageSize: 10,
        pokemonName: ""
    })

    useEffect(() => {
        const fetchPokemons = async () => {
            const pokemonData = await getPokemons(objectQuery);
            setPokemons(pokemonData.data.data);
        }
        fetchPokemons();
    }, [objectQuery])

    const handleAddNewPokemon = (pokemon) => {
        setCurrentPokemon(pokemon)
        setPokemonsTeam(prevTeam => [...prevTeam, pokemon]);
        setOpenPokemons(false);
    }

    const onSearch = (value, _e) => {
        setObjectQuery({ ...objectQuery, pokemonName: value, page: 1 })
    }

    return (
        <ConfigProvider
            button={{
                className: styles.linearGradientButton,
            }}
        >
            {currentPokemon ? (
                <Tabs
                    // onChange={onChange}
                    type="card"
                    items={pokemonsTeam.fill(null).map((_, i) => {
                        const id = String(i + 1);
                        return {
                            label: `Pokemon ${id}`,
                            key: id,
                            children: (
                                <DetailPokemon 
                                    pokemonId={currentPokemon?.Id}
                                    onPokemonsChange={onPokemonsChange}
                                />
                            ),
                        };
                    })}
                />
            ) : <></>}

            <Space className='mt-4'>
                <Button
                    onClick={() => setOpenPokemons(!openPokemons)}
                    style={{
                        borderColor: "#1759a7",
                        borderWidth: "4px",
                        backgroundColor: "rgb(234, 179, 8)",
                        color: "white",
                        marginBottom: "10px"
                    }}
                    size="large"
                    icon={<AntDesignOutlined />}>
                    Add Pokemon
                </Button>
            </Space>

            {openPokemons ? (
                <div
                    style={{
                        height: "400px",
                        overflowY: "auto",
                        border: "1px solid #ccc",
                        borderRadius: "8px",
                        padding: "8px"
                    }}>
                    <div className="flex justify-end">
                        <Search
                            onSearch={onSearch}
                            className="mb-8"
                            placeholder="Search pokemon"
                            style={{
                                width: 200,
                            }}
                        />
                    </div>
                    <List
                        itemLayout="vertical"
                        dataSource={pokemons || []}
                        renderItem={(pokemon, index) => (
                            <List.Item
                                style={{ cursor: "pointer" }}
                                onClick={() => handleAddNewPokemon(pokemon)}
                            >
                                <div
                                    className="flex items-center gap-3 p-2 border-b border-gray-300"
                                    style={{ alignItems: "center", flexWrap: "nowrap" }}
                                >
                                    <Avatar
                                        src={pokemon?.Sprites?.Image}
                                        size={36}
                                        className="flex-shrink-0"
                                    />
                                    <h4 style={{ fontSize: "10px" }} className="font-bold flex-shrink-0">{pokemon?.Name}</h4>
                                    {pokemon?.Type?.map((type, idx) => (
                                        <span
                                            key={idx}
                                            style={{
                                                backgroundColor: typeColors[type] || typeColors.Default,
                                                color: 'white',
                                                padding: '4px 8px',
                                                borderRadius: '8px',
                                                fontSize: '12px',
                                            }}
                                        >
                                            {type}
                                        </span>
                                    ))}
                                    <p style={{ fontSize: "10px" }} className="text-gray-500 truncate flex-shrink-0">
                                        {pokemon?.Abilities?.map((ability) => ability.Ability.Name).join(" | ")}
                                    </p>

                                    <div style={{ fontSize: "10px" }} className="flex gap-2 text-xxs text-center ml-auto">
                                        {Object.entries(pokemon?.Stat).map(([key, value]) => (
                                            <div
                                                key={key}
                                                className="p-1 rounded-md shadow-sm"
                                                style={{ minWidth: "40px" }}
                                            >
                                                <p className="font-semibold text-gray-500">{key}</p>
                                                <p className="font-bold text-gray-800">{value}</p>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            </List.Item>
                        )}
                    />
                </div>
            ) : (<></>)}
        </ConfigProvider>
    )
}
