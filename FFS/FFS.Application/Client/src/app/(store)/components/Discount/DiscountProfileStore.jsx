import propTypes from "prop-types";
import DiscountCopyStore from "./DiscountCopyStore";
import { Typography } from "@material-tailwind/react";
import FormatPriceHelper from "../../../../shared/components/format/FormatPriceHelper";

const DiscountProfileStore = ({ discountList }) => {
	const currentDate = new Date();
	let hasValidDiscount = false;
	return (
		<>
			<div className="p-2 border-dashed border-2 border-orange-200">
				<ul className="">
					{(discountList && discountList.length > 0) || hasValidDiscount ? (
						discountList.map((discount) => {
							const expirationDate = new Date(discount.expired);
							if (expirationDate > currentDate) {
								hasValidDiscount = true;
								return (
									<li
										key={discount.id}
										className="flex gap-4 p-2 justify-between items-center"
									>
										<svg
											xmlns="http://www.w3.org/2000/svg"
											height="24"
											width="24"
											viewBox="0 0 448 512"
											fill="orange"
										>
											<path d="M0 80V229.5c0 17 6.7 33.3 18.7 45.3l176 176c25 25 65.5 25 90.5 0L418.7 317.3c25-25 25-65.5 0-90.5l-176-176c-12-12-28.3-18.7-45.3-18.7H48C21.5 32 0 53.5 0 80zm112 32a32 32 0 1 1 0 64 32 32 0 1 1 0-64z" />
										</svg>
										{discount.description}
										<p className="text-gray-500">
											Chỉ áp dụng cho đơn hàng trên{" "}
											<span>
												{FormatPriceHelper(discount.conditionPrice)} đ
											</span>
										</p>
										<DiscountCopyStore
											codeToCopy={discount.code}
										></DiscountCopyStore>
									</li>
								);
							} else {
								return null;
							}
						})
					) : (
						<Typography>Quán chưa có mã giảm giá nào!</Typography>
					)}
				</ul>
			</div>
		</>
	);
};

DiscountProfileStore.propTypes = {
	discountList: propTypes.any.isRequired,
};

export default DiscountProfileStore;
