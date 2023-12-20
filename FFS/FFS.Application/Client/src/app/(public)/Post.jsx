import {
	Input,
	Select,
	Option,
	Card,
	CardHeader,
	CardBody,
	CardFooter,
	Typography,
	Button,
	IconButton,
} from "@material-tailwind/react";
import { useEffect, useState } from "react";
import React from "react";
import AddPost from "../(auth)/shared/components/post/AddPost";
import axiosConfig from "../../shared/api/axiosConfig";
import { useNavigate } from "react-router-dom";
import LastestPost from "./components/post/LastestPost";
import ArrowRight from "../../shared/components/icon/ArrowRight";
import ArrowLeft from "../../shared/components/icon/ArrowLeft";
import { useSelector } from "react-redux";
import ArrowBlog from "../../shared/components/icon/ArrowBlog";
import FlashSalePost from "./components/post/FlashSalePost";
import dayjs from "dayjs";
import Calender from "../../shared/components/icon/Calender";
import UserBlog from "../../shared/components/icon/UserBlog";
import CommentIcon from "../../shared/components/icon/CommentIcon";

const Post = () => {
	const accesstoken = useSelector((state) => state.auth.accessToken);
	const userInfo = useSelector((state) => state.auth.userProfile);
	const isUser = userInfo && userInfo.Role === "USER";

	const [active, setActive] = React.useState(1);
	const [posts, setPosts] = React.useState([]);
	const [postTitle, setPostTitle] = useState("");
	const [orderBy, setOrderBy] = useState("newest");
	const [pageNumber, setPageNumber] = useState(1);
	const [pageSize, setPageSize] = useState(4);
	const [totalPages, setTotalPages] = useState(1);

	let navigate = useNavigate();

	const handleReadMoreClick = (postId) => {
		navigate(`/post-details/${postId}`);
	};

	const getItemProps = (index) => ({
		variant: active === index ? "filled" : "text",
		onClick: () => {
			setActive(index);
			setPageNumber(index);
		},
		className: `rounded-full ${active === index ? "bg-primary" : ""}`,
	});

	const next = () => {
		if (active < totalPages) {
			setActive(active + 1);
			setPageNumber(pageNumber + 1);
		}
	};

	const prev = () => {
		if (active > 1) {
			setActive(active - 1);
			setPageNumber(pageNumber - 1);
		}
	};

	const fetchPostList = () => {
		axiosConfig
			.get(
				`/api/Post/GetListPosts?PostTitle=${postTitle}&OrderBy=${orderBy}&PageNumber=${pageNumber}&PageSize=${pageSize}`
			)
			.then((response) => {
				setPosts(response.entityPost);
				setTotalPages(response.metadata.totalPages);
			})
			.catch((error) => {
				console.error("Error fetching posts: ", error);
			});
	};

	useEffect(() => {
		fetchPostList();
	}, [postTitle, orderBy, pageNumber, pageSize]);

	const reloadPost = () => {
		fetchPostList();
	};

	return (
		<>
			<div className="container mt-8 p-11">
				<div className="grid grid-cols-1 md:grid-cols-3 gap-x-14">
					{/* Column 1 */}
					<div className="md:col-span-1 sticky top-0 h-screen">
						<LastestPost></LastestPost>
						<FlashSalePost></FlashSalePost>
					</div>
					{/* Column 2 */}
					<div className="md:col-span-2 ">
						<div className="flex items-center justify-between">
							<div className="w-72">
								<Input
									labelProps={{
										className: "hidden",
									}}
									className="rounded-none border border-gray-700 bg-white text-gray-900  placeholder:text-gray-500 focus:!border-gray-900 focus:!border-t-gray-900 focus:ring-gray-900/10 "
									icon={<i className="fas fa-search" />}
									placeholder="Tìm kiếm"
									value={postTitle}
									onChange={(e) => setPostTitle(e.target.value)}
								/>
							</div>

							<div className="sort_blog">
								<Select
									labelProps={{
										className: "hidden",
									}}
									className="rounded-none border border-gray-400 bg-white text-gray-900  placeholder:text-gray-500 focus:!border-gray-900 focus:!border-t-gray-900 focus:ring-gray-900/10 "
									value={orderBy}
									onChange={(value) => setOrderBy(value)}
								>
									<Option value="newest">Mới nhất</Option>
									<Option value="oldest">Cũ nhất</Option>
								</Select>
							</div>
							{accesstoken && isUser ? (
								<AddPost reloadPost={reloadPost}></AddPost>
							) : null}
						</div>
						<div className="list_post mt-8">
							<div className="grid grid-cols-2 gap-4">
								{posts && posts.length != 0 ? (
									posts.map((post) => (
										<Card
											key={post.id}
											className="w-96 border rounded-none shadow-none"
										>
											<CardHeader
												floated={false}
												className="rounded-none mt-5 mx-5"
											>
												<img
													src={post.image}
													alt="card-image"
													className="h-50 w-full object-cover object-center"
													style={{ height: "230px" }}
												/>
											</CardHeader>
											<CardBody>
												<div className="flex items-center space-x-5 mb-3">
													<div className="flex items-center space-x-1">
														<UserBlog></UserBlog>
														<span className="text-sm">{post.username}</span>
													</div>
													<div className="flex items-center space-x-1">
														<Calender></Calender>
														<span className="text-sm">
															{" "}
															{dayjs(post.createdAt).format(
																"D [Tháng] M, YYYY"
															)}
														</span>
													</div>
													<div className="flex items-center space-x-1">
														<CommentIcon></CommentIcon>
														<span className="text-sm">
															{" "}
															{post.commentNumber}
														</span>
													</div>
												</div>
												<Typography
													variant="h5"
													color="blue-gray"
													className="mb-2 two-line-text min-h-[30px]"
													style={{
														display: "-webkit-box",
														WebkitLineClamp: 1,
														WebkitBoxOrient: "vertical",
														overflow: "hidden",
													}}
												>
													{post.title}
												</Typography>
												<Typography
													className="three-line-text min-h-[70px]"
													style={{
														display: "-webkit-box",
														WebkitLineClamp: 3,
														WebkitBoxOrient: "vertical",
														overflow: "hidden",
													}}
												>
													<div
														dangerouslySetInnerHTML={{ __html: post.content }}
													></div>
												</Typography>
											</CardBody>
											<CardFooter className="pt-0">
												<Button
													variant="outlined"
													className="uppercase rounded-none flex items-center gap-1"
													color="deep-orange"
													onClick={() => handleReadMoreClick(post.id)}
												>
													Đọc Thêm
													<ArrowBlog></ArrowBlog>
												</Button>
											</CardFooter>
										</Card>
									))
								) : (
									<Typography variant="h5" className="mt-5 ml-5">
										Không có bài viết nào!
									</Typography>
								)}
							</div>

							<div className="flex items-center justify-center gap-4 mt-7">
								<Button
									variant="text"
									className="flex items-center gap-2 rounded-full"
									onClick={prev}
									disabled={active === 1}
								>
									<ArrowLeft /> Previous
								</Button>
								<div className="flex items-center gap-2">
									{Array.from({ length: totalPages }, (_, index) => (
										<IconButton key={index + 1} {...getItemProps(index + 1)}>
											{index + 1}
										</IconButton>
									))}
								</div>
								<Button
									variant="text"
									className="flex items-center gap-2 rounded-full"
									onClick={next}
									disabled={active === totalPages}
								>
									Next <ArrowRight />
								</Button>
							</div>
						</div>
					</div>
				</div>
			</div>
		</>
	);
};

export default Post;
