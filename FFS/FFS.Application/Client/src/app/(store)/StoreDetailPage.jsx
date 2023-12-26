import {
	Button,
	Input,
	Typography,
	Tooltip,
	IconButton,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import Cookies from "universal-cookie";
import { cartActions } from "../(auth)/shared/cartSlice";
import ChatService from "../(auth)/shared/chat.service";
import { chatActions } from "../(auth)/shared/chatSlice";
import ReportStore from "../(public)/components/ReportStore";
import axios from "../../shared/api/axiosConfig";
import Loading from "../../shared/components/Loading";
import { comboActions } from "../(auth)/shared/comboSlice";
import FlashSaleByStore from "../(public)/FlashSaleByStore";
import AddToWishlist from "../(public)/components/wishlist/AddToWishlist";
import { useNavigate } from "react-router-dom";
import FormatPriceHelper from "../../shared/components/format/FormatPriceHelper";
import DiscountProfileStore from "./components/Discount/DiscountProfileStore";

const cookies = new Cookies();
const uId = cookies.get("fu_foody_id");
const backgroundColors = ["bg-gray-50", "bg-gray-200"];

const StoreDetailPage = () => {
	const navigate = useNavigate();
	const { id } = useParams();
	const [storeData, setStoreData] = useState();
	const [foodList, setFoodList] = useState([]);
	const [comboList, setComboList] = useState([]);
	const [searchFood, setSearchFood] = useState("");
	const [loading, setLoading] = useState(false);
	const [discountList, setDiscountList] = useState([]);

	const dispatch = useDispatch();

	const handleAddToCart = (cartItem) => {
		if (!cookies.get("fu_foody_token")) {
			window.location.href = "/login";
		} else {
			console.log(cartItem);
			console.log("ok");
			const item = {
				foodId: cartItem.id,
				foodName: cartItem.foodName,
				storeId: id,
				img: cartItem.imageURL,
				price: cartItem.price,
				quantity: 1,
			};
			dispatch(cartActions.addToCart(item));
		}
	};

	const GetStoreInformation = async () => {
		try {
			axios
				.get(`/api/Store/DetailStore/${id}`)
				.then((response) => {
					setStoreData(response);
					setDiscountList(response.discounts);
				})
				.catch((error) => {
					console.log(error);
				})
				.finally(() => {
					setLoading(true);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	useEffect(() => {
		GetStoreInformation();
		handleSearchFood();
	}, [id]);

	const handleChangeCategory = (idCategory) => {
		console.log(idCategory);
		try {
			axios
				.get(`/api/Store/GetFoodByCategory/${id}/${idCategory}`)
				.then((response) => {
					setFoodList(response);
					setComboList([]);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	const handleSearchFood = (e) => {
		var serachTxt = e?.target.value;
		if (typeof serachTxt == "undefined") {
			serachTxt = "";
		}
		setSearchFood(serachTxt);
		try {
			axios
				.get(`/api/Food/GetFoodByStoreId/${id}`)
				.then((response) => {
					setFoodList(response);
					setComboList([]);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	const handleSearchCombo = () => {
		try {
			axios
				.get(`/api/Food/GetListCombo/${id}`)
				.then((response) => {
					console.log(response);
					setComboList(response);
					setFoodList([]);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};
	const handleViewComment = () => {
		window.location = `/store/comment/${id}`;
	};

	const handleCreateBoxChat = async () => {
		if (cookies.get("fu_foody_id") === storeData.userId) {
			return;
		}
		dispatch(chatActions.Update(true));
		await ChatService.CreateChatBox({
			fromUserId: cookies.get("fu_foody_id"),
			toUserId: storeData.userId,
		});
	};

	const handleAddToCartCombo = async (combo) => {
		if (cookies.get("fu_foody_token")) {
			window.location.href = "/login";
		} else {
			dispatch(comboActions.addToCart(combo));
		}
	};

	const getFoodByName = (e) => {
		var serachTxt = e?.target.value;
		if (typeof serachTxt == "undefined") {
			serachTxt = "";
		}
		setSearchFood(serachTxt);
		const data = {
			storeId: id,
			name: serachTxt,
		};
		try {
			axios
				.post(`/api/Store/GetFoodByName`, data)
				.then((response) => {
					console.log(response);
					setComboList([]);
					setFoodList(response);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	const getComboByName = (e) => {
		var serachTxt = e?.target.value;
		if (typeof serachTxt == "undefined") {
			serachTxt = "";
		}
		setSearchFood(serachTxt);
		const data = {
			storeId: id,
			name: serachTxt,
		};
		try {
			axios
				.post(`/api/Store/GetComboByName`, data)
				.then((response) => {
					console.log(response);
					setComboList(response);
					setFoodList([]);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	const handleSearch = (e) => {
		if (foodList.length > 0) {
			getFoodByName(e);
		} else if (comboList.length > 0) {
			getComboByName(e);
		}
	};

	return (
		<>
			{loading == false ? (
				<Loading></Loading>
			) : (
				<div>
					<div className="grid grid-cols-5 gap-10">
						<div className="col-span-2">
							<img
								className="h-64 w-full object-cover object-center"
								src={storeData.avatarURL}
								alt="nature image"
							/>
						</div>
						<div className="col-span-3 flex flex-col gap-1">
							<div className="flex items-center space-x-10">
								<span className="text-base">Quán ăn</span>

								<ReportStore uId={uId} sId={storeData.userId} />
							</div>
							<Typography variant="h2" className="">
								{storeData.storeName}
							</Typography>
							<span className="text-base">
								<i
									className="fas fa-map-marker-alt mr-1"
									aria-hidden="true"
								></i>
								{storeData.address}
							</span>
							<span className="text-base">
								<i className="fal fa-phone mr-2"></i>
								Liên hệ : {storeData.phoneNumber}
							</span>
							<span
								className="text-base text-primary cursor-pointer"
								onClick={handleViewComment}
							>
								Xem thêm lượt đánh giá
							</span>

							<div className="flex gap-1 items-center text-base">
								<span>
									{" "}
									<i className="fal fa-clock mr-2"></i>Thời gian hoạt động:
								</span>
								<span>
									{storeData.timeStart} - {storeData.timeEnd}
								</span>
							</div>
							{uId !== null && uId !== undefined ? (
								<div className="">
									<Button className="bg-primary" onClick={handleCreateBoxChat}>
										Chat Ngay
									</Button>
								</div>
							) : null}
						</div>
					</div>
					<hr className="h-px my-4 bg-gray-400 border-0 dark:bg-gray-700"></hr>
					<DiscountProfileStore
						discountList={discountList}
					></DiscountProfileStore>
					<hr className="h-px my-4 bg-gray-400 border-0 dark:bg-gray-700"></hr>
					<div className="grid grid-cols-6">
						<div className="col-span-1">
							<Typography
								variant="h6"
								color="red"
								className="text-center cursor-pointer"
								onClick={handleSearchFood}
							>
								THỰC ĐƠN ({storeData.categories.length})
							</Typography>
							<ul className="m-2 bg-gray-100 rounded">
								{storeData.categories.map((item) => (
									<li key={item.id}>
										<Typography
											className="p-1 text-center font-semibold cursor-pointer hover:bg-primary hover:text-white"
											onClick={() => handleChangeCategory(item.id)}
										>
											{item.categoryName}
										</Typography>
									</li>
								))}
							</ul>

							<Typography
								variant="h6"
								color="red"
								className="text-center cursor-pointer"
								onClick={handleSearchCombo}
							>
								COMBO ({storeData.combos.length})
							</Typography>
						</div>

						<div className="col-span-5">
							<div>
								<br></br>
							</div>
							<div className="border-solid border-l-[1px] border-gray-400">
								<div className="p-3">
									<Input
										label={comboList.length > 0 ? "Tìm combo" : "Tìm món"}
										icon={
											<svg
												width="20"
												height="20"
												viewBox="0 0 20 20"
												fill="none"
												xmlns="http://www.w3.org/2000/svg"
											>
												<path
													d="M9.0625 15.625C12.6869 15.625 15.625 12.6869 15.625 9.0625C15.625 5.43813 12.6869 2.5 9.0625 2.5C5.43813 2.5 2.5 5.43813 2.5 9.0625C2.5 12.6869 5.43813 15.625 9.0625 15.625Z"
													stroke="#191C1F"
													strokeWidth="1.5"
													strokeLinecap="round"
													strokeLinejoin="round"
												/>
												<path
													d="M13.7031 13.7031L17.5 17.5"
													stroke="#191C1F"
													strokeWidth="1.5"
													strokeLinecap="round"
													strokeLinejoin="round"
												/>
											</svg>
										}
										disabled={foodList.length < 0 ? true : false}
										defaultValue={searchFood}
										onChange={handleSearch}
									/>
								</div>
								<div className="border-solid border-t-[1px] border-gray-400">
									<ul>
										{foodList.map((item, index) => (
											<li
												className={`p-2 ${
													backgroundColors[index % backgroundColors.length]
												}`}
												key={item.id}
											>
												<div className="border-collapse grid grid-cols-6 gap-5">
													<div className="group relative col-span-2 flex lg:flex-none">
														<img
															className="w-full h-52 rounded-lg object-cover group-hover:opacity-40"
															src={item.imageURL}
														/>
														<div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
															<AddToWishlist foodId={item.id} />
															<Tooltip content="Xem chi tiết món ăn">
																<IconButton
																	variant="text"
																	className="bg-white rounded-full"
																	onClick={() =>
																		navigate(`/food-details/${item.id}`)
																	}
																>
																	<svg
																		xmlns="http://www.w3.org/2000/svg"
																		width="30"
																		height="20"
																		viewBox="0 0 550 512"
																	>
																		<path d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z" />
																	</svg>
																</IconButton>
															</Tooltip>
														</div>
													</div>
													<div className="col-span-3">
														<Typography variant="h6">
															{item.foodName}
														</Typography>
														<Typography variant="paragraph" className="py-2">
															{item.description}
														</Typography>

														{item.inventories.length > 0 &&
														item.inventories[0]?.quantity != 0 ? (
															<Typography
																variant="paragraph"
																className="py-2 text-green-500"
															>
																còn hàng ({item.inventories[0]?.quantity})
															</Typography>
														) : (
															<Typography
																variant="paragraph"
																className="py-2 text-red-500"
															>
																{/* {item.inventories[0]?.quantity} */}
																sản phẩm tạm hết hàng
															</Typography>
														)}
													</div>
													<div className="col-span-1">
														<Typography
															variant="paragraph"
															className="relative w-fit"
														>
															{FormatPriceHelper(item.price)}
															<span className="absolute font-normal top-0 -right-2 text-xs">
																đ
															</span>
														</Typography>
														{item.inventories.length > 0 &&
														item.inventories[0]?.quantity != 0 ? (
															<Button
																size="sm"
																className="bg-primary"
																onClick={() => handleAddToCart(item)}
															>
																Add to cart
															</Button>
														) : (
															<></>
														)}
													</div>
												</div>
											</li>
										))}
									</ul>
									<ul>
										{comboList.map((item, index) => (
											<li
												className={`p-2 ${
													backgroundColors[index % backgroundColors.length]
												}`}
												key={item.combo.id}
											>
												<div className="border-collapse grid grid-cols-6 gap-5">
													<div className="col-span-2">
														<img
															className="w-full h-52 rounded-lg object-cover"
															src={item.combo.image}
														/>
													</div>
													<div className="col-span-3">
														<Typography variant="h6">
															{item.combo.name}
														</Typography>
														<Typography variant="h6">Combo gồm</Typography>
														<ul className="py-2">
															{item.detail.map((detail) => (
																<li key={detail.Id}>
																	<span>{detail.FoodName}</span>
																</li>
															))}
														</ul>
													</div>
													<div className="col-span-1">
														<Typography
															variant="paragraph"
															className="relative w-fit line-through"
														>
															{item.detail.reduce(
																(accum, item) => accum + item.Price,
																0
															)}{" "}
															đ
														</Typography>
														<Typography
															variant="paragraph"
															className="relative w-fit text-red-500"
														>
															{item.detail.reduce(
																(accum, item) =>
																	accum + item.PriceAfterDiscount,
																0
															)}{" "}
															đ
														</Typography>
														<Button
															size="sm"
															className="bg-primary"
															onClick={() =>
																handleAddToCartCombo({
																	id: item.combo.id,
																	storeId: item.combo.storeId,
																	name: item.combo.name,
																	image: item.combo.image,
																	price: item.detail.reduce(
																		(accum, item) =>
																			accum + item.PriceAfterDiscount,
																		0
																	),
																	quantity: 1,
																})
															}
														>
															Add to cart
														</Button>
													</div>
												</div>
											</li>
										))}
									</ul>
								</div>
							</div>
						</div>
					</div>
					<div>
						<FlashSaleByStore storeId={id}></FlashSaleByStore>
					</div>
				</div>
			)}
		</>
	);
};

export default StoreDetailPage;
