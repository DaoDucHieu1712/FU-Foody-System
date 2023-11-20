import { Button, IconButton, Tooltip, Typography } from "@material-tailwind/react";
import PickTimeSale from "./PickTimeSale";
import { useEffect, useState } from "react";
import AddFoodSale from "./AddFoodSale";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

const TABLE_HEAD = ["Sản phẩm", "Giá gốc", "Giá đã giảm", "Phần trăm giảm (%)", "Số lượng sản phẩm khuyến mãi", "Kho hàng", ""];
const backgroundColors = ["bg-gray-50", "bg-gray-200"];


const AddFlashSale = () => {
    const navigate = useNavigate();
    const [dateStart, setDateStart] = useState('');
    const [dateEnd, setDateEnd] = useState('');
    const [foodListSale, setFoodListSale] = useState([]);
    const [formData, setFormData] = useState({});
    const [priceInput, setPriceInput] = useState(null);
    const [percentInput, setPercentInput] = useState(null);
    const [quantityInput, setQuantityInput] = useState(null);

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

    const handleDelete = (id) => {
        const updatedFood = foodListSale.filter((food) => food.foodId != id);
        setFoodListSale(updatedFood);
        toast.success("Xoá thành công!");
    }

    const handleChange = (foodId, field, value) => {
        setFormData((prevData) => ({
            ...prevData,
            [foodId]: {
                ...prevData[foodId],
                [field]: value,
            },
        }));
        console.log(formData);
    };

    const onSubmit = () => {
        Object.keys(formData).forEach((foodId) => {
            const foodData = formData[foodId];
            sendDataToDatabase(foodId, foodData);
        });
    };

    const sendDataToDatabase = (foodId, foodData) => {

    };

    const handleUpdateAll = () => {
        const updatedFormData = {};
        Object.keys(formData).forEach((foodId) => {
            const foodData = formData[foodId];
            updatedFormData[foodId] = {
                ...foodData,
                priceFood: priceInput !== null ? priceInput : foodData.priceFood,
                percentFood: percentInput !== null ? percentInput : foodData.percentFood,
                quantityFood: quantityInput !== null ? quantityInput : foodData.quantityFood,
            };
        });

        setFormData(updatedFormData);
    };

    useEffect(() => {
        const initialFormData = {};
        foodListSale.forEach((food) => {
            initialFormData[food.foodId] = {
                priceFood: 0,
                percentFood: 0,
                quantityFood: 0,
            };
        });
        setFormData(initialFormData);
    }, [foodListSale]);

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
            <div className="flex items-center justify-between p-5 mt-5 bg-gray-100 shadow-md rounded-t-lg">
                <Typography>Chỉnh sửa tất cả</Typography>
                <div>
                    <Typography>Giá đã giảm</Typography>
                    <input
                        type="number"
                        min={0}
                        value={priceInput !== null ? priceInput : ''}
                        onChange={(e) => setPriceInput(e.target.value !== '' ? e.target.value : null)}
                        className="w-20 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                    </input>
                </div>
                <div>
                    <Typography>Phần trăm khuyến mãi</Typography>
                    <input
                        type="number"
                        min={0}
                        max={100}
                        value={percentInput != null ? percentInput : ''}
                        onChange={(e) => setPercentInput(e.target.value !== '' ? e.target.value : null)}
                        className="w-20 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                    </input>
                </div>
                <div>
                    <Typography>Số lượng sản phẩm khuyến mãi</Typography>
                    <input
                        type="number"
                        min={0}
                        value={quantityInput !== null ? quantityInput : ''}
                        onChange={(e) => setQuantityInput(e.target.value !== '' ? e.target.value : null)}
                        className="w-20 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                    </input>
                </div>
                <Button className="bg-primary" onClick={() => handleUpdateAll()}>Cập nhật tất cả</Button>
            </div>
            <div>
                <form onSubmit={() => onSubmit()}>
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
                                        key={food.foodId}
                                        className={
                                            backgroundColors[index % backgroundColors.length]
                                        }
                                    >
                                        <td className="flex items-center justify-center">
                                            <img
                                                className="font-normal"
                                                src={food.imageURL}
                                                alt={food.foodName}
                                                height={100}
                                                width={100}
                                            />
                                            <Typography
                                                variant="small"
                                                color="blue-gray"
                                                className="w-24 font-normal text-left pl-4"
                                            >
                                                {food.foodName}
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
                                        <td className="w-14 lg:w-48">
                                            <input
                                                type="number"
                                                min={0}
                                                // max={}
                                                value={formData[food.foodId]?.priceFood}
                                                onChange={(e) => handleChange(food.foodId, 'priceFood', e.target.value)}
                                                className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                            </input>
                                        </td>
                                        <td className="w-14 lg:w-48">
                                            <input
                                                type="number"
                                                min={0}
                                                max={100}
                                                value={formData[food.foodId]?.percentFood}
                                                onChange={(e) => handleChange(food.foodId, 'percentFood', e.target.value)}
                                                className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                            </input>
                                        </td>
                                        <td className="w-24 lg:w-48">
                                            <input
                                                type="number"
                                                min={0}
                                                max={food.quantity}
                                                value={formData[food.foodId]?.quantityFood}
                                                onChange={(e) => handleChange(food.foodId, 'quantityFood', e.target.value)}
                                                className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                            </input>
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
                                                <Tooltip content="Xóa món ăn">
                                                    <IconButton
                                                        variant="text"
                                                        onClick={() => handleDelete(food.foodId)}
                                                    >
                                                        <svg
                                                            xmlns="http://www.w3.org/2000/svg"
                                                            width="20"
                                                            height="20"
                                                            viewBox="0 0 448 512"
                                                        >
                                                            <path d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z" />
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
                    <div className="flex my-2 gap-1 justify-end">
                        <Button onClick={() => navigate(`/flash-sale`)} variant="outlined">Hủy</Button>
                        {foodListSale.length == 0 ?
                            <Button disabled className="bg-primary">Xác nhận</Button>
                            :
                            <Button type="submit" className="bg-primary">Xác nhận</Button>}
                    </div>
                </form>
            </div>
        </>
    );
};

export default AddFlashSale;