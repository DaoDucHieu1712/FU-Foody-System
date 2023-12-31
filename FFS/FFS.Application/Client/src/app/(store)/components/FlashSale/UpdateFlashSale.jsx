import {
    Button,
    Dialog,
    IconButton,
    Tooltip,
    Typography,
} from "@material-tailwind/react";
import propTypes from "prop-types";
import { useEffect, useState } from "react";
import axios from "../../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import PickTimeSale from "./PickTimeSale";
import AddFoodSale from "./AddFoodSale";

const TABLE_HEAD = ["Sản phẩm", "Giá gốc", "Giá đã giảm", "Phần trăm giảm (%)", "Số lượng sản phẩm khuyến mãi", "Kho hàng", ""];
const backgroundColors = ["bg-gray-50", "bg-gray-200"];

const UpdateFlashSale = ({ fId, reload, dayStart, dayEnd, fsList }) => {
    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);
    const [dateStart, setDateStart] = useState(dayStart);
    const [dateEnd, setDateEnd] = useState(dayEnd);
    const [foodListSale, setFoodListSale] = useState(fsList);
    const [formData, setFormData] = useState({});
    const [priceInput, setPriceInput] = useState(null);
    const [percentInput, setPercentInput] = useState(null);
    const [quantityInput, setQuantityInput] = useState(null);

    const GetDateSale = (start, end) => {
        setDateStart(start);
        setDateEnd(end);
    }

    const GetFoodListSale = (list) => {
        const updatedList = [...foodListSale];
        list.forEach((newItem) => {
            const exists = updatedList.some((existingItem) => existingItem.foodId === newItem.foodId);
            if (!exists) {
                updatedList.push(newItem);
            }
        });

        setFoodListSale(updatedList);
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

    const handleDelete = async (id, flashSaleId) => {
        try {
            await axios.delete(`/api/FlashSale/DeleteFlashSaleDetail/${flashSaleId}/${id}`)
                .then(() => {
                    const updatedFood = foodListSale.filter((food) => food.foodId != id);
                    setFoodListSale(updatedFood);
                })
                .catch(() => {
                    const updatedFood = foodListSale.filter((food) => food.foodId != id);
                    setFoodListSale(updatedFood);
                });
        } catch (error) {
            console.error("Error occur:", error);
        }
    }

    const handleChange = (foodId, field, value) => {
        setFormData((prevData) => ({
            ...prevData,
            [foodId]: {
                ...prevData[foodId],
                [field]: value,
            },
        }));
    };

    const onSubmit = async (e) => {
        e.preventDefault();
        try {
            const formDataArray = Object.values(formData);
            const adjustedDateStart = new Date(dateStart);
            adjustedDateStart.setHours(adjustedDateStart.getHours() + 7);
            const adjustedDateEnd = new Date(dateEnd);
            adjustedDateEnd.setHours(adjustedDateEnd.getHours() + 7);
            const dataPut = {
                start: adjustedDateStart.toISOString(),
                end: adjustedDateEnd.toISOString(),
                flashSaleDetails: formDataArray,
            }
            await axios.put(`/api/FlashSale/UpdateFlashSale/${fId}`, dataPut)
                .then(() => {
                    toast.success("Sửa flash sale thành công!");
                    setOpen(false);
                    reload();
                })
                .catch((error) => {
                    console.log(error);
                });
        } catch (error) {
            console.error("Error occur:", error);
        }
    };


    const handleUpdateAll = () => {
        const updatedFormData = {};
        Object.keys(formData).forEach((foodId) => {
            const foodData = formData[foodId];

            const shouldUpdateSalePercent = foodData.priceAfterSale <= 0;
            const shouldUpdatePriceAfterSale = foodData.salePercent <= 0;

            updatedFormData[foodId] = {
                ...foodData,
                foodId: parseInt(foodId),
                priceAfterSale: shouldUpdatePriceAfterSale ? (priceInput !== null ? parseFloat(priceInput) : parseFloat(foodData.priceAfterSale)) : parseFloat(foodData.priceAfterSale),
                salePercent: shouldUpdateSalePercent ? (percentInput !== null ? parseInt(percentInput) : parseInt(foodData.salePercent)) : parseInt(foodData.salePercent),
                numberOfProductSale: quantityInput !== null ? parseInt(quantityInput) : parseInt(foodData.numberOfProductSale),
            };
        });

        setFormData(updatedFormData);
    };

    useEffect(() => {
        const initialFormData = {};
        foodListSale.forEach((food) => {
            initialFormData[food.foodId] = {
                foodId: parseInt(food.foodId),
                priceAfterSale: parseFloat(food.priceAfterSale) ? parseFloat(food.priceAfterSale) : 0,
                salePercent: parseInt(food.salePercent) ? parseInt(food.salePercent) : 0,
                numberOfProductSale: parseInt(food.numberOfProductSale) ? parseInt(food.numberOfProductSale) : 0,
            };
        });
        setFormData(initialFormData);
    }, [foodListSale]);



    return (
        <div>
            <Tooltip content="Cập nhật flash sale">
                <IconButton
                    variant="text"
                    onClick={handleOpen}
                >
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="20"
                        height="20"
                        viewBox="0 0 512 512"
                    >
                        <path d="M471.6 21.7c-21.9-21.9-57.3-21.9-79.2 0L362.3 51.7l97.9 97.9 30.1-30.1c21.9-21.9 21.9-57.3 0-79.2L471.6 21.7zm-299.2 220c-6.1 6.1-10.8 13.6-13.5 21.9l-29.6 88.8c-2.9 8.6-.6 18.1 5.8 24.6s15.9 8.7 24.6 5.8l88.8-29.6c8.2-2.7 15.7-7.4 21.9-13.5L437.7 172.3 339.7 74.3 172.4 241.7zM96 64C43 64 0 107 0 160V416c0 53 43 96 96 96H352c53 0 96-43 96-96V320c0-17.7-14.3-32-32-32s-32 14.3-32 32v96c0 17.7-14.3 32-32 32H96c-17.7 0-32-14.3-32-32V160c0-17.7 14.3-32 32-32h96c17.7 0 32-14.3 32-32s-14.3-32-32-32H96z" />
                    </svg>
                </IconButton>
            </Tooltip>
            <Dialog
                size="xl"
                open={open}
                handler={handleOpen}
                className="max-h-screen bg-transparent shadow-none overflow-y-scroll"
            >
                <div className="bg-white py-2 px-3">
                    <Typography variant="h4" className="pb-2">Chương trình Flash Sale</Typography>
                    <div className="p-2 bg-gray-100 shadow-md rounded-t-lg">
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
                    <div className="flex justify-between items-center p-2 mt-1 bg-gray-100 shadow-md rounded-t-lg">
                        <Typography variant="h6">Sản phẩm tham gia Flash Sale của Shop</Typography>
                        <AddFoodSale getFoodList={GetFoodListSale} dateS={dateStart} dateE={dateEnd}></AddFoodSale>
                    </div>
                    <div className="flex items-center justify-between p-2 mt-1 bg-gray-100 shadow-md rounded-t-lg">
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
                        <form onSubmit={(e) => onSubmit(e)}>
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
                                                <td className="w-28">
                                                    <Typography
                                                        variant="small"
                                                        color="blue-gray"
                                                        className="font-normal max-w-xs truncate"
                                                    >
                                                        {food.price}
                                                    </Typography>
                                                </td>
                                                {formData[food.foodId]?.salePercent == 0 ?
                                                    (
                                                        <td className="relative w-14 lg:w-48">
                                                            <input
                                                                type="number"
                                                                min={0}
                                                                max={food.price}
                                                                value={formData[food.foodId]?.priceAfterSale}
                                                                onChange={(e) => handleChange(food.foodId, 'priceAfterSale', e.target.value)}
                                                                className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                                            </input>
                                                            <br />
                                                            {formData[food.foodId]?.priceAfterSale > food.price && (
                                                                <span className="absolute right-0 pt-1 text-red-500 text-xs">Vui lòng nhập giá nhỏ hơn giá gốc</span>
                                                            )}
                                                        </td>
                                                    ) : (
                                                        <td className="w-14 lg:w-48">
                                                            <input
                                                                type="number"
                                                                disabled
                                                                className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                                            </input>
                                                        </td>
                                                    )}
                                                {
                                                    formData[food.foodId]?.priceAfterSale == 0 ?
                                                        (
                                                            <td className="relative w-14 lg:w-48">
                                                                <input
                                                                    type="number"
                                                                    min={0}
                                                                    max={100}
                                                                    value={formData[food.foodId]?.salePercent}
                                                                    onChange={(e) => handleChange(food.foodId, 'salePercent', e.target.value)}
                                                                    className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                                                </input>
                                                                <br />
                                                                {formData[food.foodId]?.salePercent > 100 && (
                                                                    <span className="absolute right-0 pt-1 text-red-500 text-xs">Phần trăm khuyến mãi chỉ có thể từ 0-100%</span>
                                                                )}
                                                            </td>
                                                        ) : (
                                                            <td className="w-14 lg:w-48">
                                                                <input
                                                                    type="number"
                                                                    disabled
                                                                    className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                                                </input>
                                                            </td>
                                                        )}
                                                <td className="relative w-24 lg:w-48">
                                                    <input
                                                        type="number"
                                                        min={0}
                                                        max={food.quantity}
                                                        value={formData[food.foodId]?.numberOfProductSale}
                                                        onChange={(e) => handleChange(food.foodId, 'numberOfProductSale', e.target.value)}
                                                        className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                                    </input>
                                                    <br />
                                                    {formData[food.foodId]?.numberOfProductSale > food.quantity && (
                                                        <span className="absolute right-0 pt-1 text-red-500 text-xs">Vui lòng nhập số lượng nhỏ hơn tồn kho</span>
                                                    )}
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
                                                                onClick={() => handleDelete(food.foodId, food.flashSaleId)}
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
                                <Button onClick={handleOpen} variant="outlined">Hủy</Button>
                                {foodListSale.length == 0 ?
                                    <Button disabled className="bg-primary">Xác nhận</Button>
                                    :
                                    <Button type="submit" className="bg-primary">Xác nhận</Button>}
                            </div>
                        </form>
                    </div>
                </div >
            </Dialog >
        </div >
    );
};

UpdateFlashSale.propTypes = {
    fId: propTypes.any.isRequired,
    reload: propTypes.any.isRequired,
    dayStart: propTypes.any.isRequired,
    dayEnd: propTypes.any.isRequired,
    fsList: propTypes.any.isRequired,
};

export default UpdateFlashSale;
