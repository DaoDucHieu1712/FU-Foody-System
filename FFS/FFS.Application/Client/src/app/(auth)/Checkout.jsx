import {
	Button,
	Card,
	Input,
	Radio,
	Typography,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate, useParams } from "react-router-dom";
import { cartActions } from "./shared/cartSlice";
import { toast } from "react-toastify";
import axiosConfig from "../../shared/api/axiosConfig";
import CartService from "./shared/cart.service";
import Cookies from "universal-cookie";
import CookieService from "../../shared/helper/cookieConfig";

const TABLE_HEAD = ["SẢN PHẨM", "ĐƠN GIÁ", "SỐ LƯỢNG", "THÀNH TIỀN"];

var cookies = new Cookies();

const Checkout = () => {
	const cart = useSelector((state) => state.cart);
	const { location, phoneNumber, note, fee, percent } = useParams();
	const dispatch = useDispatch();
	const navigate = useNavigate();
	const [selectedType, setSelectedType] = useState("Thanh toán khi nhận hàng");
	const [code, setCode] = useState("");
	const [totalPrice, setTotalPrice] = useState(cart.totalPrice);
	useEffect(() => {
		dispatch(cartActions.getCartTotal());
		var items = cart.list.map((item) => {
			return Number(item.storeId);
		});
		console.log(items);
	}, [cart]);

	const useDiscountHandler = async () => {
		console.log("use discount");
		var storeIds = cart.list.map((item) => {
			return Number(item.storeId);
		});
		await CartService.CheckDiscount(
			code,
			CookieService.getToken("fu_foody_id"),
			totalPrice,
			storeIds
		)
			.then((res) => {
				console.log(res);
				toast.success(`Dùng mã thành công !!`);
				setTotalPrice(totalPrice - res.discount * totalPrice);
			})
			.catch((err) => {
				toast.error(err.response.data);
			});
	};

	const CheckoutHandler = async () => {
		await CartService.CreateOrder({
			customerId: cookies.get("fu_foody_id"),
			location: location,
			phoneNumber: phoneNumber,
			note: note,
			totalPrice: totalPrice,
			orderStatus: 1,
		})
			.then(async (res) => {
				console.log(res);
				var items = cart.list.map((item) => {
					return {
						orderId: res.id,
						storeId: item.storeId,
						foodId: item.foodId,
						quantity: item.quantity,
						unitPrice: item.price,
					};
				});

				await CartService.AddOrderItem(items)
					.then((res) => {
						console.log(res);
						if (selectedType == "Chuyển khoản") {
							const data = {
								PaymentMethod: selectedType,
								OrderId: items[0].orderId,
								Status: 1,
							};
							axiosConfig
								.post("/api/Order/CreatePayment", data)
								.then((res) => {
									console.log(res);
									toast.success("Đặt hàng thành công");
									dispatch(cartActions.clearCart());
									navigate("/");

									axiosConfig
										.get("/api/Order/GetUrlPayment/" + items[0].orderId)
										.then((res) => {
											window.open(res, "_blank");
										})
										.catch((err) => {
											toast.error(err);
										});
								})
								.catch((err) => {
									toast.error(err);
								});
						} else {
							const data = {
								PaymentMethod: selectedType,
								OrderId: items[0].orderId,
								Status: 2,
							};
							axiosConfig
								.post("/api/Order/CreatePayment", data)
								.then((res) => {
									console.log(res);
									toast.success("Đặt hàng thành công");
									dispatch(cartActions.clearCart());
									navigate("/");
								})
								.catch((err) => {
									toast.error(err);
								});
						}
					})
					.catch((err) => {
						console.log(err.response.data);
					});
			})
			.catch((err) => {
				console.log(err);
				toast.error("Đã có lỗi xảy ra, vui lòng đặt lại !");
			});
	};

	return (
		<>
			<div className="grid grid-cols-3 my-10 gap-x-3">
				<div className="col-span-2 border border-borderpri rounded-lg">
					<div className="heading">
						<h1 className="font-medium uppercase text-xl p-3">Giỏ Hàng</h1>
					</div>
					{cart?.list?.length !== 0 ? (
						<div className="w-full">
							<Card className="h-full w-full rounded-none">
								<table className="w-full min-w-max table-auto text-left">
									<thead>
										<tr>
											{TABLE_HEAD.map((head) => (
												<th
													key={head}
													className="border-b border-blue-gray-100 bg-blue-gray-50 p-4"
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
										{cart?.list?.map((item) => (
											<tr key={item.foodId}>
												<td className="p-4 border-b border-blue-gray-50 flex items-center gap-x-2">
													<img src={item.img} alt="" className="w-[70px]" />
													<span>{item.foodName}</span>
												</td>
												<td className="p-4 border-b border-blue-gray-50">
													{item.price} đ
												</td>
												<td className="p-4 border-b border-blue-gray-50">
													<div className="flex items-center justify-between border p-2">
														<span>x</span>
														<p>{item.quantity}</p>
													</div>
												</td>
												<td className="p-4 border-b border-blue-gray-50">
													{item.quantity * item.price} đ
												</td>
											</tr>
										))}
									</tbody>
								</table>
							</Card>
						</div>
					) : (
						<span className="p-3 text-lg font-medium text-gray-700">
							Lỗi gòi
						</span>
					)}
				</div>
				<div className="flex flex-col gap-y-5">
					<div className="border-borderpri border pb-5 rounded-lg">
						<div className="p-3 border-b border-borderpri">
							<h1 className="font-medium">Thông tin khác</h1>
						</div>
						<div className="p-3 flex flex-col gap-y-3">
							<p>
								<span>Địa chỉ :</span> {location}
							</p>
							<p>
								<span>phone :</span> {phoneNumber}
							</p>
							<p>
								<span>note :</span> {note}
							</p>
						</div>
					</div>
					<div className="border-borderpri border pb-5 rounded-lg">
						<div className="heading">
							<h1 className="text-2xl p-3">Thanh toán giỏ hàng</h1>
						</div>
						<div className="mt-3 p-3 pb-10 border-b border-borderpri">
							<div className="flex justify-between">
								<p className="font-medium text-lg text-gray-500">
									Tổng đơn hàng
								</p>
								<span>{totalPrice} đ</span>
							</div>
							<div className="flex justify-between">
								<p className="font-medium text-lg text-gray-500">Phí ship</p>
								{fee === undefined ? (
									<span>{fee}</span>
								) : (
									<span>Chưa có thông tin</span>
								)}
							</div>
							<div className="flex justify-between">
								<p className="font-medium text-lg text-gray-500">Giảm giá</p>
								{percent === undefined ? (
									<span>{percent} %</span>
								) : (
									<span>Chưa có thông tin</span>
								)}
							</div>
						</div>
						<div className="p-3 flex justify-between">
							<p className="font-medium text-lg ">Tổng</p>
							<span>{totalPrice} đ</span>
						</div>
						<div className="p-3 w-full">
							<div>
								<Radio
									name="type"
									label="Chuyển khoản"
									onChange={() => setSelectedType("Chuyển khoản")}
								/>
								<Radio
									name="type"
									label="Thanh toán khi nhận hàng"
									defaultChecked
									onChange={() => setSelectedType("Thanh toán khi nhận hàng")}
								/>
							</div>
							<div className="flex flex-col gap-y-3">
								<Button className="bg-primary w-full" onClick={CheckoutHandler}>
									Xác Nhận Thanh toán
								</Button>
								<Link to="/cart">
									<Button className="w-full">Quay lại giỏ hàng</Button>
								</Link>
							</div>
						</div>
					</div>

					<div className="border-borderpri border pb-5 rounded-lg">
						<div className="p-3 border-b border-borderpri">
							<h1 className="font-medium">Mã Giảm giá</h1>
						</div>
						<div className="p-3 flex flex-col gap-y-3">
							<Input
								label="Mã giảm giá"
								onChange={(e) => setCode(e.target.value)}
							></Input>
							<Button
								className="bg-blue-500 w-full"
								onClick={useDiscountHandler}
							>
								Sử dụng mã giảm giá
							</Button>
						</div>
					</div>
				</div>
			</div>
		</>
	);
};

export default Checkout;
