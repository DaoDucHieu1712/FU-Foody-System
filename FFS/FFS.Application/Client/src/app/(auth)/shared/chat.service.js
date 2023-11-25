import axiosConfig from "../../../shared/api/axiosConfig";

const ChatService = {
	async getAllByUserId(userId) {
		const url = `/api/Chat/GetAllByUserId/${userId}`;
		return axiosConfig.get(url);
	},
	async FindById(id) {
		const url = `/api/Chat/FindById/${id}`;
		return axiosConfig.get(url);
	},
	async CreateChatBox(data) {
		const url = `/api/Chat/CreateChatBox`;
		return axiosConfig.post(url, data);
	},
	async SendMessage(data) {
		const url = `/api/Chat/SendMessage`;
		return axiosConfig.post(url, data);
	},
};

export default ChatService;
