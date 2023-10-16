import axios from "axios";
import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { useLocation } from "react-router-dom";

const ResetPasswordPage = () => {
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const token = searchParams.get("token");
  const email = searchParams.get("email");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);

  useEffect(() => {
    // Validate the token and email here if needed
  }, [token, email]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!password) {
      toast.error('Mật khẩu không được để trống!');
      return;
    }

    if (!confirmPassword) {
      toast.error('Xác nhận mật khẩu không được để trống!');
      return;
    }

    if (password !== confirmPassword) {
      toast.error('Mật khẩu và xác nhận mật khẩu phải khớp nhau!');
      return;
    }

    // Send a request to the server to reset the password
    try {
      const response = await axios.post(
        "https://localhost:7025/api/Authenticate/ResetPassword/reset-password",
        
        {
          token: token,
          email: email,
          password: password,
        }
      );

      if (response.status === 200) {
        const data = response.data;
        if (data.isSucceed) {
          toast.success("Mật khẩu đã được đặt lại thành công!");
          setIsSuccess(true);
        } else {
          setErrorMessage(data.data);
        }
      } else {
        toast.error("Đã xảy ra lỗi khi đặt lại mật khẩu!");
      }
    } catch (error) {
      console.error("An error occurred:", error);
      toast.error("Đã xảy ra lỗi khi đặt lại mật khẩu!");
    }
  };

  return (
    <div className="bg-gray-50">
      <div className="p-2">
        <div className="max-w-md w-full mx-auto mt-8 p-8 bg-white shadow-md rounded-lg">
          <h2 className="mt-6 text-center text-3xl text-gray-600">
            Đặt lại mật khẩu
          </h2>
          <p className="mt-2 text-center text-sm text-gray-400">
            Đặt lại mật khẩu cho tài khoản
          </p>
          <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
            <div>
              <label className="block text-gray-400 text-sm font-bold mb-2">
                Mật khẩu mới
              </label>
              <input
                type="password"
                className="w-full px-3 py-2 placeholder-gray-400 text-gray-700 border"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="nhập mật khẩu mới..."
              />
            </div>
            <div>
              <label className="block text-gray-400 text-sm font-bold mb-2">
                Nhập lại mật khẩu
              </label>
              <input
                type="password"
                className="w-full px-3 py-2 placeholder-gray-400 text-gray-700 border"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                placeholder="nhập lại mật khẩu..."
              />
            </div>
            <div>
              <button
                type="submit"
                className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-primary focus:outline-none focus:ring focus:ring-indigo-200"
              >
                Đặt Lại Mật Khẩu
              </button>
            </div>
            {errorMessage && (
              <div className="text-red-600 text-center mt-4">
                {errorMessage}
              </div>
            )}

            {isSuccess && (
              <div className="text-green-600 text-center mt-4">
                Mật khẩu đã được đặt lại thành công!
              </div>
            )}
          </form>
          <div className="mt-4 text-center">
            <p className="text-sm text-gray-600">
              Quay lại{" "}
              <a href="/login" className="text-blue-600">
                Đăng nhập
              </a>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ResetPasswordPage;
