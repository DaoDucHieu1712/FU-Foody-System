import {
	Button,
	Card,
	CardBody,
	CardFooter,
	CardHeader,
	Typography,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import PaymentStatus from "../(store)/components/order/PaymentStatus";
import OrderService from "../(store)/shared/order.service";

const TABLE_HEAD = [
	"Id",
	"Khách hàng",
	"Số điện thoại",
	"Thời gian đặt",
	"Note",
	"Tổng tiền",
	"",
];

const TABLE_ROWS = [
	{
		img: "https://demos.creative-tim.com/test/corporate-ui-dashboard/assets/img/team-3.jpg",
		name: "John Michael",
		email: "john@creative-tim.com",
		job: "Manager",
		org: "Organization",
		online: true,
		date: "23/04/18",
	},
	{
		img: "https://demos.creative-tim.com/test/corporate-ui-dashboard/assets/img/team-2.jpg",
		name: "Alexa Liras",
		email: "alexa@creative-tim.com",
		job: "Programator",
		org: "Developer",
		online: false,
		date: "23/04/18",
	},
	{
		img: "https://demos.creative-tim.com/test/corporate-ui-dashboard/assets/img/team-1.jpg",
		name: "Laurent Perrier",
		email: "laurent@creative-tim.com",
		job: "Executive",
		org: "Projects",
		online: false,
		date: "19/09/17",
	},
	{
		img: "https://demos.creative-tim.com/test/corporate-ui-dashboard/assets/img/team-4.jpg",
		name: "Michael Levi",
		email: "michael@creative-tim.com",
		job: "Programator",
		org: "Developer",
		online: true,
		date: "24/12/08",
	},
	{
		img: "https://demos.creative-tim.com/test/corporate-ui-dashboard/assets/img/team-5.jpg",
		name: "Richard Gran",
		email: "richard@creative-tim.com",
		job: "Manager",
		org: "Executive",
		online: false,
		date: "04/10/21",
	},
];

const OrderAvailablePage = () => {
	const userProfile = useSelector((state) => state.auth.userProfile);
	const navigate = useNavigate();

	const [orders, setOrders] = useState([]);
	const [page, setPage] = useState(1);
	const [IsReceiver, setIsReceiver] = useState();

	const getOrder = async () => {
		await OrderService.GetOrderIdel(page).then((res) => {
			console.log(res);
			setOrders(res);
		});
	};

	useEffect(() => {
		checkReceiverHandler();
		getOrder();
	}, [page]);

	const checkReceiverHandler = async () => {
		await OrderService.CheckReceiverOrder(userProfile.id).then((res) => {
			setIsReceiver(res);
		});
	};

	if (IsReceiver == false) {
		navigate("/shipper/order-pending");
	}

	// const handleClickRecieveOrder = async (id) => {
	// 	await axios
	// 		.put(`/api/Order/ReceiveOrderUnbook/${userProfile.id}/${id}`)
	// 		.then((res) => {
	// 			getOrder();
	// 			toast.success(res);
	// 		})
	// 		.catch((err) => {
	// 			toast.error(err.response.data);
	// 		});
	// };
	return (
		<>
			<Card className="h-full w-full">
				<CardHeader floated={false} shadow={false} className="rounded-none">
					<div className="mb-8 flex items-center justify-between gap-8">
						<div>
							<Typography
								variant="h5"
								color="blue-gray"
								className="uppercase font-medium text-primary"
							>
								Các đơn hàng mới nhất
							</Typography>
							<Typography color="gray" className="mt-1 text-sm font-normal">
								Thông tin đơn hàng chưa được nhận..
							</Typography>
						</div>
					</div>
				</CardHeader>
				<CardBody className="px-0">
					<table className="mt-4 w-full min-w-max table-auto text-left">
						<thead>
							<tr>
								{TABLE_HEAD.map((head) => (
									<th
										key={head}
										className="cursor-pointer border-y border-blue-gray-100 bg-blue-gray-50/50 p-4 transition-colors hover:bg-blue-gray-50"
									>
										<Typography
											variant="small"
											color="blue-gray"
											className="flex items-center justify-between gap-2 font-normal leading-none opacity-70"
										>
											{head}
										</Typography>
									</th>
								))}
							</tr>
						</thead>
						<tbody>
							{orders.list?.map((item, index) => {
								const isLast = index === TABLE_ROWS.length - 1;
								const classes = isLast
									? "p-4"
									: "p-4 border-b border-blue-gray-50";

								return (
									<tr key={item.id}>
										<td className={classes}>
											<span>#{item.id}</span>
										</td>
										<td className={classes}>{item.customerName}</td>
										<td className={classes}>{item.phoneNumber}</td>
										<td className={classes}>
											{item.createdAt.replace("T", " ").slice(0, 16)}
										</td>
										<td className={classes}>
											<Typography
												variant="small"
												color="blue-gray"
												className="font-normal w-[100px] whitespace-nowrap text-ellipsis"
											>
												{item.note}
											</Typography>
										</td>
										<td className={classes}>
											<p>{item.totalPrice} $</p>
											<p>{item.paymentMethod}</p>
											<PaymentStatus status={item.paymentStatus} />
										</td>
										<td className={`${classes}`}>
											<div className="flex gap-x-3 text-blue-500">
												<Link to={`/shipper/order-available/${item.id}`}>
													<Button size="sm" className="bg-primary">
														xem chi tiết
													</Button>
												</Link>
											</div>
										</td>
									</tr>
								);
							})}
						</tbody>
					</table>
				</CardBody>
				<CardFooter className="flex items-center justify-between border-t border-blue-gray-50 p-4">
					<Typography variant="small" color="blue-gray" className="font-normal">
						Page {orders.pageIndex} of {orders?.totalPages}
					</Typography>
					<div className="flex gap-2">
						<Button
							variant="outlined"
							size="sm"
							disabled={!orders?.hasPrevious}
							onClick={() => setPage(page - 1)}
						>
							Previous
						</Button>
						<Button
							variant="outlined"
							size="sm"
							disabled={!orders?.hasNext}
							onClick={() => setPage(page + 1)}
						>
							Next
						</Button>
					</div>
				</CardFooter>
			</Card>
		</>
	);
};

export default OrderAvailablePage;
