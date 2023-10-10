import axios from "axios";
import { useState } from "react";


const Login = () => {
  const tabs = [
    { id: "login", label: "Đăng Nhập" },
    { id: "register", label: "Đăng Ký" },
  ];

  const [activeTab, setActiveTab] = useState("login");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e) => {
    setPassword(e.target.value);
  };
  const handleTabChange = (tab) => {
    setActiveTab(tab);
  };

  const handleLogin = () => {
    // Xử lý đăng nhập ở đây, ví dụ: gửi email và password đến máy chủ
    console.log("Email:", email);
    console.log("Password:", password);
  };

  const  handleLoginGoogle = async () => {
    try {
    
        const response = await axios.get('https://localhost:7025/api/Authenticate/GoogleSignIn');
    
        console.log('API Response:', response.data);
    
      } catch (error) {
        console.error('Error:', error);
      }
  }


  return (
    <div className="flex justify-center mt-5">
      <div className="w-1/4 p-4 text-sm font-medium text-center text-gray-500 border-b border-gray-200 dark:text-gray-400 dark:border-gray-700 shadow-lg">
        <ul className="flex flex-wrap mb-5 border-b-stone-500">
          {tabs.map((tab) => (
            <li key={tab.id} className="mr-2">
              <a
                href="#"
                onClick={() => handleTabChange(tab.id)}
                className={`inline-block p-4 ${
                  tab.id === activeTab
                    ? "text-blue-600 border-b-2 border-blue-600 rounded-t-lg active dark:text-blue-500 dark:border-blue-500"
                    : "border-b-2 border-transparent rounded-t-lg hover:text-gray-600 hover:border-gray-300 dark:hover:text-gray-300"
                } ${
                  tab.disabled
                    ? "cursor-not-allowed text-gray-400 dark:text-gray-500"
                    : ""
                }`}
                aria-current={tab.id === activeTab ? "page" : undefined}
                disabled={tab.disabled}
              >
                {tab.label}
              </a>
            </li>
          ))}
        </ul>

        {activeTab === "login" ? (
          <div>
            <label htmlFor="email" className="block mb-2 font-semibold text-left">
              Email
            </label>
            <input
              type="email"
              id="email"
              placeholder="Email"
              value={email}
              onChange={handleEmailChange}
              className="w-full border p-2 mb-4 rounded"
            />
            <label htmlFor="password" className="block mb-2 font-semibold text-left">
              Mật khẩu
            </label>
            <input
              type="password"
              id="password"
              placeholder="Mật khẩu"
              value={password}
              onChange={handlePasswordChange}
              className="w-full border p-2 mb-4 rounded"
            />
            <button
              className="bg-primary text-white p-2 rounded w-full mb-8"
              onClick={handleLogin}
              
            >
              Đăng nhập
            </button>
            <hr></hr>
            <button className="bg-white text-black p-2 rounded mt-2 w-full border flex justify-center" onClick={handleLoginGoogle}>
                <p className="pr-2">
                <i className="fab fa-google"></i>
                </p>
              <p>
              Đăng nhập qua Google 
              </p>
            </button>
          </div>
        ) : (
          <div>{/* Thêm phần giao diện cho tab "Đăng ký" ở đây */}</div>
        )}
      </div>
    </div>
  );
};

export default Login;
