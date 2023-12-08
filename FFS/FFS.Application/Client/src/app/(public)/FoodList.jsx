import {
	Button,
	IconButton,
	Input,
	Option,
	Radio,
	Select,
	Spinner,
	Tooltip,
	Typography,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import axios from "../../shared/api/axiosConfig";
import AddToWishlist from "./components/wishlist/AddToWishlist";
import { useNavigate, useParams } from "react-router-dom";
import FormatPriceHelper from "../../shared/components/format/FormatPriceHelper";

const filter = [
	{ id: "", name: "Tất cả" },
	{ id: 1, name: "Flash sale" },
	{ id: 2, name: "Bán chạy nhất" },
	{ id: 3, name: "Đánh giá hàng đầu" },
	{ id: 4, name: "Sản phẩm mới" },
];

const FoodList = () => {
	const { foodNameSearch } = useParams();
	const navigate = useNavigate();
	const [foodList, setFoodList] = useState([]);
	const [foodNameFilter, setFoodNameFilter] = useState('');
	const [foodFilter, setFoodFilter] = useState("");
	const [pageNumber, setPageNumber] = useState(1);
	const pageSize = 12;
	const [totalPages, setTotalPages] = useState(0);
	const [category, setCategory] = useState([]);
	const [selectedCategory, setSelectedCategory] = useState("");
	const [priceRange, setPriceRange] = useState("");
	const [minPrice, setMinPrice] = useState();
	const [maxPrice, setMaxPrice] = useState();

	const GetListCategory = async () => {
		try {
			axios
				.get("/api/Category/ListTop8PopularCategory")
				.then((response) => {
					setCategory([{ id: "", categoryName: "Tất cả" }, ...response]);
				})
				.catch((error) => {
					console.log(error);
					toast.error("Lấy phân loại thất bại!");
				});
		} catch (error) {
			console.error("Category: " + error);
		}
	};

	const GetListAllFood = async () => {
		try {
			axios
				.get(`/api/Food/ListAllFood`, {
					params: {
						CatName: selectedCategory,
						Search: foodNameFilter,
						PriceMin: minPrice,
						PriceMax: maxPrice,
						FilterFood: foodFilter,
						PageNumber: pageNumber,
						PageSize: pageSize,
					},
				})
				.then((response) => {
					setFoodList(response.foodDTOs);
					setTotalPages(response.metadata.totalPages);
				})
				.catch((error) => {
					console.log(error);
					toast.error("Lấy phân loại thất bại!");
				});
		} catch (error) {
			console.error("Category: " + error);
		}
	};

	const handleFoodListByCategory = async (e) => {
		setSelectedCategory(e.target.value);
	};

	const handlePriceRangeChange = (value, min, max) => {
		setPriceRange(value);
		setMinPrice(min);
		setMaxPrice(max);
	};

	const handlePageChange = (newPage) => {
		if (newPage >= 1 && newPage <= totalPages) {
			setPageNumber(newPage);
		}
	};

	useEffect(() => {
		GetListCategory();
	}, []);

	useEffect(() => {
		GetListAllFood();
	}, [
		foodNameFilter,
		foodFilter,
		minPrice,
		maxPrice,
		selectedCategory,
		pageNumber,
	]);

	useEffect(() => {
		setFoodNameFilter(foodNameSearch || '');
	}, [foodNameSearch]);

	return (
		<div className="flex gap-5 my-16">
			<div className="flex w-60 flex-col">
				<Typography variant="h6">DANH MỤC PHỔ BIẾN</Typography>
				{category ? (
					category.map((category) => (
						<Radio
							key={category.id}
							label={category.categoryName}
							checked={selectedCategory == category.categoryName}
							onChange={handleFoodListByCategory}
							value={category.categoryName}
						/>
					))
				) : (
					<Spinner></Spinner>
				)}
				<hr className="h-px my-2 bg-gray-200 border-0 dark:bg-gray-700" />
				<Typography variant="h6">GIÁ CẢ</Typography>
				<div className="flex gap-2 justify-center items-center">
					<label>Từ:</label>
					<input
						className="border rounded p-1 w-16"
						type="number"
						value={minPrice}
						onChange={(e) => setMinPrice(e.target.value)}
					/>
					<label>Đến:</label>
					<input
						className="border rounded p-1 w-16"
						type="number"
						value={maxPrice}
						onChange={(e) => setMaxPrice(e.target.value)}
					/>
				</div>
				<div>
					<Radio
						label="Tất cả"
						checked={priceRange === "all"}
						onChange={() => handlePriceRangeChange("all", null, null)}
					/>
				</div>
				<div>
					<Radio
						label="Dưới 35.000đ"
						checked={priceRange === "range1"}
						onChange={() => handlePriceRangeChange("range1", 0, 35000)}
					/>
				</div>
				<div>
					<Radio
						label="35.000đ - 50.000đ"
						checked={priceRange === "range2"}
						onChange={() => handlePriceRangeChange("range2", 35000, 50000)}
					/>
				</div>
				<div>
					<Radio
						label="50.000đ - 100.000đ"
						checked={priceRange === "range3"}
						onChange={() => handlePriceRangeChange("range3", 50000, 100000)}
					/>
				</div>
				<div>
					<Radio
						label="Trên 100.000đ"
						checked={priceRange === "range4"}
						onChange={() => handlePriceRangeChange("range4", "", "")}
					/>
				</div>
			</div>
			<div className="w-full">
				<div className="flex justify-between">
					<div className="w-96">
						<Input
							label="Món ăn, tên loại..."
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
							value={foodNameFilter}
							onChange={(e) => setFoodNameFilter(e.target.value)}
						/>
					</div>
					<div className="flex items-center">
						<Typography className="w-24">Bộ lọc: </Typography>
						<Select
							className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
							onChange={(e) => {
								setFoodFilter(e);
							}}
							label="Chọn loại"
						>
							{filter.map((filterItem) => (
								<Option key={filterItem.id} value={filterItem.id.toString()}>
									{filterItem.name}
								</Option>
							))}
						</Select>
					</div>
				</div>
				<hr className="h-px my-2 bg-gray-300 border-0 dark:bg-gray-700" />
				<div className="grid m-2 gap-1 grid-flow-row-dense grid-cols-2 grid-rows-3 lg:grid-cols-4">
					{foodList && foodList.length != 0 ? (
						foodList.map((food) => (
							<div
								key={food.id}
								className="px-1 pt-1 pb-2 border-solid border-2"
							>
								<div className="group relative flex">
									<img
										src={food.imageURL}
										alt="image 1"
										className="h-36 w-80 object-cover lg:w-64 group-hover:opacity-40"
									/>
									{food.salePercent > 0 ?
										(
											<div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
												<Typography className="text-white font-semibold">{food.salePercent}%</Typography>
											</div>
										) : (
											null
										)}
									<div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
										<AddToWishlist foodId={food.id} />
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
									<Typography variant="h6" className="w-36 pointer-events-none">
										{food.foodName}
									</Typography>
									{food.priceAfterSale <= 0 && food.salePercent <= 0 ?
										(
											<div className="flex gap-5">
												<Typography
													color="blue"
													className="relative w-fit pointer-events-none"
												>
													{FormatPriceHelper(food.price)}
													<span className="absolute font-normal top-0 -right-2 text-xs">
														đ
													</span>
												</Typography>
											</div>
										) : (
											null
										)}
									{food.priceAfterSale > 0 ?
										(
											<div className="flex gap-5">
												<Typography
													color="gray"
													className="relative w-fit line-through pointer-events-none"
												>
													{FormatPriceHelper(food.price)}
													<span className="absolute font-normal top-0 -right-2 text-xs">
														đ
													</span>
												</Typography>
												<Typography
													color="blue"
													className="relative w-fit pointer-events-none"
												>
													{ FormatPriceHelper(food.priceAfterSale)}
													<span className="absolute font-normal top-0 -right-2 text-xs">
														đ
													</span>
												</Typography>
											</div>
										) : (
											null
										)}
									{food.salePercent > 0 ?
										(
											<div className="flex gap-5">
												<Typography
													color="gray"
													className="relative w-fit line-through pointer-events-none"
												>
													{FormatPriceHelper(food.price)}
													<span className="absolute font-normal top-0 -right-2 text-xs">
														đ
													</span>
												</Typography>
												<Typography
													color="blue"
													className="relative w-fit pointer-events-none"
												>
													{FormatPriceHelper(food.price - (food.price * food.salePercent / 100))}
													<span className="absolute font-normal top-0 -right-2 text-xs">
														đ
													</span>
												</Typography>
											</div>
										) : (
											null
										)}
								</div>
							</div>
						))
					) : (
						<Typography variant="h5" className="mt-5 ml-5">
							Không có sản phẩm nào!
						</Typography>
					)}
				</div>
				<div className="flex items-center justify-between border-t border-blue-gray-50 p-4">
					<Button
						variant="outlined"
						size="sm"
						onClick={() => handlePageChange(pageNumber - 1)}
					>
						Previous
					</Button>
					<div className="flex items-center gap-2">
						{Array.from({ length: totalPages }, (_, i) => (
							<IconButton
								key={i}
								variant={i + 1 === pageNumber ? "outlined" : "text"}
								size="sm"
								onClick={() => handlePageChange(i + 1)}
							>
								{i + 1}
							</IconButton>
						))}
					</div>
					<Button
						variant="outlined"
						size="sm"
						onClick={() => handlePageChange(pageNumber + 1)}
					>
						Next
					</Button>
				</div>
			</div>
		</div>
	);
};

export default FoodList;
