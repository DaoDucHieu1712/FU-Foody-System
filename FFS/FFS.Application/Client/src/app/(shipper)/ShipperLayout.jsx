import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import NotFoundPage from "../NotFoundPage";
import Loading from "../../shared/components/Loading";
import LogoShipper from "../../shared/components/logo/logo-shipper";
import OrderIcon from "../../shared/components/icon/Order";
import User from "../../shared/components/icon/User";
import Done from "../../shared/components/icon/Done";
import Revenue from "../../shared/components/icon/Revenue";
import Logout from "../../shared/components/icon/Logout";
import Wind from "../../shared/components/icon/Wind";
import Shipping from "../../shared/components/icon/Shipping";
import CookieService from "../../shared/helper/cookieConfig";
import { setAccessToken } from "../../redux/auth";
import { useDispatch } from "react-redux";
import { cartActions } from "../(auth)/shared/cartSlice";
import { comboActions } from "../(auth)/shared/comboSlice";

const ShipperLayout = () => {
	const userId = CookieService.getToken("fu_foody_id");
	const dispatch = useDispatch();
	const navigate = useNavigate();
	const [email, setEmail] = useState();
	const [notFound, setNotFound] = useState(false);
	const [loading, setLoading] = useState(true); // Add loading state
	const getCurrentUser = () => {
		axios
			.get("api/Authenticate/GetCurrentUser")
			.then((res) => {
				if (res.Role != "SHIPPER") {
					setNotFound(true);
				}
				setEmail(res.email);
				console.log(res);
			})
			.catch((err) => {
				console.log(err);
				navigate("/login");
			})
			.finally(() => {
				setLoading(false); // Set loading to false when the API call completes
			});
	};
	useEffect(() => {
		getCurrentUser();
	}, []);
	if (loading) {
		return <Loading></Loading>;
	}
	const handleLogout = () => {
		CookieService.removeToken("fu_foody_token"); // Remove the user token
		CookieService.removeToken("fu_foody_role");
		CookieService.removeToken("fu_foody_id");
		CookieService.removeToken("fu_foody_email");
		dispatch(setAccessToken(null));
		dispatch(cartActions.clearCart());
		dispatch(comboActions.clearCart());
		window.location.href = "/login";
	};
	return (
		<>
			{notFound == true ? (
				<NotFoundPage />
			) : (
				<div className="flex">
					<div className="fixed p-4 flex flex-col gap-y-12 h-[100vh] shadow-md w-[20vw] bg-primary">
						<div className="flex items-center justify-center border-b pb-8 border-gray-300">
							<a href="/shipper/view-statistic" className="flex items-center">
								<Wind />
								<LogoShipper />
							</a>
						</div>
						<div className="list flex flex-col  items-start gap-y-8 text-md font-medium text-white">
							<NavLink
								to="/shipper/profile"
								className="uppercase flex items-center"
							>
								<User />
								<span className="p-2">Thông tin cá nhân</span>
							</NavLink>
							<NavLink
								to="/shipper/order-pending"
								className="uppercase flex items-center"
							>
								<Shipping />
								<span className="p-2">Đơn hàng đang giao</span>
							</NavLink>
							<NavLink
								to="/shipper/order-available"
								className="uppercase flex items-center"
							>
								<OrderIcon />
								<span className="p-2">Đơn hàng khả dụng</span>
							</NavLink>
							<NavLink
								to="/shipper/order-shipped"
								className="uppercase flex items-center"
							>
								<Done />
								<span className="p-2">Đơn hàng đã hoàn thành</span>
							</NavLink>
							<NavLink
								to="/shipper/view-statistic"
								className="uppercase flex items-center"
							>
								<Revenue />
								<span className="p-2">Thống kê</span>
							</NavLink>

							<button
								className="uppercase flex items-center"
								onClick={handleLogout}
							>
								<Logout />
								<span className="p-2">Đăng xuất</span>
							</button>
						</div>
					</div>
					<section className="w-[80vw] ml-[20vw]">
						<div className="flex items-center justify-end bg-primary p-9">
							<div>
								<p className="text-white font-medium cursor-pointer text-xl">
									{email}
								</p>
							</div>
						</div>
						<div className="p-10">
							<Outlet></Outlet>
						</div>
					</section>
				</div>
			)}
		</>
	);
};

export default ShipperLayout;
