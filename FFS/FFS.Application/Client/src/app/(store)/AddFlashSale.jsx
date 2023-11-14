import { IconButton, Tooltip, Typography } from "@material-tailwind/react";
import PickTimeSale from "./components/FlashSale/PickTimeSale";
import { useState } from "react";
import AddFoodSale from "./components/FlashSale/AddFoodSale";

const TABLE_HEAD = ["Id", "Tên đồ ăn", "Ảnh", "Loại", "Tồn kho", ""];
const backgroundColors = ["bg-gray-50", "bg-gray-200"];

const AddFlashSale = () => {
    const [dateStart, setDateStart] = useState('');
    const [dateEnd, setDateEnd] = useState('');
    const [foodListSale, setFoodListSale] = useState([]);

    const GetDateSale = (start, end) => {
        setDateStart(start);
        setDateEnd(end);
    }

    const GetFoodListSale = (list) => {
        setFoodListSale(list);
    }

    const formatDate = (date) => {
        const day = date.getDate().toString().padStart(2, '0');
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const year = date.getFullYear();
        return `${day}/${month}/${year}`;
    };

    const formatTime = (date) => {
        const hours = date.getHours().toString().padStart(2, '0');
        const minutes = date.getMinutes().toString().padStart(2, '0');
        return `${hours}:${minutes}`;
    };

    return (
        <>
            <Typography variant="h4" className="pb-4">Tạo chương trình Flash Sale của Shop</Typography>
            <div className="p-5 bg-gray-100 shadow-md rounded-t-lg">
                <PickTimeSale getDate={GetDateSale}></PickTimeSale>
                {dateStart && dateEnd ?
                    (
                        <Typography variant="h6" className="pt-4 text-orange-900">
                            Ngày bắt đầu: {formatDate(new Date(dateStart))} - Ngày kết thúc: {formatDate(new Date(dateEnd))} <br />
                            Giờ bắt đầu: {formatTime(new Date(dateStart))} - Giờ kết thúc: {formatTime(new Date(dateEnd))}
                        </Typography>
                    )
                    : null
                }
            </div>
            <div className="flex justify-between items-center p-5 mt-5 bg-gray-100 shadow-md rounded-t-lg">
                <Typography variant="h6">Sản phẩm tham gia Flash Sale của Shop</Typography>
                <AddFoodSale getFoodList={GetFoodListSale}></AddFoodSale>
            </div>
            <div>
                <table className="w-full min-w-max table-auto text-center">
                    <thead>
                        <tr>
                            {TABLE_HEAD.map((head) => (
                                <th
                                    key={head}
                                    className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4"
                                >
                                    <Typography
                                        variant="small"
                                        color="blue-gray"
                                        className="font-normal leading-none opacity-70"
                                    >
                                        {head}
                                    </Typography>
                                </th>
                            ))}
                        </tr>
                    </thead>
                    <tbody>
                        {foodListSale ?
                            foodListSale.map((food, index) => (
                                <tr
                                    key={food.id}
                                    className={
                                        backgroundColors[index % backgroundColors.length]
                                    }
                                >
                                    <td>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-bold"
                                        >
                                            {food.foodId}
                                        </Typography>
                                    </td>
                                    <td>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-normal max-w-xs truncate"
                                        >
                                            {food.foodName}
                                        </Typography>
                                    </td>
                                    <td>
                                        <img
                                            className="font-normal mx-auto"
                                            src={food.imageURL}
                                            alt={food.foodName}
                                            height={100}
                                            width={100}
                                        />
                                    </td>
                                    <td>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-normal"
                                        >
                                            {food.categoryName}
                                        </Typography>
                                    </td>
                                    <td>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-normal"
                                        >
                                            {food.quantity}
                                        </Typography>
                                    </td>
                                    <td>
                                        <div className="h-full flex justify-center items-center">
                                            <Tooltip content="Xem món ăn">
                                                <IconButton
                                                    variant="text"
                                                >
                                                    <svg
                                                        xmlns="http://www.w3.org/2000/svg"
                                                        width="20"
                                                        height="20"
                                                        viewBox="0 0 576 512"
                                                    >
                                                        <path d="M288 32c-80.8 0-145.5 36.8-192.6 80.6C48.6 156 17.3 208 2.5 243.7c-3.3 7.9-3.3 16.7 0 24.6C17.3 304 48.6 356 95.4 399.4C142.5 443.2 207.2 480 288 480s145.5-36.8 192.6-80.6c46.8-43.5 78.1-95.4 93-131.1c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C433.5 68.8 368.8 32 288 32zM144 256a144 144 0 1 1 288 0 144 144 0 1 1 -288 0zm144-64c0 35.3-28.7 64-64 64c-7.1 0-13.9-1.2-20.3-3.3c-5.5-1.8-11.9 1.6-11.7 7.4c.3 6.9 1.3 13.8 3.2 20.7c13.7 51.2 66.4 81.6 117.6 67.9s81.6-66.4 67.9-117.6c-11.1-41.5-47.8-69.4-88.6-71.1c-5.8-.2-9.2 6.1-7.4 11.7c2.1 6.4 3.3 13.2 3.3 20.3z" />
                                                    </svg>
                                                </IconButton>
                                            </Tooltip>
                                        </div>
                                    </td>
                                </tr>
                            ))
                            : <Typography>Chưa thêm sản phẩm nào</Typography>}
                    </tbody>
                </table>

            </div>
        </>
    );
};

export default AddFlashSale;