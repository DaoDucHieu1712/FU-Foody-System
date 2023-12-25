import propTypes from "prop-types";
import Message from "./Message";
import ChatService from "../../chat.service";
import Cookies from "universal-cookie";
import { useEffect, useRef, useState } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

var cookie = new Cookies();

const BoxMain = ({ boxid }) => {
	const url = import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL + "/chatHub";
	var connection = new HubConnectionBuilder().withUrl(url).build();

	connection.on("FuFoodySendMessage", function () {
		loadBox();
	});

	connection
		.start()
		.then()
		.catch(function (err) {
			return console.error(err.toString());
		});

	const [box, setBox] = useState();
	const [msg, setMsg] = useState("");
	const mesRef = useRef(null);

	useEffect(() => {
		if (mesRef?.current) {
			mesRef.current.scrollTop = mesRef.current.scrollHeight + 50;
		}
	}, [box?.messages]);

	const loadBox = async () => {
		await ChatService.FindById(boxid).then((res) => {
			setBox(res);
		});
	};

	useEffect(() => {
		loadBox();
	}, [boxid]);

	const handleMsg = async () => {
		if (msg.length === 0) return;
		var data = {
			chatId: boxid,
			userId: cookie.get("fu_foody_id"),
			content: msg,
		};
		await ChatService.SendMessage(data).then(() => setMsg(""));
		setMsg("");
	};

	return (
		<>
			<div className="col-span-4">
				<div className="box-name p-3 shadow-md">
					<p>
						{cookie.get("fu_foody_id") === box?.fromUserId
							? box?.toUserName
							: box?.fromUserName}
					</p>
				</div>
				<div className="box-main h-[500px] overflow-y-scroll" ref={mesRef}>
					{box?.messages?.map((item) => {
						return (
							<Message
								key={item.id}
								IsCurrentUser={cookie.get("fu_foody_id") === item.userId}
								content={item.content}
								CreatedAt={item.createdAt.replace("T", " ").slice(0, 16)}
							/>
						);
					})}
				</div>
				<div className="rounded-md p-2 flex justify-between items-center gap-x-3 border border-gray-500">
					<input
						type="text"
						placeholder="type ..."
						className="w-full outline-none"
						onChange={(e) => setMsg(e.target.value)}
						value={msg}
						onKeyDown={(e) => {
							if (e.key === "Enter") {
								handleMsg();
							}
						}}
					/>
					<div className="cursor-pointer hover:opacity-80" onClick={handleMsg}>
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
								d="M6 12L3.269 3.126A59.768 59.768 0 0121.485 12 59.77 59.77 0 013.27 20.876L5.999 12zm0 0h7.5"
							/>
						</svg>
					</div>
				</div>
			</div>
		</>
	);
};
BoxMain.propTypes = {
	boxid: propTypes.any,
};
export default BoxMain;
