import axios from "axios";
import { useState } from "react";
import { toast } from "react-toastify";
import { Input } from "@material-tailwind/react";
import { useNavigate } from "react-router-dom";

const ForgotPasswordPage = () => {
	const [email, setEmail] = useState("");
	const navigate = useNavigate();
	const handleSubmit = async (e) => {
		e.preventDefault();
		const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
		if (!email) {
			toast.error("Email không được để trống!");
			return;
		} else if (!emailRegex.test(email)) {
			toast.error("Vui lòng nhập đúng định dạng email!");
			return;
		}

		try {
			const response = await axios.post(
				`${
					import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL
				}/api/Authenticate/ForgotPassword`,
				null,
				{
					params: { email: email },
				}
			);

			if (response.status === 200) {
				const data = response.data;
				if (data.isSucceed) {
					toast.success(
						"Yêu cầu đã được gửi thành công, vui lòng kiểm tra email!"
					);
				} else {
					if (data.message === "Email is an FPT emai") {
						toast.error(data.data);
						setTimeout(() => {
							navigate("/login");
						}, 5000);
					} else {
						toast.error(data.data);
					}
				}
			} else {
				toast.error("Đã xảy ra lỗi khi gửi yêu cầu!");
			}
		} catch (error) {
			console.error("An error occurred:", error);
			toast.error("Đã xảy ra lỗi khi gửi yêu cầu!");
		}
	};

	return (
		<div className="bg-gray-50">
			<div className="p-2">
				<div className="max-w-md w-full mx-auto mt-8 p-8 bg-white shadow-md rounded-lg">
					<h2 className="mt-6 text-center text-3xl text-gray-600">
						Quên mật khẩu
					</h2>
					<p className="mt-2 text-center text-sm text-gray-400">
						Nhập email để đặt lại mật khẩu
					</p>
					<form className="mt-8 space-y-6" onSubmit={handleSubmit}>
						<div>
							<div className="w-90">
								<Input
									size="lg"
									label="Email"
									value={email}
									onChange={(e) => setEmail(e.target.value)}
								/>
							</div>
						</div>
						<div>
							<button
								type="submit"
								className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-primary focus:outline-none focus:ring focus:ring-indigo-200"
							>
								Gửi mã
							</button>
						</div>
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
