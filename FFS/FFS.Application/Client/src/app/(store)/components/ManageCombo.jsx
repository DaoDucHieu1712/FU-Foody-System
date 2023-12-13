import { yupResolver } from "@hookform/resolvers/yup";
import {
	Button,
	Checkbox,
	Dialog,
	DialogBody,
	DialogFooter,
	DialogHeader,
	Input,
	Tab,
	TabPanel,
	Tabs,
	TabsBody,
	TabsHeader,
} from "@material-tailwind/react";
import * as yup from "yup";
import propTypes from "prop-types";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import Combo from "./Combo";
import Close from "../../../shared/components/icon/Close";
import ErrorText from "../../../shared/components/text/ErrorText";
import UpdateImage from "../../../shared/components/form/UpdateImage";
const schema = yup.object({
	name: yup.string().required("Hãy ghi tên combo món ăn!"),
	percent: yup.number().positive("% phải lớn hơn 0"),
});

const ManageCombo = ({ reload, storeId, foodList }) => {
	const [showCombo, setShowCombo] = useState(false);
	const [open, setOpen] = useState(false);
	const handleOpen = () => setOpen((cur) => !cur);
	const [isModalOpen, setIsModalOpen] = useState(false);

	const [selectAll, setSelectAll] = useState(false);
	const [checkedItems, setCheckedItems] = useState({});
	const [combos, setCombos] = useState([]);
	const [currentCombo, setCurrentCombo] = useState(0);
	const [isEditCombo, setIsEditCombo] = useState();

	const [foodListAdd, setFoodListAdd] = useState([]);

	const {
		register,
		setValue,
		handleSubmit,
		formState: { errors },
		reset,
	} = useForm({
		resolver: yupResolver(schema),
		mode: "onSubmit",
	});

	const onSubmit = async (data) => {
		const listInt = [];
		for (const key in checkedItems) {
			if (checkedItems[key]) {
				listInt.push(parseInt(key));
			}
		}

		try {
			const dataUpdate = {
				...data,
				image: data.image ?? currentCombo.image,
				StoreId: storeId,
				IdFoods: listInt,
			};
			axios
				.put("/api/Food/UpdateCombo/" + currentCombo.id, dataUpdate)
				.then((res) => {
					toast.success(res);
					setIsEditCombo(!isEditCombo);
					getAllCombo();
					reset();
				})
				.catch(() => {
					toast.error("thất bại");
				});
			console.log(dataUpdate);
		} catch (error) {
			console.error("Error add location: ", error);
		}
	};

	const toggleSelectAll = () => {
		setSelectAll(!selectAll);
		const newCheckedItems = {};
		foodList.forEach((food) => {
			newCheckedItems[food.id] = !selectAll;
		});
		setCheckedItems(newCheckedItems);
	};

	const handleCheckboxChange = (foodId) => {
		const newCheckedItems = {
			...checkedItems,
			[foodId]: !checkedItems[foodId],
		};
		setCheckedItems(newCheckedItems);
	};

	const openModal = () => {
		console.log(foodList);
		setIsModalOpen(true);
	};

	const closeModal = () => {
		setIsModalOpen(false);
	};

	useEffect(() => {
		getAllCombo();
	}, [open]);

	const getAllCombo = () => {
		axios
			.get("/api/Food/GetListCombo/" + storeId)
			.then((res) => {
				setCombos(res);
			})
			.catch(() => {
				toast.error("Lấy combo thất bại");
			});
	};

	const GetDetailCombo = async (id) => {
		const response = await axios.get("/api/Food/GetDetailCombo/" + id);
		const comboDetails = response;
		const excludedFoodIds = comboDetails.map((foodItem) => foodItem.foodId);

		// Create a new array with the filtered results
		const filteredFoodList = foodList.filter((food) =>
			excludedFoodIds.includes(food.id)
		);

		const updatedCheckedItems = {};

		filteredFoodList.forEach((food) => {
			updatedCheckedItems[food.id] = true;
		});

		setCheckedItems(updatedCheckedItems);
	};

	const handleDelelteDetailCombo = (id) => {
		axios
			.delete("/api/Food/DeleteDetail/" + id)
			.then((res) => {
				getAllCombo();
			})
			.catch(() => {
				toast.error("thất bại");
			});
	};

	const handleUpdateCombo = () => {
		const listInt = [];
		for (const key in checkedItems) {
			if (checkedItems[key]) {
				listInt.push(parseInt(key));
			}
		}
		const data = {
			Name: currentCombo.name,
			StoreId: currentCombo.storeId,
			Image: currentCombo.image,
			Percent: currentCombo.percent,
			IdFoods: listInt,
		};
		axios
			.put("/api/Food/UpdateCombo/" + currentCombo.id, data)
			.then((res) => {
				console.log(res);
				getAllCombo();
			})
			.catch(() => {
				toast.error("thất bại");
			});
	};

	return (
		<div>
			<div className="flex">
				<Combo foodList={foodList} reload={getAllCombo} storeId={storeId} />
				<Button
					className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900 ml-5"
					onClick={handleOpen}
				>
					Quản lí combo
				</Button>
			</div>

			<Dialog
				size="xl"
				open={open}
				handler={handleOpen}
				className="bg-white shadow-none overflow-y-scroll h-[90%] relative"
			>
				<DialogHeader>
					Quản lí combo{" "}
					<Button
						className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900 ml-5"
						onClick={() => {
							setIsEditCombo(!isEditCombo);
						}}
					>
						Sửa combo ({currentCombo.name})
					</Button>
					<Dialog
						size="md"
						open={isEditCombo}
						handler={() => setIsEditCombo(!isEditCombo)}
						className="bg-transparent shadow-none"
					>
						<form
							className="form bg-white rounded px-4 py-4"
							onSubmit={handleSubmit(onSubmit)}
						>
							<p className="font-bold text-2xl text-center mb-4">
								Sửa combo {currentCombo.name}
							</p>
							<div className="mb-4">
								<Input
									className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
									type="text"
									label="Tên combo"
									defaultValue={currentCombo.name}
									{...register("name")}
								></Input>
								{errors.name && (
									<ErrorText text={errors.name.message}></ErrorText>
								)}
							</div>
							<div className="mb-4">
								<Input
									className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
									type="number"
									label="% giảm giá"
									defaultValue={currentCombo.percent}
									{...register("percent")}
								></Input>
								{errors.price && (
									<ErrorText text={errors.price.message}></ErrorText>
								)}
							</div>
							<UpdateImage
								url={currentCombo.image}
								name="image"
								onChange={setValue}
							></UpdateImage>

							<button
								type="submit"
								className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center"
							>
								Thêm
							</button>
						</form>
					</Dialog>
				</DialogHeader>
				<DialogBody>
					<Tabs value="html" orientation="vertical">
						<TabsHeader className="">
							{combos.map((item) => (
								<Tab
									key={`combo${item.combo.id}`}
									value={item.combo.id}
									onClick={() => {
										setCurrentCombo(item.combo);
										GetDetailCombo(item.combo.id);
									}}
								>
									<div className="items-center gap-2 border-b border-b-blue-gray-300">
										<img
											className="h-40 w-40 object-cover object-center"
											src={item.combo.image}
										/>
										{item.combo.name}
									</div>
								</Tab>
							))}
						</TabsHeader>
						<TabsBody>
							{combos.map((item) =>
								item.detail.map((detail, index) => {
									return (
										<TabPanel
											key={`detail_${index}`}
											value={detail.Id}
											className="py-0"
										>
											<div className="grid grid-cols-3 gap-1 gap-y-4 items-center">
												<p className="text-center">{detail.FoodName}</p>
												<img
													className="w-full h-[70%] object-contain"
													src={detail.ImageURL}
												/>
												<div className="text-center">
													<Button
														onClick={() =>
															handleDelelteDetailCombo(detail.IdDetail)
														}
													>
														Xóa
													</Button>
												</div>
											</div>
										</TabPanel>
									);
								})
							)}
							<div className="col-span-3 text-center mt-5">
								<Button
									onClick={() => {
										setIsModalOpen(!isModalOpen);
									}}
								>
									Thêm sản phẩm mới
								</Button>
								<Dialog
									size="md"
									open={isModalOpen}
									handler={closeModal}
									className="bg-transparent shadow-none"
								>
									<div className="bg-white rounded-lg h-min">
										<div className="p-5 flex justify-end">
											<span className="cursor-pointer" onClick={closeModal}>
												<Close />
											</span>
										</div>
										<table className="w-full">
											<thead>
												<tr>
													<th>Ảnh</th>
													<th>Món</th>
													<th></th>
												</tr>
												<tr>
													<th></th>
													<th></th>
													<th>
														<Checkbox
															checked={selectAll}
															onChange={toggleSelectAll}
														/>
													</th>
												</tr>
											</thead>
											<tbody>
												{foodList.map((food) => {
													return (
														<tr key={`food${food.id}`}>
															<td>
																{" "}
																<img
																	className="font-normal mx-auto"
																	src={food.ImageURL}
																	alt={food.FoodName}
																	height={100}
																	width={100}
																/>
															</td>
															<td>{food.FoodName}</td>
															<td className="text-center">
																<Checkbox
																	checked={checkedItems[food.id] || false}
																	onChange={() => handleCheckboxChange(food.id)}
																/>
															</td>
														</tr>
													);
												})}
											</tbody>
										</table>

										<div className=" p-5 text-center">
											<button
												onClick={() => {
													closeModal();
													handleUpdateCombo();
												}}
												className="bg-primary text-white w-[50%] h-10 rounded-xl"
											>
												Thêm
											</button>
										</div>
									</div>
								</Dialog>
							</div>
						</TabsBody>
					</Tabs>
				</DialogBody>
				<DialogFooter>
					<Button
						variant="text"
						color="red"
						onClick={() => handleOpen(null)}
						className="mr-1"
					>
						<span>Cancel</span>
					</Button>
				</DialogFooter>
			</Dialog>
		</div>
	);
};
ManageCombo.propTypes = {
	reload: propTypes.any.isRequired,
	storeId: propTypes.any.isRequired,
	foodList: propTypes.any.isRequired,
};
export default ManageCombo;
