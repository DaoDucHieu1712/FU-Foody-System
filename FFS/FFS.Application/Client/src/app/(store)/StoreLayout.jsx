import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import NotFoundPage from "../NotFoundPage";
import Loading from "../../shared/components/Loading";
import { useDispatch } from "react-redux";
import CookieService from "../../shared/helper/cookieConfig";
import { setAccessToken } from "../../redux/auth";
import StoreIcon from "../../shared/components/icon/Store";
import Revenue from "../../shared/components/icon/Revenue";
import Logout from "../../shared/components/icon/Logout";
import Inventory from "../../shared/components/icon/Inventory";
import Category from "../../shared/components/icon/Category";
import Fork from "../../shared/components/icon/Fork";
import Discount from "../../shared/components/icon/Discount";
import FlashSale from "../../shared/components/icon/FlashSale";
import OrderIcon from "../../shared/components/icon/Order";
import User from "../../shared/components/icon/User";
import { cartActions } from "../(auth)/shared/cartSlice";
import { comboActions } from "../(auth)/shared/comboSlice";

const navigations = [
	{ href: "/store/edit", name: "Cập nhật thông tin", icon: <User /> },
	{ href: "/store/manager", name: "Thống kê cửa hàng", icon: <Revenue /> },
	{ href: "/store/inventory", name: "Kho món ăn của tôi", icon: <Inventory /> },
	{ href: "/store/category", name: "Danh mục của tôi", icon: <Category /> },
	{ href: "/store/food", name: "Thực phẩm", icon: <Fork /> },
	{ href: "/store/discount", name: "Mã giảm giá", icon: <Discount /> },
	{
		href: "/store/flash-sale",
		name: "Chương trình Flashsale",
		icon: <FlashSale />,
	},
	{ href: "/store/order", name: "Đơn hàng", icon: <OrderIcon /> },
];

const StoreLayout = () => {
	const navigate = useNavigate();
	const dispatch = useDispatch();
	const [email, setEmail] = useState();
	const [notFound, setNotFound] = useState(null);
	const [loading, setLoading] = useState(true); // Add loading state
	const getCurrentUser = () => {
		axios
			.get("api/Authenticate/GetCurrentUser")
			.then((res) => {
				if (res.Role != "STOREOWNER") {
					setNotFound(true);
				}
				setEmail(res.Email);
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
	return (
		<>
			{notFound == true ? (
				<NotFoundPage />
			) : (
				<div className="flex">
					<div className="fixed p-3 flex flex-col gap-y-12 h-[100vh] shadow-md w-[20vw] bg-primary">
						<div className="">
							<a
								href="/store/manager"
								className="logo flex items-center justify-center border-b pb-8 border-gray-300"
							>
								<StoreIcon />
								<p className="text-white p-3 text-xl">Quản lí cửa hàng</p>
							</a>
						</div>
						<div className="list flex flex-col justify-start items-center gap-y-10 text-md font-medium text-white ml-10">
							{navigations.map((item) => {
								return (
									<NavLink
										key={item.href}
										to={item.href}
										className="uppercase flex items-center w-full"
									>
										{item.icon} <p className="px-4">{item.name}</p>
									</NavLink>
								);
							})}
							<button
								className="uppercase flex items-center w-full"
								onClick={() => {
									CookieService.removeToken("fu_foody_token"); // Remove the user token
									CookieService.removeToken("fu_foody_role");
									CookieService.removeToken("fu_foody_id");
									CookieService.removeToken("fu_foody_email");
									dispatch(cartActions.clearCart());
									dispatch(comboActions.clearCart());
									dispatch(setAccessToken(null));
									window.location.href = "/login"; // Redirect to the login page
								}}
							>
								<Logout />
								<p className="px-4">Đăng xuất</p>
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

export default StoreLayout;
