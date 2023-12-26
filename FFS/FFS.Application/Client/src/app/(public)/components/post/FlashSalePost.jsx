import {
	IconButton,
	Spinner,
	Tooltip,
	Typography,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import axios from "../../../../shared/api/axiosConfig";
import AddToWishlist from "../wishlist/AddToWishlist";
import FoodCart from "../HomePage/FoodCart";
import FormatPriceHelper from "../../../../shared/components/format/FormatPriceHelper";

const FlashSalePost = () => {
	const navigate = useNavigate();
	const [flashSale, setFlashSale] = useState([]);

	const GetListFilterFood = async () => {
		try {
			axios
				.get(`/api/Food/ListAllFood?FilterFood=2&PageNumber=1&PageSize=3`)
				.then((response) => {
					setFlashSale(response.foodDTOs);
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

	return (
		<>
			<div className="food_newest border p-4 mt-6">
				<h1 className="text-lg font-bold uppercase">Món ăn yêu thích</h1>
				{flashSale ? (
					flashSale.map((flashSaleItem) => (
						<div
							key={flashSaleItem.id}
							className="flex my-2 justify-between items-center hover:shadow-lg"
						>
							<div className="group relative flex">
								<img
									src={flashSaleItem.imageURL}
									alt={flashSaleItem.foodName}
									className="h-[100px] w-44 object-cover group-hover:opacity-40"
								/>
								<div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
									<AddToWishlist foodId={flashSaleItem.id} />
									<Tooltip content="Xem chi tiết món ăn">
										<IconButton
											variant="text"
											className="bg-white rounded-full"
											onClick={() =>
												navigate(`/food-details/${flashSaleItem.id}`)
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
							<div className="w-2/5">
								<Typography variant="h6" className="pointer-events-none">
									{flashSaleItem.foodName}
								</Typography>
								<Typography
									color="blue"
									className="pb-2 relative w-fit pointer-events-none"
								>
									{FormatPriceHelper(flashSaleItem.price)}
									<span className="absolute font-normal top-0 -right-2 text-xs">
										đ
									</span>
								</Typography>
							</div>
						</div>
					))
				) : (
					<Spinner></Spinner>
				)}
			</div>
		</>
	);
};

export default FlashSalePost;
