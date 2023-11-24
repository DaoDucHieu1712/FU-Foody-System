import React from "react";
import { useParams } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import moment from "moment";
import "moment/dist/locale/vi";
import {
	Tabs,
	TabsHeader,
	TabsBody,
	Tab,
	TabPanel,
	Avatar,
	Button,
	Typography,
  Menu, MenuHandler, MenuList, MenuItem,
} from "@material-tailwind/react";
import Elips from "../../shared/components/icon/Elips";

const ShipperDetailsPage = () => {
	moment.locale("vi");
	const { id } = useParams();
	const [activeTab, setActiveTab] = useState("reviews");
	const [reviews, setReviews] = useState([]);
	const [shipperData, setShipperData] = useState([]);

	const GetShipperInformation = async () => {
		try {
			axios
				.get(`/api/Authenticate/GetShipperById/${id}`)
				.then((response) => {
					console.log(response);
					setShipperData(response);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	useEffect(() => {
		GetShipperInformation();
	}, [id]);

	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await axios.get(
					`/api/Comment/GetAllCommentByShipperId/${id}`
				);

				// Assuming your API response contains an array of reviews
				setReviews(response);
			} catch (error) {
				console.error("Error fetching reviews:", error);
			}
		};

		fetchData();
	}, [id]);

	return (
		<>
			<div className="container mx-auto  px-20 py-8">
				<div className="grid grid-cols-4 sm:grid-cols-12 gap-6 px-4">
					<div className="col-span-4 sm:col-span-3 sticky top-0 h-screen">
						<div className="bg-white shadow rounded-lg p-6">
							<div className="flex flex-col items-center">
								<img
									src={shipperData.avatar}
									className="w-32 h-32 bg-gray-300 rounded-full mb-3 shrink-0"
								></img>
								<h1 className="text-xl font-bold">
									{shipperData.firstName} {shipperData.lastName}
								</h1>

								<div className="mt-3 flex flex-wrap gap-4 justify-center">
									<a
										href="#"
										className="bg-primary hover:bg-orange-900 text-white py-2 px-4 rounded"
									>
										Nhắn tin
									</a>
									<a
										href="#"
										className="bg-gray-300 hover:bg-gray-400 text-gray-700 py-2 px-4 rounded"
									>
										Báo cáo
									</a>
								</div>
							</div>
							<hr className="my-6 border-t border-gray-300" />
							<div className="flex flex-col">
								<span className="text-gray-600 uppercase font-bold tracking-wider mb-2">
									Hoạt động cá nhân
								</span>
								<ul>
									{/* <li className="mb-2">Đã đăng {user.totalPost} bài viết</li>
									<li className="mb-2">
										Đã có {user.totalRecentComments} bình luận trong tuần qua
									</li> */}
								</ul>
							</div>
						</div>
					</div>
					<div className="col-span-4 sm:col-span-9">
						{/* Tabs */}
						<Tabs value={activeTab} className="flex-col">
							<TabsHeader
								className="rounded-none border-b border-blue-gray-50 bg-transparent p-0"
								indicatorProps={{
									className:
										"bg-transparent border-b-2 border-orange-900 shadow-none rounded-none",
								}}
							>
								<Tab
									value="reviews"
									onClick={() => setActiveTab("reviews")}
									className={activeTab === "reviews" ? "text-orange-900" : ""}
								>
									Đánh giá ({reviews.length})
								</Tab>
								<Tab
									value="overview"
									onClick={() => setActiveTab("overview")}
									className={activeTab === "overview" ? "text-orange-900" : ""}
								>
									Tổng quan
								</Tab>
							</TabsHeader>
							<TabsBody>
								<TabPanel value="reviews">
									{reviews.map((review, index) => (
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
														<MenuItem>Báo cáo đánh giá</MenuItem>
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
									))}
								</TabPanel>
							</TabsBody>
						</Tabs>
					</div>
				</div>
			</div>
		</>
	);
};

export default ShipperDetailsPage;
