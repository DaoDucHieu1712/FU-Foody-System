import { useQuery } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import Cookies from "universal-cookie";
import ChatService from "./shared/chat.service";
import { chatActions } from "./shared/chatSlice";
import BoxMain from "./shared/components/chat/BoxMain";
import { HubConnectionBuilder } from "@microsoft/signalr";

var cookie = new Cookies();

const ChatPage = () => {
	const chatSelector = useSelector((state) => state.chat);
	const [currentBox, setCurrentBox] = useState();
	const dispatch = useDispatch();
	const url = import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL + "/chatHub";
	var connection = new HubConnectionBuilder().withUrl(url).build();

	connection.on("FuFoodyCreateBox", function (userbox) {
		var currentUser = cookie.get("fu_foody_id");
		if (currentUser === userbox.toUser || currentUser === userbox.fromUser) {
			dispatch(chatActions.Update(true));
			boxsQuery.refetch();
		}
	});

	connection
		.start()
		.then()
		.catch(function (err) {
			return console.error(err.toString());
		});

	const boxsQuery = useQuery({
		queryKey: ["boxs-by-user"],
		queryFn: async () => {
			return ChatService.getAllByUserId(cookie.get("fu_foody_id"));
		},
	});

	useEffect(() => {
		console.log(currentBox);
	}, []);

	const ShowChatHandler = () => {
		dispatch(chatActions.Update(!chatSelector.IsShow));
	};

	const handleCloseChat = () => {
		dispatch(chatActions.Update(!chatSelector.IsShow));
	};

	return (
		<>
			<div
				className={`fixed ${
					chatSelector.IsShow ? "hidden" : ""
				} bottom-1 right-1 py-3 px-6 border bg-gray-300 border-gray-300 shadow-md flex items-center gap-x-3 cursor-pointer hover:opacity-70`}
				onClick={ShowChatHandler}
			>
				<div className="text-primary font-bold ">
					<svg
						xmlns="http://www.w3.org/2000/svg"
						fill="none"
						viewBox="0 0 24 24"
						strokeWidth={1.5}
						stroke="currentColor"
						className="w-6 h-6"
					>
						<path
							strokeLinecap="round"
							strokeLinejoin="round"
							d="M7.5 8.25h9m-9 3H12m-9.75 1.51c0 1.6 1.123 2.994 2.707 3.227 1.129.166 2.27.293 3.423.379.35.026.67.21.865.501L12 21l2.755-4.133a1.14 1.14 0 01.865-.501 48.172 48.172 0 003.423-.379c1.584-.233 2.707-1.626 2.707-3.228V6.741c0-1.602-1.123-2.995-2.707-3.228A48.394 48.394 0 0012 3c-2.392 0-4.744.175-7.043.513C3.373 3.746 2.25 5.14 2.25 6.741v6.018z"
						/>
					</svg>
				</div>
				<span className="text-primary text-bold">Chat</span>
			</div>
			<div
				className={`fixed ${
					!chatSelector.IsShow ? "hidden" : ""
				} bottom-0 right-0 w-[750px] [550px] border rounded-lg border-gray-500 bg-white pb-4`}
			>
				<div className="flex justify-between border-b border-gray-500">
					<div className="text-primary text-xl font-bold px-3 py-1">Chat</div>
					<span
						className="text-primary text-xl font-bold cursor-pointer px-3 py-1"
						onClick={handleCloseChat}
					>
						x
					</span>
				</div>
				<div className="main grid grid-cols-6 gap-x-2">
					<div className="chat-list col-span-2 border-r w-full h-full border-gray-500 p-1">
						<div className="p-1">
							<input
								size="md"
								placeholder="Tìm kiếm"
								className="w-full border p-1 outline-none"
							/>
						</div>
						<div className="h-[500px] flex flex-col gap-y-1 p-1 overflow-y-scroll">
							{boxsQuery.data?.map((item) => {
								return (
									<div
										key={item.id}
										className={`${
											currentBox === item.id ? "bg-gray-300" : ""
										} flex gap-x-3 rounded-md cursor-pointer hover:bg-gray-300 p-2`}
										onClick={() => setCurrentBox(item.id)}
									>
										<div className="item-avt">
											<img
												className="w-10 h-10 rounded-full"
												src={
													cookie.get("fu_foody_id") === item.fromUserId
														? item.toUserImage
														: item.fromUserImage
												}
												alt="chat-avt"
											/>
										</div>
										<div className="flex items-center justify-center max-w-[120px]">
											<p className="">
												{cookie.get("fu_foody_id") === item.fromUserId
													? item.toUserName
													: item.fromUserName}
											</p>
										</div>
									</div>
								);
							})}
						</div>
					</div>
					{currentBox ? (
						<BoxMain boxid={currentBox} />
					) : (
						<div className="p-3 whitespace-nowrap text-primary">
							Chọn box để chat nào ..
						</div>
					)}
				</div>
			</div>
		</>
	);
};

export default ChatPage;
