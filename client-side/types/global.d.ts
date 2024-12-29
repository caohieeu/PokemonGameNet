interface UserRegister {
    username: string,
    displayName: string,
    password: string,
    email: string,
    phone: string
}

interface User {
    username: string;
    displayName: string;
    email: string;
    phoneNumber: string;
    imagePath: string;
    roles: string[];
    moves: string[] | null;
    pokemons: Pokemon[];
    dateCreated: string;
}

interface Pokemon {
    id: number;
    name: string;
    type: string[];
    sprites: Sprites;
    species: Species;
    stat: Stats;
    abilities: Ability[];
    height: number;
    weight: number;
    moves: Move[];
}

interface Sprites {
    image: string;
    back: string;
    front: string;
}

interface Species {
    name: string;
    url: string;
}

interface Stats {
    hp: number;
    atk: number;
    defense: number;
    spAtk: number;
    spDef: number;
    speed: number;
}

interface Ability {
    ability: {
        name: string;
        url: string;
    };
    isHidden: boolean;
    slot: number;
}

interface Move {
    id: number;
    name: string;
    type: {
        name: string;
        url: string | null;
    };
    power: number;
    priority: number;
    pp: number;
    accuracy: number;
}
