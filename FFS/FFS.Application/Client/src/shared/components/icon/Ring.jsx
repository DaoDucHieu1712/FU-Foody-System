import { Badge } from "@material-tailwind/react";
import { useSelector } from "react-redux";
import { selectNotifications } from "../../../app/(auth)/shared/notificationSlice";

const Ring = () => {

  const notifications = useSelector(selectNotifications);

  const unreadCount = notifications.filter((notification) => !notification.isRead).length;
	return (
		<>
    <Badge content={unreadCount} withBorder className="top-[24%] right-[24%] min-w-[20px] min-h-[20px] py-0" >
    <svg
        width="33"
        height="33"
        viewBox="0 0 33 33"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M7.0249 15.0004C7.0249 13.8218 7.25704 12.6547 7.70808 11.5658C8.15911 10.4769 8.82021 9.48751 9.65361 8.65411C10.487 7.8207 11.4764 7.15961 12.5653 6.70857C13.6542 6.25754 14.8213 6.02539 15.9999 6.02539C17.1785 6.02539 18.3456 6.25754 19.4345 6.70857C20.5234 7.15961 21.5128 7.8207 22.3462 8.65411C23.1796 9.48751 23.8407 10.4769 24.2917 11.5658C24.7428 12.6547 24.9749 13.8218 24.9749 15.0004V15.0004C24.9749 19.4754 25.9124 22.0754 26.7374 23.5004C26.825 23.6522 26.8712 23.8243 26.8714 23.9995C26.8715 24.1748 26.8256 24.347 26.7383 24.4989C26.6509 24.6508 26.5252 24.7771 26.3737 24.8651C26.2221 24.9531 26.0501 24.9998 25.8749 25.0004H6.1249C5.94966 24.9998 5.77766 24.9531 5.62613 24.8651C5.4746 24.7771 5.34886 24.6508 5.26151 24.4989C5.17416 24.347 5.12826 24.1748 5.12842 23.9995C5.12857 23.8243 5.17478 23.6522 5.2624 23.5004C6.0874 22.0754 7.0249 19.4754 7.0249 15.0004Z"
          fill="white"
          stroke="#191C1F"
          strokeWidth="1.5"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
        <path
          d="M12 25V26C12 27.0609 12.4214 28.0783 13.1716 28.8284C13.9217 29.5786 14.9391 30 16 30C17.0609 30 18.0783 29.5786 18.8284 28.8284C19.5786 28.0783 20 27.0609 20 26V25"
          fill="white"
        />
        <path
          d="M12 25V26C12 27.0609 12.4214 28.0783 13.1716 28.8284C13.9217 29.5786 14.9391 30 16 30C17.0609 30 18.0783 29.5786 18.8284 28.8284C19.5786 28.0783 20 27.0609 20 26V25"
          stroke="#191C1F"
          strokeWidth="1.5"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
        <path
          d="M22.9248 4C24.9609 5.28526 26.6041 7.10584 27.6748 9.2625"
          stroke="white"
          strokeWidth="2"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
       
        {/* <path
          d="M24.0002 16.5673C28.1791 16.5673 31.5668 13.1796 31.5668 9.00065C31.5668 4.8217 28.1791 1.43398 24.0002 1.43398C19.8212 1.43398 16.4335 4.8217 16.4335 9.00065C16.4335 13.1796 19.8212 16.5673 24.0002 16.5673Z"
          fill="#EE5858"
          stroke="white"
          strokeWidth="1.8"
        /> */}
      </svg>
    </Badge>
			

			
		</>
	);
};

export default Ring;
