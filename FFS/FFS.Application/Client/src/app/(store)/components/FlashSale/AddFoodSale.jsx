import { Button, Card, CardBody, CardFooter, CardHeader, Checkbox, Dialog, IconButton, Input, Typography } from "@material-tailwind/react";
import { useEffect, useState } from "react";
import ArrowRight from "../../../../shared/components/icon/ArrowRight";
import ArrowLeft from "../../../../shared/components/icon/ArrowLeft";
import CookieService from "../../../../shared/helper/cookieConfig";
import axios from "../../../../shared/api/axiosConfig";
import propTypes from "prop-types";

const AddFoodSale = ({ getFoodList }) => {
    const uId = CookieService.getToken("fu_foody_id");

    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

    const [inventory, setInventory] = useState([]);
    const [foodNameFilter, setFoodNameFilter] = useState("");
    const [storeId, setStoreId] = useState(0);
    const [pageNumber, setPageNumber] = useState(1);
    const pageSize = 5;
    const [totalPages, setTotalPages] = useState(1);
    const [active, setActive] = useState(1);
    const [isCheckedAll, setIsCheckedAll] = useState(false);
    const [checkedItems, setCheckedItems] = useState({});

    const handleCheckAll = () => {
        const newCheckedState = !isCheckedAll;
        setIsCheckedAll(newCheckedState);

        const newCheckedItems = {};
        inventory.forEach(({ id }) => {
            newCheckedItems[id] = newCheckedState;
        });
        setCheckedItems(newCheckedItems);
    };

    const handleCheckSingle = (itemId) => {
        const newCheckedItems = { ...checkedItems, [itemId]: !checkedItems[itemId] };
        setCheckedItems(newCheckedItems);

        const areAllChecked = Object.values(newCheckedItems).every((isChecked) => isChecked);
        setIsCheckedAll(areAllChecked);
    };

    const getItemProps = (index) => ({
        variant: active === index ? "filled" : "text",
        onClick: () => {
            setActive(index);
            setPageNumber(index);
        },
        className: `rounded-full ${active === index ? "bg-primary" : ""}`,
    });

    const next = () => {
        if (active < totalPages) {
            setActive(active + 1);
            setPageNumber(pageNumber + 1);
        }
    };

    const prev = () => {
        if (active > 1) {
            setActive(active - 1);
            setPageNumber(pageNumber - 1);
        }
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

    const fetchInventory = async () => {
        try {
            const response = await axios.get("/api/Inventory/GetInventories", {
                params: {
                    StoreId: storeId,
                    FoodName: foodNameFilter,
                    PageNumber: pageNumber,
                    PageSize: pageSize,
                },
            });
            setInventory(response.entityInventory);
            console.log(inventory);
            setTotalPages(response.metadata.totalPages);
        } catch (error) {
            console.error("Error fetching inventory data:", error);
        }
    };

    const handleSubmit = () => {
        const selectedItems = inventory.filter(({ id }) => checkedItems[id]);

        const selectedFoodList = selectedItems.map(
            ({ foodId, imageURL, foodName, quantity, categoryName }) => ({               
                foodId,
                imageURL,
                foodName,
                quantity,
                categoryName,
            })
        );

        getFoodList(selectedFoodList);
        handleOpen();
    };

    useEffect(() => {
        fetchInventory();
    }, [storeId, foodNameFilter, pageNumber, pageSize]);

    useEffect(() => {
        GetStoreByUid();
    }, []);

    const TABLE_HEAD = [
        <Checkbox key="checkbox-header" label="Chọn tất cả" checked={isCheckedAll} onChange={handleCheckAll}></Checkbox>,
        "Ảnh món ăn",
        "Tên món ăn",
        "Phân loại",
        "Tồn kho"
    ];

    return (
        <>
            <Button color="white" className="flex mt-2 gap-2 text-orange-500 border-solid border-2 border-orange-500" onClick={handleOpen}>
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
            <Dialog
                size="lg"
                open={open}
                handler={handleOpen}
                className="max-h-screen bg-transparent shadow-none overflow-y-scroll"
            >
                <Card className="h-full w-full rounded-r-none" shadow={false} floated={false}>
                    <CardHeader floated={false} shadow={false} className="rounded-none">
                        <Typography variant="h3" className="text-center">Thêm sản phẩm Flash Sale</Typography>
                        <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
                            <div className="w-72">
                                <Input
                                    label="Tên món ăn"
                                    icon={<i className="fas fa-search" />}
                                    value={foodNameFilter}
                                    onChange={(e) => setFoodNameFilter(e.target.value)}
                                />
                            </div>
                        </div>
                    </CardHeader>
                    <CardBody className="p-0 mt-1">
                        <table className="w-full min-w-max table-auto">
                            <thead>
                                <tr>
                                    {TABLE_HEAD.map((head) => (
                                        <th
                                            key={head}
                                            className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-1"
                                        >
                                            <Typography
                                                variant="small"
                                                color="black"
                                                className="font-normal leading-none opacity-70"
                                            >
                                                {head}
                                            </Typography>
                                        </th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {inventory.map(
                                    (
                                        {
                                            id,
                                            imageURL,
                                            foodName,
                                            quantity,
                                            categoryName,
                                        },
                                        index
                                    ) => {
                                        const isLast = index === inventory.length - 1;
                                        const isCheckboxColumn = true;
                                        const commonClasses = isLast
                                            ? "p-2 text-center"
                                            : "p-2 border-b border-blue-gray-50 text-center";

                                        const classes = isCheckboxColumn
                                            ? `${commonClasses} w-36`
                                            : commonClasses;
                                        const imageClasses = isLast
                                            ? "p-1 flex items-center justify-center"
                                            : "p-1 border-b border-blue-gray-50 flex items-center justify-center";
                                        const isItemChecked = checkedItems[id];
                                        return (
                                            <tr key={id}>
                                                <td className={classes}>
                                                    {isCheckboxColumn && (
                                                        <Checkbox
                                                            checked={isItemChecked}
                                                            onChange={() => handleCheckSingle(id)}
                                                        ></Checkbox>
                                                    )}
                                                </td>
                                                <td className={imageClasses}>
                                                    <img
                                                        className="h-[5.3rem] w-24 object-cover object-center"
                                                        src={imageURL}
                                                        alt={foodName}
                                                    />
                                                </td>
                                                <td className={classes}>
                                                    <Typography
                                                        variant="small"
                                                        color="blue-gray"
                                                        className="font-normal"
                                                    >
                                                        {foodName}
                                                    </Typography>
                                                </td>
                                                <td className={classes}>
                                                    <Typography
                                                        variant="small"
                                                        color="blue-gray"
                                                        className="font-normal"
                                                    >
                                                        {categoryName}
                                                    </Typography>
                                                </td>
                                                <td className={classes}>
                                                    <Typography
                                                        variant="small"
                                                        color="blue-gray"
                                                        className="font-normal"
                                                    >
                                                        {quantity}
                                                    </Typography>
                                                </td>
                                            </tr>
                                        );
                                    }
                                )}
                            </tbody>
                        </table>
                    </CardBody>

                    <CardFooter className="flex items-center justify-between border-t border-blue-gray-50 p-2">
                        <Button
                            variant="text"
                            className="flex items-center gap-2 rounded-full"
                            onClick={prev}
                            disabled={active === 1}
                        >
                            <ArrowLeft /> Previous
                        </Button>
                        <div className="flex items-center gap-2">
                            {Array.from({ length: totalPages }, (_, index) => (
                                <IconButton key={index + 1} {...getItemProps(index + 1)}>
                                    {index + 1}
                                </IconButton>
                            ))}
                        </div>
                        <Button
                            variant="text"
                            className="flex items-center gap-2 rounded-full"
                            onClick={next}
                            disabled={active === totalPages}
                        >
                            Next <ArrowRight />
                        </Button>
                    </CardFooter>
                    <div className="flex px-3 pb-2 gap-2 justify-end w-full">
                        <Button variant="text" onClick={handleOpen}>Hủy</Button>
                        <Button className="bg-primary right-0" onClick={handleSubmit}>Xác nhận</Button>
                    </div>
                </Card>
            </Dialog>
        </>
    );
};

AddFoodSale.propTypes = {
    getFoodList: propTypes.any.isRequired,
}

export default AddFoodSale;