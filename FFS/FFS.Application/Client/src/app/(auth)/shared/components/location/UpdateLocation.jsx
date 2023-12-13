import { useEffect, useState } from "react";
import propTypes from "prop-types";
import {
	Dialog,
	Input,
	Option,
	Select,
	Textarea,
} from "@material-tailwind/react";
import { useForm } from "react-hook-form";
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

const UpdateLocation = ({ item, reload }) => {
	console.log(item);
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
			};
			axios
				.put(`/api/Location/UpdateLocation/${item.id}`, newLocation)
				.then(() => {
					toast.success("Cập nhật địa chỉ thành công!");
					reload();
					setOpen(false);
				})
				.catch((error) => {
					toast.error("Cập nhật địa chỉ thất bại!");
					console.log(error);
				});
		} catch (error) {
			console.error("Error update location: ", error);
		}
	};

	return (
		<>
			<p
				className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600"
				onClick={handleOpen}
			>
				Cập nhật
			</p>
			<Dialog
				size="md"
				open={open}
				handler={handleOpen}
				className="bg-transparent shadow-none"
			>
				<form
					className="bg-white rounded px-4 py-4 mb-4"
					onSubmit={handleSubmit(onSubmit)}
				>
					<p className="font-bold text-2xl text-center mb-4">
						Chỉnh sửa địa chỉ
					</p>
					<p></p>
					<div className="mb-4">
						<Input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
							type="text"
							label="Người nhận"
							defaultValue={item.receiver}
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
							defaultValue={item.phoneNumber}
							{...register("phoneNumber")}
						></Input>
						{errors.phoneNumber && (
							<ErrorText text={errors.phoneNumber.message}></ErrorText>
						)}
					</div>
					<div className="mb-4">
						<input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none"
							type="text"
							placeholder="Thành phố Hà Nội"
						></input>
					</div>
					<div className="mb-4">
						<input
							className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none"
							type="text"
							placeholder="Huyện Thạch Thất"
							readOnly
						></input>
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
						{errors.ward && <ErrorText text={errors.ward.message}></ErrorText>}
					</div>
					<div className="mb-4">
						<Textarea
							className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
							size="md"
							label="Địa chỉ cụ thể (Trọ, Thôn)"
							defaultValue={item.address}
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
							defaultValue={item.description}
							{...register("description")}
						></Textarea>
						{errors.description && (
							<ErrorText text={errors.description.message}></ErrorText>
						)}
					</div>
					<button
						type="submit"
						className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center"
					>
						Cập nhật địa chỉ
					</button>
				</form>
			</Dialog>
		</>
	);
};
UpdateLocation.propTypes = {
	item: propTypes.any.isRequired,
	reload: propTypes.any.isRequired,
	wardList: propTypes.any.isRequired,
};
export default UpdateLocation;
