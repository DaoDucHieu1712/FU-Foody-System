import { useLocation } from "react-router-dom";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";

const ConfirmPaymentPage = () => {
	const location = useLocation();

	// Access the query parameters from the location object
	const queryParams = new URLSearchParams(location.search);
	const orderId = queryParams.get("vnp_TxnRef");
	const code = queryParams.get("vnp_ResponseCode");

	// Use the values as needed
	console.log("Order ID:", orderId);
	console.log("Status:", code);

	const ConfirmPayment = async () => {
		const data = {
			OrderId: orderId,
			Response: code,
		};
		await axios
			.post("/api/Order/ConfirmPayment", data)
			.then((res) => {
				toast.success(res);

				// Delay for 3 seconds before closing the window
				setTimeout(() => {
					window.close();
				}, 3000);
			})
			.catch((err) => {
				toast.error("Có lỗi xảy ra!");
			});
	};

	ConfirmPayment();
};

export default ConfirmPaymentPage;
