import { Button, IconButton, Input, Option, Radio, Select, Spinner, Tooltip, Typography } from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import CookieService from "../../shared/helper/cookieConfig";
import { toast } from "react-toastify";

const filter = [{ id: 1, name: "Flash sale" }, { id: 2, name: "Bán chạy nhất" }, { id: 3, name: "Đánh giá hàng đầu" }, { id: 4, name: "Sản phẩm mới" }]

const FoodList = () => {
    const [storeId, setStoreId] = useState(0);
    const [foodList, setFoodList] = useState([]);
    const [foodNameFilter, setFoodNameFilter] = useState("");
    const [pageNumber, setPageNumber] = useState(1);
    const pageSize = 10;
    const [totalPages, setTotalPages] = useState(0);
    const [category, setCategory] = useState([]);
    const [selectedCategory, setSelectedCategory] = useState("");
    const [priceRange, setPriceRange] = useState('');
    const [minPrice, setMinPrice] = useState('');
    const [maxPrice, setMaxPrice] = useState('');
    const uId = CookieService.getToken("fu_foody_id");

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

    const ListCategory = async () => {
        try {
            axios
                .get("/api/Category/ListCategoryByStoreId/" + storeId)
                .then((response) => {
                    setCategory(response.data.result);
                    setCategory([{ id: "", categoryName: 'All' }, ...response.data.result]);
                })
                .catch((error) => {
                    console.log(error);
                    toast.error("Lấy phân loại thất bại!");
                });
        } catch (error) {
            console.error("Category: " + error);
        }
    };

    const handleFoodListByCategory = async (e) => {
        setSelectedCategory(e.target.value);
    };

    const handlePriceRangeChange = (value, min, max) => {
        setPriceRange(value);
        setMinPrice(min);
        setMaxPrice(max);
    };

    const handlePageChange = (newPage) => {
        if (newPage >= 1 && newPage <= totalPages) {
            setPageNumber(newPage);
        }
    };

    useEffect(() => {
        GetStoreByUid();
        ListCategory();
    }, [uId]);

    return (
        <div className="flex gap-5">
            <div className="flex w-60 flex-col">
                <Typography variant="h6">DANH MỤC</Typography>
                {category ?
                    category.map((category) => (
                        <label key={category.id}>
                            <input
                                type="radio"
                                name="category"
                                value={category.id}
                                checked={selectedCategory === category.id}
                                onChange={handleFoodListByCategory}
                            />
                            {category.name}
                        </label>
                    ))
                    : <Spinner></Spinner>}
                <hr className="h-px my-2 bg-gray-200 border-0 dark:bg-gray-700" />
                <Typography variant="h6">GIÁ CẢ</Typography>
                <div className="flex gap-2 justify-center items-center">
                    <label>Từ:</label>
                    <input
                        className="border rounded p-1 w-16"
                        type="number"
                        value={minPrice}
                        onChange={(e) => setMinPrice(e.target.value)}
                    />
                    <label>Đến:</label>
                    <input
                        className="border rounded p-1 w-16"
                        type="number"
                        value={maxPrice}
                        onChange={(e) => setMaxPrice(e.target.value)}
                    />
                </div>
                <div>
                    <Radio
                        checked={priceRange === 'all'}
                        onChange={() => handlePriceRangeChange('all', 0, 0)}
                    />
                    <label>Tất cả</label>
                </div>
                <div>
                    <Radio
                        checked={priceRange === 'range1'}
                        onChange={() => handlePriceRangeChange('range1', 0, 35)}
                    />
                    <label>Dưới 35.000đ</label>
                </div>
                <div>
                    <Radio
                        checked={priceRange === 'range2'}
                        onChange={() => handlePriceRangeChange('range2', 35, 50)}
                    />
                    <label>35.000đ - 50.000đ</label>
                </div>
                <div>
                    <Radio
                        checked={priceRange === 'range3'}
                        onChange={() => handlePriceRangeChange('range3', 50, 100)}
                    />
                    <label>50.000đ - 100.000đ</label>
                </div>
                <div>
                    <Radio
                        checked={priceRange === 'range4'}
                        onChange={() => handlePriceRangeChange('range4', 100, 999)}
                    />
                    <label>Trên 100.000đ</label>
                </div>
            </div>
            <div className="w-full">
                <div className="flex justify-between">
                    <div className="w-96">
                        <Input
                            label="Tìm kiếm"
                            icon={
                                <svg
                                    width="20"
                                    height="20"
                                    viewBox="0 0 20 20"
                                    fill="none"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path
                                        d="M9.0625 15.625C12.6869 15.625 15.625 12.6869 15.625 9.0625C15.625 5.43813 12.6869 2.5 9.0625 2.5C5.43813 2.5 2.5 5.43813 2.5 9.0625C2.5 12.6869 5.43813 15.625 9.0625 15.625Z"
                                        stroke="#191C1F"
                                        strokeWidth="1.5"
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                    />
                                    <path
                                        d="M13.7031 13.7031L17.5 17.5"
                                        stroke="#191C1F"
                                        strokeWidth="1.5"
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                    />
                                </svg>
                            }
                        // value={foodNameFilter}
                        // onChange={(e) => setFoodNameFilter(e.target.value)}
                        />
                    </div>
                    <div className="flex items-center">
                        <Typography className="w-24">Bộ lọc: </Typography>
                        <Select
                            className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
                            onChange={(e) => {
                                console.log(e);
                            }}
                            label="Chọn loại"
                        >
                            {filter.map((filter) => (
                                <Option key={filter.id} value={filter.id.toString()}>
                                    {filter.name}
                                </Option>
                            ))}
                        </Select>
                    </div>
                </div>
                <div className="grid m-2 gap-1 grid-flow-row-dense grid-cols-2 grid-rows-3 lg:grid-cols-4">
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-80 object-cover lg:w-64 group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    {/* Lặp */}
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex lg:flex-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-60 object-cover group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex lg:flex-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-60 object-cover group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex lg:flex-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-60 object-cover group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex lg:flex-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-60 object-cover group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex lg:flex-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-60 object-cover group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex lg:flex-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-60 object-cover group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    <div className="px-1 pt-1 pb-2 border-solid border-2">
                        <div className="group relative flex lg:flex-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-36 w-60 object-cover group-hover:opacity-40"
                            />
                            <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm group-hover:opacity-40">
                                <Typography className="text-white font-semibold">HOT</Typography>
                            </div>
                            <div className="absolute hidden h-full w-full justify-around items-center group-hover:flex">
                                <Tooltip content="Thêm yêu thích">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 512 512"
                                        >
                                            <path
                                                d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
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
                                <Tooltip content="Xem chi tiết món ăn">
                                    <IconButton variant="text" className="bg-white rounded-full">
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="30"
                                            height="20"
                                            viewBox="0 0 550 512"
                                        >
                                            <path
                                                d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                            />
                                        </svg>
                                    </IconButton>
                                </Tooltip>
                            </div>
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE</Typography>
                            <Typography color="blue" className="pb-2 pointer-events-none">25.000đ</Typography>
                        </div>
                    </div>
                    {/* Hết lặp */}
                </div>
                <div className="flex items-center justify-between border-t border-blue-gray-50 p-4">
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
        </div>
    );
};

export default FoodList;