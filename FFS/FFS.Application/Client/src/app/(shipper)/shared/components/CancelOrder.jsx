import {
	Button,
	Dialog,
	DialogBody,
	DialogFooter,
	DialogHeader,
	Textarea,
} from "@material-tailwind/react";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import OrderService from "../../../(store)/shared/order.service";
import propTypes from "prop-types";
import { toast } from "react-toastify";

const CancelOrder = ({ id }) => {
	const [open, setOpen] = useState(false);
	const [reason, setReason] = useState("");
	const handleOpen = () => setOpen(!open);

	const orderQuery = useQuery({
		queryKey: ["my-order-detail"],
		queryFn: async () => {
			return await OrderService.GetOrderDetail(id);
		},
	});

	const handleUpdateOrder = () => {
		OrderService.CancelOrderWithShipper(id, reason).then(() => {
			setOpen(!open);
			toast.success("Hủy đơn hàng thành công !!!");
			location.reload();
		});
	};
	return (
		<>
			<Button onClick={handleOpen} className="bg-primary w-full">
				Hủy đơn hàng
			</Button>
			<Dialog open={open} handler={handleOpen}>
				<DialogHeader className="text-primary">Hủy đơn hàng</DialogHeader>
				<DialogBody>
					<h1 className="mb-3 text-xl uppercase font-semibold">
						Bạn xác nhận hủy đơn hàng #{orderQuery.data?.id}
					</h1>
					<div>
						<Textarea
							label="Lí do hủy đơn hàng"
							className="mt-3"
							value={reason}
							onChange={(e) => setReason(e.target.value)}
						/>
					</div>
				</DialogBody>
				<DialogFooter>
					<Button
						variant="text"
						color="red"
						onClick={handleOpen}
						className="mr-1"
					>
						<span>Hủy</span>
					</Button>
					<Button className="bg-primary" onClick={handleUpdateOrder}>
						<span>Xác nhận</span>
					</Button>
				</DialogFooter>
			</Dialog>
		</>
	);
};
CancelOrder.propTypes = {
	id: propTypes.any,
};

export default CancelOrder;
