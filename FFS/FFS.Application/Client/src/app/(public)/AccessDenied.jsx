import CookieService from "../../shared/helper/cookieConfig";

const AccessDenied = () => {
	const handleBackBtn = () => {
		const role = CookieService.getToken("fu_foody_role");
		var href = "";
		if (role === "Shipper") {
			href = "/shipper/order-available";
		} else if (role === "Admin") {
			href = "/admin/dashboard";
		} else if (role === "User") {
			href = "/";
		} else if (role === "StoreOwner") {
			href = "/store/manager";
		}
		window.location.href = href;
	};
	return (
		<>
			<div className="w-full h-full relative">
				<img
					className="w-full h-screen object-cover"
					src="/src/assets/403.png"
					alt="not-found"
				/>
				<p
					className="absolute top-[65%] right-[50%] z-10 px-4 py-2 bg-primary text-white cursor-pointer hover:opacity-80"
					onClick={handleBackBtn}
				>
					Quay về trang chủ
				</p>
			</div>
		</>
	);
};

export default AccessDenied;
