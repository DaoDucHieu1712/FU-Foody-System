import { IconButton, Tooltip, Typography } from "@material-tailwind/react";
import { useEffect, useState } from "react";
// import FoodCart from "./FoodCart";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import axios from "../../shared/api/axiosConfig";
import AddToWishlist from "./components/wishlist/AddToWishlist";
import FormatPriceHelper from "../../shared/components/format/FormatPriceHelper";
import propTypes from "prop-types";
import FormatDateTimeString from "../../shared/components/format/FormatDateTime";

const FlashSaleByStore = ({ storeId }) => {
	const navigate = useNavigate();
	const [flashSale, setFlashSale] = useState([]);

	const GetListFilterFood = async () => {
		try {
			axios
				.get(`/api/FlashSale/ListFlashSaleInTimeByStore/${storeId}`)
				.then((response) => {
					console.log(response);
					setFlashSale(response.flashSaleDTOs);
				})
				.catch((error) => {
					console.log(error);
					toast.error("Lấy sản phẩm flash sale thất bại!");
				});
		} catch (error) {
			console.error("Category: " + error);
		}
	};

	useEffect(() => {
		GetListFilterFood();
	}, []);

	const calculateCountdown = (endTime) => {
		const now = new Date();
		const end = new Date(endTime);
		const timeDifference = end - now;

		if (timeDifference <= 0) {
			return "Đã kết thúc";
		}

		const seconds = Math.floor((timeDifference % (1000 * 60)) / 1000);
		const minutes = Math.floor(
			(timeDifference % (1000 * 60 * 60)) / (1000 * 60)
		);
		const hours = Math.floor(
			(timeDifference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
		);

		return `${hours}h ${minutes}m ${seconds}s`;
	};
	const [countdowns, setCountdowns] = useState({});

	useEffect(() => {
		const intervalId = setInterval(() => {
			const updatedCountdowns = {};
			flashSale.forEach((flashSaleItem) => {
				updatedCountdowns[flashSaleItem.id] = calculateCountdown(
					flashSaleItem.end
				);
			});
			setCountdowns(updatedCountdowns);
		}, 1000);

		return () => clearInterval(intervalId);
	}, [flashSale]);

	return (
		<>
			<Typography variant="h5" className="mb-2">
				FLASH SALE
			</Typography>
			{flashSale && flashSale.length > 0 ? (
				flashSale.map((flashSaleItem) => (
					<>
						{flashSaleItem.flashSaleStatus === "Đang diễn ra" && (
							<>
								<h3 className="text-primary italic font-bold mb-2">
									{flashSaleItem.flashSaleStatus}
								</h3>
								<h5>Kết thúc sau: {countdowns[flashSaleItem.id]}</h5>
							</>
						)}

						{flashSaleItem.flashSaleStatus === "Sắp diễn ra" && (
							<>
								<h3 className="text-blue-300 italic font-bold mb-2 mt-4">
									{flashSaleItem.flashSaleStatus}
								</h3>
								<h5>
									Thời gian: {FormatDateTimeString(flashSaleItem.start)} -{" "}
									{FormatDateTimeString(flashSaleItem.end)}
								</h5>
							</>
						)}
						<div
							key={flashSaleItem.id}
							className="flex p-1 my-1 items-center border-r-2 border-solid border-blue-gray-100 shadow-lg"
						>
							{flashSaleItem.flashSaleDetails &&
							flashSaleItem.flashSaleDetails.length > 0
								? flashSaleItem.flashSaleDetails.map((detailItem) => (
										<div key={detailItem.id} className="group relative flex">
											<img
												src={detailItem.imageURL}
												alt={detailItem.foodName}
												className="h-28 w-44 object-cover group-hover:opacity-40"
											/>
											{detailItem.salePercent > 0 ? (
												<div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
													<Typography className="text-white font-semibold">
														{detailItem.salePercent}%
													</Typography>
												</div>
											) : null}
											<div className="absolute hidden h-full w-3/ justify-around items-center group-hover:flex">
												<AddToWishlist foodId={detailItem.foodId} />
												<Tooltip content="Thêm giỏ hàng">
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
												</Tooltip>
												<Tooltip content="Xem chi tiết món ăn">
													<IconButton
														variant="text"
														className="bg-white rounded-full"
														onClick={() =>
															navigate(`/food-details/${detailItem.foodId}`)
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

											<div className="w-2/5">
												<Typography
													variant="h6"
													className="pointer-events-none"
												>
													{detailItem.foodName}
												</Typography>
												{detailItem.priceAfterSale <= 0 &&
												detailItem.salePercent <= 0 ? (
													<>
														<Typography
															color="blue"
															className="relative w-fit pointer-events-none"
														>
															{FormatPriceHelper(detailItem.price)}
															<span className="absolute font-normal top-0 -right-2 text-xs">
																đ
															</span>
														</Typography>
													</>
												) : null}
												{detailItem.priceAfterSale > 0 ? (
													<>
														<Typography
															color="gray"
															className="relative w-fit line-through pointer-events-none"
														>
															{FormatPriceHelper(detailItem.price)}
															<span className="absolute font-normal top-0 -right-2 text-xs">
																đ
															</span>
														</Typography>
														<Typography
															color="blue"
															className="relative w-fit pointer-events-none"
														>
															{FormatPriceHelper(detailItem.priceAfterSale)}
															<span className="absolute font-normal top-0 -right-2 text-xs">
																đ
															</span>
														</Typography>
													</>
												) : null}
												{detailItem.salePercent > 0 ? (
													<>
														<Typography
															color="gray"
															className="relative w-fit line-through pointer-events-none"
														>
															{FormatPriceHelper(detailItem.price)}
															<span className="absolute font-normal top-0 -right-2 text-xs">
																đ
															</span>
														</Typography>
														<Typography
															color="blue"
															className="relative w-fit pointer-events-none"
														>
															{FormatPriceHelper(
																detailItem.price -
																	(detailItem.price * detailItem.salePercent) /
																		100
															)}
															<span className="absolute font-normal top-0 -right-2 text-xs">
																đ
															</span>
														</Typography>
													</>
												) : null}
											</div>
											{/* Additional content for each flashSaleDetail item */}
										</div>
								  ))
								: null}
						</div>
					</>
				))
			) : (
				<Typography className="pt-5" color="gray" variant="h6">
					Chưa có Flash Sale nào!
				</Typography>
			)}
		</>
	);
};

FlashSaleByStore.propTypes = {
	storeId: propTypes.any.isRequired,
};

export default FlashSaleByStore;
