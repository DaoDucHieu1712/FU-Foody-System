import FoodCard from "./FoodCard";
import axios from "../../../../shared/api/axiosConfig";
import { useEffect, useState } from "react";

const RecommendList = () => {
	const [foods, setFoods] = useState([]);

	const GetListRecommend = async () => {
		await axios.get("/api/Food/GetFoodRecommend").then((res) => {
			setFoods(res);
		});
	};

	useEffect(() => {
		GetListRecommend();
	}, []);

	return (
		<>
			<div className="grid grid-flow-row-dense grid-cols-4 gap-2 lg:grid-rows-2 lg:grid-cols-5">
				{/* Lặp */}
				{foods.map((item) => {
					return <FoodCard key={item.id} item={item}></FoodCard>;
				})}

				{/* Hết lặp */}
			</div>
		</>
	);
};

export default RecommendList;
