import propTypes from "prop-types";

const Message = ({ content, IsCurrentUser, CreatedAt }) => {
	return (
		<>
			<div
				className={`flex items-center gap-x-3 p-3 ${
					IsCurrentUser ? "flex-row-reverse" : ""
				} `}
			>
				<div className="flex flex-col gap-1 max-w-[70%]">
					<span className="bg-green-200 max-w-[100%] text-sm bg-slate-200 p-2 rounded-lg text-black break-words">
						{content}
					</span>
					<span className="text-sm text-gray-700">{CreatedAt}</span>
				</div>
			</div>
		</>
	);
};

Message.propTypes = {
	content: propTypes.any,
	IsCurrentUser: propTypes.any,
	CreatedAt: propTypes.any,
};

export default Message;
