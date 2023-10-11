import axios from "axios";
import { useState } from "react";

const ForgotPasswordPage = () => {
  const [email, setEmail] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!emailRegex.test(email)) {
      setErrorMessage("Vui lòng nhập đúng định dạng email!");
      setIsSuccess(false);
      return;
    }
    try {
      const response = await axios.post(
        "https://localhost:7025/api/Authenticate/ForgotPassword",
        null,
        {
          params: { email: email },
        }
      );

      if (response.status === 200) {
        const data = response.data;
        if (data.isSucceed) {
          // Handle success here, for example, show a success message.
          setIsSuccess(true);
          setErrorMessage("");
        } else {
          // Handle the case where the email is not found.
          setErrorMessage(data.data);
          setIsSuccess(false);
        }
      } else {
        // Handle network errors or other issues with the API call.
        setErrorMessage("Đã xảy ra lỗi khi gửi yêu cầu!");
        setIsSuccess(false);
      }
    } catch (error) {
      console.error("An error occurred:", error);
      setErrorMessage("Đã xảy ra lỗi khi gửi yêu cầu!");
      setIsSuccess(false);
    }
  };
  return (
    <div className="bg-gray-50">
      <div className="p-2">
        <div className="max-w-md w-full mx-auto mt-8 p-8 bg-white shadow-md rounded-lg">
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-600">
            Quên Mật Khẩu
          </h2>
          <p className="mt-2 text-center text-sm text-gray-600">
            Nhập email hoặc số điện thoại để nhận mã xác nhận
          </p>
          <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
            <div>
              <label className="block text-gray-600 text-sm font-bold mb-2">
                Email / Số điện thoại
              </label>
              <input
                type="text"
                className="w-full px-3 py-2 placeholder-gray-400 text-gray-700 border"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div>
              <button
                type="submit"
                className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-primary focus:outline-none focus:ring focus:ring-indigo-200"
              >
                Gửi mã
              </button>
            </div>
            {errorMessage && (
              <div className="text-red-600 text-center mt-4">
                {errorMessage}
              </div>
            )}

            {isSuccess && (
              <div className="text-green-600 text-center mt-4">
                Password reset email sent successfully!
              </div>
            )}
          </form>
          <div className="mt-4 text-center">
            <p className="text-sm text-gray-600">
              Đã có tài khoản?{" "}
              <a href="/login" className="text-blue-600">
                Đăng nhập
              </a>
            </p>
            <p className="text-sm text-gray-600">
              Chưa có tài khoản?{" "}
              <a href="/register" className="text-blue-600">
                Đăng ký
              </a>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ForgotPasswordPage;
