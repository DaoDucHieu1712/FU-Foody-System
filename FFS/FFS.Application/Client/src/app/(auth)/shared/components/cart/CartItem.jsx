import propTypes from "prop-types";
import DeleteIcon from "../../../../../shared/components/icon/DeleteIcon";
import { useDispatch, useSelector } from "react-redux";
import { cartActions } from "../../cartSlice";
import axios from "../../../../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";

const CartItem = ({ item }) => {
	const dispatch = useDispatch();
	const [quantity, setQuantity] = useState();
	const cart = useSelector((state) => state.cart);

	const loadInventory = async () => {
		await axios
			.get(`/api/Inventory/GetInventory/${item.foodId}`)
			.then((res) => {
				setQuantity(res.quantity);
				console.log(res.quantity);
			});
	};

	useEffect(() => {
		loadInventory();
	}, []);

	const IncrementHandler = () => {
		if (
			cart.list.filter((x) => x.foodId === item.foodId)[0].quantity >= quantity
		) {
			toast.error("Không được mua quá số lượng !!");
			return;
		}

		dispatch(cartActions.increaseItemQuantity(item));
	};

	const DescrementHandler = () => {
		dispatch(cartActions.decreaseItemQuantity(item));
	};

	const RemoveHandler = () => {
		dispatch(cartActions.removeItem(item));
	};
	return (
		<>
			<tr key={item.foodId}>
				<td className="p-4 border-b border-blue-gray-50 flex items-center gap-x-2">
					<div onClick={RemoveHandler}>
						<DeleteIcon></DeleteIcon>
					</div>
					<img src={item.img} alt="" className="w-[70px]" />
					<span>{item.foodName}</span>
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

CartItem.propTypes = {
	item: propTypes.any,
};

export default CartItem;
