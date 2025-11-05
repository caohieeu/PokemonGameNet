import MainLayout from "../components/layouts/MainLayout";
import Header from "./Header";
import Pokedex from "./pokedex/Pokedex";

export default function index() {
    return (
        <div className="px-4 sm:px-6 md:px-8 lg:px-36">
            <MainLayout>
                <Pokedex />
            </MainLayout>
        </div>
    )
}