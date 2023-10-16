import { useEffect, useState } from "react";
import { Button, Input } from "@material-tailwind/react";
import {
  Tabs,
  TabsHeader,
  Tab,
} from "@material-tailwind/react";
import GoogleLogin from "@leecheuk/react-google-login";
import axios from "axios";
import { gapi } from "gapi-script";
import { toast } from "react-toastify";

const Login = () => {
  const tabs = [
    {
      label: "Đăng nhập",
      value: "login",
      desc: `Đăng nhập`,
    },
    {
      label: "Đăng kí",
      value: "register",
      desc: `Đăng kí`,
    },
  ];

  const [activeTab, setActiveTab] = useState("login");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setError] = useState(null);

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e) => {
    setPassword(e.target.value);
  };

  const handleLogin = async () => {
    // Validate email format
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!email.match(emailRegex)) {
      setError("Vui lòng nhập đúng định dạng email!");
      return;
    }

    try {
      const response = await fetch(
        "https://localhost:7025/api/Authenticate/LoginByEmail",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ email, password }),
        }
      );

      if (response.ok) {
        const data = await response.json();
        const token = data.token.result;

        // Save the token to local storage or a secure store
        localStorage.setItem("token", token);

        // Redirect to the home page
        window.location.href = "/";
      } else {
        setError("Email hoặc mật khẩu không hợp lệ !");
      }
    } catch (errorMessage) {
      setError("Có lỗi xảy ra. Vui lòng thử lại !");
    }
  };

  
  const responseGoogle = (response) => {
    console.log(response);
    const data = {idToken : response.tokenId};
    axios
    .post("https://localhost:7025/api/Authenticate/LoginGoogle", data)
    .then((response) => {
      console.log(response)
    })
    .catch((error) => {
      toast.error(error.response.data)
    });
  };
  
  useEffect(() => {
    function start() {
      gapi.client.init({
        clientId: '267743841624-ak3av56ebuurfa5g6ipo135tm3pt5cda.apps.googleusercontent.com',
        scope: 'email',
      });
    }

    gapi.load('client:auth2', start);
  }, []);

  return (
    <div className="flex justify-center mt-5">
      <div className="w-1/4 p-4 text-sm font-medium text-center text-gray-500 border-b border-gray-200 dark:text-gray-400 dark:border-gray-700 shadow-lg">
        <Tabs value={activeTab} className="mb-6">
          <TabsHeader
            className="rounded-none border-b border-blue-gray-50 bg-transparent p-0"
            indicatorProps={{
              className:
                "bg-transparent border-b-2 border-primary shadow-none rounded-none",
            }}
          >
            {tabs.map(({ label, value }) => (
              <Tab
                key={value}
                value={value}
                onClick={() => setActiveTab(value)}
                className={activeTab === value ? "text-primary" : ""}
              >
                {label}
              </Tab>
            ))}
          </TabsHeader>
        </Tabs>

        {activeTab === "login" ? (
          <div>
            <div className="mb-4">
              <Input
                label="Email"
                type="email"
                id="email"
                value={email}
                onChange={handleEmailChange}
                className="w-full border p-2 rounded"
              />
            </div>
            <div className="mb-4">
              <Input
                label="Mật khẩu"
                type="password"
                id="password"
                value={password}
                onChange={handlePasswordChange}
                className="w-full border p-2 rounded"
              />
            </div>
            {errorMessage && (
              <div className="text-red-600 text-center mt-4">
                {errorMessage}
              </div>
            )}
            <Button
              className="bg-primary text-white p-2 rounded w-full mb-8 h-10"
              onClick={handleLogin}
            >
              Đăng nhập
            </Button>
            <hr></hr>
             <GoogleLogin
               clientId="267743841624-ak3av56ebuurfa5g6ipo135tm3pt5cda.apps.googleusercontent.com"
               buttonText="Đăng nhập bằng Google"
               onSuccess={responseGoogle}
               onFailure={responseGoogle}
               cookiePolicy={"single_host_origin"}
            ></GoogleLogin>
            ;
          </div>
        ) : (
          <div>{/* Thêm phần giao diện cho tab "Đăng ký" ở đây */}</div>
        )}
      </div>
    </div>
  );
};

export default Login;
