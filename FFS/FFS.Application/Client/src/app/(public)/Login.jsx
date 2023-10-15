import { useState } from "react";



const Login = () => {
  const tabs = [
    { id: "login", label: "Đăng Nhập" },
    { id: "register", label: "Đăng Ký" },
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
  const handleTabChange = (tab) => {
    setActiveTab(tab);
  };

  // const handleLogin = () => {
  //   // Xử lý đăng nhập ở đây, ví dụ: gửi email và password đến máy chủ
  //   console.log("Email:", email);
  //   console.log("Password:", password);
  // };
  const handleLogin = async () => {
    // Validate email format
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!email.match(emailRegex)) {
      setError('Vui lòng nhập đúng định dạng email!');
      return;
    }

   
    try {
      const response = await fetch('https://localhost:7025/api/Authenticate/LoginByEmail', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      if (response.ok) {
        const data = await response.json();
        const token = data.token.result;

        // Save the token to local storage or a secure store
        localStorage.setItem('token', token);
  
        // Redirect to the home page
        window.location.href = '/'; 

      } else {
        
        setError('Email hoặc mật khẩu không hợp lệ !');
      }
    } catch (errorMessage) {
      
      setError('Có lỗi xảy ra. Vui lòng thử lại !');
    }
  };
  

  const  handleLoginGoogle = async () => {
    window.location.href = 'https://localhost:7025/api/Authenticate/GoogleSignIn';
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
            {errorMessage && (
              <div className="text-red-600 text-center mt-4">
                {errorMessage}
              </div>
            )}
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
