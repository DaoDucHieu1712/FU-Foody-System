import {
	Button,
	Card,
	Input,
	Option,
	Select,
	Textarea,
	Typography,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { toast } from "react-toastify";
import Cookies from "universal-cookie";
import CartService from "./shared/cart.service";
import { cartActions } from "./shared/cartSlice";
import CartItem from "./shared/components/cart/CartItem";
import LocationService from "./shared/location.service";
import { useNavigate } from "react-router-dom";
import CookieService from "../../shared/helper/cookieConfig";

const TABLE_HEAD = ["SẢN PHẨM", "ĐƠN GIÁ", "SỐ LƯỢNG", "THÀNH TIỀN"];

const cookies = new Cookies();

const CartPage = () => {
	const cart = useSelector((state) => state.cart);
	const navigate = useNavigate();
	const dispatch = useDispatch();
	const [locations, setLocations] = useState([]);
	const [location, setLocation] = useState("");
	const [phone, setPhone] = useState("");
	const [note, setNote] = useState("");
	const [code, setCode] = useState("");

	const getLocations = async () => {
		const email = cookies.get("fu_foody_email");
		await LocationService.getAll(email).then((res) => {
			setLocations(res);
			console.log(res);
		});
	};

	useEffect(() => {
		dispatch(cartActions.getCartTotal());
		getLocations();
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
			cart.totalPrice,
			storeIds
		)
			.then((res) => {
				console.log(res);
				toast.success(`Dùng mã thành công !!`);
				dispatch(cartActions.useDiscount(res));
			})
			.catch((err) => {
				toast.error(err.response.data);
			});
	};

	const CheckoutHandler = async () => {
		await CartService.CreateOrder({
			customerId: cookies.get("fu_foody_id"),
			location: location,
			phoneNumber: phone,
			note: note,
			totalPrice: cart.totalPrice,
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
						unitPrice: item.unitPrice,
					};
				});
				await CartService.AddOrderItem(items)
					.then((res) => {
						console.log(res);
						toast.success("Đặt hàng thành công");
						dispatch(cartActions.clearCart());
						navigate("/");
					})
					.catch((err) => {
						console.log(err.response.data);
					});
				await CartService.UseDiscount(
					code,
					CookieService.getToken("fu_foody_id")
				);
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
						<h1 className="font-medium uppercase text-lg p-3">Giỏ Hàng</h1>
					</div>
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
										<CartItem key={item.foodId} item={item}></CartItem>
									))}
								</tbody>
							</table>
						</Card>
					</div>
				</div>
				<div className="flex flex-col gap-y-5">
					<div className="border-borderpri border pb-5 rounded-lg">
						<div className="p-3 border-b border-borderpri">
							<h1 className="font-medium">Thông tin khác</h1>
						</div>
						<div className="p-3 flex flex-col gap-y-3">
							<Input
								label="Số điện thoại"
								type="number"
								onChange={(e) => setPhone(e.target.value)}
							></Input>
							<Select label="Địa chỉ" onChange={(e) => setLocation(e)}>
								{locations.map((item) => {
									return (
										<Option key={item.id} value={item.address}>
											{item.address}
										</Option>
									);
								})}
							</Select>
							<Textarea
								label="Ghi chú"
								className="h-[185px]"
								onChange={(e) => setNote(e.target.value)}
							/>
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
								<span>{cart.totalPrice} đ</span>
							</div>
							<div className="flex justify-between">
								<p className="font-medium text-lg text-gray-500">Phí ship</p>
								<span>Free</span>
							</div>
							<div className="flex justify-between">
								<p className="font-medium text-lg text-gray-500">Giảm giá</p>
								<span>chưa có thông tin</span>
							</div>
						</div>
						<div className="p-3 flex justify-between">
							<p className="font-medium text-lg ">Tổng</p>
							<span>{cart.totalPrice} đ</span>
						</div>
						<div className="p-3 w-full">
							<Button className="bg-primary w-full" onClick={CheckoutHandler}>
								Thanh toán
							</Button>
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

export default CartPage;
