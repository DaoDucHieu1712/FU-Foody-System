import { IconButton, Tooltip } from "@material-tailwind/react";

const FoodCart = () => {
    return (
        <Tooltip content="Thêm giỏ hàng">
            <IconButton variant="text" className="bg-white rounded-full">
                <svg
                    width="30"
                    height="2.5em"
                    viewBox="0 0 30 40"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                >
                    <path
                        d="M10 35C11.1046 35 12 34.1046 12 33C12 31.8954 11.1046 31 10 31C8.89543 31 8 31.8954 8 33C8 34.1046 8.89543 35 10 35Z"
                        fill="black"
                    />
                    <path
                        d="M23 35C24.1046 35 25 34.1046 25 33C25 31.8954 24.1046 31 23 31C21.8954 31 21 31.8954 21 33C21 34.1046 21.8954 35 23 35Z"
                        fill="black"
                    />
                    <path
                        d="M5.2875 15H27.7125L24.4125 26.55C24.2948 26.9692 24.0426 27.3381 23.6948 27.6001C23.3471 27.862 22.9229 28.0025 22.4875 28H10.5125C10.0771 28.0025 9.65293 27.862 9.30515 27.6001C8.95738 27.3381 8.70524 26.9692 8.5875 26.55L4.0625 10.725C4.0027 10.5159 3.8764 10.3321 3.70271 10.2012C3.52903 10.0704 3.31744 9.99977 3.1 10H1"
                        stroke="black"
                        strokeWidth="2"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                    />
                </svg>
            </IconButton>
        </Tooltip>
    );
};

export default FoodCart;