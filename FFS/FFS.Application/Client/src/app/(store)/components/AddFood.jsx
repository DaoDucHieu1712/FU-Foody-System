import { yupResolver } from "@hookform/resolvers/yup";
import {
	Button,
	Dialog,
	Input,
	Option,
	Select,
	Textarea,
} from "@material-tailwind/react";
import * as yup from "yup";
import propTypes from "prop-types";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import ErrorText from "../../../shared/components/text/ErrorText";
import UploadImage from "../../../shared/components/form/UploadImage";

const schema = yup.object({
	category: yup.number().required("Hãy chọn loại!"),
	name: yup.string().required("Hãy ghi tên món ăn!"),
	description: yup.string().required("Hãy ghi mô tả món ăn!"),
	price: yup
		.number()
		.positive("Giá tiền phải lớn hơn 0")
		.required("Hãy nhập giá món ăn!"),
	imageURL: yup.string().required("Hãy thêm ảnh!"),
});

const AddFood = ({ reload, storeId }) => {
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
	const [category, setCategory] = useState([]);
	const handleOpen = () => setOpen((cur) => !cur);

	const ListCategory = async () => {
		try {
			axios
				.get("/api/Category/ListCategoryByStoreId?StoreId=" + storeId)
				.then((response) => {
					setCategory(response.entityCatetory);
				})
				.catch((error) => {
					console.log(error);
					toast.error("Lấy phân loại thất bại!");
				});
		} catch (error) {
			console.error("Category: " + error);
		}
	};

	useEffect(() => {
		ListCategory();
	}, [open]);

	const onSubmit = async (data) => {
		try {
			const newFood = {
				foodName: data.name,
				description: data.description,
				price: data.price,
				categoryId: data.category,
				imageURL: data.imageURL,
				storeId: storeId,
			};
			axios
				.post("/api/Food/AddFood", newFood)
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

	return (
		<div>
			<Button
				className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
				onClick={handleOpen}
			>
				Thêm món ăn
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
					<p className="font-bold text-2xl text-center mb-4">Thêm món ăn mới</p>
					<div className="mb-4">
						<Input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
							type="text"
							label="Tên món ăn"
							{...register("name")}
						></Input>
						{errors.name && <ErrorText text={errors.name.message}></ErrorText>}
					</div>
					<div className="mb-4">
						<Textarea
							className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
							size="md"
							label="Mô tả món ăn"
							{...register("description")}
						></Textarea>
						{errors.description && (
							<ErrorText text={errors.description.message}></ErrorText>
						)}
					</div>
					<div className="mb-4">
						<Input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
							type="number"
							label="Giá tiền (nghìn VND)"
							{...register("price")}
						></Input>
						{errors.price && (
							<ErrorText text={errors.price.message}></ErrorText>
						)}
					</div>
					<UploadImage name="imageURL" onChange={setValue}></UploadImage>
					{errors.imageURL && <ErrorText text={errors.imageURL.message} />}
					<div className="inline-block relative mb-4">
						<Select
							className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
							{...register("category")}
							onChange={(e) => {
								setValue("category", parseInt(e));
							}}
							label="Chọn loại"
						>
							{category ? (
								category.map((category) => (
									<Option key={category.id} value={category.id.toString()}>
										{category.categoryName}
									</Option>
								))
							) : (
								<Option>Lỗi</Option>
							)}
						</Select>
						{errors.category && (
							<ErrorText text={errors.category.message}></ErrorText>
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
AddFood.propTypes = {
	reload: propTypes.any.isRequired,
	storeId: propTypes.any.isRequired,
};
export default AddFood;
