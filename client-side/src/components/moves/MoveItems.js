import React, { useEffect, useState } from 'react'
import { typeColors } from '../../constants/TypeColors'
import { useFetchDetailMove } from '../../hooks/useFetchDetailMove'
import { Avatar } from 'antd';

export default function MoveItems({ move }) {

    const getMove = useFetchDetailMove();

    const [moveDetail, setMoveDetail] = useState();

    const getCategoryImage = (category) => {
        const moveImage = {
            physical: 'https://play.pokemonshowdown.com/sprites/categories/Physical.png',
            special: 'https://play.pokemonshowdown.com/sprites/categories/Special.png',
            status: 'https://play.pokemonshowdown.com/sprites/categories/Status.png',
        }

        return moveImage[category]
    }

    useEffect(() => {
        const fetch = async () => {
            const data = await getMove(move?.Id);

            const moveRes = {};

            var categoryMove = data?.damage_class?.name;

            moveRes.name = move?.Name;
            moveRes.category = categoryMove;
            moveRes.type = move?.Type?.Name;
            moveRes.typeImage = getCategoryImage(categoryMove);
            moveRes.power = move?.Power;
            moveRes.accuracy = move?.Accuracy;
            moveRes.pp = move?.PP;
            moveRes.effect = data?.effect_entries[0]?.short_effect;

            setMoveDetail(moveRes);
        }
        fetch()
    }, [move])

    return (
        <div
            key={moveDetail?.Id}
            className="flex items-center gap-4 border-b py-2"
        >
            <span 
                className="font-semibold"
                style={{ minWidth: "90px", textAlign: "left" }}>
                    {moveDetail?.name}
            </span>

            <span
                style={{
                    backgroundColor: typeColors[moveDetail?.type] || typeColors.Default,
                    color: 'white',
                    padding: '4px 10px',
                    borderRadius: '8px',
                    fontSize: '12px',
                    minWidth: '56px',
                    margin: 0
                }}
            >
                {moveDetail?.type}
            </span>
            <Avatar 
                style={{ width: "25px", height: "25px", minWidth: "25px", margin: 0 }}
                src={moveDetail?.typeImage || 'https://via.placeholder.com/50'} />

            <div style={{ 
                fontSize: "10px",
                display: "flex",
                gap: "8px",
                // justifyContent: "space-between",
                marginLeft: "0"
             }} className="flex text-xxs text-center ml-auto">
                <div
                    className="p-1 rounded-md shadow-sm flex flex-col item-center"
                    style={{ width: "15px" }}
                >
                    <p className="font-semibold text-gray-500">pp</p>
                    <p className="font-bold text-gray-800">{moveDetail?.pp}</p>
                </div>
                <div
                    className="p-1 rounded-md shadow-sm flex flex-col item-center"
                    style={{ width: "25px" }}
                >
                    <p className="font-semibold text-gray-500">power</p>
                    <p className="font-bold text-gray-800">{moveDetail?.power}</p>
                </div>
                <div
                    className="p-1 rounded-md shadow-sm flex flex-col item-center"
                    style={{ width: "30px" }}
                >
                    <p className="font-semibold text-gray-500">accuracy</p>
                    <p className="font-bold text-gray-800">{moveDetail?.accuracy}</p>
                </div>
                <div
                    className="p-1 rounded-md ml-1 shadow-sm flex flex-col item-center"
                    style={{ width: "30px", marginTop: 10 }}
                >
                    <p className="text-gray-800">{moveDetail?.effect}</p>
                </div>
            </div>
        </div>
    )
}
