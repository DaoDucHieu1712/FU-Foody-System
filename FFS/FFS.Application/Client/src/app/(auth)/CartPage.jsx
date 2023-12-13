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
import { Navigate } from "react-router-dom";
import { toast } from "react-toastify";
import Cookies from "universal-cookie";
import axiosConfig from "../../shared/api/axiosConfig";
import CartService from "./shared/cart.service";
import { cartActions } from "./shared/cartSlice";
import { checkoutActions } from "./shared/checkoutSlice";
import { comboActions } from "./shared/comboSlice";
import CartItem from "./shared/components/cart/CartItem";
import ComboItem from "./shared/components/cart/ComboItem";
import LocationService from "./shared/location.service";
import FormatPriceHelper from "../../shared/components/format/FormatPriceHelper";

const TABLE_HEAD = ["SẢN PHẨM", "ĐƠN GIÁ", "SỐ LƯỢNG", "THÀNH TIỀN"];

const cookies = new Cookies();

const CartPage = () => {
	const accesstoken = useSelector((state) => state.auth.accessToken);
	const comboSelector = useSelector((state) => state.combo);
	const cart = useSelector((state) => state.cart);
	const dispatch = useDispatch();
	const [locations, setLocations] = useState([]);
	const [location, setLocation] = useState("");
	const [phone, setPhone] = useState("");
	const [note, setNote] = useState("");
	const [feeShip, setFeeShip] = useState();

	const getLocations = async () => {
		const email = cookies.get("fu_foody_email");
		await LocationService.getAll(email).then((res) => {
			setLocations(res);
		});
	};

	useEffect(() => {
		dispatch(cartActions.getCartTotal());
		dispatch(comboActions.getCartTotal());
		getLocations();
		var items = cart.list.map((item) => {
			return Number(item.storeId);
		});
		console.log(items);
	}, [cart, comboSelector]);

	const handleSelectLocation = async (location) => {
		setPhone(location.phoneNumber);
		setNote(location.description);
		setLocation(
			`${location.address}, ${location.wardName}, ${location.districtName}, ${location.provinceName}`
		);
		const items = cart.list.map(({ foodName, quantity }) => ({
			name: foodName,
			quantity: quantity,
		}));

		var storeId = cart.list[0].storeId;
		console.log(cart.list[0]);
		await getLocationByUserId(storeId)
			.then(async (res) => {
				console.log(res);
				await CartService.CalcFeeShip(
					res.districtID,
					location.districtID,
					location.wardCode,
					items
				)
					.then((res) => {
						console.log(res.data.data.total);
						setFeeShip(res.data.data.total);
						dispatch(
							cartActions.updateTotalPrice(
								cart.totalPrice + res.data.data.total
							)
						);
						// setTotalPrice(cart.totalPrice + res.data.data.total);
					})
					.catch((err) => {
						toast.error(err.data);
					});
			})
			.catch((err) => {
				toast.error(err.data);
			});
	};

	const getLocationByUserId = (id) => {
		return axiosConfig.get("/api/Location/GetLocation/" + id);
	};

	if (!accesstoken) {
		return <Navigate to="/login" replace={true} />;
	}

	const handleCheckOut = async () => {
		dispatch(checkoutActions.SetInfo({ location, note, phone, feeShip }));
		window.location.href = "/checkout";
	};

	return (
		<>
			<div className="grid grid-cols-3 my-10 gap-x-3">
				<div className="col-span-2 border border-borderpri rounded-lg">
					<div className="heading">
						<h1 className="font-medium uppercase text-xl p-3">Giỏ Hàng</h1>
					</div>
					{cart?.list?.length !== 0 || comboSelector?.list?.length !== 0 ? (
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
										{comboSelector.list.map((item) => (
											<ComboItem key={item.id} item={item} />
										))}
									</tbody>
								</table>
							</Card>
						</div>
					) : (
						<span className="p-3 text-lg font-medium text-gray-700">
							Chưa có gì trong giỏ của bạn á !!
						</span>
					)}
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
								defaultValue={phone}
								disabled
							></Input>
							<Select
								label="Địa chỉ"
								onChange={(item) => handleSelectLocation(item)}
							>
								{locations.map((item) => {
									return (
										<Option key={item.id} value={item}>
											{`${item.address}, ${item.wardName}, ${item.districtName}, ${item.provinceName}`}
										</Option>
									);
								})}
							</Select>
							<Textarea
								label="Ghi chú"
								className="h-[185px]"
								onChange={(e) => setNote(e.target.value)}
								defaultValue={note}
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
								<span>
									{FormatPriceHelper(
										cart.totalPrice + comboSelector.totalPrice
									)}{" "}
									đ
								</span>
							</div>
							<div className="flex justify-between">
								<p className="font-medium text-lg text-gray-500">Phí ship</p>
								{feeShip ? (
									<span>{FormatPriceHelper(feeShip)} đ</span>
								) : (
									<span>Chưa có thông tin</span>
								)}
							</div>
						</div>
						<div className="p-3 flex justify-between">
							<p className="font-medium text-lg ">Tổng</p>
							<span>
								{feeShip
									? FormatPriceHelper(
											cart.totalPrice + comboSelector.totalPrice + feeShip
									  )
									: FormatPriceHelper(
											cart.totalPrice + comboSelector.totalPrice
									  )}
								đ
							</span>
						</div>
						<div className="p-3 w-full">
							{/* <Link to={`/checkout/${location}/${phone}/${note}/${feeShip}`}> */}
							<Button
								disabled={
									location.length === 0 ||
									(cart.list.length === 0 && comboSelector.list.length === 0)
								}
								className="bg-primary w-full"
								onClick={handleCheckOut}
							>
								Thanh toán
							</Button>
							{/* </Link> */}
						</div>
					</div>
				</div>
			</div>
		</>
	);
};

export default CartPage;
