import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "../../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import { Button } from "@material-tailwind/react";

const ShipperApplication = () => {
	const { id } = useParams();
	const [user, setUser] = useState();

	const getShipper = async () => {
		await axios
			.get(`/api/Authenticate/GetUserInformation/${id}`)
			.then((res) => {
				setUser(res);
			});
	};

	useEffect(() => {
		getShipper();
	}, [id]);

	const handleClickBtn = async (id, username, action) => {
		var txt = "";
		if (action == "Accept") {
			txt = `Bạn có chắc chắn duyệt giao hàng ${user.userName}!`;
		} else {
			txt = `Bạn có chắc chắn từ chối duyệt giao hàng ${user.storeName}!`;
		}
		const check = confirm(txt);
		if (check) {
			const data = {
				id: id,
				username: username,
				action: action,
			};

			try {
				await axios
					.post(`/api/Admin/ApproveUser`, data)
					.then((res) => {
						toast.success(res);
						window.location.href = "/admin/request-account";
					})
					.catch((error) => {
						toast.error("Có lỗi xảy ra!");
						console.log(error);
					});
			} catch (error) {
				console.log("Food error: " + error);
			}
		}
	};

	return (
		<>
			<div className="grid grid-cols-3 gap-x-3">
				<div className="flex flex-col gap-y-3">
					<h1 className="font-medium">
						<span>Tên Shipper : </span>
						{user?.firstName}
					</h1>
					<div className="">
						<img src={user?.avatar} className="w-full h-[350px] object-cover" />
					</div>
				</div>
				<div className="col-span-2 flex flex-col mt-10 ml-20 gap-y-10">
					<div className="text-medium text-xl">
						<p>
							<span>Email : </span>
							{user?.email}
						</p>
					</div>
					<div className="text-medium text-xl">
						<p>
							<span>Số điện thoại : </span>
							{user?.phoneNumber ? user?.phoneNumber : "0123456789"}
						</p>
					</div>
					<div className="text-medium text-xl">
						<p>
							<span>Ngày sinh : </span>
							{user?.birthDay.slice(0, 10)}
						</p>
					</div>
					<div className="text-medium text-xl">
						<p>
							<span>Giới tính: </span>
							{user?.gender ? "Nam" : "Nữ"}
						</p>
					</div>
					<div className="flex gap-x-3">
						<Button
							className="bg-red-500"
							onClick={() => handleClickBtn(user.id, user.userName, "Reject")}
						>
							Từ chối
						</Button>
						<Button
							className="bg-green-500"
							onClick={() => handleClickBtn(user.id, user.userName, "Accept")}
						>
							Chấp nhận
						</Button>
					</div>
				</div>
			</div>
		</>
	);
};

export default ShipperApplication;
