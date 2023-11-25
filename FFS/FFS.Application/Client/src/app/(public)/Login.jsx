import { useEffect, useState } from "react";
import { Button, Input } from "@material-tailwind/react";
import { Tabs, TabsHeader, Tab } from "@material-tailwind/react";
import GoogleLogin from "@leecheuk/react-google-login";
import axios from "../../shared/api/axiosConfig";
import { useDispatch } from "react-redux";
import { gapi } from "gapi-script";
import { toast } from "react-toastify";
import { FaLock } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import CookieService from "../../shared/helper/cookieConfig";
import dayjs from "dayjs";
import { setAccessToken } from "../../redux/auth";

const Login = () => {
	const tabs = [
		{
			label: "Đăng nhập",
			value: "login",
			desc: `Đăng nhập`,
		},
	];

	const [activeTab, setActiveTab] = useState("login");
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [errorMessage, setError] = useState(null);
	const navigate = useNavigate();
	const dispatch = useDispatch();

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
			await axios
				.post("/api/Authenticate/LoginByEmail", {
					email,
					password,
				})
				.then((res) => {
					console.log(res);

					const token = res.userClient.token;
					CookieService.saveToken(
						"fu_foody_email",
						res.userClient.email,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);

					CookieService.saveToken(
						"fu_foody_id",
						res.userClient.userId,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);

					CookieService.saveToken(
						"fu_foody_token",
						res.userClient.token,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);

					CookieService.saveToken(
						"fu_foody_role",
						res.userClient.role,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);
					dispatch(setAccessToken(token));
					toast.success("Đăng nhập thành công !!");
					if (res.userClient.role == "Shipper") {
						window.location.href = "/shipper/order-available";
					} else if (res.userClient.role == "Admin") {
						window.location.href = "/admin/dashboard";
					} else if (res.userClient.role == "StoreOwner") {
						window.location.href = "/store/food";
					} else {
						window.location.href = "/";
					}
				})
				.catch((err) => {
					console.log(err);
					toast.error(err.response.data);
				});
		} catch (errorMessage) {
			setError("Có lỗi xảy ra. Vui lòng thử lại !" + errorMessage);
		}
	};

	const responseGoogle = (response) => {
		console.log(response);
		const data = { idToken: response.tokenId };
		axios
			.post("/api/Authenticate/LoginGoogle", data)
			.then((response) => {
				if (response.userClient.userId.length > 0) {
					// Save the token using Cookies
					CookieService.saveToken(
						"fu_foody_email",
						response.userClient.email,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);

					CookieService.saveToken(
						"fu_foody_id",
						response.userClient.userId,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);

					CookieService.saveToken(
						"fu_foody_token",
						response.userClient.token,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);

					CookieService.saveToken(
						"fu_foody_role",
						response.userClient.role,
						dayjs()
							.add(10000 - 300, "second")
							.toDate()
					);

					toast.success("Đăng nhập thành công !");
					// Navigate to the home page
					window.location.href = "/";
				}
			})
			.catch((error) => {
				toast.error(error.response.data);
			});
	};

	useEffect(() => {
		function start() {
			gapi.client.init({
				clientId:
					"267743841624-ak3av56ebuurfa5g6ipo135tm3pt5cda.apps.googleusercontent.com",
				scope: "email",
			});
		}

		gapi.load("client:auth2", start);
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
							className="bg-primary text-white p-2 rounded w-full mb-3 h-10"
							onClick={handleLogin}
						>
							Đăng nhập
						</Button>
						<div className="flex justify-center">
							<a
								href="/forgot-password"
								className="text-blue-500 mb-4 flex items-center px-4 py-2 rounded-full border border-blue-500 hover:bg-blue-500 hover:text-white transition duration-300"
							>
								<FaLock className="mr-2" />
								<span className="font-medium">Quên mật khẩu</span>
							</a>
						</div>
						<p className="mb-2">hoặc</p>
						<GoogleLogin
							clientId="267743841624-ak3av56ebuurfa5g6ipo135tm3pt5cda.apps.googleusercontent.com"
							buttonText="Đăng nhập bằng Google"
							onSuccess={responseGoogle}
							onFailure={responseGoogle}
							cookiePolicy={"single_host_origin"}
						></GoogleLogin>
					</div>
				) : (
					<div>{/* Thêm phần giao diện cho tab "Đăng ký" ở đây */}</div>
				)}
			</div>
		</div>
	);
};

export default Login;
