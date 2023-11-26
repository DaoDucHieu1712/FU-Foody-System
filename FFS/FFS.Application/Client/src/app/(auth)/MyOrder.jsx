import { useQuery } from "@tanstack/react-query";
import CookieService from "../../shared/helper/cookieConfig";
import OrderService from "../(store)/shared/order.service";
import { useState } from "react";
import {
	Button,
	IconButton,
	Input,
	Option,
	Select,
	Typography,
} from "@material-tailwind/react";
import OrderStatus from "../(store)/components/order/OrderStatus";
import { Link } from "react-router-dom";

const TABLE_HEAD = [
	"Mã Đơn hàng",
	"Khách hàng",
	"Thời gian",
	"Địa điểm",
	"Shipper",
	"Trạng thái",
	"Tổng tiền",
	"Chi tiết",
];

const MyOrder = () => {
	const [page, setPage] = useState(1);
	const [startDate, setStartDate] = useState("");
	const [endDate, setEndDate] = useState("");

	const [sortType, setSortType] = useState("");
	const [status, setStatus] = useState("");
	const [orderId, setOrderId] = useState("");
	const ordersQuery = useQuery({
		queryKey: ["my-order", page, startDate, endDate, status, sortType, orderId],
		queryFn: async (context) => {
			var id = await CookieService.getToken("fu_foody_id");
			return OrderService.FindByUser(id, context);
		},
	});

	return (
		<>
			<div className="container mx-auto">
				<div className="flex justify-between p-2">
					<div className="">
						<h1 className="text-2xl text-orange-400 font-bold mb-3">
							Đơn hàng của tôi !!
						</h1>
					</div>
				</div>
				<div className="mt-5">
					<div className="flex items-center justify-end gap-x-3">
						<div className="form-group">
							<Input
								color="blue"
								label="Mã đơn hàng"
								type="text"
								onChange={(e) => setOrderId(e.target.value)}
							/>
						</div>
						<div className="form-group">
							<Input
								color="blue"
								label="Ngày đặt bắt đầu"
								type="date"
								onChange={(e) => setStartDate(e.target.value)}
							/>
						</div>
						<div className="form-group">
							<Input
								color="blue"
								label="Ngày đặt kết thúc"
								type="date"
								onChange={(e) => setEndDate(e.target.value)}
							/>
						</div>

						<div className="form-group">
							<Select
								label="Trạng thái"
								color="blue"
								onChange={(e) => setStatus(e)}
							>
								<Option value="">Tất cả</Option>
								<Option value="1">Đang chờ</Option>
								<Option value="2">Đang giao</Option>
								<Option value="3">Đã hủy</Option>
								<Option value="4">Đã nhận hàng</Option>
							</Select>
						</div>

						<div className="form-group">
							<Select
								label="Sắp xếp"
								color="blue"
								onChange={(e) => setSortType(e)}
							>
								<Option value="">Mặc định</Option>
								<Option value="date-asc">Ngày giao - tăng dần</Option>
								<Option value="date-desc">Ngày giao - giảm dần</Option>
								<Option value="price-asc">Giá - tăng dần</Option>
								<Option value="price-desc">Giá - giảm dần</Option>
							</Select>
						</div>
					</div>
				</div>
				<div className="mt-12">
					<table className="w-full min-w-max table-auto text-left">
						<thead>
							<tr>
								{TABLE_HEAD.map((head) => (
									<th
										key={head}
										className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4"
									>
										<Typography
											variant="small"
											color="blue-gray"
											className="font-normal leading-none opacity-70"
										>
											{head}
										</Typography>
									</th>
								))}
							</tr>
						</thead>
						<tbody>
							{ordersQuery.data?.list?.map((item, index) => {
								const isLast = index === ordersQuery.data?.list?.length - 1;
								const classes = isLast
									? "p-4 text-sm"
									: "p-4 border-b border-blue-gray-50 text-sm";
								return (
									<tr key={item.id}>
										<td className={classes}>#{item.id}</td>
										<td className={classes}>{item.customerName}</td>
										<td className={classes}>
											<div>
												<span>Thời gian đặt : </span>
												<span>
													{item.createdAt
														.toString()
														.replace("T", " ")
														.slice(0, 19)}
												</span>
											</div>
											<div>
												<span>Thời gian giao : </span>
												<span>
													{item.shipDate
														?.toString()
														.replace("T", " ")
														.slice(0, 19)}
												</span>
											</div>
										</td>
										<td className={classes}>{item.location}</td>
										<td className={classes}>
											{item.shipperName && (
												<>
													<span>{item.shipperName}</span>
													<Link
														href="link danh gia"
														className="px-6 py-2 text-light-blue-500 font-medium rounded-lg cursor-pointer"
													>
														đánh giá
													</Link>
												</>
											)}
										</td>
										<td className={classes}>
											<OrderStatus status={item.orderStatus}></OrderStatus>
										</td>
										<td className={classes}>{item.totalPrice} $</td>
										<td className={classes}>
											<div className="flex gap-x-3">
												<Link
													to={`/my-order/${item.id}`}
													className="px-6 py-2 text-light-blue-500 font-medium rounded-lg cursor-pointer"
												>
													chi tiết đơn hàng
												</Link>
											</div>
										</td>
									</tr>
								);
							})}
						</tbody>
					</table>
					<div className="flex items-center justify-between border-t border-blue-gray-50 p-4">
						<Button
							variant="outlined"
							color="blue-gray"
							size="sm"
							disabled={page === 1}
							onClick={() => setPage(page - 1)}
						>
							Previous
						</Button>
						<div className="flex items-center gap-2">
							{(() => {
								let rows = [];
								for (let i = 1; i <= ordersQuery.data?.total; i++) {
									rows.push(
										<IconButton
											key={i}
											variant="outlined"
											color="blue-gray"
											size="sm"
											className={
												page === i ? "bg-blue-gray-500 text-white" : ""
											}
											onClick={() => setPage(i)}
										>
											{i}
										</IconButton>
									);
								}
								return rows;
							})()}
						</div>
						<Button
							variant="outlined"
							color="blue-gray"
							size="sm"
							disabled={page >= ordersQuery.data?.total}
							onClick={() => setPage(page + 1)}
						>
							Next
						</Button>
					</div>
				</div>
			</div>
		</>
	);
};

export default MyOrder;
