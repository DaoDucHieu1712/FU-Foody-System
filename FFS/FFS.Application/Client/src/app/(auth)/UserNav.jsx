import { useState, useEffect, useRef } from "react";
import User from "../../shared/components/icon/User";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { setAccessToken } from "../../redux/auth";
import CookieService from "../../shared/helper/cookieConfig";
import axios from "../../shared/api/axiosConfig";
import ProfilePage from "./ProfilePage";


const UserNav = () => {
  const [userInfo, setUserInfo] = useState(null);
  const [showTooltip, setShowTooltip] = useState(false);
  const tooltipRef = useRef(null);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const toggleTooltip = () => {
    setShowTooltip(!showTooltip);
  };

  const handleLogout = () => {
    CookieService.removeToken("fu_foody_token"); // Remove the user token
    dispatch(setAccessToken(null));
    navigate("/Login"); // Redirect to the login page
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

    const fetchUserInfo = async () => {
      try {
        const response = await axios.get("/api/Authenticate/GetCurrentUser");
        setUserInfo(response);
        console.log(response);
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    };

    fetchUserInfo();
  }, []);

  const handleProfileClick = () => {
    navigate("/profile");
  };

  //css
  // Inline styles for the tooltip and triangle
  const tooltipStyle = {
    position: "absolute",
    top: "130%",
    left: "-120%",
    transform: "translateX(-50%)",
    width: "160px",
    backgroundColor: "#fff",
    border: "0px solid #ccc",
    borderRadius: "2px",
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
      <div onClick={toggleTooltip}>
        <User className="text-xl cursor-pointer" />

        {/* {userInfo && (
          <div className="flex items-center space-x-1">
            <img
              className="w-9 h-9 rounded-full"
              src={userInfo.avatar}
              alt="Avatar"
            />
            <span className="font-sm text-white">{userInfo.userName}</span>
            
          </div>
          
        )} */}
      </div>
      {showTooltip && (
        <div style={tooltipStyle}>
          <div style={triangleStyle} />

          <ul className="">
            <li className="text-base cursor-pointer hover:bg-gray-100 px-3 py-1.5">
              Tài Khoản Của Tôi
            </li>
            <li className="text-base cursor-pointer hover:bg-gray-100 px-3 py-1.5">
              Đơn Mua
            </li>
            <li
              onClick={handleLogout}
              className="text-base cursor-pointer hover:bg-gray-100 px-3 py-1.5"
            >
              Đăng Xuất
            </li>
          </ul>
        </div>
      )}
    </div>
  );
};

export default UserNav;
