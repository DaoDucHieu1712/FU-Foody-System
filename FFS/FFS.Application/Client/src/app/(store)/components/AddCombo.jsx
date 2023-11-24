import { yupResolver } from "@hookform/resolvers/yup";
import { Button, Checkbox, Dialog, Input } from "@material-tailwind/react";
import * as yup from "yup";
import propTypes from "prop-types";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import ErrorText from "../../../shared/components/text/ErrorText";
import UploadImage from "../../../shared/components/form/UploadImage";
import Close from "../../../shared/components/icon/Close";
const schema = yup.object({
	name: yup.string().required("Hãy ghi tên combo món ăn!"),
	percent: yup.number().positive("% phải lớn hơn 0"),
	imageURL: yup.string().required("Hãy thêm ảnh!"),
});

const AddCombo = ({ reload, storeId, foodList }) => {
	const {
		register,
		setValue,
		handleSubmit,
		formState: { errors },
	} = useForm({
		resolver: yupResolver(schema),
		mode: "onSubmit",
	});

	const [open, setOpen] = useState(false);
	const handleOpen = () => setOpen((cur) => !cur);
	const [isModalOpen, setIsModalOpen] = useState(false);
	const [inputValue, setInputValue] = useState("");

	const [selectAll, setSelectAll] = useState(false);
	const [checkedItems, setCheckedItems] = useState({});

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
		setInputValue(""); // Clear the input when closing the modal
	};

	const handleInputChange = (e) => {
		setInputValue(e.target.value);
	};

	useEffect(() => {}, [open]);

	const onSubmit = async (data) => {
		try {
			const newFood = {
				Name: data.name,
				Percent: data.description,
				price: data.price,
				imageURL: data.imageURL,
				storeId: storeId,
			};
			axios
				.post("/api/Food/AddCombo", newFood)
				.then(() => {
					toast.success("Thêm món ăn mới thành công!");
					reload();
					setOpen(false);
				})
				.catch(() => {
					toast.error("Thêm món ăn mới thất bại!");
					setOpen(false);
				});
		} catch (error) {
			console.error("Error add location: ", error);
		}
	};

	const handleAddFood = () => {
		console.log(checkedItems);
	};
	return (
		<div>
			<Button
				className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
				onClick={handleOpen}
			>
				Thêm combo món ăn
			</Button>
			<Dialog
				size="md"
				open={open}
				handler={handleOpen}
				className="bg-transparent shadow-none"
			>
				<form
					className="form bg-white rounded px-4 py-4"
					onSubmit={handleSubmit(onSubmit)}
				>
					<p className="font-bold text-2xl text-center mb-4">
						Thêm Combo món ăn mới
					</p>
					<div className="mb-4">
						<Input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
							type="text"
							label="Tên combo"
							{...register("name")}
						></Input>
						{errors.name && <ErrorText text={errors.name.message}></ErrorText>}
					</div>
					<div className="mb-4">
						<Input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
							type="number"
							label="% giảm giá"
							{...register("percent")}
						></Input>
						{errors.price && (
							<ErrorText text={errors.price.message}></ErrorText>
						)}
					</div>
					<UploadImage name="imageURL" onChange={setValue}></UploadImage>
					{errors.imageURL && <ErrorText text={errors.imageURL.message} />}
					<div className="mb-4">
						<button type="button" onClick={openModal}>
							Thêm món
						</button>
						{isModalOpen && (
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
													<tr key={food.id}>
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
											onClick={handleAddFood}
											className="bg-primary text-white w-[50%] h-10 rounded-xl"
										>
											Thêm
										</button>
									</div>
								</div>
							</Dialog>
						)}
					</div>
					<button
						type="submit"
						className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center"
					>
						Thêm
					</button>
				</form>
			</Dialog>
		</div>
	);
};
AddCombo.propTypes = {
	reload: propTypes.any.isRequired,
	storeId: propTypes.any.isRequired,
	foodList: propTypes.any.isRequired,
};
export default AddCombo;
