import { Button, Spinner } from "@material-tailwind/react";
import { useQuery } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router-dom";
import OrderStatus from "../(store)/components/order/OrderStatus";
import OrderService from "../(store)/shared/order.service";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import { useSelector } from "react-redux";

const OrderIdelDetail = () => {
	const userProfile = useSelector((state) => state.auth.userProfile);
	const navigate = useNavigate();
	const { id } = useParams();

	const orderQuery = useQuery({
		queryKey: ["order-idel-detail"],
		queryFn: async () => {
			return await OrderService.GetOrderDetail(id);
		},
	});

	const handleClickRecieveOrder = async () => {
		await axios
			.put(`/api/Order/ReceiveOrderUnbook/${userProfile.id}/${id}`)
			.then((res) => {
				toast.success(res);
				navigate("/shipper/order-available");
			})
			.catch((err) => {
				toast.error(err.response.data);
			});
	};

	return (
		<>
			<div className="container mx-auto grid grid-cols-3 gap-x-12 my-20">
				<div className="col-span-2">
					<h1 className="font-bold text-2xl mb-3">Đơn Hàng Này</h1>
					<p>
						có {orderQuery.data?.orderDetails.length} sản phẩm trong đơn hàng #
						{orderQuery.data?.id}
					</p>
					<>
						<div className="m-10 flex flex-col gap-y-5 cart mt-8 border-1 rounded-md border-gray-400">
							{orderQuery.isLoading ? (
								<Spinner />
							) : (
								orderQuery.data?.orderDetails.map((item) => {
									return (
										<>
											<div
												key={item.id}
												className="w-full cart-item flex justify-between rounded-lg gap-x-3 p-3 border border-gray-200"
											>
												<div className="flex gap-x-3">
													<img
														src={item.imageURL}
														alt=""
														className="w-[100px] object-cover"
													/>
													<div className="flex flex-col gap-y-1 text-sm">
														<p className="font-bold w-[300px]">
															{item.foodName || item.comboName}
														</p>
														<p>{item.unitPrice} $</p>
													</div>
												</div>
												<div className="flex flex-col gap-y-3">
													<p className="text-sm">{item.unitPrice} $</p>
													<div className="flex items-center justify-center gap-x-1">
														<button className="">x</button>
														<span>{item.quantity}</span>
													</div>
												</div>
											</div>
										</>
									);
								})
							)}
						</div>
					</>
				</div>
				<div className="flex flex-col gap-y-4">
					<h1 className="pb-3 border-b font-bold text-2xl border-gray-200">
						Đơn hàng : #{orderQuery.data?.id}
					</h1>
					<div className="flex justify-between items-center border-b pb-3 border-gray-200">
						<p className="font-bold text-lg">Khách hàng :</p>
						<p className="text-red-500  font-bold text-lg">
							{orderQuery.data?.customerName}
						</p>
					</div>
					<div className="flex justify-between items-center border-b pb-3 border-gray-200">
						<p className="font-bold text-lg">Nhân viên giao hàng :</p>
						<p className="text-red-500  font-bold text-lg">
							{orderQuery.data?.shipperName}
						</p>
					</div>
					<div className="flex justify-between items-center border-b pb-3 border-gray-200">
						<p className="font-bold text-lg">Tổng tiền :</p>
						<p className="text-red-500  font-bold text-lg">
							{orderQuery.data?.totalPrice} $
						</p>
					</div>
					<div className="flex justify-between items-center border-b pb-3 border-gray-200">
						<p className="font-bold text-lg">Ngày đặt :</p>
						<p className="text-red-500  font-bold text-lg">
							{orderQuery.data?.createdAt.toString().slice(0, 10)}
						</p>
					</div>
					<div className="my-3 flex justify-between items-center gap-x-3">
						<p className="font-bold text-lg">Trạng thái : </p>
						<OrderStatus status={orderQuery.data?.orderStatus}></OrderStatus>
					</div>
					<ul className="info">
						<div className="flex justify-between gap-y-2 flex-col border-b pb-3 border-gray-200">
							<p className="font-bold text-lg">Địa chỉ :</p>
							<p className="font-medium text-sm">{orderQuery.data?.location}</p>
						</div>
					</ul>
					<ul className="info">
						<div className="flex justify-between gap-y-2 flex-col border-b pb-3 border-gray-200">
							<p className="font-bold text-lg">Số điện thoại :</p>
							<p className="font-medium text-sm">
								{orderQuery.data?.phoneNumber}
							</p>
						</div>
					</ul>
					<div className="mt-3 border-gray-700  text-black rounded-md">
						<p className="">
							<span className="text-lg font-semibold">Note</span> :{" "}
							{orderQuery.data?.note}
						</p>
					</div>
					<div className="mt-3 border-gray-700  text-black rounded-md w-full">
						<Button
							className="bg-primary w-full"
							onClick={handleClickRecieveOrder}
						>
							Nhận đơn này
						</Button>
					</div>
				</div>
			</div>
		</>
	);
};

export default OrderIdelDetail;
