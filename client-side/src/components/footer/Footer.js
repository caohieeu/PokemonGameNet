import React, { useEffect, useState } from "react";

export default function Footer() {
    return (
        <footer className="bg-gray-800 text-center text-white py-4">
            <p className="mx-2">
                Pokémon names and sprites © 1995-{new Date().getFullYear()} Nintendo, The Pokémon Company and Gamefreak
            </p>
        </footer>
    )
}