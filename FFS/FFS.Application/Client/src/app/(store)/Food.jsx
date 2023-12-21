import {
	Card,
	CardHeader,
	Typography,
	Button,
	CardBody,
	CardFooter,
	IconButton,
	Tooltip,
	Input,
} from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import AddFood from "./components/AddFood";
import ManageCombo from "./components/ManageCombo";
import UpdateFood from "./components/UpdateFood";
import DeleteFood from "./components/DeleteFood";
import { toast } from "react-toastify";
import CookieService from "../../shared/helper/cookieConfig";
import { useNavigate } from "react-router-dom";

const TABLE_HEAD = ["Tên đồ ăn", "Ảnh", "Mô tả", "Loại", ""];

const Food = () => {
	const navigate = useNavigate();
	const backgroundColors = ["bg-gray-50", "bg-gray-200"];
	const [foodList, setFoodList] = useState([]);
	const [foodNameFilter, setFoodNameFilter] = useState("");
	const [pageNumber, setPageNumber] = useState(1);
	const pageSize = 10;
	const [totalPages, setTotalPages] = useState(0);
	const [storeId, setStoreId] = useState(0);
	const uId = CookieService.getToken("fu_foody_id");

	const GetStoreByUid = async () => {
		try {
			await axios
				.get(`/api/Store/GetStoreByUid?uId=${uId}`)
				.then((response) => {
					setStoreId(response.id);
					reloadList();
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.log("Get Store By Uid error: " + error);
		}
	};
	const handleExportExcel = () => {
		const fileDownloadUrl = `${
			import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL
		}/api/Store/ExportFood/exportfood?id=${storeId}`;

		window.location.href = fileDownloadUrl;
	};
	const reloadList = async () => {
		const dataPost = {
			uId: uId,
			FoodName: foodNameFilter,
			PageNumber: pageNumber,
			PageSize: pageSize,
		};
		try {
			await axios
				.post(`/api/Food/ListFood`, dataPost)
				.then((res) => {
					setFoodList(res.data);
					const totalPages = Math.ceil(res.totalPage / dataPost.PageSize);
					setTotalPages(totalPages);
				})
				.catch((error) => {
					toast.error("Có lỗi xảy ra!");
					console.log(error);
				});
		} catch (error) {
			console.log("Food error: " + error);
		}
	};

	useEffect(() => {
		reloadList();
	}, [pageNumber, foodNameFilter]);

	useEffect(() => {
		GetStoreByUid();
	}, []);

	const handlePageChange = (newPage) => {
		if (newPage >= 1 && newPage <= totalPages) {
			setPageNumber(newPage);
		}
	};

	return (
		<>
			<Card className="h-full w-full px-2 py-2">
				<CardHeader floated={false} shadow={false} className="rounded-none">
					<div className="mb-4 flex flex-col justify-between gap-8 md:flex-row md:items-center">
						<Typography variant="h4" color="blue-gray">
							Danh sách sản phẩm
						</Typography>
						<div className="flex gap-5">
							<Button
								className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
								onClick={handleExportExcel}
							>
								Xuất Excel
							</Button>
							<AddFood reload={reloadList} storeId={storeId}></AddFood>
							<ManageCombo
								reload={reloadList}
								storeId={storeId}
								foodList={foodList}
							></ManageCombo>
						</div>
						<div className="w-full shrink-0 gap-2 px-2 py-2 md:w-max">
							<div className="w-full md:w-72">
								<Input
									label="Tìm kiếm"
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
						</div>
					</div>
				</CardHeader>
				<CardBody className="px-1">
					<table className="w-full min-w-max table-auto text-center">
						<thead>
							<tr>
								{TABLE_HEAD.map((head) => (
									<th
										key={head}
										className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4"
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
							{foodList ? (
								foodList.map((food, index) => (
									<tr
										key={food.id}
										className={
											backgroundColors[index % backgroundColors.length]
										}
									>
										<td>
											<Typography
												variant="small"
												color="blue-gray"
												className="font-normal max-w-xs truncate"
											>
												{food.FoodName}
											</Typography>
										</td>
										<td>
											<img
												className="font-normal mx-auto"
												src={food.ImageURL}
												alt={food.FoodName}
												height={100}
												width={100}
											/>
										</td>
										<td>
											<Typography
												variant="small"
												color="blue-gray"
												className="font-normal max-w-xs truncate"
											>
												{food.Description}
											</Typography>
										</td>
										<td>
											<Typography
												variant="small"
												color="blue-gray"
												className="font-normal"
											>
												{food.CategoryName}
											</Typography>
										</td>
										<td>
											<div className="h-full flex justify-center items-center">
												<Tooltip content="Xem món ăn">
													<IconButton
														variant="text"
														onClick={() => navigate(`/food-details/${food.id}`)}
													>
														<svg
															xmlns="http://www.w3.org/2000/svg"
															width="20"
															height="20"
															viewBox="0 0 576 512"
														>
															<path d="M288 32c-80.8 0-145.5 36.8-192.6 80.6C48.6 156 17.3 208 2.5 243.7c-3.3 7.9-3.3 16.7 0 24.6C17.3 304 48.6 356 95.4 399.4C142.5 443.2 207.2 480 288 480s145.5-36.8 192.6-80.6c46.8-43.5 78.1-95.4 93-131.1c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C433.5 68.8 368.8 32 288 32zM144 256a144 144 0 1 1 288 0 144 144 0 1 1 -288 0zm144-64c0 35.3-28.7 64-64 64c-7.1 0-13.9-1.2-20.3-3.3c-5.5-1.8-11.9 1.6-11.7 7.4c.3 6.9 1.3 13.8 3.2 20.7c13.7 51.2 66.4 81.6 117.6 67.9s81.6-66.4 67.9-117.6c-11.1-41.5-47.8-69.4-88.6-71.1c-5.8-.2-9.2 6.1-7.4 11.7c2.1 6.4 3.3 13.2 3.3 20.3z" />
														</svg>
													</IconButton>
												</Tooltip>
												<UpdateFood
													reload={reloadList}
													foodData={food}
													storeId={storeId}
												></UpdateFood>
												<DeleteFood
													reload={reloadList}
													id={food.id}
												></DeleteFood>
											</div>
										</td>
									</tr>
								))
							) : (
								<tr>
									<td colSpan={TABLE_HEAD.length}>
										<p>Chưa có sản phẩm</p>
									</td>
								</tr>
							)}
						</tbody>
					</table>
				</CardBody>
				<CardFooter className="flex items-center justify-between border-t border-blue-gray-50 p-4">
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
				</CardFooter>
			</Card>
		</>
	);
};

export default Food;
