import { Button, IconButton, Tooltip, Typography } from "@material-tailwind/react";
import PickTimeSale from "./PickTimeSale";
import { useEffect, useState } from "react";
import AddFoodSale from "./AddFoodSale";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import axios from "../../../../shared/api/axiosConfig";
import CookieService from "../../../../shared/helper/cookieConfig";

const TABLE_HEAD = ["Sản phẩm", "Giá gốc (nghìn VND)", "Giá đã giảm", "Phần trăm giảm (%)", "Số lượng sản phẩm khuyến mãi", "Kho hàng", ""];
const backgroundColors = ["bg-gray-50", "bg-gray-200"];


const AddFlashSale = () => {
    const navigate = useNavigate();
    const uId = CookieService.getToken("fu_foody_id");
    const [storeId, setStoreId] = useState(0);
    const [dateStart, setDateStart] = useState('');
    const [dateEnd, setDateEnd] = useState('');
    const [foodListSale, setFoodListSale] = useState([]);
    const [formData, setFormData] = useState([]);
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

    const GetStoreByUid = async () => {
        try {
            await axios
                .get(`/api/Store/GetStoreByUid?uId=${uId}`)
                .then((response) => {
                    setStoreId(response.id);
                })
                .catch((error) => {
                    console.log(error);
                });
        } catch (error) {
            console.log("Get Store By Uid error: " + error);
        }
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
    };

    const handleSubmit = async () => {
        try {
            const formDataArray = Object.values(formData);
            const adjustedDateStart = new Date(dateStart);
            adjustedDateStart.setHours(adjustedDateStart.getHours() + 7);
            const adjustedDateEnd = new Date(dateEnd);
            adjustedDateEnd.setHours(adjustedDateEnd.getHours() + 7);
            const dataPost = {
                storeId: storeId,
                start: adjustedDateStart.toISOString(),
                end: adjustedDateEnd.toISOString(),
                flashSaleDetails: formDataArray,
            }
            await axios.post("/api/FlashSale/CreateFlashSale", dataPost)
                .then(() => {
                    toast.success("Thêm flash sale thành công!");
                    setTimeout(() => {
                        navigate(`/store/flash-sale`);
                    }, 3000);
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
                priceAfterSale: parseFloat(0),
                salePercent: parseInt(0),
                numberOfProductSale: parseInt(0),
            };
        });
        setFormData(initialFormData);
    }, [foodListSale]);

    useEffect(() => {
        GetStoreByUid();
    }, []);

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
                {dateStart.trim() != '' && dateEnd.trim() != '' ?
                    (
                        <AddFoodSale getFoodList={GetFoodListSale} dateS={dateStart} dateE={dateEnd}></AddFoodSale>
                    ) : (
                        <Button disabled color="white" className="flex mt-2 gap-2 text-orange-500 border-solid border-2 border-orange-500">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                height="1.2em"
                                viewBox="0 0 448 512"
                            >
                                <path
                                    d="M256 80c0-17.7-14.3-32-32-32s-32 14.3-32 32V224H48c-17.7 0-32 14.3-32 32s14.3 32 32 32H192V432c0 17.7 14.3 32 32 32s32-14.3 32-32V288H400c17.7 0 32-14.3 32-32s-14.3-32-32-32H256V80z"
                                    fill="orange"
                                />
                            </svg>
                            Thêm sản phẩm
                        </Button>
                    )}
            </div>
            <div className="flex items-center justify-between p-5 mt-5 bg-gray-100 shadow-md rounded-t-lg">
                <Typography variant="h6">Chỉnh sửa tất cả</Typography>
                <div>
                    <Typography>Giảm đồng giá</Typography>
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
                <form>
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
                                        {
                                            formData[food.foodId]?.salePercent == 0 ?
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
                                                )
                                        }
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
                                                            min={0}
                                                            max={100}
                                                            disabled
                                                            className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                                        </input>
                                                    </td>
                                                )
                                        }
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
                        <Button onClick={() => navigate(`/store/flash-sale`)} variant="outlined">Hủy</Button>
                        {foodListSale.length == 0 ?
                            <Button disabled className="bg-primary">Xác nhận</Button>
                            :
                            <Button onClick={() => handleSubmit()} className="bg-primary">Xác nhận</Button>}
                    </div>
                </form>
            </div>
        </>
    );
};


export default AddFlashSale;