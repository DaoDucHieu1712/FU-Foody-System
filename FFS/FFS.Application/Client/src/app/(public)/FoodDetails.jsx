import {
	Button,
	IconButton,
	Rating,
	Spinner,
	Textarea,
	Tooltip,
	Typography,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import AddToWishlist from "./components/wishlist/AddToWishlist";
import WishlistDetails from "./components/wishlist/WishlistDetails";
import moment from "moment";
import CookieService from "../../shared/helper/cookieConfig";
import { useDispatch } from "react-redux";
import { cartActions } from "../(auth)/shared/cartSlice";

const FoodDetails = () => {
	const userId = CookieService.getToken("fu_foody_id");
	const { id } = useParams();
	const dispatch = useDispatch();
	const navigate = useNavigate();
	const [value, setValue] = useState(0);
	const [foodData, setFoodData] = useState(null);
	const [foodList, setFoodList] = useState([]);
	const [categoryId, setCategoryId] = useState(0);
	const [openComment, setOpenComment] = useState(true);
	const [commentTxt, setCommentTxt] = useState("");
	const increaseValue = () => {
		setValue(value + 1);
	};

	const decreaseValue = () => {
		if (value > 0) {
			setValue(value - 1);
		}
	};

	const handleopenComent = () => {
		setOpenComment((cur) => !cur);
	};

	const GetFoodData = async () => {
		try {
			await axios
				.get(`/api/Food/GetFoodById/${id}`)
				.then((response) => {
					setFoodData(response.data.result);
					setCategoryId(response.data.result.categoryId);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred food detail!", error);
		}
	};

	const GetFoodByCategoryId = async () => {
		try {
			await axios
				.get(`/api/Food/GetFoodByCategoryid/${categoryId}`)
				.then((response) => {
					setFoodList(response);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred food detail!", error);
		}
	};

	useEffect(() => {
		GetFoodData();
	}, [id]);

	useEffect(() => {
		GetFoodByCategoryId();
	}, [categoryId]);

	// const handleComment = () => {
	// 	const data = {
	// 		Content: commentTxt,
	// 		UserId: userId,
	// 		FoodId: id,
	// 	};
	// 	try {
	// 		axios
	// 			.post("/api/Food/CommentFood", data)
	// 			.then(() => {
	// 				setCommentTxt("");
	// 				setOpenComment(true);
	// 				GetFoodData();
	// 			})
	// 			.catch((error) => {
	// 				console.log(error);
	// 			});
	// 	} catch (error) {
	// 		console.error("Error occur: ", error);
	// 	}

	// 	console.log(commentTxt);
	// };

	return (
		<>
			{foodData ? (
				<div className="container mt-8 mb-8 px-12 py-4">
					<Typography variant="h4" className="pb-3">
						Thông tin món ăn
					</Typography>
					<div className="grid grid-cols-[4fr,6fr] gap-12">
						<div className="Sidebar">
							<img
								className="h-72 w-full object-fill object-center"
								src={foodData.imageURL}
								alt={foodData.foodName}
							/>
						</div>
						<div className="content-food">
							<div className="flex gap-2 items-center">
								<Typography className="font-semibold">Cửa hàng: </Typography>
								<Typography
									color="orange"
									variant="h5"
									className="cursor-pointer"
									onClick={() => navigate(`/store/detail/${foodData.store.id}`)}
								>
									{foodData.store.storeName}
								</Typography>
							</div>
							{foodData.rateAverage !== 0 ? (
								<div className="flex items-center gap-2 font-bold pointer-events-none">
									Đánh giá:
									<Rating value={Math.round(foodData.rateAverage)} readonly />
									<Typography className="font-semibold">
										{foodData.rateAverage} Sao
									</Typography>
								</div>
							) : (
								<div
									className="font-bold pointer-events-none"
									style={{ color: "gray" }}
								>
									Chưa có đánh giá
								</div>
							)}
							<div className="food-name">
								<Typography variant="h2">{foodData.foodName}</Typography>
								<p className="text-base">
									Phân loại: {foodData.category.categoryName}
								</p>
								<p className="flex gap-1 text-base">
									Tình trạng:{" "}
									{foodData?.inventories[0] !== null &&
									foodData?.inventories[0]?.quantity > 0 ? (
										<p className="text-green-800 font-bold">còn hàng</p>
									) : (
										<p className="text-red-800 font-bold">hết hàng</p>
									)}
								</p>
								<p className="text-base">Mô tả: {foodData.description}</p>
								<p className="my-1 text-base font-bold flex items-center ">
									<span className="rounded-full">
										<svg
											className="w-4 h-4 text-blue-500 dark:text-white"
											aria-hidden="true"
											xmlns="http://www.w3.org/2000/svg"
											fill="none"
											viewBox="0 0 11 20"
										>
											<path
												stroke="currentColor"
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M1.75 15.363a4.954 4.954 0 0 0 2.638 1.574c2.345.572 4.653-.434 5.155-2.247.502-1.813-1.313-3.79-3.657-4.364-2.344-.574-4.16-2.551-3.658-4.364.502-1.813 2.81-2.818 5.155-2.246A4.97 4.97 0 0 1 10 5.264M6 17.097v1.82m0-17.5v2.138"
											/>
										</svg>
									</span>
									<span className="text-blue-500">
										{foodData.price}.000 VND
									</span>
								</p>

								<hr></hr>
								<div className="flex items-center space-x-3 mt-3">
									{/* <div className="h-10 w-28">
										<div className="flex flex-row h-10 w-full relative bg-transparent border border-gray-300">
											<button
												className="h-full w-20 rounded-sm cursor-pointer outline-none"
												onClick={decreaseValue}
											>
												<span className="m-auto text-2xl font-thin">−</span>
											</button>
											<input
												type="number"
												className="text-center w-full font-semibold text-md hover:text-black focus:text-black cursor-default flex items-center outline-none"
												value={value}
												readOnly
											/>
											<button
												className="h-full w-20 rounded-sm cursor-pointer outline-none"
												onClick={increaseValue}
											>
												<span className="m-auto text-2xl font-thin">+</span>
											</button>
										</div>
									</div> */}

									<div className="">
										<button
											className="flex items-center space-x-2  text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-sm text-sm w-full px-5 py-2.5 text-center"
											disabled={foodData.inventories[0].quantity <= 0}
											onClick={() =>
												dispatch(
													cartActions.addToCart({
														foodId: foodData.id,
														foodName: foodData.foodName,
														quantity: 1,
														price: foodData.price,
														img: foodData.imageURL,
													})
												)
											}
										>
											THÊM VÀO GIỎ HÀNG
											<svg
												className="w-4 h-4 text-white ml-1"
												aria-hidden="true"
												xmlns="http://www.w3.org/2000/svg"
												fill="none"
												viewBox="0 0 18 20"
											>
												<path
													stroke="currentColor"
													strokeLinecap="round"
													strokeLinejoin="round"
													width="2"
													d="M6 15a2 2 0 1 0 0 4 2 2 0 0 0 0-4Zm0 0h8m-8 0-1-4m9 4a2 2 0 1 0 0 4 2 2 0 0 0 0-4Zm-9-4h10l2-7H3m2 7L3 4m0 0-.792-3H1"
												/>
											</svg>
										</button>
									</div>
								</div>
								<div className="flex items-center space-x-2 mt-3">
									<div className="h-10 w-28"></div>
									<div>
										<WishlistDetails foodId={id}></WishlistDetails>
									</div>
								</div>
							</div>
						</div>
					</div>

					{/* RECOMMENT FOOD */}
					<div className="py-2">
						<Typography variant="h4">Đồ ăn liên quan</Typography>
						<div className="grid py-3 grid-flow-row-dense grid-cols-3 xl:grid-cols-6">
							{foodList ? (
								foodList.splice(0, 6).map((food, index) => (
									<div key={index} className="px-1 pt-1 border-solid border-2">
										<div className="group relative flex lg:flex-none">
											<img
												src={food.imageURL}
												alt={food.foodName}
												className="h-36 w-72 xl:w-60 object-cover group-hover:opacity-40"
											/>
											<div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
												<Typography className="text-white font-semibold">
													HOT
												</Typography>
											</div>
											<div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
												<AddToWishlist foodId={food.id} />
												{/* <Tooltip content="Thêm giỏ hàng">
													<IconButton
														variant="text"
														className="bg-white rounded-full"
													>
														<svg
															width="30"
															height="2.5em"
															viewBox="0 0 30 40"
															fill="none"
															xmlns="http://www.w3.org/2000/svg"
														>
															<path
																d="M10 35C11.1046 35 12 34.1046 12 33C12 31.8954 11.1046 31 10 31C8.89543 31 8 31.8954 8 33C8 34.1046 8.89543 35 10 35Z"
																fill="black"
															/>
															<path
																d="M23 35C24.1046 35 25 34.1046 25 33C25 31.8954 24.1046 31 23 31C21.8954 31 21 31.8954 21 33C21 34.1046 21.8954 35 23 35Z"
																fill="black"
															/>
															<path
																d="M5.2875 15H27.7125L24.4125 26.55C24.2948 26.9692 24.0426 27.3381 23.6948 27.6001C23.3471 27.862 22.9229 28.0025 22.4875 28H10.5125C10.0771 28.0025 9.65293 27.862 9.30515 27.6001C8.95738 27.3381 8.70524 26.9692 8.5875 26.55L4.0625 10.725C4.0027 10.5159 3.8764 10.3321 3.70271 10.2012C3.52903 10.0704 3.31744 9.99977 3.1 10H1"
																stroke="black"
																strokeWidth="2"
																strokeLinecap="round"
																strokeLinejoin="round"
															/>
														</svg>
													</IconButton>
												</Tooltip> */}
												<Tooltip content="Xem chi tiết món ăn">
													<IconButton
														variant="text"
														className="bg-white rounded-full"
														onClick={() => navigate(`/food-details/${food.id}`)}
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
										<div>
											<Typography
												variant="h6"
												className="w-36 pointer-events-none"
											>
												{food.foodName}
											</Typography>
											<Typography
												color="blue"
												className="relative w-fit pointer-events-none"
											>
												{food.price}
												<span className="absolute font-normal top-0 -right-2 text-xs">
													đ
												</span>
											</Typography>
										</div>
									</div>
								))
							) : (
								<Typography variant="h5" color="deep-orange" className="w-96">
									Không có món ăn nào cùng loại
								</Typography>
							)}
						</div>
					</div>
					{/* END RECOMMENT FOOD */}

					{/* COMMENT */}
					<div>
						<Typography variant="h4" className="mb-10">
							Đánh giá
						</Typography>
						{/* <Typography variant="small" className="pb-2">
							Mô tả bình luận
						</Typography>
						<Textarea
							className="shadow appearance-none border rounded w-full text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
							size="md"
							label="Bạn nghĩ gì về món ăn này..."
							value={commentTxt}
							onChange={(e) => setCommentTxt(e.target.value)}
						></Textarea>
						<Button
							type="submit"
							className="w-fit bg-primary"
							onClick={handleComment}
						>
							Bình luận
						</Button> */}
						<div className="py-2">
							{/* SUB COMMENT */}
							{openComment ? (
								<div className="ml-10 mt-1">
									{foodData.comments.map((comment) => (
										<div
											key={comment.commentDate}
											className="flex justify-start gap-2 mb-4 border-b pb-2"
										>
											<img
												src={comment.user.avatar}
												alt="image 1"
												className="h-14 w-14 rounded-full object-cover"
											></img>
											<div>
												<div className="flex flex-col gap-1">
													<div className="Rating">
														<Rating value={comment.rate} readonly />
													</div>
													<Typography
														variant="small"
														className="font-bold flex gap-x-1"
													>
														{comment.user.userName}
														<Typography variant="small">
															- {moment(comment.commentDate).fromNow()}
														</Typography>
													</Typography>
													{comment.imageURL && (
														<div className="div">
															<img src={comment.imageURL} alt="" />
														</div>
													)}
												</div>
												<Typography variant="paragraph">
													{comment.content}
												</Typography>
												<div className="flex gap-2">
													<Typography
														variant="small"
														className="cursor-pointer hover:text-orange-900"
													></Typography>
													<Typography
														variant="small"
														className="cursor-pointer hover:text-orange-900"
													></Typography>
												</div>
											</div>
										</div>
									))}
								</div>
							) : null}
							{/* END SUB COMMENT */}
							<hr className="h-px my-2 bg-gray-200 border-0 dark:bg-gray-700" />
						</div>
						{/* END COMMENT */}
					</div>
					{/* END COMMENT */}
				</div>
			) : (
				<Spinner></Spinner>
			)}
		</>
	);
};

export default FoodDetails;
