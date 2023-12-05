import axiosConfig from "../../../shared/api/axiosConfig";

const ComboSerive = {
	async GetFoods(id) {
		const url = `/api/Food/GetDetailCombo/${id}`;
		return axiosConfig.get(url);
	},
};

export default ComboSerive;
