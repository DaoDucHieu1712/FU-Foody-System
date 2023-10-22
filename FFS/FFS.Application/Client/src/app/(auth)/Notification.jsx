import { useState, useEffect, useRef } from "react";
import axios from "axios";
import Ring from "../../shared/components/icon/Ring";

const Notification = () => {
  const [notifications, setNotifications] = useState([]);
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
    axios
      .get("https://localhost:7025/api/Notification/ListNotification")
      .then((response) => {
        setNotifications(response.data);
      })
      .catch((error) => {
        console.error("Error fetching notifications:", error);
      });

    // Remove event click outside
    return () => {
      window.removeEventListener("click", handleClickOutside);
    };
  }, []);

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
            {notifications.map((notification) => (
              <li
                key={notification.id}
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
