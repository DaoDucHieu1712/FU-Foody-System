import dayjs from "dayjs";
import { useForm } from "react-hook-form";
import { useDispatch, useSelector } from "react-redux";
import { NavLink, Outlet, useLocation } from "react-router-dom";
import { toast } from "react-toastify";
import * as yup from "yup";

const navigations = [
	{ href: "/profile", name: "Hồ sơ"},
	{ href: "/location", name: "Địa chỉ"},
	{ href: "/change-password", name: "Đổi mật khẩu"},
	
];
const ProfileLayout = () => {
    const location = useLocation();
	const dispatch = useDispatch();
	const user = useSelector((state) => state.auth.userProfile);
	
	return (
		<>
			{user && (
				<div className="grid grid-cols-5 my-12 gap-4 p-8">
					<div className="flex flex-col col-span-1">
						<div className="flex gap-x-2 items-center p-3 border-b border-gray-300">
							<img
								src={user.avatar}
								alt=""
								className="rounded-full w-[45px] h-[45px]"
							/>
							<span className="text-center font-medium">
								{user.firstName} {user.lastName}
							</span>
						</div>

						<div className="mt-3">
							<ul className="flex flex-col items-center gap-y-3 text-gray-700">
                            {navigations.map((item) => {
								return (
									<NavLink
										key={item.href}
										to={item.href}
                                        className={location.pathname === item.href ? "text-primary" : ""}
									>
										{item.name}
									</NavLink>
								);
							})}
								
								<li>
									<a href="/my-order">Đơn hàng</a>
								</li>
								<li>
									<a href="#">Thông báo</a>
								</li>
							</ul>
						</div>
					</div>

					<div className="flex flex-col col-span-4 shadow-md px-3">
                    <Outlet></Outlet>
					</div>
				</div>
			)}
		</>
	);
};

export default ProfileLayout;
