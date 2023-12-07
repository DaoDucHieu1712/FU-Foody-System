import {
	Button,
	Input,
	Option,
	Radio,
	Select,
	Textarea,
} from "@material-tailwind/react";
import UploadImage from "../../shared/components/form/UploadImage";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import ErrorText from "../../shared/components/text/ErrorText";
import AuthServices from "./shared/auth.service";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import axioss from "axios";
import { useState } from "react";

const schema = yup.object({
	storeName: yup
		.string()
		.min(8, "Tên cửa hàng phải từ 8 ký tự trở lên")
		.required("Tên cửa hàng không thể để trống !"),
	avatarURL: yup.string().required(),
	description: yup.string().required("Mô tả không thể để trống !"),
	// address: yup.string().required("Địa chỉ không thể để trống !"),
	phoneNumber: yup.string().required("Số điện thoại không thể để trống !"),
	timeStart: yup.date().required("Hãy chọn thời gian mở cửa !"),
	timeEnd: yup.date().required("Hãy chọn thời gian đóng cửa !"),
	firstName: yup.string().required("Họ không thể để trống"),
	lastName: yup.string().required("Tên không thể để trống"),
	gender: yup.boolean().required("Giới tính không thể để trống !"),
	// .oneOf(["male", "female"], "Bạn chỉ được chọn Nam or Nữ"),
	email: yup
		.string()
		.email("Email không hợp lệ !")
		.required("Email không thể để trống !"),
	password: yup
		.string()
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
			"Mật khẩu phải chứa ít nhất 1 chữ thường , 1 chữ hoa và 1 ký tự đặc biệt"
		)
		.required("Vui lòng điền mật khẩu !"),
	passwordConfirm: yup
		.string()
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,

			"Mật khẩu phải chứa ít nhất 1 chữ thường , 1 chữ hoa và 1 ký tự đặc biệt"
		)
		.required("Vui lòng điền xác nhận mật khẩu !"),
});

const StoreRegisterPage = () => {
	const [listProvince, setListProvince] = useState([]);
	const [listDistrict, setListDistrict] = useState([]);
	const [listWard, setListWard] = useState([]);

	const defaultProvince = {
		ProvinceID: 201,
		ProvinceName: "Hà Nội",
	};

	const defaultDistrict = {
		DistrictID: 1808,
		DistrictName: "Huyện thạch thất",
	};

	const [province, setProvince] = useState(defaultProvince);
	const [district, setDistrict] = useState(defaultDistrict);
	const [ward, setWard] = useState();

	const headers = {
		Token: "6c942378-8c0f-11ee-a6e6-e60958111f48",
	};

	const getProvince = () => {
		axioss
			.get(
				"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province",
				{ headers }
			)
			.then((res) => {
				setListProvince(res.data.data);
			})
			.catch((err) => {
				toast.error("Lấy danh sách tỉnh thất bại");
			});
	};
	const getDistrict = () => {
		console.log(province);
		const data = {
			province_id: province.ProvinceID,
		};
		axioss
			.get(
				"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district",
				{
					params: data,
					headers: headers,
				}
			)
			.then((res) => {
				setListDistrict(res.data.data);
				console.log(res);
			})
			.catch((err) => {
				toast.error("Lấy danh sách quận, huyện thất bại");
			});
	};
	const getWard = () => {
		axioss
			.get(
				"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=" +
					district.DistrictID,
				{ headers }
			)
			.then((res) => {
				console.log(res);
				setListWard(res.data.data);
			})
			.catch((err) => {
				toast.error("Lấy danh sách tỉnh thất bại");
			});
	};

	const navigate = useNavigate();

	const {
		register,
		handleSubmit,
		setValue,
		formState: { errors },
	} = useForm({
		resolver: yupResolver(schema),
		mode: "onChange",
	});

	const onSubmitHandler = async (data) => {
		const newLocation = {
			email: data.email,
			...province,
			...district,
			...ward,
			address: data.address,
			description: data.description || null,
			phoneNumber: data.phoneNumber,
		};
		console.log(data);
		data.allow = true;
		data.avatar = data.avatarURL;
		data.location = newLocation;

		AuthServices.storeRegister(data)
			.then((res) => {
				console.log(res);
				toast.success(
					"Tài khoản của bạn đã đăng kí thành công, Xin vui lòng đợi xét duyệt từ Admin!"
				);
				navigate("/login");
			})
			.catch((err) => {
				toast.error(err.response.data);
			});
	};

	return (
		<>
			<div className="container mx-auto shadow-sm mb-16">
				<div className="heading mb-10 mt-10">
					<h1 className="font-bold uppercase">
						Cùng trở thành Người bán Hàng cùng FU
					</h1>
				</div>
				<form onSubmit={handleSubmit(onSubmitHandler)}>
					<div className="flex gap-x-10">
						<div className="flex flex-col gap-4 w-full">
							<div className="w-full">
								<Input label="Tên Cửa Hàng" {...register("storeName")} />
								{errors.storeName && (
									<ErrorText text={errors.storeName.message} />
								)}
							</div>
							<div className="w-full">
								<UploadImage name="avatarURL" onChange={setValue} />
								{errors.avatarURL && (
									<ErrorText text={errors.avatarURL.message} />
								)}
							</div>
							<div className="inline-block relative mb-4">
								<Select
									className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
									onChange={(value) =>
										setWard({
											WardCode: value.WardCode,
											WardName: value.WardName,
										})
									}
									label="Chọn xã"
									onClick={getWard}
								>
									{listWard.map((ward) => (
										<Option key={ward.WardCode} value={ward}>
											{ward.WardName}
										</Option>
									))}
								</Select>
								{errors.ward && (
									<ErrorText text={errors.ward.message}></ErrorText>
								)}
							</div>
							<div className="mb-4">
								<Textarea
									className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
									size="md"
									label="Địa chỉ cụ thể (Trọ, Thôn)"
									{...register("address")}
								></Textarea>
								{errors.address && (
									<ErrorText text={errors.address.message}></ErrorText>
								)}
							</div>
							<div className="w-full">
								<Textarea
									label="Mô tả"
									{...register("description")}
									className="h-[185px]"
								/>
								{errors.description && (
									<ErrorText text={errors.description.message} />
								)}
							</div>
						</div>
						<div className="flex flex-col gap-4 w-full">
							<div className="w-full">
								<Input label="Hostline" {...register("phoneNumber")} />
								{errors.phoneNumber && (
									<ErrorText text={errors.phoneNumber.message} />
								)}
							</div>
							<div className="w-full">
								<Input
									type="datetime-local"
									label="Thời gian mở cửa"
									{...register("timeStart")}
								/>
								{errors.timeStart && (
									<ErrorText text={errors.timeStart.message} />
								)}
							</div>
							<div className="w-full">
								<Input
									type="datetime-local"
									label="Thời gian đóng cửa"
									{...register("timeEnd")}
								/>
								{errors.timeEnd && <ErrorText text={errors.timeEnd.message} />}
							</div>
							<div className="w-full flex gap-x-3">
								<Input label="Họ" {...register("firstName")} />
								<Input label="Tên" {...register("lastName")} />
								{errors.firstName && (
									<ErrorText text={errors.firstName.message} />
								)}
								{errors.lastName && (
									<ErrorText text={errors.lastName.message} />
								)}
							</div>
							<div className="w-full">
								<h2 className="font-medium text-gray-700">Giới Tính</h2>
								<div className="flex gap-10">
									<Radio
										name="gender"
										value="true"
										label="Nam"
										{...register("gender")}
									/>
									<Radio
										name="gender"
										value="false"
										{...register("gender")}
										label="Nữ"
									/>
								</div>
								{errors.gender && <ErrorText text={errors.gender.message} />}
							</div>
							<div className="w-full">
								<Input label="Email" {...register("email")} />
								{errors.email && <ErrorText text={errors.email.message} />}
							</div>
							<div className="w-full">
								<Input
									type="password"
									label="Mật Khẩu"
									{...register("password")}
								/>
								{errors.password && (
									<ErrorText text={errors.password.message} />
								)}
							</div>
							<div className="w-full">
								<Input
									type="password"
									label="Xác Nhận Mật Khẩu"
									{...register("passwordConfirm")}
								/>
								{errors.passwordConfirm && (
									<ErrorText text={errors.passwordConfirm.message} />
								)}
							</div>
							<div className="mt-3">
								<Button type="submit" className="bg-primary w-full">
									Đăng ký
								</Button>
							</div>
						</div>
					</div>
				</form>
			</div>
		</>
	);
};

export default StoreRegisterPage;
