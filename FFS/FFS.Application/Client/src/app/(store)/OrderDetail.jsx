import { useParams } from "react-router-dom";
import OrderService from "./shared/order.service";
import { useQuery } from "@tanstack/react-query";
import { Spinner } from "@material-tailwind/react";
import OrderItem from "./components/order/OrderItem";
import OrderStatus from "./components/order/OrderStatus";
import { Link } from "react-router-dom";

const OrderDetail = () => {
	const { id } = useParams();

	const orderQuery = useQuery({
		queryKey: ["order-detail"],
		queryFn: async () => {
			return await OrderService.GetOrderDetail(id);
		},
	});

	return (
		<>
			<div className="container mx-auto grid grid-cols-3 gap-x-12">
				<div className="col-span-2">
					<h1 className="font-bold text-2xl mb-3">Chi tiết hóa đơn</h1>
					<p>
						Có {orderQuery.data?.orderDetails.length} sản phẩm trong đơn hàng #
						{orderQuery.data?.id}
					</p>
					<>
						<div className="flex flex-col gap-y-5 cart mt-8 border-1 rounded-md border-gray-400">
							{orderQuery.isLoading ? (
								<Spinner />
							) : (
								orderQuery.data?.orderDetails.map((item) => {
									return <OrderItem key={item.id} item={item}></OrderItem>;
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
							{/* {orderQuery.data?.shipperName} */}
							<Link
								to={`/shipper/details/${orderQuery.data?.shipperId}`}
								className="px-6 py-2 text-blue-500 font-bold text-lg cursor-pointer"
							>
								{orderQuery.data?.shipperName}
							</Link>
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
							{orderQuery.data?.createdAt.toString().slice(0, 10)} $
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
					<ul className="info">
						<div className="flex justify-between gap-y-2 flex-col border-b pb-3 border-gray-200">
							<p className="font-bold text-lg">Phí ship :</p>
							<p className="font-medium text-sm">{orderQuery.data?.shipFee}</p>
						</div>
					</ul>
					<div className="mt-3 border-gray-700  text-black rounded-md">
						<p>{orderQuery.data?.note}</p>
					</div>
				</div>
			</div>
		</>
	);
};

export default OrderDetail;
