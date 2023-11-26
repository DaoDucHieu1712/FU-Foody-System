import { Button } from "@material-tailwind/react";
import propTypes from "prop-types";
import ReviewFood from "../../../(public)/components/ReviewFood";
import CookieService from "../../../../shared/helper/cookieConfig";

const OrderItem = ({ item }) => {
	const uid = CookieService.getToken("fu_foody_id"); 
	return (
		<>
			<div className="w-full cart-item flex justify-between rounded-lg gap-x-3 p-3 border border-gray-200">
				<div className="flex gap-x-3">
					<img src={item.imageURL} alt="" className="w-[100px] object-cover" />
					<div className="flex flex-col gap-y-1 text-sm">
						<p className="font-bold w-[300px]">{item.foodName}</p>
						<p>{item.unitPrice} $</p>
					</div>
				</div>
				<div className="flex flex-col gap-y-3">
					<p className="text-sm">{item.unitPrice} $</p>
					<div className="flex items-center justify-center gap-x-1">
						<button className="">x</button>
						<span>{item.quantity}</span>
					</div>
				</div>
				<div className="flex items-center justify-center">
					{/* <Button size="sm" className="bg-primary">
						đánh giá
					</Button> */}
					<ReviewFood idUser={uid} idFood={item.foodId}></ReviewFood>
				</div>
			</div>
		</>
	);
};

OrderItem.propTypes = {
	item: propTypes.any,
};

export default OrderItem;
