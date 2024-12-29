import React, { useState } from 'react';
import { Input, Button } from "@nextui-org/react";
import { EyeFilledIcon } from '../../assets/icon/EyeFilledIcon';
import { EyeSlashFilledIcon } from '../../assets/icon/EyeSlashFilledIcon';
import { Form } from "antd";
import { useRegister } from '../../hooks/useRegister';
import 'flowbite';

export default function Register() {
    const register = useRegister();
    const [isVisiblePass, setIsVisiblePass] = React.useState(false);
    const [form] = Form.useForm();

    const toggleVisibilityPass = () => setIsVisiblePass(!isVisiblePass);

    const onFinish = async (data) => {
        await register(data)
    };

    return (
        <div className="md:flex md:flex-row flex-col">
            <div className="w-full flex justify-center items-center">
                <div className="bg-[url('https://img.upanh.tv/2024/10/17/thumb-1920-206263.png')] 
        bg-no-repeat bg-cover h-64 md:h-screen w-full" >
                    <div className='h-10'>
                        <p>asdasdkmoas</p>
                        <p>asdasdkmoas</p>
                    </div>
                </div>
            </div>
            <div className="flex flex-col space-y-4 md:space-y-6 md:w-1/3 w-full p-10 md:order-first order-last">
                <p className='text-xl md:text-3xl font-sans font-bold w-full'
                    style={{ marginRight: "10rem" }}>
                    Login to play pokemon game
                </p>
                <p>
                    Have an account? <a href="/login" className='text-textBlue1'>Sign In</a>
                </p>
                <Form
                    form={form}
                    name="normal_login"
                    onFinish={onFinish}
                    className="mb-5"
                >
                    <Form.Item name={"DisplayName"}>
                        <Input
                            label="Character Name"
                        />
                    </Form.Item>
                    <Form.Item name={"UserName"}>
                        <Input
                            label="UserName"
                        />
                    </Form.Item>
                    <Form.Item name={"Password"}>
                        <Input
                            label="Password"
                            variant="bordered"
                            endContent={
                                <button className="focus:outline-none" type="button" onClick={toggleVisibilityPass} aria-label="toggle password visibility">
                                    {isVisiblePass ? (
                                        <EyeSlashFilledIcon className="text-2xl text-default-400 pointer-events-none" />
                                    ) : (
                                        <EyeFilledIcon className="text-2xl text-default-400 pointer-events-none" />
                                    )}
                                </button>
                            }
                            type={isVisiblePass ? "text" : "password"}
                            className="w-full"
                        />
                    </Form.Item>
                    <Form.Item name={"Email"}>
                        <Input type="email" label="Email" />
                    </Form.Item>
                    <Form.Item name={"PhoneNumber"}>
                        <Input type="tel" label="PhoneNumber" />
                    </Form.Item>
                    {/* <Form.Item style={{marginBottom: "50px"}}>
                        <button
                            id="dropdownDefaultButton"
                            data-dropdown-toggle="dropdown"
                            class="text-default-400 w-full border text-center focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-5 py-2.5 inline-flex items-center justify-between dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                            type="button"
                        >
                            Choose Role
                            <svg
                                class="w-2.5 h-2.5"
                                aria-hidden="true"
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 10 6"
                            >
                                <path
                                    stroke="currentColor"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                    stroke-width="2"
                                    d="m1 1 4 4 4-4"
                                />
                            </svg>
                        </button>

                        <div id="dropdown" class="z-10 hidden w-full bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700">
                            <ul class="py-2 text-sm text-gray-700 dark:text-gray-200" aria-labelledby="dropdownDefaultButton">
                            <li>
                                <a href="#" class="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Player</a>
                            </li>
                            <li>
                                <a href="#" class="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Admin</a>
                            </li>
                            </ul>
                        </div>
                    </Form.Item> */}
                    <Form.Item className='w-full'>
                        <Button
                            className='w-full'
                            type="submit"
                            style={{
                                backgroundColor: 'rgb(125, 125, 235)',
                                color: '#fff'
                            }}>
                            Register
                        </Button>
                    </Form.Item>
                </Form>
            </div>
        </div>
    )
}
