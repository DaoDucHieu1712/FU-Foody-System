import { useState, useEffect, useRef } from "react";
import axios from "../../shared/api/axiosConfig";
import Ring from "../../shared/components/icon/Ring";
import * as signalR from "@microsoft/signalr";
import CookieService from "../../shared/helper/cookieConfig";

const Notification = () => {
  const userId = CookieService.getToken("fu_foody_id");
  const [notifications, setNotifications] = useState([]);
  const [forceRender, setForceRender] = useState(0);
  const [showTooltip, setShowTooltip] = useState(false);
  const tooltipRef = useRef(null);

  const toggleTooltip = () => {
    setShowTooltip(!showTooltip);
  };

  useEffect(() => {
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
        setNotifications(response);
        console.log(response);
      } catch (error) {
        console.error("Error fetching notifications:", error);
      }
    };

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7025/notificationHub")
      .build();

    connection
      .start()
      .then(() => {
        console.log("SignalR Connected");
      })
      .catch((err) => console.error(err));

    connection.on("ReceiveNotification", (message) => {
      console.log("Received notification:", message);
      // You can handle real-time notifications here
      setNotifications((prevNotifications) => {
        const newNotifications = [...prevNotifications, message];
        const limitedNotifications = newNotifications.slice(0, 10);
        return limitedNotifications;
      });
      setForceRender((prev) => prev + 1);
    });

    // Fetch initial notifications
    fetchNotifications();

    // Cleanup function
    return () => {
      connection.stop();
      window.removeEventListener("click", handleClickOutside);
    };
    // Remove event click outside
  }, [userId, forceRender]);
  useEffect(() => {
    console.log("Updated notifications:", notifications);
  }, [notifications]);

  //css
  // Inline styles for the tooltip and triangle
  const tooltipStyle = {
    position: "absolute",
    top: "130%",
    left: "-350%",
    transform: "translateX(-50%)",
    width: "380px",
    padding: "10px",
    backgroundColor: "#fff",
    border: "0px solid #ccc",
    borderRadius: "4px",
    boxShadow: "0 2px 4px rgba(0, 0, 0, 0.2)",
    zIndex: "1",
    opacity: showTooltip ? "1" : "0",
    transition: "opacity 0.2s ease-in-out",
  };

  const triangleStyle = {
    content: "",
    position: "absolute",
    top: "-19px",
    left: "85%",
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
          <h3 className="text-base mb-2">Thông Báo Mới Nhận</h3>
          <ul>
            {notifications.reverse().map((notification, index) => (
              <li
                key={index} 
                className="mb-2 p-2 rounded-lg hover:bg-gray-100"
              >
                <div className="font-bold uppercase">{notification.title}</div>
                <div className="text-sm">{notification.content}</div>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default Notification;
