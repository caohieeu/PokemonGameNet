import React, { useEffect, useState } from "react";
import { Card, CardBody, CardFooter, Image } from "@nextui-org/react";
import { Input, Pagination, Drawer, Row, Col, Divider } from 'antd';
import { useGetPokemons } from "../../hooks/useGetPokemons";
import { useGetDetailPokemon } from "../../hooks/useGetDetailPokemon";
import EvolutionChain from "../../components/EvolutionChain";
import "./Pokedex.css"
import BaseStat from "../../components/BaseStat";
import Moves from "../../components/Moves";
import PokemonMoves from "../../components/PokemonMoves";
import { typeColors } from "../../constants/TypeColors";

const { Search } = Input;

export default function Pokedex() {
  const [pokemons, setPokemons] = useState(null);
  const [pokemonId, setPokemonId] = useState(null);
  const [pokemonDetail, setPokemonDetail] = useState({});
  const [totalPage, setTotalPage] = useState(0);
  const [open, setOpen] = useState(false);
  const [objectQuery, setObjectQuery] = useState({
    page: 1,
    pageSize: 20,
    pokemonName: ""
  })
  const getPokemons = useGetPokemons();
  const getPokemon = useGetDetailPokemon(pokemonId);

  useEffect(() => {
    const fetchPokemons = async () => {
      const pokemonData = await getPokemons(objectQuery);
      setTotalPage(pokemonData.data.total_pages);
      setPokemons(pokemonData.data.data);
    }
    fetchPokemons();
  }, [objectQuery])

  const DescriptionItem = ({ title, content }) => (
    <div className="site-description-item-profile-wrapper">
      <p className="site-description-item-profile-p-label">{title}:</p>
      {content}
    </div>
  );

  const onSearch = (value, _e) => {
    setObjectQuery({ ...objectQuery, pokemonName: value, page: 1 })
  }

  const onChangePage = (page) => {
    setObjectQuery({ ...objectQuery, page: page })
  }

  const showDrawer = async (pokemonId) => {
    const pokemonDetailData = await getPokemon(pokemonId);
    setPokemonDetail(pokemonDetailData.data)
    console.log(pokemonDetailData);
    setOpen(true);
  };
  const onClose = () => {
    setOpen(false);
  };

  return (
    <div className="mx-32">
      <h1 className="text-center text-4xl font-bold my-6 text-[#555555]">
        Pokedex
      </h1>
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
      <div className="gap-2 grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 sm:grid-cols-3 px-0">
        <>
          <Drawer style={{padding: 20}} width={670} placement="right" closable={false} onClose={onClose} open={open}>
            <p
              className="site-description-item-profile-p"
              style={{
                marginBottom: 24,
                fontWeight: "bold",
                fontSize: 19
              }}
            >
              {pokemonDetail?.Name}
            </p>
            <div className="flex justify-center gap-4">
              <img
                className="w-48 h-48 object-contain"
                src={pokemonDetail?.Sprites?.Image}
                alt="Pokemon Front"
              />
              <img
                className="w-48 h-48 object-contain"
                src={pokemonDetail?.Sprites?.Back}
                alt="Pokemon Back"
              />
              <img
                className="w-48 h-48 object-contain"
                src={pokemonDetail?.Sprites?.Front}
                alt="Pokemon Front Alternate"
              />
            </div>
            <p className="site-description-item-profile-p">{"#" + pokemonDetail?.Id}</p>
            <Row>
              <Col span={12}>
                <DescriptionItem title="Name" content={pokemonDetail?.Name} />
              </Col>
              <Col span={12}>
                <DescriptionItem title="Type" content={pokemonDetail?.Type?.map((type, idx) => {
                  return (
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
                  );
                })} />
              </Col>
            </Row>
            <Row>
              <Col span={12}>
                <DescriptionItem title="Abilities" content={pokemonDetail?.Abilities?.map((ability) =>
                  ability.Ability.Name).join(" | ")} />
              </Col>
              <Col span={12}>
                <DescriptionItem title="Country" content="China🇨🇳" />
              </Col>
            </Row>
            <Row>
              <Col span={12}>
                <DescriptionItem title="Weight" content={`${pokemonDetail?.Weight} g`} />
              </Col>
              <Col span={12}>
                <DescriptionItem title="Height" content={`${pokemonDetail?.Height / 10} m`}/>
              </Col>
            </Row>
            
            <Divider />
            <p className="site-description-item-profile-p">Evolution</p>
            <Row>
              <Col span={24}>
                {/* <DescriptionItem
                  title="Message"
                  content="Make things as simple as possible but no simpler."
                /> */}
                <EvolutionChain pokemonId={pokemonDetail?.Id} />
              </Col>
            </Row>

            <Divider />
            <p className="site-description-item-profile-p">Stat Base</p>
            <Row>
              <Col span={24}>
                <BaseStat stat={pokemonDetail?.Stat} />
              </Col>
            </Row>
            
            <Divider />
            <p className="site-description-item-profile-p">Moves</p>
            <Row>
              <Col span={24}>
                {/* <Moves /> */}
                <PokemonMoves pokemonId={pokemonDetail?.Id} />
              </Col>
            </Row>
            <Divider />
          </Drawer>
        </>
        {pokemons?.map((item, index) => (
          <Card shadow="sm" key={index} isPressable onPress={() => showDrawer(item.Id)}>
            <CardBody className="overflow-visible p-0">
              <Image
                shadow="sm"
                radius="lg"
                width="100%"
                alt={item.Name}
                className="w-full object-cover h-auto"
                src={item.Sprites.Image}
              />
              <b className="p-2">No: {item.Id}</b>
            </CardBody>
            <CardFooter className="text-small justify-between items-center">
              <b>{item.Name}</b>
              <div style={{ display: 'flex', gap: '8px', marginLeft: 'auto' }}>
                {item.Type.map((type, idx) => {
                  return (
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
                  );
                })}
              </div>
            </CardFooter>
          </Card>
        ))}
      </div>
      <Pagination
        onChange={onChangePage}
        className="flex justify-center w-full my-8"
        defaultCurrent={1}
        total={totalPage * 10}
      />
    </div>
  );
}
