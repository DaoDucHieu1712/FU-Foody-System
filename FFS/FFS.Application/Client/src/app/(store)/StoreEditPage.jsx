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
import AuthServices from "../(public)/shared/auth.service";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import axioss from "axios";
import React, { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import { useSelector } from "react-redux";
import Loading from "../../shared/components/Loading";
import UpdateImage from "../../shared/components/form/UpdateImage";
import Cookies from "universal-cookie";
import { element } from "prop-types";

const schema = yup.object({
	storeName: yup
		.string()
		.min(8, "Tên cửa hàng phải từ 8 ký tự trở lên")
		.required("Tên cửa hàng không thể để trống !"),
	description: yup.string().required("Mô tả không thể để trống !"),
	address: yup.string().required("Địa chỉ không thể để trống !"),
	phoneNumber: yup.string().required("Số điện thoại không thể để trống !"),
	timeStart: yup.string().required("Hãy chọn thời gian mở cửa !"),
	timeEnd: yup.string().required("Hãy chọn thời gian đóng cửa !"),
});

const cookie = new Cookies();

const StoreEditPage = () => {
	const user = useSelector((state) => state.auth.userProfile);
	const [listProvince, setListProvince] = useState([]);
	const [listDistrict, setListDistrict] = useState([]);
	const [listWard, setListWard] = useState([]);
	const [selectedWard, setSelectedWard] = useState({});
	const [storeInfor, setStoreInfor] = useState();
	const [timeS, setTimeS] = useState("");
	const [timeE, setTimeE] = useState("");

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
	const getWard = async () => {
		return axioss
			.get(
				"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=" +
					district.DistrictID,
				{ headers }
			)
			.then(async (res) => {
				setListWard(res.data.data);
				return res.data.data;
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
			...province,
			...district,
			...ward,
			address: data.address,
			description: data.description || null,
			phoneNumber: data.phoneNumber,
		};
		console.log(data);
		data.UserId = cookie.get("fu_foody_id");
		data.location = newLocation;
		AuthServices.storeUpdate(storeInfor.store.id, data)
			.then((res) => {
				console.log(res);
				toast.success("Tài khoản của bạn đã cập nhật thành công");
			})
			.catch((err) => {
				toast.error(err.response.data);
			});
	};

	const GetStore = async () => {
		try {
			const res = await axios.get("/api/Store/GetStore/" + user.id);
			setStoreInfor(res); // Assuming the data you need is in res.data
			setTimeE(res.store.timeEnd);
			setTimeS(
				res.store.timeStart.length == 4
					? "0" + res.store.timeStart
					: res.store.timeStart
			);

			return res;
		} catch (error) {
			// Handle errors
			console.error("Error fetching store information:", error);
		}
	};

	useEffect(() => {
		const fetchData = async () => {
			await GetStore().then(async (infor) => {
				await getWard().then((res) => {
					const ward1 = res.find((ward) => {
						return ward.WardCode === infor.location.wardCode;
					});
					setSelectedWard(ward1);
				});
			});
		};

		fetchData();
	}, [user]);
	return (
		<>
			{storeInfor ? (
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
									<Input
										label="Tên Cửa Hàng"
										defaultValue={storeInfor.store.storeName}
										{...register("storeName")}
									/>
									{errors.storeName && (
										<ErrorText text={errors.storeName.message} />
									)}
								</div>
								<div className="w-full">
									<UpdateImage
										url={storeInfor.store.avatarURL}
										name="avatarURL"
										onChange={setValue}
									/>
									{errors.avatarURL && (
										<ErrorText text={errors.avatarURL.message} />
									)}
								</div>
								<div className="inline-block relative mb-4">
									<Select
										// selected={(element) =>
										// 	element &&
										// 	React.cloneElement(element, {
										// 		disabled: true,
										// 		className:
										// 			"flex items-center opacity-100 px-0 gap-2 pointer-events-none",
										// 	})
										// }
										className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
										onChange={(value) =>
											setWard({
												WardCode: value.WardCode,
												WardName: value.WardName,
											})
										}
										label="Chọn xã"
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
							</div>
							<div className="flex flex-col gap-4 w-full">
								<div className="w-full">
									<Input
										label="Hostline"
										{...register("phoneNumber")}
										defaultValue={storeInfor.store.phoneNumber}
									/>
									{errors.phoneNumber && (
										<ErrorText text={errors.phoneNumber.message} />
									)}
								</div>
								<div className="w-full">
									<Input
										type="time"
										label="Thời gian mở cửa"
										value={timeS}
										onInput={(e) => setTimeS(e.target.value)}
										{...register("timeStart")}
									/>

									{errors.timeStart && (
										<ErrorText text={errors.timeStart.message} />
									)}
								</div>
								<div className="w-full">
									<Input
										type="time"
										label="Thời gian đóng cửa"
										value={timeE}
										onInput={(e) => setTimeS(e.target.value)}
										{...register("timeEnd")}
									/>
									{errors.timeEnd && (
										<ErrorText text={errors.timeEnd.message} />
									)}
								</div>
								<div className="mb-4">
									<Textarea
										className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
										size="md"
										label="Địa chỉ cụ thể (Trọ, Thôn)"
										defaultValue={storeInfor.location.address}
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
										defaultValue={storeInfor.store.description}
									/>
									{errors.description && (
										<ErrorText text={errors.description.message} />
									)}
								</div>
								<div className="mt-3">
									<Button type="submit" className="bg-primary w-full">
										Cập nhật thông tin liên hệ
									</Button>
								</div>
							</div>
						</div>
					</form>
				</div>
			) : (
				<Loading />
			)}
		</>
	);
};

export default StoreEditPage;
