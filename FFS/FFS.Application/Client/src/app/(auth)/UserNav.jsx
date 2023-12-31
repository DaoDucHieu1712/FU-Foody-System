import { useState, useEffect, useRef } from "react";
import User from "../../shared/components/icon/User";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { setAccessToken } from "../../redux/auth";
import CookieService from "../../shared/helper/cookieConfig";
import axios from "../../shared/api/axiosConfig";
import { useSelector } from "react-redux";
import { cartActions } from "./shared/cartSlice";
import { comboActions } from "./shared/comboSlice";

const UserNav = () => {
	const userInfo = useSelector((state) => state.auth.userProfile);
	console.log(userInfo);

	const [showTooltip, setShowTooltip] = useState(false);
	const tooltipRef = useRef(null);
	const navigate = useNavigate();
	const dispatch = useDispatch();
	const toggleTooltip = () => {
		setShowTooltip(!showTooltip);
	};

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

	useEffect(() => {
		//Đóng thông báo khi click bên ngoài
		const handleClickOutside = (event) => {
			if (tooltipRef.current && !tooltipRef.current.contains(event.target)) {
				setShowTooltip(false);
			}
		};

		//Skien nhap chuột vào cửa sổ
		window.addEventListener("click", handleClickOutside);
	});
	const handleProfileClick = () => {
		navigate("/profile");
	};

	const handleMyOrderClick = () => {
		navigate("/my-order");
	};

	//css
	// Inline styles for the tooltip and triangle
	const tooltipStyle = {
		position: "absolute",
		top: "134%",
		left: "-110%",
		transform: "translateX(-50%)",
		width: "160px",
		backgroundColor: "#fff",
		border: "0px solid #ccc",
		borderRadius: "2px",
		boxShadow: "0 2px 4px rgba(0, 0, 0, 0.2)",
		zIndex: "1",
		opacity: showTooltip ? "1" : "0",
		transition: "opacity 0.2s ease-in-out",
	};

	const triangleStyle = {
		content: "",
		position: "absolute",
		top: "-18px",
		left: "82%",
		transform: "translateX(-50%)",
		borderWidth: "10px",
		borderStyle: "solid",
		borderColor: `transparent transparent #fff transparent`,
	};
	//

	return (
		<>
			<div
				className="relative inline-block text-left cursor-pointer"
				ref={tooltipRef}
			>
				<div onClick={toggleTooltip}>
					{/* <User className="text-xl cursor-pointer" /> */}

					{userInfo && (
						<img
							className="w-8 h-8 rounded-full"
							src={userInfo.avatar}
							alt="Avatar"
						/>
					)}
				</div>
				{showTooltip && (
					<div
						style={tooltipStyle}
						className="divide-y divide-gray-100 rounded-lg shadow"
					>
						<div style={triangleStyle} />
						<div
							onClick={() =>
								navigate(
									`/user-detail/${CookieService.getToken("fu_foody_id")}`
								)
							}
							className="cursor-pointer px-4 py-2 text-sm text-gray-900 dark:text-white"
						>
							<div>
								{userInfo.firstName} {userInfo.lastName}
							</div>
							<div className="font-medium truncate">{userInfo.email}</div>
						</div>
						<ul className="py-2 text-sm text-gray-700 dark:text-gray-200">
							<li>
								<a
									href={`/user-detail/${CookieService.getToken("fu_foody_id")}`}
									className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white cursor-pointer"
								>
									Trang cá nhân
								</a>
							</li>
							<li>
								<a
									onClick={handleProfileClick}
									className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white cursor-pointer"
								>
									Tài khoản của tôi
								</a>
							</li>
							<li>
								<a
									onClick={handleMyOrderClick}
									className="cursor-pointer block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white"
								>
									Đơn Mua
								</a>
							</li>
							<li>
								<a
									onClick={handleLogout}
									className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white cursor-pointer"
								>
									Đăng Xuất
								</a>
							</li>
						</ul>
					</div>
				)}
			</div>
		</>
	);
};

export default UserNav;
