import {
	Button,
	Input,
	Radio,
	Tabs,
	TabsHeader,
	TabsBody,
	Tab,
	TabPanel,
	MenuHandler,
	Menu,
	MenuList,
	MenuItem,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import ErrorText from "../../shared/components/text/ErrorText";
import UpdateImage from "../../shared/components/form/UpdateImage";
import { useSelector, useDispatch } from "react-redux";
import { setUserProfile } from "../../redux/auth";
import dayjs from "dayjs";
import moment from "moment";
import Elips from "../../shared/components/icon/Elips";
import ReportShipper from "../(public)/components/ReportShipper";
import ReportUser from "../(public)/components/ReportUser";

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
const ShipperProfile = () => {
	const dispatch = useDispatch();
	const user = useSelector((state) => state.auth.userProfile);
	const [activeTab, setActiveTab] = useState("profile");
	const [reviews, setReviews] = useState([]);

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

	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await axios.get(
					`/api/Comment/GetAllCommentByShipperId/${user.id}`
				);

				// Assuming your API response contains an array of reviews
				setReviews(response);
			} catch (error) {
				console.error("Error fetching reviews:", error);
			}
		};

		fetchData();
	}, [user.id]);

	const onSubmit = async (data) => {
		try {
			const updatedUserData = {
				...user,
				avatar: data.avatar ? data.avatar : user.avatar,
				firstName: data.firstName,
				lastName: data.lastName,
				gender: data.gender === "true",
				allow: user.allow,
				birthDay: dayjs(data.birthDay).format("YYYY-MM-DD"),
			};

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
			<Tabs value={activeTab} className="flex-col">
				<TabsHeader
					className="rounded-none border-b border-blue-gray-50 bg-transparent p-0"
					indicatorProps={{
						className:
							"bg-transparent border-b-2 border-orange-900 shadow-none rounded-none",
					}}
				>
					<Tab
						value="profile"
						onClick={() => setActiveTab("profile")}
						className={activeTab === "profile" ? "text-orange-900" : ""}
					>
						Hồ sơ của tôi
					</Tab>
					<Tab
						value="reviews"
						onClick={() => setActiveTab("reviews")}
						className={activeTab === "reviews" ? "text-orange-900" : ""}
					>
						Đánh giá ({reviews.length})
					</Tab>
				</TabsHeader>
				<TabsBody>
					<TabPanel value="reviews">
						{reviews.length === 0 ? (
							<p className="text-center text-gray-500">
								Shipper này chưa có đánh giá nào.
							</p>
						) : (
							reviews.map((review, index) => (
								<div key={index} className="mb-4 relative">
									{/* Three dots menu icon */}

									<div className="absolute top-2 right-3">
												<Menu>
													<MenuHandler>
														<button
															type="button"
															data-te-ripple-init
															data-te-ripple-color="light"
														>
															<Elips />
														</button>
													</MenuHandler>
													<MenuList>
													<ReportUser uId={review.userId} sId={user.id}></ReportUser>
													</MenuList>
												</Menu>
											</div>
									<div className="w-full mx-auto rounded-lg bg-white border border-gray-200 p-5 text-black font-light mb-6">
										<div className="w-full flex items-center">
											<div className="flex items-center mb-4">
												<img
													className="w-10 h-10 me-2 rounded-full"
													src={review.avatar}
													alt=""
												/>
												<div className="font-medium dark:text-white">
													<p>
														{review.username}{" "}
														<span className="block text-xs text-gray-500 dark:text-gray-400">
															{moment(review.createdAt).fromNow()}
														</span>
													</p>
												</div>
											</div>
										</div>
										<div className="w-full">
											<p className="text-base">{review.noteForShipper}</p>
										</div>
									</div>
								</div>
							))
						)}
					</TabPanel>

					<TabPanel value="profile">
						{user && (
							<div className="grid grid-cols-5 gap-4 ">
								<div className="flex flex-col col-span-5  px-3">
									<div className=" p-3">
										<p className="text-gray-400 text-sm">
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
													<span className="font-semibold text-gray-500">
														Tên
													</span>
													<div className="flex w-full col-span-2 gap-x-3">
														<div>
															<Input
																label="First Name"
																{...register("firstName")}
																defaultValue={user.firstName}
															/>
															{errors.firstName && (
																<ErrorText
																	text={errors.firstName.message}
																></ErrorText>
															)}
														</div>

														<div>
															<Input
																label="Last Name"
																{...register("lastName")}
																defaultValue={user.lastName}
															/>
															{errors.lastName && (
																<ErrorText
																	text={errors.lastName.message}
																></ErrorText>
															)}
														</div>
													</div>
												</div>

												<div className="grid grid-cols-3">
													<span className="font-semibold text-gray-500 ">
														Email
													</span>
													<p className="col-span-2">
														{user.email}{" "}
														<a href="#" className="text-blue-500">
															Thay đổi
														</a>
													</p>
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
													<span className="font-semibold text-gray-500">
														Giới tính
													</span>
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
													<span className="font-semibold text-gray-500">
														Ngày sinh
													</span>
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
							</div>
						)}
					</TabPanel>
				</TabsBody>
			</Tabs>
		</>
	);
};

export default ShipperProfile;
