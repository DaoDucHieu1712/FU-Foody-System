import { yupResolver } from "@hookform/resolvers/yup";
import {
	Button,
	Dialog,
	DialogBody,
	DialogFooter,
	DialogHeader,
	Input,
	Typography,
	IconButton,
} from "@material-tailwind/react";
import * as yup from "yup";
import propTypes from "prop-types";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import axios from "../../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import ErrorText from "../../../../shared/components/text/ErrorText";
const schema = yup.object({
	categoryName: yup.string().required("Hãy nhập tên phân loại!"),
});
const UpdateCategory = ({ id, storeId, categoryName, reloadCategory }) => {
	const [open, setOpen] = useState(false);
	const handleOpen = () => setOpen(!open);
	const {
		register,
		setValue,
		handleSubmit,
		formState: { errors },
	} = useForm({
		resolver: yupResolver(schema),
		mode: "onSubmit",
	});

	const onSubmit = async (data) => {
		const newCategory = {
			categoryName: data.categoryName,
			storeId: storeId,
		};

		await axios
			.put(`/api/Category/Update/${id}`, newCategory)
			.then((res) => {
				toast.success("Cập nhật danh mục mới thành công!");
				reloadCategory();
				setOpen(false);
			})
			.catch((err) => {
				if (err.response && err.response.status === 409) {
					toast.error("Danh mục đã tồn tại. Không thể cập nhật.");
				} else {
					console.error("Error updating category: ", err);
				}
			});
	};
	return (
		<>
			<IconButton variant="text" onClick={handleOpen}>
				<i className="fas fa-pencil" />
			</IconButton>

			<Dialog open={open} size="sm" handler={handleOpen}>
				<form onSubmit={handleSubmit(onSubmit)}>
					<div className="flex items-center justify-between">
						<DialogHeader className="flex flex-col items-start">
							<Typography className="mb-1" variant="h4">
								CHỈNH SỬA DANH MỤC
							</Typography>
						</DialogHeader>
						<svg
							xmlns="http://www.w3.org/2000/svg"
							viewBox="0 0 24 24"
							fill="currentColor"
							className="mr-3 h-5 w-5"
							onClick={handleOpen}
						>
							<path
								fillRule="evenodd"
								d="M5.47 5.47a.75.75 0 011.06 0L12 10.94l5.47-5.47a.75.75 0 111.06 1.06L13.06 12l5.47 5.47a.75.75 0 11-1.06 1.06L12 13.06l-5.47 5.47a.75.75 0 01-1.06-1.06L10.94 12 5.47 6.53a.75.75 0 010-1.06z"
								clipRule="evenodd"
							/>
						</svg>
					</div>
					<DialogBody>
						<div className="grid gap-6">
							<div className="w-full mb-4">
								<Input
									label="Danh mục của bạn"
									type="text"
									{...register("categoryName")}
									defaultValue={categoryName}
									className="rounded-none"
								/>
								{errors.categoryName && (
									<ErrorText text={errors.categoryName.message}></ErrorText>
								)}
							</div>
						</div>
					</DialogBody>
					<DialogFooter className="space-x-2">
						<Button variant="text" color="deep-orange" onClick={handleOpen}>
							Hủy
						</Button>
						<Button variant="gradient" color="deep-orange" type="submit">
							Cập nhật
						</Button>
					</DialogFooter>
				</form>
			</Dialog>
		</>
	);
};

export default UpdateCategory;
