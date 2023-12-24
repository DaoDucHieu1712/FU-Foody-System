import { useState, useEffect, useRef } from "react";
import axios from "../../shared/api/axiosConfig";
import Ring from "../../shared/components/icon/Ring";
import { HubConnectionBuilder } from "@microsoft/signalr";
import CookieService from "../../shared/helper/cookieConfig";
import {
	addNotification,
	selectNotifications,
	markAllAsRead,
} from "./shared/notificationSlice";
import { useDispatch, useSelector } from "react-redux";
import { Typography } from "@material-tailwind/react";
import moment from "moment";
import "moment/dist/locale/vi";
moment.locale("vi");

const Notification = () => {
	const dispatch = useDispatch();
	const notifications = useSelector(selectNotifications);
	const userId = CookieService.getToken("fu_foody_id");
	const [showTooltip, setShowTooltip] = useState(false);
	const tooltipRef = useRef(null);

	const toggleTooltip = () => {
		setShowTooltip(!showTooltip);
	};

	const markAllAsReadHandler = async () => {
		try {
			await axios.post(`/api/Notification/MarkAllAsRead/${userId}`);
			dispatch(markAllAsRead()); // Dispatch the markAllAsRead action
		} catch (error) {
			console.error("Error marking notifications as read:", error);
		}
	};

	useEffect(
		() => {
			//Đóng thông báo khi click bên ngoài
			const handleClickOutside = (event) => {
				if (tooltipRef.current && !tooltipRef.current.contains(event.target)) {
					setShowTooltip(false);
				}
			};

			//Skien nhap chuột vào cửa sổ
			window.addEventListener("click", handleClickOutside);

			//Call api

			const fetchNotifications = async () => {
				try {
					const response = await axios.get(
						`/api/Notification/GetNotificationsByUserId/${userId}`
					);
					const notificationsFromApi = response;
					console.log(response);
					notificationsFromApi.forEach((notification) => {
						dispatch(addNotification(notification));
					});
				} catch (error) {
					console.error("Error fetching notifications:", error);
				}
			};

			const url =
				import.meta.env.VITE_FU_FOODY_PUBLIC_API_BASE_URL + "/notificationHub";
			var connection = new HubConnectionBuilder()
				.withUrl(url)
				.withAutomaticReconnect()
				.build();

			connection
				.start()
				.then(() => {
					console.log("SignalR Connected");
					connection.invoke("JoinGroup", userId);
				})
				.catch((err) => console.error(err));

			connection.on("ReceiveNotification", (message) => {
				console.log("Notification received:", message);
				dispatch(addNotification(message));
			});
			fetchNotifications();

			return () => {
				connection.stop();
				window.removeEventListener("click", handleClickOutside);
			};
			// Remove event click outside
		},
		[dispatch],
		[userId]
	);
	const reversedNotifications = [...notifications].reverse();

	//css
	// Inline styles for the tooltip and triangle
	const tooltipStyle = {
		position: "absolute",
		top: "110%",
		left: "-350%",
		transform: "translateX(-50%)",
		width: "380px",
		backgroundColor: "#fff",
		boxShadow: "0 2px 4px rgba(0, 0, 0, 0.2)",
		zIndex: "1",
		opacity: showTooltip ? "1" : "0",
		transition: "opacity 0.2s ease-in-out",
	};

	const triangleStyle = {
		content: "",
		position: "absolute",
		top: "-19px",
		left: "84.5%",
		transform: "translateX(-50%)",
		borderWidth: "10px",
		borderStyle: "solid",
		borderColor: `transparent transparent #fff transparent`,
	};
	//

	return (
		<div className="relative inline-block text-left" ref={tooltipRef}>
			<div className="cursor-pointer" onClick={toggleTooltip}>
				<Ring className="text-xl" />
			</div>
			{showTooltip && (
				<div style={tooltipStyle}>
					<div style={triangleStyle} />

					<ul>
						<li className="header flex items-center justify-between border-b border-orange-900 p-4">
							<Typography variant="h5" className="font-medium">
								Thông báo
							</Typography>
							<div className="">
								<a
									className="text-primary cursor-pointer"
									onClick={markAllAsReadHandler}
								>
									Đánh dấu đã đọc
								</a>
							</div>
						</li>
						<li
							className="body"
							style={{
								overflowY: "auto",
								maxHeight: "350px",
								overflowX: "hidden",
							}}
						>
							<ul className="m-0 p-0">
								{notifications.length === 0 ? (
									<li className="px-4 py-3">
										<div  className="text-base">
											Không có thông báo mới.
										</div>
									</li>
								) : (
									reversedNotifications.map((notification, index) => (
										<li
											key={index}
											className={`px-4 py-3 hover:text-primary cursor-pointer ${
												notification.isRead ? "" : "bg-orange-50"
											}`}
										>
											<div className="font-semibold">{notification.title}</div>
											<div className="text-sm">{notification.content} </div>
											{/* <div className="text-sm">{moment(notification.createdAt).format("DD/MM/yyyy")}</div> */}
										</li>
									))
								)}
							</ul>
						</li>
					</ul>
				</div>
			)}
		</div>
	);
};

export default Notification;
