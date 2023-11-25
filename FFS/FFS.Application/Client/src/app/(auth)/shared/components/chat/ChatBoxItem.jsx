import propTypes from "prop-types";
import { useEffect } from "react";
import Cookies from "universal-cookie";

var cookie = new Cookies();

const ChatBoxItem = ({ item }) => {
	useEffect(() => {
		console.log(item);
	}, []);
	return (
		<>
			<div className="flex gap-x-3 rounded-md cursor-pointer hover:bg-gray-300 p-2">
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
				<div className="flex items-center justify-center">
					<p className="">
						{cookie.get("fu_foody_id") === item.fromUserId
							? item.toUserName
							: item.fromUserName}
					</p>
				</div>
			</div>
		</>
	);
};

ChatBoxItem.propTypes = {
	item: propTypes.any,
	setCurrentBox: propTypes.any,
};

export default ChatBoxItem;
