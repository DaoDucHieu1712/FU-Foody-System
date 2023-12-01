import { Button, Input, Typography } from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import Cookies from "universal-cookie";
import { cartActions } from "../(auth)/shared/cartSlice";
import ChatService from "../(auth)/shared/chat.service";
import ReportStore from "../(public)/components/ReportStore";
import axios from "../../shared/api/axiosConfig";
import Loading from "../../shared/components/Loading";
import { chatActions } from "../(auth)/shared/chatSlice";

const cookies = new Cookies();
const uId = cookies.get("fu_foody_id");
const backgroundColors = ["bg-gray-50", "bg-gray-200"];

const StoreDetailPage = () => {
	const { id } = useParams();
	const [storeData, setStoreData] = useState();
	const [foodList, setFoodList] = useState([]);
	const [comboList, setComboList] = useState([]);
	const [searchFood, setSearchFood] = useState("");
	const [loading, setLoading] = useState(false);

	const dispatch = useDispatch();

	const handleAddToCart = (cartItem) => {
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
	};

	const GetStoreInformation = async () => {
		try {
			axios
				.get(`/api/Store/DetailStore/${id}`)
				.then((response) => {
					setStoreData(response);
					setFoodList(response.foods);
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
	}, [id]);

	const handleChangeCategory = (idCategory) => {
		console.log(idCategory);
		try {
			axios
				.get(`/api/Store/GetFoodByCategory/${id}/${idCategory}`)
				.then((response) => {
					setFoodList(response);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	const handleSearchFood = (e) => {
		var serachTxt = e.target.value;
		if (typeof serachTxt == "undefined") {
			serachTxt = "";
		}
		setSearchFood(serachTxt);
		try {
			axios
				.get(`/api/Store/GetFoodByName?name=${serachTxt}`)
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
		dispatch(chatActions.Update(true));
		await ChatService.CreateChatBox({
			fromUserId: cookies.get("fu_foody_id"),
			toUserId: storeData.userId,
		});
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
								className="w-full h-64 object-fill object-center"
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
								<i className="fas fa-map mr-1"></i>
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
									<i className="fal fa-clock mr-2"></i>Hoạt động từ:
								</span>
								<span>
									{storeData.timeStart.slice(11, 16)} :{" "}
									{storeData.timeEnd.slice(11, 16)}
								</span>
							</div>
							<div className="">
								<Button className="bg-primary" onClick={handleCreateBoxChat}>
									Chat Ngay
								</Button>
							</div>
						</div>
					</div>
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
										label="Tìm món"
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
										onChange={handleSearchFood}
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
													<div className="col-span-2">
														<img
															className="w-full h-52 rounded-lg object-cover"
															src={item.imageURL}
														/>
													</div>
													<div className="col-span-3">
														<Typography variant="h6">
															{item.foodName}
														</Typography>
														<Typography variant="paragraph" className="py-2">
															{item.description}
														</Typography>
													</div>
													<div className="col-span-1">
														<Typography
															variant="paragraph"
															className="relative w-fit"
														>
															{item.price}.000
															<span className="absolute font-normal top-0 -right-2 text-xs">
																đ
															</span>
														</Typography>
														<Button
															size="sm"
															className="bg-primary"
															onClick={() => handleAddToCart(item)}
														>
															Add to cart
														</Button>
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
															className="relative w-fit"
														>
															Giảm giá {item.combo.percent}%
														</Typography>
														<Button
															size="sm"
															className="bg-primary"
															// onClick={() => handleAddToCartCombo(item.detail)}
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
				</div>
			)}
		</>
	);
};

export default StoreDetailPage;
