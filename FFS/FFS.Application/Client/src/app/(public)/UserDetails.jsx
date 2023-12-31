import {
	Menu,
	MenuHandler,
	MenuList,
	Typography,
	Avatar,
	Input,
	Button,
	MenuItem,
} from "@material-tailwind/react";
import Elips from "../../shared/components/icon/Elips";
import axios from "../../shared/api/axiosConfig";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import CookieService from "../../shared/helper/cookieConfig";
import moment from "moment";
import "moment/dist/locale/vi";
import { useDispatch } from "react-redux";
import ReportUser from "./components/ReportUser";
import ChatService from "../(auth)/shared/chat.service";
import { chatActions } from "../(auth)/shared/chatSlice";
import Cookies from "universal-cookie";
import ViewLikePost from "../(auth)/shared/components/post/ViewLikePost";
import UpdatePost from "../(auth)/shared/components/post/UpdatePost";
import DeletePost from "../(auth)/shared/components/post/DeletePost";
import dayjs from "dayjs";

const cookies = new Cookies();

moment.locale("vi");
const UserDetails = () => {
	const { id } = useParams();
	const dispatch = useDispatch();
	const accesstoken = useSelector((state) => state.auth.accessToken);
	const userInfo = useSelector((state) => state.auth.userProfile);
	const avartar = userInfo?.avatar;
	const username = userInfo?.firstName + " " + userInfo?.lastName;
	const [user, setUser] = useState([]);
	const [isReact, setIsReact] = useState(false);
	const [postIdToCheck, setPostIdToCheck] = useState(null);
	const [openComment, setOpenComment] = useState(false);
	const userId = CookieService.getToken("fu_foody_id");
	const [commentTxt, setCommentTxt] = useState("");
	const [post, setPost] = useState("");
	const isUser = userId === id;
	const GetUserInformation = async () => {
		try {
			axios
				.get(`/api/Authenticate/GetUserInformation/${id}`)
				.then((response) => {
					console.log(response);
					setUser(response);
					response.posts.forEach((post) => {
						setPostIdToCheck(post.id);
						setPost(post);

						const checkReact = post.reactPosts.some(
							(reactPost) =>
								reactPost.userId === userId && reactPost.isLike === true
						);

						if (checkReact) {
							setIsReact(true);
						}
					});
				});
		} catch (error) {
			console.error("An error occurred", error);
		}
	};

	const reloadPost = () => {
		GetUserInformation();
	};
	const handleComment = (postId) => {
		const data = {
			Content: commentTxt,
			UserId: userId,
			PostId: postId,
		};
		try {
			axios
				.post("/api/Post/CommentPost", data)
				.then(() => {
					setCommentTxt("");
					setOpenComment(true);
					GetUserInformation();
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.error("Error occur: ", error);
		}

		console.log(commentTxt);
	};

	const handleReactPost = async (postId) => {
		try {
			const dataPost = {
				userId: userId,
				postId: postId,
			};
			await axios.post("/api/Post/ReactPost", dataPost);

			// Update the state to reflect the change
			setPost((prevPost) => {
				// const hasReacted = prevPost.reactPosts.some(
				// 	(react) => react.userId === userId && react.postId === postId
				// );
				console.log(isReact);
				console.log(prevPost.reactPosts);

				const updatedReactPosts = isReact
					? [
							...prevPost.reactPosts.map((react) =>
								react.userId === userId && react.postId === postId
									? { ...react, isLike: false }
									: react
							),
					  ]
					: [
							...prevPost.reactPosts,
							{
								userId: userId,
								avartar: avartar, // Make sure you have avartar defined
								username: username, // Make sure you have username defined
								postId: postId,
								isLike: true,
							},
					  ];
				console.log(prevPost.reactPosts);

				return {
					...prevPost,
					reactPosts: updatedReactPosts,
					likeNumber: isReact
						? prevPost.likeNumber - 1
						: prevPost.likeNumber + 1,
				};
			});

			// Update the isReact state
			setIsReact((prevIsReact) => !prevIsReact);
		} catch (error) {
			console.error("Error occur: ", error);
		}
	};

	useEffect(() => {
		GetUserInformation();
	}, [id, isReact]);
	const handleopenComent = () => {
		setOpenComment((cur) => !cur);
	};

	const handleCreateBoxChat = async () => {
		if (cookies.get("fu_foody_id") === id) {
			return;
		}
		dispatch(chatActions.Update(true));
		await ChatService.CreateChatBox({
			fromUserId: cookies.get("fu_foody_id"),
			toUserId: id,
		});
	};

	return (
		<>
			<div className="container mx-auto  px-20 py-8">
				<div className="grid grid-cols-4 sm:grid-cols-12 gap-6 px-4">
					<div className="col-span-4 sm:col-span-3 sticky top-0 h-screen">
						<div className="bg-white shadow rounded-lg p-6">
							<div className="flex flex-col items-center">
								<img
									src={user.avatar}
									className="w-32 h-32 bg-gray-300 rounded-full mb-3 shrink-0"
								></img>
								<h1 className="text-xl font-bold">{user.userName}</h1>

								<div className="mt-3 flex flex-wrap gap-4 justify-center items-center">
									
									{userId !== null && userId !== undefined && userId !== id ? (
										<>
											<a
												onClick={handleCreateBoxChat}
												className="bg-primary hover:bg-orange-900 text-white py-2 px-4 rounded"
											>
												Nhắn tin
											</a>
											<a>
												<ReportUser uId={userId} sId={id}></ReportUser>
											</a>
										</>
									) : null}
								</div>
							</div>
							<hr className="my-6 border-t border-gray-300" />
							<div className="flex flex-col">
								<span className="text-gray-600 uppercase font-bold tracking-wider mb-2">
									Hoạt động cá nhân
								</span>
								<ul>
									<li className="mb-2">Đã đăng {user.totalPost} bài viết</li>
									<li className="mb-2">
										Đã có {user.totalRecentComments} bình luận trong tuần qua
									</li>
								</ul>
							</div>
						</div>
					</div>
					<div className="col-span-4 sm:col-span-9">
						{user.posts && user.posts.length > 0 ? (
							user.posts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt)).map((post) => (
								<div key={post.id} className="bg-white shadow rounded-lg mb-4">
									<div className="md:col-span-2 py-4 px-6">
										{/* POST AUTHOR */}
										<div className="flex justify-between items-center py-3">
											<div className="flex items-center gap-2 mt-2">
												<Avatar src={post.avatar} />
												<div>
													<Typography variant="h6">{post.username}</Typography>
													<Typography
														variant="small"
														color="gray"
														className="font-normal"
													>
														{dayjs(post.createdAt).format("D [Tháng] M, YYYY")}
													</Typography>
												</div>
											</div>
											<div className="flex items-center space-x-2">
												<Menu>
													<MenuHandler>
														<button
															type="button"
															data-te-ripple-init
															data-te-ripple-color="light"
														>
															<Elips></Elips>
														</button>
													</MenuHandler>

													{userId === post.userId  ? (
												<MenuList>
													<UpdatePost
														post={post}
														reloadPost={reloadPost}
													></UpdatePost>
													<DeletePost
														post={post}
														reloadPost={reloadPost}
													></DeletePost>
												</MenuList>
											) : (
												<MenuList>
													<ReportUser
														uId={userId}
														sId={post.userId}
													></ReportUser>
												</MenuList>
											)}
													
												</Menu>
											</div>
										</div>

										{/* END POST AUTHOR */}

										{/* POST CONTENT */}
										<div className="text-justify py-2">
											<div
												dangerouslySetInnerHTML={{ __html: post.content }}
											></div>
											<img
												src={post.image}
												className="w-full h-[450px] object-cover mt-3"
											/>
										</div>
										{/* END POST CONTENT */}

										{/* POST EVENTS */}
										<div className=" py-2">
											<div className="flex items-center justify-between">
												<div className="flex flex-row-reverse items-center">
													<ViewLikePost
														likeNumber={post.likeNumber}
														likedBy={post.reactPosts}
													></ViewLikePost>
												</div>
												<div className="text-gray-500">
													<span onClick="" style={{ cursor: "pointer" }}>
														{post.commentNumber} bình luận
													</span>
												</div>
											</div>
										</div>
										{/* END POST EVENTS */}

										{/* POST ACTION */}
										<div className="py-2 px-8">
											<div className="border border-gray-200 border-l-0 border-r-0 py-1">
												<div className="flex space-x-2">
													<div
														onClick={() => handleReactPost(post.id)}
														className="w-1/2 flex space-x-2 justify-center items-center hover:bg-gray-100 text-xl py-2 rounded-lg cursor-pointer text-gray-500"
													>
														{postIdToCheck === post.id && isReact ? (
															<span
																className="flex gap-1 items-center text-orange-900
                        text-sm font-semibold"
															>
																<svg
																	xmlns="http://www.w3.org/2000/svg"
																	height="1em"
																	viewBox="0 0 512 512"
																>
																	<path
																		d="M47.6 300.4L228.3 469.1c7.5 7 17.4 10.9 27.7 10.9s20.2-3.9 27.7-10.9L464.4 300.4c30.4-28.3 47.6-68 47.6-109.5v-5.8c0-69.9-50.5-129.5-119.4-141C347 36.5 300.6 51.4 268 84L256 96 244 84c-32.6-32.6-79-47.5-124.6-39.9C50.5 55.6 0 115.2 0 185.1v5.8c0 41.5 17.2 81.2 47.6 109.5z"
																		fill="pink"
																	/>
																</svg>
																Thích
															</span>
														) : (
															<span className="text-sm font-semibold">
																Thích
															</span>
														)}
													</div>
													<div className="w-1/2 flex space-x-2 justify-center items-center hover:bg-gray-100 text-xl py-2 rounded-lg cursor-pointer text-gray-500">
														<i className="bx bx-comment"></i>
														<span className="text-sm font-semibold">
															Bình luận
														</span>
													</div>
												</div>
											</div>
										</div>
										{/* END POST ACTION */}
										<Typography
											variant="small"
											className="cursor-pointer hover:text-orange-900 py-2"
											onClick={handleopenComent}
										>
											<i className="fal fa-angle-double-down p-1"></i>Xem bình
											luận
										</Typography>
										{/* SUB COMMENT */}
										{openComment ? (
											<div className="mt-1">
												{post.comments.map((comment) => (
													<div
														key={comment.id}
														className="flex justify-start gap-2 mb-4"
													>
														<img
															src={comment.avartar}
															alt="image 1"
															className="h-14 w-14 rounded-full object-cover"
														></img>
														<div>
															<div className="flex gap-1">
																<Typography
																	variant="small"
																	className="font-bold"
																>
																	{comment.userName}
																</Typography>
																<Typography variant="small">
																	- {moment(comment.commentDate).fromNow()}
																</Typography>
															</div>
															<Typography variant="paragraph">
																{comment.content}
															</Typography>
															<div className="flex gap-2">
																<Typography
																	variant="small"
																	className="cursor-pointer hover:text-orange-900"
																>
																	<i className="fal fa-heart pr-1"></i>Thích
																</Typography>
																<Typography
																	variant="small"
																	className="cursor-pointer hover:text-orange-900"
																>
																	<i className="fal fa-angle-double-right fa-rotate-90 p-1"></i>
																	Xem bình luận
																</Typography>
															</div>
														</div>
													</div>
												))}
											</div>
										) : null}
										{/* END SUB COMMENT */}
										<div className="px-8 mb-4">
											{accesstoken ? (
												<div className="flex space-x-2">
													{userInfo && (
														<img
															src={userInfo.avatar}
															alt="Profile picture"
															className="w-9 h-9 rounded-full"
														/>
													)}
													<div className="flex-1 flex bg-gray-100 ">
														<Input
															type="text"
															placeholder="Viết bình luận..."
															name="content"
															value={commentTxt}
															onChange={(e) => setCommentTxt(e.target.value)}
														/>
													</div>
													<Button
														className="h-1/2 bg-primary"
														onClick={() => handleComment(post.id)}
													>
														Bình Luận
													</Button>
												</div>
											) : null}
										</div>

										{/* // END POST */}
									</div>
								</div>
							))
						) : (
							<div className="bg-white shadow rounded-lg p-6 text-center">
								Chưa có bài viết
							</div>
						)}
					</div>
				</div>
			</div>
		</>
	);
};
export default UserDetails;
