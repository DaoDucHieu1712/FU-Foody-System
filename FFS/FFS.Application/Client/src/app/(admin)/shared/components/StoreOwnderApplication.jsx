import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "../../../../shared/api/axiosConfig";
import { Button } from "@material-tailwind/react";
import { toast } from "react-toastify";

const StoreOwnderApplication = () => {
	const { id } = useParams();
	const [store, setStore] = useState();
	const [user, setUser] = useState();

	const loadStore = async () => {
		await axios.get(`/api/Store/GetStoreByUid?uId=${id}`).then((res) => {
			setStore(res);
		});
	};

	const loadStoreOwner = async () => {
		await axios
			.get(`/api/Authenticate/GetUserInformation/${id}`)
			.then((res) => {
				setUser(res);
			});
	};

	useEffect(() => {
		loadStoreOwner();
		loadStore();
		console.log(id);
	}, [id]);

	const handleClickBtn = async (id, username, action) => {
		var txt = "";
		if (action == "Accept") {
			txt = `Bạn có chắc chắn duyệt cửa hàng ${store.storeName}!`;
		} else {
			txt = `Bạn có chắc chắn từ chối duyệt cửa hàng ${store.storeName}!`;
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
						<span>Tên cửa hàng : </span>
						{store?.storeName}
					</h1>
					<div className="">
						<img
							src={store?.avatarURL}
							className="w-full h-[350px] object-cover"
						/>
					</div>
					<p>{store?.description}</p>
				</div>
				<div className="col-span-2 flex flex-col mt-10 ml-20 gap-y-10">
					<div className="text-medium text-xl">
						<p>
							<span>Chủ sở hữu : </span>
							{user?.userName}
						</p>
					</div>
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
							<span>Địa chỉ : </span>
							{store?.address}
						</p>
					</div>
					<div className="text-medium text-xl">
						<p>
							<span>Giờ mở cửa: </span>
							{store?.timeStart} - {store?.timeEnd}
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

export default StoreOwnderApplication;
