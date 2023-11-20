import { Button, Dialog, DialogBody, DialogFooter, DialogHeader, IconButton, Tooltip, Typography } from "@material-tailwind/react";
import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig"
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import UpdateFlashSale from "./components/FlashSale/UpdateFlashSale";

const TABLE_HEAD = ["Khung giờ", "Số sản phẩm", "Trạng thái", ""];
const backgroundColors = ["bg-gray-50", "bg-gray-200"];

const FlashSale = () => {
    const navigate = useNavigate();
    const [listSale, setListSale] = useState([]);
    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

    const GetListSale = async () => {
        try {
            axios
                .get(`/api/Store/GetStoreByUid?uId=`)
                .then((response) => {
                    setListSale(response);
                })
                .catch((error) => {
                    console.log(error);
                })
        } catch (error) {
            console.log(error);
        }
    };

    const handleDelete = async (iddd) => {
        try {
            axios
                .get(`/api/Store/GetStoreByUid?uId=${iddd}`)
                .then(() => {
                    toast.success("Xóa thành công!");
                })
                .catch((error) => {
                    console.log(error);
                })
        } catch (error) {
            console.log(error);
        }
    };

    useEffect(() => {
        GetListSale();
    }, []);

    return (
        <>
            <Typography variant="h3">Flash Sale Của Shop</Typography>
            <div className="p-5 mt-5 bg-gray-100 shadow-md rounded-t-lg">
                <div className="pb-5 flex items-center justify-between">
                    <Typography variant="h5">Danh sách chương trình</Typography>
                    <Button
                        className="flex gap-1 items-center bg-primary"
                        onClick={() => navigate(`/flash-sale/add`)}
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            height="1.2em"
                            viewBox="0 0 448 512"
                        >
                            <path
                                d="M256 80c0-17.7-14.3-32-32-32s-32 14.3-32 32V224H48c-17.7 0-32 14.3-32 32s14.3 32 32 32H192V432c0 17.7 14.3 32 32 32s32-14.3 32-32V288H400c17.7 0 32-14.3 32-32s-14.3-32-32-32H256V80z"
                                fill="white"
                            />
                        </svg>
                        Tạo Flash Sale</Button>
                </div>
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
                        {listSale.length > 0 ?
                            listSale.map((food, index) => (
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

                                            className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                        </input>
                                    </td>
                                    <td className="w-14 lg:w-48">
                                        <input
                                            type="number"

                                            className="w-14 lg:w-24 px-1 border-2 border-gray-300 rounded-md">
                                        </input>
                                    </td>
                                    <td className="w-24 lg:w-48">
                                        <input
                                            type="number"

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
                                            <Tooltip content="Xóa flash sale">
                                                <IconButton
                                                    variant="text"
                                                    onClick={handleOpen}
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
                            : null}
                    </tbody>
                </table>
                <UpdateFlashSale></UpdateFlashSale>
            </div>
            <Dialog
                size="xs"
                open={open}
                handler={handleOpen}
                className="bg-white shadow-none"
            >
                <DialogHeader>Xác nhận xóa</DialogHeader>
                <DialogBody divider>Bạn có muốn xóa món ăn này không?</DialogBody>
                <DialogFooter>
                    <Button
                        variant="text"
                        color="red"
                        onClick={handleOpen}
                        className="mr-1"
                    >
                        <span>Quay lại</span>
                    </Button>
                    <Button variant="gradient" color="green" onClick={handleDelete}>
                        <span>Xác nhận</span>
                    </Button>
                </DialogFooter>
            </Dialog>
        </>
    );
};

export default FlashSale;