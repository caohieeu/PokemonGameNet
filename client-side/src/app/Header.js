import React, { useEffect, useState } from "react";
import { Navbar, NavbarBrand, NavbarMenuToggle, NavbarMenu, NavbarMenuItem, NavbarContent, NavbarItem, Link } from "@nextui-org/react";
import { Avatar } from "@nextui-org/react";
import logo from '../assets/img/pokemon-logo.png';
import { Dropdown, DropdownTrigger, DropdownMenu, DropdownSection, DropdownItem, Button, User } from "@nextui-org/react";
import { PlusIcon } from "../assets/icon/PlusIcon";
import { useUser } from "../hooks/useUser";
import { useLogout } from "../hooks/useLogout";
import { SERVER_URI } from "../utils/Uri";
import { useLocation } from "react-router-dom";

export default function Header() {
    const [user, setUser] = useState();
    const [activeTab, setActiveTab] = useState("pokdex");
    const location = useLocation();
    const getUSer = useUser();
    const logoutUser = useLogout();
    const menuItems = [
        "Home",
        "Pokedex",
        "Chat Rooms",
        "Ranking",
        "Log Out",
    ];

    const items = [
        {
            key: "new",
            label: "New file",
        },
        {
            key: "delete",
            label: "Log out",
        }
    ];

    useEffect(() => {
        const path = location.pathname;
        if (path === "/home") {
            setActiveTab("home");
        } else if (path === "/pokedex") {
            setActiveTab("pokedex");
        } else if (path === "/leader-board") {
            setActiveTab("ranking");
        } else if (path === "/chat-rooms") {
            setActiveTab("chatRooms");
        }
         else {
            setActiveTab("");
        }
    }, [location]);

    useEffect(() => {
        const fetchUser = async () => {
            const userData = await getUSer();
            console.log(userData)
            setUser(userData);
        }
        fetchUser();
    }, []);

    //if (!user) return (<div>loading...</div>)

    return (
        <Navbar disableAnimation isBordered>
            <NavbarContent className="sm:hidden" justify="start">
                <NavbarMenuToggle />
            </NavbarContent>

            <NavbarContent className="sm:hidden pr-3" justify="start">
                <NavbarBrand>
                    <img
                        src={logo}
                        style={{ width: 100 }}
                    />
                </NavbarBrand>
            </NavbarContent>

            <NavbarContent className="hidden sm:flex gap-4 px-0" justify="center">
                <NavbarBrand>
                    <img
                        src={logo}
                        style={{ width: 100 }}
                    />
                    {/* <p className="font-bold text-inherit">ACME</p> */}
                </NavbarBrand>
                <NavbarItem isActive={activeTab === "home"}>
                    <Link
                        onClick={() => setActiveTab("home")}
                        color={activeTab === "home" ? "warning" : "foreground"}
                        href="/home">
                        Home
                    </Link>
                </NavbarItem>
                <NavbarItem isActive={activeTab === "pokedex"}>
                    <Link
                        onClick={() => setActiveTab("pokedex")}
                        href="/pokedex"
                        aria-current="page"
                        color={activeTab === "pokedex" ? "warning" : "foreground"}>
                        Pokedex
                    </Link>
                </NavbarItem>
                <NavbarItem isActive={activeTab === "chatRooms"}>
                    <Link
                        onClick={() => setActiveTab("chatRooms")}
                        color={activeTab === "chatRooms" ? "warning" : "foreground"}
                        href="/chat-rooms"
                    >
                        Chat Rooms
                    </Link>
                </NavbarItem>
                <NavbarItem isActive={activeTab === "leader-board"}>
                    <Link
                        onClick={() => setActiveTab("leader-board")}
                        color={activeTab === "leader-board" ? "warning" : "foreground"}
                        href="/leader-board"
                    >
                        Leader Board
                    </Link>
                </NavbarItem>
                <NavbarItem>
                    <Link 
                        className="px-4 py-1 bg-[rgb(125,125,235)] text-white rounded-md"
                        color="foreground" 
                        href="/play">
                        Play
                    </Link>
                </NavbarItem>
            </NavbarContent>

            <NavbarContent justify="end">
                {!user && <><NavbarItem className="hidden lg:flex">
                    <Link href="login" className="text-textBlue1">Login</Link>
                </NavbarItem>
                    <NavbarItem>
                        <Button as={Link} color="warning" href="/register" variant="flat">
                            Sign Up
                        </Button>
                    </NavbarItem></>}
                {user && <><NavbarItem>
                    <Dropdown
                        showArrow
                        radius="sm"
                        classNames={{
                            base: "before:bg-default-200", // change arrow background
                            content: "p-0 border-small border-divider bg-background",
                        }}
                    >
                        <DropdownTrigger>
                            <a href="#">
                                <Avatar showFallback src={`${SERVER_URI}${user?.data?.ImagePath}`} />
                            </a>
                        </DropdownTrigger>
                        <DropdownMenu
                            aria-label="Custom item styles"
                            disabledKeys={["profile"]}
                            className="p-3"
                            itemClasses={{
                                base: [
                                    "rounded-md",
                                    "text-default-500",
                                    "transition-opacity",
                                    "data-[hover=true]:text-foreground",
                                    "data-[hover=true]:bg-default-100",
                                    "dark:data-[hover=true]:bg-default-50",
                                    "data-[selectable=true]:focus:bg-default-50",
                                    "data-[pressed=true]:opacity-70",
                                    "data-[focus-visible=true]:ring-default-500",
                                ],
                            }}
                        >
                            <DropdownSection aria-label="Profile & Actions" showDivider>
                                <DropdownItem
                                    isReadOnly
                                    key="profile"
                                    className="h-14 gap-2 opacity-100"
                                >
                                    <User
                                        name={user?.data?.DisplayName || "not found"}
                                        description={user?.data?.Email || "not found"}
                                        classNames={{
                                            name: "text-default-600",
                                            description: "text-default-500",
                                        }}
                                        avatarProps={{
                                            size: "sm",
                                            src: `${SERVER_URI}${user?.data?.ImagePath}`,
                                        }}
                                    />
                                </DropdownItem>
                                <DropdownItem key="dashboard">
                                    Dashboard
                                </DropdownItem>
                                <DropdownItem key="settings">Settings</DropdownItem>
                                <DropdownItem
                                    key="new_project"
                                    endContent={<PlusIcon className="text-large" />}
                                >
                                    New Project
                                </DropdownItem>
                            </DropdownSection>

                            <DropdownSection aria-label="Preferences" showDivider>
                                <DropdownItem key="quick_search" shortcut="⌘K">
                                    Quick search
                                </DropdownItem>
                                <DropdownItem
                                    isReadOnly
                                    key="theme"
                                    className="cursor-default"
                                    endContent={
                                        <select
                                            className="z-10 outline-none w-16 py-0.5 rounded-md text-tiny group-data-[hover=true]:border-default-500 border-small border-default-300 dark:border-default-200 bg-transparent text-default-500"
                                            id="theme"
                                            name="theme"
                                        >
                                            <option>System</option>
                                            <option>Dark</option>
                                            <option>Light</option>
                                        </select>
                                    }
                                >
                                    Theme
                                </DropdownItem>
                            </DropdownSection>

                            <DropdownSection aria-label="Help & Feedback">
                                <DropdownItem key="help_and_feedback">
                                    Help & Feedback
                                </DropdownItem>
                                <DropdownItem
                                    onPress={logoutUser}
                                    key="logout">
                                    Log Out
                                </DropdownItem>
                            </DropdownSection>
                        </DropdownMenu>
                    </Dropdown>

                </NavbarItem></>}
            </NavbarContent>

            <NavbarMenu>
                {menuItems.map((item, index) => (
                    <NavbarMenuItem key={`${item}-${index}`}>
                        <Link
                            className="w-full"
                            color={
                                index === 2 ? "warning" : index === menuItems.length - 1 ? "danger" : "foreground"
                            }
                            href="#"
                            size="lg"
                        >
                            {item}
                        </Link>
                    </NavbarMenuItem>
                ))}
            </NavbarMenu>
        </Navbar>
    )
}