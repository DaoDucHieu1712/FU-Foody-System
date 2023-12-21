import { yupResolver } from "@hookform/resolvers/yup";
import { Button, Input, Radio } from "@material-tailwind/react";
import dayjs from "dayjs";
import { useForm } from "react-hook-form";
import { useDispatch, useSelector } from "react-redux";
import { toast } from "react-toastify";
import * as yup from "yup";
import { setUserProfile } from "../../redux/auth";
import axios from "../../shared/api/axiosConfig";
import UpdateImage from "../../shared/components/form/UpdateImage";
import ErrorText from "../../shared/components/text/ErrorText";
import { NavLink } from "react-router-dom";

const schema = yup.object({
	firstName: yup.string().required("Hãy điền tên!"),
	lastName: yup.string().required("Hãy điền họ!"),
	birthDay: yup
		.date()
		.required("Hãy chọn ngày sinh!")
		.test(
			"is-valid-birth-year",
			"Năm sinh không hợp lệ !",
			(value) => value && dayjs(value).isBefore(dayjs().subtract(18, "years"))
		),
});
const ProfilePage = () => {
	const dispatch = useDispatch();
	const user = useSelector((state) => state.auth.userProfile);
	const birthDateOnly =
		user && user.birthDay && typeof user.birthDay === "string"
			? user.birthDay.split("T")[0]
			: "";

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
		try {
			const updatedUserData = {
				...user,
				avatar: data.avatar ? data.avatar : user.avatar,
				firstName: data.firstName,
				lastName: data.lastName,
				gender: data.gender === "true",
				allow: true,
				birthDay: dayjs(data.birthDay).format("YYYY-MM-DD"),
			};
			console.log("updated", updatedUserData);
			const response = await axios.put(
				`/api/Authenticate/Profile?email=${user.email}`,
				updatedUserData
			);
			console.log("edit", response);

			if (response) {
				dispatch(setUserProfile(updatedUserData));
				toast.success("Sửa profile thành công!");
			}
		} catch (error) {
			console.error("Error updating profile: ", error);
			toast.error("Sửa profile thất bại!");
		}
	};
	return (
		<>
			{user && (
				<div>
					<div className="border-b border-gray-300 p-3">
						<h1 className="text-xl">Hồ sơ của tôi</h1>
						<p className="text-gray-400">
							Quản lý thông tin hồ sơ để bảo mật thông tin
						</p>
					</div>
					<form onSubmit={handleSubmit(onSubmit)}>
						<div className="flex mt-3 p-3 pb-20 items-center">
							<div className="flex flex-col gap-y-6 w-3/4 pr-6">
								<div className="grid grid-cols-3">
									<span className="font-semibold text-gray-500 ">
										Tên đăng nhập
									</span>
									<p className="col-span-2">{user.userName}</p>
								</div>
								<div className="grid grid-cols-3">
									<span className="font-semibold text-gray-500">Tên</span>
									<div className="flex w-full col-span-2 gap-x-3">
										<div>
											<Input
												label="First Name"
												{...register("firstName")}
												defaultValue={user.firstName}
											/>
											{errors.firstName && (
												<ErrorText text={errors.firstName.message}></ErrorText>
											)}
										</div>

										<div>
											<Input
												label="Last Name"
												{...register("lastName")}
												defaultValue={user.lastName}
											/>
											{errors.lastName && (
												<ErrorText text={errors.lastName.message}></ErrorText>
											)}
										</div>
									</div>
								</div>

								<div className="grid grid-cols-3">
									<span className="font-semibold text-gray-500 ">Email</span>
									<p className="col-span-2">{user.email}</p>
								</div>
								{user.phoneNumber ? (
									<div className="grid grid-cols-3">
										<span className="font-semibold text-gray-500 ">
											Số điện thoại
										</span>
										<p className="col-span-2">{user.phoneNumber}</p>
									</div>
								) : null}

								<div className="grid grid-cols-3">
									<span className="font-semibold text-gray-500">Giới tính</span>
									<div className="flex gap-10 col-span-2">
										<Radio
											{...register("gender", { required: true })}
											name="gender"
											value="true"
											label="Nam"
											defaultChecked={user.gender}
										/>
										<Radio
											{...register("gender", { required: true })}
											name="gender"
											label="Nữ"
											value="false"
											defaultChecked={!user.gender}
										/>
									</div>
								</div>
								<div className="grid grid-cols-3">
									<span className="font-semibold text-gray-500">Ngày sinh</span>
									<div className="col-span-1 flex flex-col">
										<Input
											{...register("birthDay")}
											type="date"
											className="w-full"
											label="Ngày sinh"
											defaultValue={birthDateOnly}
										/>
										{errors.birthDay && (
											<ErrorText
												className="mt-1 text-red-500"
												text={errors.birthDay.message}
											></ErrorText>
										)}
									</div>
								</div>

								<div className="grid grid-cols-3 mt-4">
									<Button type="submit" className="bg-primary">
										Lưu
									</Button>
								</div>
							</div>
							<div className="border-l-2 border-gray-400 pl-6 w-1/4">
								<UpdateImage
									name="avatar"
									onChange={setValue}
									url={user.avatar}
								></UpdateImage>
							</div>
						</div>
					</form>
					</div>
			)}
		</>
	);
};

export default ProfilePage;
