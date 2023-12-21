import { useEffect, useState } from "react";
import React from "react";
import axios from "../../shared/api/axiosConfig";

import {
	Card,
	CardHeader,
	Input,
	Typography,
	Button,
	CardBody,
	CardFooter,
	Tabs,
	TabsHeader,
	Tab,
	IconButton,
} from "@material-tailwind/react";
import ArrowRight from "../../shared/components/icon/ArrowRight";
import ArrowLeft from "../../shared/components/icon/ArrowLeft";
import CookieService from "../../shared/helper/cookieConfig";
import AddCategory from "./components/category/AddCategory";
import UpdateCategory from "./components/category/UpdateCategory";
import DeleteCategory from "./components/category/DeleteCategory";
import dayjs from "dayjs";

const TABLE_HEAD = ["Mã danh mục", "Tên danh mục", "Ngày tạo", ""];

const uId = CookieService.getToken("fu_foody_id");

const Category = () => {
	const [category, setCategory] = useState([]);
	const [categoryNameFilter, setCategoryNameFilter] = useState("");
	const [storeId, setStoreId] = useState(0);
	const [pageNumber, setPageNumber] = useState(1);
	const [pageSize, setPageSize] = useState(4);
	const [totalPages, setTotalPages] = useState(1);
	const [active, setActive] = React.useState(1);

	const handleExportExcel = () => {
		const fileDownloadUrl = `${
			import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL
		}/api/Category/ExportCategory?id=${storeId}`;

		window.location.href = fileDownloadUrl;
	};
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

	const fetchCategory = async () => {
		try {
			const response = await axios.get("api/Category/ListCategoryByStoreId", {
				params: {
					StoreId: storeId,
					CategoryName: categoryNameFilter,
					PageNumber: pageNumber,
					PageSize: pageSize,
				},
			});
			setCategory(response.entityCatetory);
			setTotalPages(response.metadata.totalPages);
		} catch (error) {
			console.error("Error fetching category data:", error);
		}
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
	useEffect(() => {
		fetchCategory();
	}, [storeId, categoryNameFilter, pageNumber, pageSize]);

	useEffect(() => {
		GetStoreByUid();
	}, []);
	const reloadCategory = async () => {
		await fetchCategory();
	};

	return (
		<>
			<Card className="h-full w-full  px-2 py-2" shadow={false} floated={false}>
				<CardHeader floated={false} shadow={false} className="rounded-none">
					<div className="mb-4 flex flex-col justify-between gap-8 md:flex-row md:items-center">
						<Typography variant="h4" color="blue-gray">
							Danh sách danh mục
						</Typography>
						<div className="flex gap-5">
							<Button
								className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
								onClick={handleExportExcel}
							>
								Xuất Excel
							</Button>
							<AddCategory
								reloadCategory={reloadCategory}
								storeId={storeId}
							></AddCategory>
						</div>
						<div className="w-full shrink-0 gap-2 px-2 py-2 md:w-max">
							<div className="w-full md:w-72">
								<Input
									label="Tên danh mục"
									icon={<i className="fas fa-search" />}
									value={categoryNameFilter}
									onChange={(e) => setCategoryNameFilter(e.target.value)}
								/>
							</div>
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
							{category.map(
								({ id, storeId, categoryName, createdAt }, index) => {
									const isLast = index === category.length - 1;
									const classes = isLast
										? "p-4"
										: "p-4 border-b border-blue-gray-50";

									return (
										<tr key={id}>
											<td className={classes}>
												<Typography
													variant="small"
													color="blue-gray"
													className="font-normal"
												>
													{id}
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
													{dayjs(createdAt).format("DD/MM/YYYY")}
												</Typography>
											</td>

											<td className={classes}>
												<UpdateCategory
													storeId={storeId}
													id={id}
													categoryName={categoryName}
													reloadCategory={reloadCategory}
												/>
												<DeleteCategory
													id={id}
													reloadCategory={reloadCategory}
												/>
											</td>
										</tr>
									);
								}
							)}
						</tbody>
					</table>
				</CardBody>

				<CardFooter className="flex items-center justify-between border-t border-blue-gray-50 py-4">
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
export default Category;
