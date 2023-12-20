import { useEffect, useState } from "react";
import React from "react";
import axios from "../../../../../shared/api/axiosConfig";
import AddInventory from "../inventory/AddInventory";
import UpdateInventory from "../inventory/UpdateInventory";
import DeleteInventory from "../inventory/DeleteInventory";

import {
	Card,
	CardHeader,
	Input,
	Typography,
	Button,
	CardBody,
	CardFooter,
	IconButton,
} from "@material-tailwind/react";
import FormatDateString from "../../../../../shared/components/format/FormatDate";
import ArrowRight from "../../../../../shared/components/icon/ArrowRight";
import ArrowLeft from "../../../../../shared/components/icon/ArrowLeft";
import CookieService from "../../../../../shared/helper/cookieConfig";
import ExportInventory from "./ExportInventory";

const TABLE_HEAD = [
	"Ảnh món ăn",
	"Tên món ăn",
	"Ngày tạo",
	"Ngày chỉnh sửa",
	"Phân loại",
	"Số lượng",
	"",
];

const uId = CookieService.getToken("fu_foody_id");

const Inventory = () => {
	const [inventory, setInventory] = useState([]);
	const [foodNameFilter, setFoodNameFilter] = useState("");
	const [storeId, setStoreId] = useState(0);
	const [pageNumber, setPageNumber] = useState(1);
	const [pageSize, setPageSize] = useState(4);
	const [totalPages, setTotalPages] = useState(1);
	const [active, setActive] = React.useState(1);

	const getItemProps = (index) => ({
		variant: active === index ? "filled" : "text",
		onClick: () => {
			setActive(index);
			setPageNumber(index);
		},
		className: `rounded-full ${active === index ? "bg-primary" : ""}`,
	});

	const next = () => {
		if (active < totalPages) {
			setActive(active + 1);
			setPageNumber(pageNumber + 1);
		}
	};

	const prev = () => {
		if (active > 1) {
			setActive(active - 1);
			setPageNumber(pageNumber - 1);
		}
	};
	const handleExportExcel = () => {
		const fileDownloadUrl = `${
			import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL
		}/api/Store/ExportInventory/exportinventory?id=${storeId}`;

		window.location.href = fileDownloadUrl;
	};

	const GetStoreByUid = async () => {
		try {
			await axios
				.get(`/api/Store/GetStoreByUid?uId=${uId}`)
				.then((response) => {
					setStoreId(response.id);
					console.log(response.id);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.log("Get Store By Uid error: " + error);
		}
	};

	const fetchInventory = async () => {
		try {
			const response = await axios.get("/api/Inventory/GetInventories", {
				params: {
					StoreId: storeId,
					FoodName: foodNameFilter,
					PageNumber: pageNumber,
					PageSize: pageSize,
				},
			});
			setInventory(response.entityInventory);
			setTotalPages(response.metadata.totalPages);
		} catch (error) {
			console.error("Error fetching inventory data:", error);
		}
	};

	useEffect(() => {
		fetchInventory();
	}, [storeId, foodNameFilter, pageNumber, pageSize]);

	useEffect(() => {
		GetStoreByUid();
	}, []);
	const reloadInventory = async () => {
		await fetchInventory();
	};

	return (
		<>
			<div className="w-full h-auto">
				<div className="flex items-center justify-between">
					<p className="mx-5 mt-2 font-bold text-3xl pointer-events-none">
						Tồn kho
					</p>
					{/* <AddInventory
            storeId={storeId}
            reloadInventory={reloadInventory}
          ></AddInventory> */}
				</div>

				<hr className="h-px my-4 bg-gray-200 border-0 dark:bg-gray-700" />
			</div>

			<Card className="h-full w-full" shadow={false}>
				<CardHeader floated={false} shadow={false} className="rounded-none">
					<div className="flex flex-col items-center justify-between gap-4 md:flex-row mt-3">
						<div className="ExportExcel">
							<button
								className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-5 py-2.5 text-center"
								onClick={handleExportExcel}
							>
								Xuất Excel
							</button>
						</div>

						<div className="w-full md:w-72">
							<Input
								label="Tên món ăn"
								icon={<i className="fas fa-search" />}
								value={foodNameFilter}
								onChange={(e) => setFoodNameFilter(e.target.value)}
							/>
						</div>
					</div>
				</CardHeader>
				<CardBody className="p-0 mt-3">
					<table className="mt-4 w-full min-w-max table-auto text-left">
						<thead>
							<tr>
								{TABLE_HEAD.map((head) => (
									<th
										key={head}
										className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4"
									>
										<Typography
											variant="small"
											color="black"
											className="font-normal leading-none opacity-70"
										>
											{head}
										</Typography>
									</th>
								))}
							</tr>
						</thead>
						<tbody>
							{inventory.map(
								(
									{
										id,
										foodId,
										imageURL,
										foodName,
										quantity,
										createdAt,
										updatedAt,
										categoryName,
									},
									index
								) => {
									const isLast = index === inventory.length - 1;
									const classes = isLast
										? "p-4"
										: "p-4 border-b border-blue-gray-50";

									return (
										<tr key={id}>
											<td className={classes}>
												<div className="flex items-center gap-3">
													<img
														className="h-20 w-20 object-cover object-center"
														src={imageURL}
														alt="nature image"
													/>
												</div>
											</td>
											<td className={classes}>
												<Typography
													variant="small"
													color="blue-gray"
													className="font-normal"
												>
													{foodName}
												</Typography>
											</td>

											<td className={classes}>
												<Typography
													variant="small"
													color="blue-gray"
													className="font-normal"
												>
													{FormatDateString(createdAt)}
												</Typography>
											</td>
											<td className={classes}>
												<Typography
													variant="small"
													color="blue-gray"
													className="font-normal"
												>
													{FormatDateString(updatedAt)}
												</Typography>
											</td>
											<td className={classes}>
												<Typography
													variant="small"
													color="blue-gray"
													className="font-normal"
												>
													{categoryName}
												</Typography>
											</td>
											<td className={classes}>
												<Typography
													variant="small"
													color="blue-gray"
													className="font-normal"
												>
													{quantity}
												</Typography>
											</td>
											<td className={classes}>
												<UpdateInventory
													storeId={storeId}
													foodId={foodId}
													foodName={foodName}
													quantity={quantity}
													reloadInventory={reloadInventory}
												/>
												<ExportInventory
													storeId={storeId}
													foodId={foodId}
													foodName={foodName}
													quantity={quantity}
													reloadInventory={reloadInventory}
												/>
												<DeleteInventory
													inventoryId={id}
													reloadInventory={reloadInventory}
												/>
											</td>
										</tr>
									);
								}
							)}
						</tbody>
					</table>
				</CardBody>

				<CardFooter className="flex items-center justify-between border-t border-blue-gray-50 p-4">
					<Button
						variant="text"
						className="flex items-center gap-2 rounded-full"
						onClick={prev}
						disabled={active === 1}
					>
						<ArrowLeft /> Previous
					</Button>
					<div className="flex items-center gap-2">
						{Array.from({ length: totalPages }, (_, index) => (
							<IconButton key={index + 1} {...getItemProps(index + 1)}>
								{index + 1}
							</IconButton>
						))}
					</div>
					<Button
						variant="text"
						className="flex items-center gap-2 rounded-full"
						onClick={next}
						disabled={active === totalPages}
					>
						Next <ArrowRight />
					</Button>
				</CardFooter>
			</Card>
		</>
	);
};
export default Inventory;
