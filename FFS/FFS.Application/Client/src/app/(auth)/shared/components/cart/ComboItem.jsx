import propTypes from "prop-types";
import { useDispatch } from "react-redux";
import { comboActions } from "../../comboSlice";
import DeleteIcon from "../../../../../shared/components/icon/DeleteIcon";
import { useEffect, useState } from "react";
import ComboSerive from "../../combo.service";

const ComboItem = ({ item }) => {
	const [foods, setFoods] = useState([]);
	const dispatch = useDispatch();

	const loadFoods = async () => {
		await ComboSerive.GetFoods(item.id).then((res) => {
			setFoods(res);
		});
	};

	useEffect(() => {
		loadFoods();
	}, []);

	const IncrementHandler = () => {
		dispatch(comboActions.increaseItemQuantity(item));
		console.log("+", item);
	};

	const DescrementHandler = () => {
		dispatch(comboActions.decreaseItemQuantity(item));
		console.log("-", item);
	};

	const RemoveHandler = () => {
		dispatch(comboActions.removeItem(item));
	};
	return (
		<>
			<tr key={item.id}>
				<td className="p-4 border-b border-blue-gray-50 flex items-center gap-x-2">
					<div onClick={RemoveHandler}>
						<DeleteIcon></DeleteIcon>
					</div>
					<img src={item.image} alt="" className="w-[70px]" />
					<div>
						<h1 className="text-xl font-semibold uppercase">{item.name}</h1>
						<p className="font-medium">Combo bao gồm : </p>
						<div className="flex flex-col gap-y-1">
							{foods.map((item) => (
								<span key={item.Id} className="text-sm text-gray-700">
									{item.FoodName}
								</span>
							))}
						</div>
					</div>
				</td>
				<td className="p-4 border-b border-blue-gray-50">{item.price} đ</td>
				<td className="p-4 border-b border-blue-gray-50">
					<div className="flex items-center justify-between border p-2">
						<span
							className="text-gray-500 text-3xl font-medium cursor-pointer"
							onClick={DescrementHandler}
						>
							-
						</span>
						<p>{item.quantity}</p>
						<span
							className="text-3xl font-medium cursor-pointer"
							onClick={IncrementHandler}
						>
							+
						</span>
					</div>
				</td>
				<td className="p-4 border-b border-blue-gray-50">
					{item.quantity * item.price} đ
				</td>
			</tr>
		</>
	);
};

ComboItem.propTypes = {
	item: propTypes.any,
};

export default ComboItem;
