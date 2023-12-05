import { useState } from "react";
import propTypes from "prop-types";
import {
	Button,
	Dialog,
	Input,
	Option,
	Select,
	Textarea,
} from "@material-tailwind/react";
import { set, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import ErrorText from "../../../../../shared/components/text/ErrorText";
import axios from "../../../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import Cookies from "universal-cookie";
import axioss from "axios";

const cookies = new Cookies();

const regexPhoneNumber = /(0[3|5|7|8|9])+([0-9]{8})\b/g;
const schema = yup.object({
	address: yup.string().required("Hãy ghi thêm thông tin!"),
	description: yup.string().nullable(),
	phoneNumber: yup
		.string()
		.matches(regexPhoneNumber, "Vui lòng nhập đúng định dạng số điện thoại!")
		.required("Hãy nhập số điện thoại!"),
	receiver: yup.string().required("Hãy nhập tên người nhận!"),
});

const AddLocation = ({ reload, user }) => {
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

	const {
		register,
		setValue,
		handleSubmit,
		formState: { errors },
	} = useForm({
		resolver: yupResolver(schema),
		mode: "onSubmit",
	});

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

	const [open, setOpen] = useState(false);
	const handleOpen = () => setOpen((cur) => !cur);

	const onSubmit = async (data) => {
		var email = cookies.get("fu_foody_email");
		try {
			const newLocation = {
				email: email,
				...province,
				...district,
				...ward,
				address: data.address,
				description: data.description || null,
				receiver: data.receiver,
				phoneNumber: data.phoneNumber,
				userId: user.id,
			};
			console.log(newLocation);
			axios
				.post(`/api/Location/AddLocation`, newLocation)
				.then(() => {
					toast.success("Thêm địa chỉ mới thành công!");
					reload();
					setOpen(false);
					setValue("ward", null);
					setValue("address", null);
					setValue("description", null);
					setValue("phoneNumber", null);
					setValue("receiver", null);
				})
				.catch((error) => {
					toast.error("Thêm địa chỉ thất bại!");
					setOpen(false);
					console.log(error);
				});
		} catch (error) {
			console.error("Error add location: ", error);
		}
	};

	return (
		<>
			<Button
				className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
				onClick={handleOpen}
			>
				+ Thêm địa chỉ mới
			</Button>
			<Dialog
				size="md"
				open={open}
				handler={handleOpen}
				className="bg-transparent shadow-none"
			>
				<form
					className="form bg-white rounded p-4 mb-4"
					onSubmit={handleSubmit(onSubmit)}
				>
					<p className="font-bold text-2xl text-center mb-4">
						{user.Role == "STOREOWNER"
							? "Thêm địa chỉ mới cho quán"
							: "Thêm địa chỉ mới"}
					</p>
					<div className="mb-4">
						<Input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
							type="text"
							label="Người nhận"
							{...register("receiver")}
						></Input>
						{errors.receiver && (
							<ErrorText text={errors.receiver.message}></ErrorText>
						)}
					</div>
					<div className="mb-4">
						<Input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
							type="text"
							label="Số điện thoại"
							{...register("phoneNumber")}
						></Input>
						{errors.phoneNumber && (
							<ErrorText text={errors.phoneNumber.message}></ErrorText>
						)}
					</div>
					{/* <div className="mb-4">
						<Select
							className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
							onChange={(value) =>
								setProvince({
									ProvinceID: value.ProvinceID,
									ProvinceName: value.ProvinceID,
								})
							}
							label="Chọn Tỉnh"
							onClick={getProvince}
						>
							{listProvince.map((province) => (
								<Option key={province.ProvinceID} value={province}>
									{province.ProvinceName}
								</Option>
							))}
						</Select>
					</div>
					<div className="mb-4">
						<Select
							className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
							onChange={(value) =>
								setDistrict({
									DistrictID: value.DistrictID,
									DistrictName: value.DistrictID,
								})
							}
							label="Chọn Huyện"
							onClick={getDistrict}
						>
							{listDistrict.map((district) => (
								<Option key={district.DistrictID} value={district}>
									{district.DistrictName}
								</Option>
							))}
						</Select>
					</div> */}
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
						{errors.ward && <ErrorText text={errors.ward.message}></ErrorText>}
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
					<div className="mb-4">
						<Textarea
							className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
							size="md"
							label="Thông tin ghi chú thêm"
							{...register("description")}
						></Textarea>
					</div>
					<button
						type="submit"
						className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center"
					>
						Thêm địa chỉ
					</button>
				</form>
			</Dialog>
		</>
	);
};
AddLocation.propTypes = {
	reload: propTypes.any.isRequired,
	user: propTypes.any.isRequired,
};
export default AddLocation;
