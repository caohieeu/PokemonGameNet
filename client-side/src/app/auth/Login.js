import React from 'react';
import { Input, Button } from "@nextui-org/react";
import { EyeFilledIcon } from '../../assets/icon/EyeFilledIcon';
import { EyeSlashFilledIcon } from '../../assets/icon/EyeSlashFilledIcon';
import { Form } from "antd";
import { useLogin } from '../../hooks/useLogin';
import logo from '../../assets/img/pokemon-logo.png';

export default function Login() {
  const [isVisiblePass, setIsVisiblePass] = React.useState(false);
  const [form] = Form.useForm();
  const login = useLogin();

  const toggleVisibilityPass = () => setIsVisiblePass(!isVisiblePass);

  const onFinish = async (data) => {
    await login(data)
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
      <div className="flex flex-col space-y-4 md:space-y-6 
      md:w-1/3 w-full p-10 md:order-first order-last">
        <img
          className='flex-auto'
          style={{width: 200, height: 200}}
          src={logo}
        />
        <p className='text-xl md:text-3xl font-sans font-bold w-full'
          style={{ marginRight: "10rem" }}>
          Login to play pokemon game
        </p>
        <p>
          Don't have an account? <a href="/register" className='text-textBlue1'>Sign Up</a>
        </p>
        <Form
          form={form}
          name="normal_login"
          onFinish={onFinish}
        >
          <Form.Item name={"username"}>
            <Input
              label="Username"
            />
          </Form.Item>
          <Form.Item name={"password"}>
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
          <Form.Item>
            <Button
              className='w-full'
              type="submit"
              style={{
                backgroundColor: 'rgb(125, 125, 235)',
                color: '#fff'
              }}>
              Login
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  )
}
