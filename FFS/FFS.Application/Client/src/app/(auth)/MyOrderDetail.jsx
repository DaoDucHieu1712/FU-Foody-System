import { useQuery } from "@tanstack/react-query";
import OrderService from "../(store)/shared/order.service";
import OrderStatus from "../(store)/components/order/OrderStatus";
import {
	Button,
	Dialog,
	DialogBody,
	DialogFooter,
	DialogHeader,
	Spinner,
	Textarea,
} from "@material-tailwind/react";
import OrderItem from "../(store)/components/order/OrderItem";
import { useState } from "react";
import { toast } from "react-toastify";
import { useParams } from "react-router-dom";
import ReviewStore from "../(public)/components/ReviewStore";
import CookieService from "../../shared/helper/cookieConfig";

const MyOrderDetail = () => {
	const [open, setOpen] = useState(false);
	const [reason, setReason] = useState("");
	const handleOpen = () => setOpen(!open);

	const { id } = useParams();

	const orderQuery = useQuery({
		queryKey: ["my-order-detail"],
		queryFn: async () => {
			return await OrderService.GetOrderDetail(id);
		},
	});

	const handleUpdateOrder = () => {
		OrderService.CancelOrderWithCustomer(id, reason).then(() => {
			setOpen(!open);
			toast.success("Hủy đơn hàng thành công !!!");
			location.reload();
		});
	};
	const email = CookieService.getToken("fu_foody_email");
	const role = CookieService.getToken("fu_foody_role");

	return (
		<>
			<div className="container mx-auto grid grid-cols-3 gap-x-12 my-20">
				<div className="col-span-2">
					<h1 className="font-bold text-2xl mb-3">Đơn hàng của bạn</h1>
					<p>
						Bạn có {orderQuery.data?.orderDetails.length} sản phẩm trong đơn
						hàng #{orderQuery.data?.id}
					</p>
					<>
						<div className="m-10 flex flex-col gap-y-5 cart mt-8 border-1 rounded-md border-gray-400">
							{orderQuery.isLoading ? (
								<Spinner />
							) : (
								orderQuery.data?.orderDetails.map((item) => {
									return <OrderItem key={item.id} item={item} />;
								})
							)}
						</div>
					</>
					{role !== "StoreOwner" && orderQuery.data?.orderStatus === 3 ? (
						<ReviewStore
							email={email}
							idStore={orderQuery.data?.orderDetails[0].storeId}
							idShipper={orderQuery.data?.shipperId}
							storeName={orderQuery.data?.orderDetails[0].storeName }
						/>
					) : (
						<></>
					)}
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
						{orderQuery.data?.orderStatus === 4 && (
							<p>
								<span className="text-lg font-semibold">Lý do hủy</span> :{" "}
								{orderQuery.data?.cancelReason}
							</p>
						)}
					</div>
					<div className="mt-3 border-gray-700  text-black rounded-md w-full">
						{orderQuery.data?.orderStatus === 1 && (
							<>
								<Button onClick={handleOpen} className="bg-primary w-full">
									Hủy đơn hàng
								</Button>
								<Dialog open={open} handler={handleOpen}>
									<DialogHeader className="text-primary">
										Hủy đơn hàng
									</DialogHeader>
									<DialogBody>
										<h1 className="mb-3 text-xl uppercase font-semibold">
											Bạn xác nhận hủy đơn hàng #{orderQuery.data?.id}
										</h1>
										<div>
											<Textarea
												label="Lí do hủy đơn hàng"
												className="mt-3"
												value={reason}
												onChange={(e) => setReason(e.target.value)}
											/>
										</div>
									</DialogBody>
									<DialogFooter>
										<Button
											variant="text"
											color="red"
											onClick={handleOpen}
											className="mr-1"
										>
											<span>Hủy</span>
										</Button>
										<Button className="bg-primary" onClick={handleUpdateOrder}>
											<span>Xác nhận</span>
										</Button>
									</DialogFooter>
								</Dialog>
							</>
						)}
					</div>
				</div>
			</div>
		</>
	);
};

export default MyOrderDetail;
