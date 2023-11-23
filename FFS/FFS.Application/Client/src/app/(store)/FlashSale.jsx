import { Button, Dialog, DialogBody, DialogFooter, DialogHeader, IconButton, Input, Tooltip, Typography } from "@material-tailwind/react";
import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig"
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import UpdateFlashSale from "./components/FlashSale/UpdateFlashSale";
import CookieService from "../../shared/helper/cookieConfig";

const TABLE_HEAD = ["Khung giờ", "Số sản phẩm", "Trạng thái", ""];
const backgroundColors = ["bg-gray-50", "bg-gray-200"];

const FlashSale = () => {
    const uId = CookieService.getToken("fu_foody_id");
    const navigate = useNavigate();
    const [listSale, setListSale] = useState([]);
    const [dayStartFilter, setDayStartFilter] = useState();
    const [dayEndFilter, setDayEndFilter] = useState();
    const [storeId, setStoreId] = useState(0);
    const [pageNumber, setPageNumber] = useState(1);
    const pageSize = 5;
    const [totalPages, setTotalPages] = useState(1);
    const [flashSaleDelete, setFlashSaleDelete] = useState(0);

    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

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

    const GetListSale = async () => {
        try {
            axios
                .get(`/api/FlashSale/ListFlashSaleByStore/${storeId}`,
                    {
                        params:
                        {
                            Start: dayStartFilter,
                            End: dayEndFilter,
                            PageNumber: pageNumber,
                            PageSize: pageSize
                        }
                    })
                .then((response) => {
                    setListSale(response.flashSaleDTOs);
                    setTotalPages(response.metadata.totalPages);
                })
                .catch((error) => {
                    console.log(error);
                })
        } catch (error) {
            console.log(error);
        }
    };

    const handleDelete = async () => {
        try {
            axios
                .delete(`/api/FlashSale/DeleteFlashSale/${flashSaleDelete}`)
                .then(() => {
                    toast.success("Xóa thành công!");
                    GetListSale();
                    setOpen(false);
                })
                .catch((error) => {
                    console.log(error);
                })
        } catch (error) {
            console.log(error);
        }
    };

    const handlePageChange = (newPage) => {
        if (newPage >= 1 && newPage <= totalPages) {
            setPageNumber(newPage);
        }
    };

    useEffect(() => {
        GetStoreByUid();
    }, [uId]);

    useEffect(() => {
        GetListSale();
    }, [storeId, pageNumber, dayStartFilter, dayEndFilter]);

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
            <Typography variant="h3">Flash Sale Của Shop</Typography>
            <div className="p-5 mt-5 bg-gray-100 shadow-md rounded-t-lg">
                <div className="pb-5 flex items-center justify-between">
                    <Typography variant="h5">Danh sách chương trình</Typography>
                    <div>
                        <Input
                            label="Ngày bắt đầu"
                            value={dayStartFilter || ""}
                            onChange={(e) => setDayStartFilter(e.target.value)}
                            type="datetime-local"
                        >
                        </Input>
                    </div>
                    <div>
                        <Input
                            label="Ngày kết thúc"
                            value={dayEndFilter || ""}
                            onChange={(e) => setDayEndFilter(e.target.value)}
                            type="datetime-local"
                        >
                        </Input>
                    </div>
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
                            listSale.map((sale, index) => (
                                <tr
                                    key={sale.id}
                                    className={
                                        backgroundColors[index % backgroundColors.length]
                                    }
                                >
                                    <td className="lg:flex lg:justify-center">
                                        <Typography
                                            color="blue-gray"
                                            className="font-semibold text-left pl-4"
                                        >
                                            Từ: {formatDate(new Date(sale.start))} - {formatTime(new Date(sale.start))}
                                        </Typography>
                                        <Typography
                                            color="blue-gray"
                                            className="font-semibold text-left pl-4"
                                        >
                                            Đến: {formatDate(new Date(sale.end))} - {formatTime(new Date(sale.end))}
                                        </Typography>
                                    </td>
                                    <td>
                                        <Typography
                                            variant="small"
                                            color="blue-gray"
                                            className="font-normal"
                                        >
                                            {sale.noOfParticipateFoodSale}
                                        </Typography>
                                    </td>
                                    <td>
                                        {sale.flashSaleStatus == "Sắp diễn ra" ? (
                                            <Typography
                                                variant="h6"
                                                color="orange"
                                                className="max-w-xs truncate"
                                            >
                                                Sắp diễn ra
                                            </Typography>
                                        ) : null}
                                        {sale.flashSaleStatus == "Chưa diễn ra" ? (
                                            <Typography
                                                variant="h6"
                                                color="gray"
                                                className="max-w-xs truncate"
                                            >
                                                Chưa diễn ra
                                            </Typography>
                                        ) : null}
                                        {sale.flashSaleStatus == "Đang diễn ra" ? (
                                            <Typography
                                                variant="h6"
                                                color="green"
                                                className="max-w-xs truncate"
                                            >
                                                Đang diễn ra
                                            </Typography>
                                        ) : null}
                                        {sale.flashSaleStatus == "Đã kết thúc" ? (
                                            <Typography
                                                variant="h6"
                                                color="red"
                                                className="max-w-xs truncate"
                                            >
                                                Đã kết thúc
                                            </Typography>
                                        ) : null}
                                    </td>
                                    <td>
                                        <div className="h-full flex justify-center items-center">
                                            <UpdateFlashSale fId={sale.id} reload={GetListSale} dayStart={sale.start} dayEnd={sale.end} fsList={sale.flashSaleDetails}></UpdateFlashSale>
                                            <Tooltip content="Xóa flash sale">
                                                <IconButton
                                                    variant="text"
                                                    onClick={() => {
                                                        handleOpen();
                                                        setFlashSaleDelete(sale.id);
                                                    }}
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
                <div className="flex w-full items-center justify-between border-t border-blue-gray-50 p-4">
                    <Button
                        variant="outlined"
                        size="sm"
                        onClick={() => handlePageChange(pageNumber - 1)}
                    >
                        Previous
                    </Button>
                    <div className="flex items-center gap-2">
                        {Array.from({ length: totalPages }, (_, i) => (
                            <IconButton
                                key={i}
                                variant={i + 1 === pageNumber ? "outlined" : "text"}
                                size="sm"
                                onClick={() => handlePageChange(i + 1)}
                            >
                                {i + 1}
                            </IconButton>
                        ))}
                    </div>
                    <Button
                        variant="outlined"
                        size="sm"
                        onClick={() => handlePageChange(pageNumber + 1)}
                    >
                        Next
                    </Button>
                </div>

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