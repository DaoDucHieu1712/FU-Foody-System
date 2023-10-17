import axios from "axios";
import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { useLocation, useNavigate   } from "react-router-dom";
import { Input } from "@material-tailwind/react";

const ResetPasswordPage = () => {
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const token = searchParams.get("token");
  const email = searchParams.get("email");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const navigate = useNavigate();

  const passwordRegex =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$/;

  useEffect(() => {
    // Validate the token and email here if needed
  }, [token, email]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    
    const errors = [];

  if (!password) {
    errors.push("Mật khẩu không được để trống!");
  }

  if (!password.match(passwordRegex)) {
    errors.push(
      "Mật khẩu phải chứa ít nhất một chữ thường, một chữ hoa, một chữ số và một ký tự đặc biệt (#$^+=!*()@%&), và có ít nhất 8 ký tự."
    );
  }

  if (!confirmPassword) {
    errors.push("Xác nhận mật khẩu không được để trống!");
  }

  if (password !== confirmPassword) {
    errors.push("Mật khẩu và xác nhận mật khẩu phải khớp nhau!");
  }

  if (errors.length > 0) {
    // Display all accumulated error messages
    errors.forEach((error) => toast.error(error));
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

      const data = response.data;
      if(data.code === 200){
        toast.success("Mật khẩu đã được đặt lại thành công!")
        setIsSuccess(true);
        setTimeout(() => {
          navigate("/login");
        }, 5000); // Redirect after 5 seconds (adjust the delay as needed)
      }
       else if (data.code === 400) {
        setErrorMessage(data.message);
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
          {/* Conditionally render the form based on isSuccess and errorMessage */}
          {isSuccess ? (
            <div className="text-green-600 text-center mt-4">
              Mật khẩu đã được đặt lại thành công!
            </div>
          ) : (
          <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
            <div>
              <div className="relative w-90">
                <Input
                  size="lg"
                  type={showPassword ? "text" : "password"}
                  label="Mật khẩu mới"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <button
                  type="button"
                  className="absolute inset-y-0 right-0 flex items-center px-4 text-gray-600"
                  onClick={() => setShowPassword(!showPassword)} // Updated onClick handler
                >
                  {showPassword ? ( // Check the 'showPassword' state
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      strokeWidth={1.5}
                      stroke="currentColor"
                      className="w-5 h-5"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M3.98 8.223A10.477 10.477 0 001.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.45 10.45 0 0112 4.5c4.756 0 8.773 3.162 10.065 7.498a10.523 10.523 0 01-4.293 5.774M6.228 6.228L3 3m3.228 3.228l3.65 3.65m7.894 7.894L21 21m-3.228-3.228l-3.65-3.65m0 0a3 3 0 10-4.243-4.243m4.242 4.242L9.88 9.88"
                      />
                    </svg>
                  ) : (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      strokeWidth={1.5}
                      stroke="currentColor"
                      className="w-5 h-5"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M2.036 12.322a1.012 1.012 0 010-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178z"
                      />
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
                      />
                    </svg>
                  )}
                </button>
              </div>
            </div>
            <div>
              <div className="relative w-90">
                <Input
                  size="lg"
                  type={showConfirmPassword ? "text" : "password"}
                  label="Nhập lại mật khẩu"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
                 <button
                  type="button"
                  className="absolute inset-y-0 right-0 flex items-center px-4 text-gray-600"
                  onClick={() => setShowConfirmPassword(!showConfirmPassword)} // Updated onClick handler
                >
                  {showConfirmPassword ? ( // Check the 'showPassword' state 
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      strokeWidth={1.5}
                      stroke="currentColor"
                      className="w-5 h-5"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M3.98 8.223A10.477 10.477 0 001.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.45 10.45 0 0112 4.5c4.756 0 8.773 3.162 10.065 7.498a10.523 10.523 0 01-4.293 5.774M6.228 6.228L3 3m3.228 3.228l3.65 3.65m7.894 7.894L21 21m-3.228-3.228l-3.65-3.65m0 0a3 3 0 10-4.243-4.243m4.242 4.242L9.88 9.88"
                      />
                    </svg>
                  ) : (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 24 24"
                      strokeWidth={1.5}
                      stroke="currentColor"
                      className="w-5 h-5"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M2.036 12.322a1.012 1.012 0 010-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178z"
                      />
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
                      />
                    </svg>
                  )}
                </button>
              </div>
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
          )}

           {errorMessage && !isSuccess && (
            <div className="mt-4 text-center">
              <p className="text-sm text-gray-600">
                Quay lại{" "}
                <a href="/login" className="text-blue-600">
                  Đăng nhập
                </a>
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ResetPasswordPage;
