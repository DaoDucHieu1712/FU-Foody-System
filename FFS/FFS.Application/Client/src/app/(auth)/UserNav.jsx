import { useState, useEffect, useRef } from 'react';
import User from "../../shared/components/icon/User";
const UserNav = () => {
  
  const [showTooltip, setShowTooltip] = useState(false);
  const tooltipRef = useRef(null);

  const toggleTooltip = () => {
    setShowTooltip(!showTooltip);
  };

  const handleLogout = () => {
    
    localStorage.removeItem('token'); // Remove the user token
    window.location.href = '/login'; // Redirect to the login page
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

  }, []);

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
      </div>
      {showTooltip && (
        <div style={tooltipStyle}>
          <div style={triangleStyle} />
          
          <ul className=''>
          <li className="text-base cursor-pointer hover:bg-gray-100 px-3 py-1.5" >Tài Khoản Của Tôi</li>
            <li className="text-base cursor-pointer hover:bg-gray-100 px-3 py-1.5">Đơn Mua</li>
            <li onClick={handleLogout} className="text-base cursor-pointer hover:bg-gray-100 px-3 py-1.5">Đăng Xuất</li>
          </ul>
        </div>
      )}
    </div>
  );

};

export default UserNav;
