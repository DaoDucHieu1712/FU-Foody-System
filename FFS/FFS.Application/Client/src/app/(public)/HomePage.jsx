import { Button, Carousel, IconButton, Rating, Spinner, Tooltip, Typography } from "@material-tailwind/react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import axios from "../../shared/api/axiosConfig";

const HomePage = () => {
    const navigate = useNavigate();
    const [flashSale, setFlashSale] = useState([]);
    const [bestSeller, setBestSeller] = useState([]);
    const [bestRating, setBestRating] = useState([]);
    const [newFood, setNewFood] = useState([]);

    const GetListFilterFood = async () => {
        try {
            axios
                .get(`/api/Food/ListAllFood?FilterFood=1&PageNumber=1&PageSize=5`)
                .then((response) => {
                    setFlashSale(response.foodDTOs);
                })
                .catch((error) => {
                    console.log(error);
                    toast.error("Lấy sản phẩm flash sale thất bại!");
                });
            axios
                .get(`/api/Food/ListAllFood?FilterFood=2&PageNumber=1&PageSize=5`)
                .then((response) => {
                    setBestSeller(response.foodDTOs);
                })
                .catch((error) => {
                    console.log(error);
                    toast.error("Lấy sản phẩm bán chạy nhất thất bại!");
                });
            axios
                .get(`/api/Food/ListAllFood?FilterFood=3&PageNumber=1&PageSize=5`)
                .then((response) => {
                    setBestRating(response.foodDTOs);
                })
                .catch((error) => {
                    console.log(error);
                    toast.error("Lấy sản phẩm đánh giá hàng đầu thất bại!");
                });
            axios
                .get(`/api/Food/ListAllFood?FilterFood=4&PageNumber=1&PageSize=5`)
                .then((response) => {
                    setNewFood(response.foodDTOs);
                })
                .catch((error) => {
                    console.log(error);
                    toast.error("Lấy sản phẩm mới nhất thất bại!");
                });
        } catch (error) {
            console.error("Category: " + error);
        }
    };

    useEffect(() => {
        GetListFilterFood();
    }, []);


    return (
        <div>
            {/* Carousel */}
            <div className="py-4 grid grid-flow-row-dense grid-rows-4 grid-cols-1 lg:grid-rows-2 lg:grid-cols-3 gap-2 ">
                <div className="row-span-2 lg:col-span-2">
                    <Carousel autoplay loop autoplayDelay={7000} transition={{ duration: 2 }} className=" bg-gray-50">
                        <div className="flex justify-around items-center overflow-hidden">
                            <div>
                                <Typography variant="h3" className="pointer-events-none">Siêu đại tiệc</Typography>
                                <Typography className="w-48 md:w-auto pb-5 pointer-events-none">Giảm tối đa 20% cho các bạn đến quán...</Typography>
                                <Button className="flex items-center gap-2 text-white font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900">
                                    Xem ngay
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        height="1.5em"
                                        viewBox="0 0 512 512"
                                        fill="white"
                                    >
                                        <path
                                            d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z"
                                        />
                                    </svg>
                                </Button>
                            </div>
                            <div className="relative pointer-events-none">
                                <img
                                    src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                    alt="image 1"
                                    className="h-56 w-64 pt-5 object-cover"
                                />
                                <div className="absolute top-0 md:right-0 h-12 w-12 bg-primary rounded-full text-white font-bold transform -translate-x-1/2 md:translate-x-1/2">
                                    <div className="flex h-full justify-center items-center">
                                        115k
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="flex justify-around items-center overflow-hidden">
                            <div>
                                <Typography variant="h3" className="pointer-events-none">Siêu đại tiệc</Typography>
                                <Typography className="w-48 md:w-auto pb-5 pointer-events-none">Giảm tối đa 20% cho các bạn đến quán...</Typography>
                                <Button className="flex items-center gap-2 text-white font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900">
                                    Xem ngay
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        height="1.5em"
                                        viewBox="0 0 512 512"
                                        fill="white"
                                    >
                                        <path
                                            d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z"
                                        />
                                    </svg>
                                </Button>
                            </div>
                            <div className="relative pointer-events-none">
                                <img
                                    src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                    alt="image 1"
                                    className="h-56 w-64 pt-5 object-cover"
                                />
                                <div className="absolute top-0 md:right-0 h-12 w-12 bg-primary rounded-full text-white font-bold transform -translate-x-1/2 md:translate-x-1/2">
                                    <div className="flex h-full justify-center items-center">
                                        115k
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="flex justify-around items-center overflow-hidden">
                            <div>
                                <Typography variant="h3" className="pointer-events-none">Siêu đại tiệc</Typography>
                                <Typography className="w-48 md:w-auto pb-5 pointer-events-none">Giảm tối đa 20% cho các bạn đến quán...</Typography>
                                <Button className="flex items-center gap-2 text-white font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900">
                                    Xem ngay
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        height="1.5em"
                                        viewBox="0 0 512 512"
                                        fill="white"
                                    >
                                        <path
                                            d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z"
                                        />
                                    </svg>
                                </Button>
                            </div>
                            <div className="relative pointer-events-none">
                                <img
                                    src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                    alt="image 1"
                                    className="h-56 w-64 pt-5 object-cover"
                                />
                                <div className="absolute top-0 md:right-0 h-12 w-12 bg-primary rounded-full text-white font-bold transform -translate-x-1/2 md:translate-x-1/2">
                                    <div className="flex h-full justify-center items-center">
                                        115k
                                    </div>
                                </div>
                            </div>
                        </div>
                    </Carousel>
                </div>
                <div className="bg-black">
                    <div className="flex justify-around items-center">
                        <div>
                            <Typography variant="paragraph" color="yellow" className="text-xs pointer-events-none">Giảm giá</Typography>
                            <Typography color="white" className="pb-2 font-semibold pointer-events-none">Cơm trộn</Typography>
                            <Button size="sm" className="flex items-center gap-1 text-white text-center font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900">
                                Mua ngay
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    height="1.5em"
                                    viewBox="0 0 512 512"
                                    fill="white"
                                >
                                    <path
                                        d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z"
                                    />
                                </svg>
                            </Button>
                        </div>
                        <div className="relative pointer-events-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-32 w-48 py-3 object-cover"
                            />
                            <div className="absolute top-0 right-2 h-6 w-fit px-2 bg-yellow-300 rounded-sm text-black text-sm font-semibold transform translate-y-1">
                                <div className="flex h-full justify-center items-center">
                                    10% off
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="bg-gray-50">
                    <div className="flex justify-around">
                        <div className="pointer-events-none">
                            <img
                                src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                alt="image 1"
                                className="h-32 w-48 py-1 object-cover"
                            />
                        </div>
                        <div>
                            <Typography variant="h6" className="w-36 pointer-events-none">Trà TMORE </Typography>
                            <Typography color="blue" className="pb-2 relative w-fit pointer-events-none">
                                25.000
                                <span className="absolute font-normal top-0 -right-2 text-xs">
                                    đ
                                </span>
                            </Typography>
                            <Button size="sm" className="flex items-center gap-1 text-white text-center font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900 ">
                                Mua ngay
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    height="1.5em"
                                    viewBox="0 0 512 512"
                                    fill="white"
                                >
                                    <path
                                        d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z"
                                    />
                                </svg>
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
            {/* End Carousel */}

            {/* Deal price */}
            <div className="py-4">
                <div className="flex px-4 pb-2 justify-between">
                    <div className="flex items-center">
                        <Typography variant="h5">Deal hôm nay</Typography>
                        <p className="px-5">Kết thúc</p>

                    </div>
                    <p onClick={() => navigate(`/food-list`)} className="flex gap-2 items-center font-medium text-blue-600 dark:text-blue-500 cursor-pointer hover:underline">
                        Xem tất cả
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            height="1em"
                            viewBox="0 0 512 512"
                            fill="rgb(30 136 229 / var(--tw-text-opacity))"
                        >
                            <path
                                d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z"
                            />
                        </svg>
                    </p>
                </div>
                <div className="grid grid-flow-row-dense grid-cols-4 lg:grid-rows-2 lg:grid-cols-5">
                    <div className="inline-flex px-1 pt-1 pb-2 border-solid border-2 col-span-4 lg:inline lg:row-span-2 lg:col-span-1">
                        <div className="lg:pb-1">
                            <Typography variant="small" className="px-2 w-fit font-semibold bg-yellow-500">32% OFF</Typography>
                            <div className="px-2 relative flex lg:flex-none pointer-events-none">
                                <img
                                    src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                                    alt="image 1"
                                    className="h-36 w-60 py-1 object-cover"
                                />
                                <div className="absolute top-0 left-0 h-6 w-fit px-2 bg-primary rounded-sm">
                                    <Typography className="text-white font-semibold">HOT</Typography>
                                </div>
                            </div>
                        </div>
                        <div className="lg:py-2">
                            <div className="flex items-center gap-2 font-bold">
                                <Rating value={4} readonly />
                                <Typography variant="small" className="font-semibold">
                                    4.7
                                </Typography>
                            </div>
                            <Typography variant="h6">Chimico - Cơm Trộn & Kim Chi Tỏi Đen</Typography>
                            <Typography color="blue" className="relative w-fit pointer-events-none">
                                50.000
                                <span className="absolute font-normal top-0 -right-2 text-xs">
                                    đ
                                </span>
                            </Typography>
                            <Typography variant="small">Giảm 25k khi đặt combo Pepsi. Số lượng có hạn mỗi ngày</Typography>
                        </div>
                        <div className="flex lg:pt-3 justify-around items-center">
                            <Tooltip content="Thêm yêu thích">
                                <IconButton variant="text">
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        width="30"
                                        height="20"
                                        viewBox="0 0 448 512"
                                    >
                                        <path
                                            d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z"
                                        />
                                    </svg>
                                </IconButton>
                            </Tooltip>
                            <Button size="sm" className="flex items-center w-fit text-white font-bold bg-primary">
                                <svg
                                    width="30"
                                    height="2em"
                                    viewBox="0 0 38 38"
                                    fill="none"
                                    xmlns="http://www.w3.org/2000/svg"
                                >
                                    <path
                                        d="M10 35C11.1046 35 12 34.1046 12 33C12 31.8954 11.1046 31 10 31C8.89543 31 8 31.8954 8 33C8 34.1046 8.89543 35 10 35Z"
                                        fill="white"
                                    />
                                    <path
                                        d="M23 35C24.1046 35 25 34.1046 25 33C25 31.8954 24.1046 31 23 31C21.8954 31 21 31.8954 21 33C21 34.1046 21.8954 35 23 35Z"
                                        fill="white"
                                    />
                                    <path
                                        d="M5.2875 15H27.7125L24.4125 26.55C24.2948 26.9692 24.0426 27.3381 23.6948 27.6001C23.3471 27.862 22.9229 28.0025 22.4875 28H10.5125C10.0771 28.0025 9.65293 27.862 9.30515 27.6001C8.95738 27.3381 8.70524 26.9692 8.5875 26.55L4.0625 10.725C4.0027 10.5159 3.8764 10.3321 3.70271 10.2012C3.52903 10.0704 3.31744 9.99977 3.1 10H1"
                                        stroke="white"
                                        strokeWidth="2"
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                    />
                                </svg>
                                THÊM GIỎ HÀNG
                            </Button>
                            <Tooltip content="Xem chi tiết món ăn">
                                <IconButton variant="text">
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        width="30"
                                        height="20"
                                        viewBox="0 0 448 512"
                                    >
                                        <path
                                            d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z"
                                        />
                                    </svg>
                                </IconButton>
                            </Tooltip>
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
                            <Typography color="blue" className="pb-2 relative w-fit pointer-events-none">
                                25.000
                                <span className="absolute font-normal top-0 -right-2 text-xs">
                                    đ
                                </span>
                            </Typography>
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
            </div>
            {/* End Deal Price */}

            {/* Banner */}
            <div className="flex justify-around h-72 w-full bg-gray-50 items-center">
                <div>
                    <p className="px-2 py-1 w-fit text-xs text-white bg-blue-500 pointer-events-none">Up to 20%</p>
                    <Typography variant="h3" className="pointer-events-none">Khai trương quán</Typography>
                    <Typography className="pb-5 pointer-events-none">Giảm tối đa 20% cho các bạn đến quán...</Typography>
                    <Button className="flex items-center gap-2 text-white text-center font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900">
                        Xem ngay
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            height="1.5em"
                            viewBox="0 0 512 512"
                            fill="white"
                        >
                            <path
                                d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z"
                            />
                        </svg>
                    </Button>
                </div>
                <div className="relative pointer-events-none">
                    <img
                        src="https://images.unsplash.com/photo-1497436072909-60f360e1d4b1?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2560&q=80"
                        alt="image 1"
                        className="h-72 w-96 object-cover"
                    />
                    <div className="absolute top-0 h-20 w-20 bg-primary rounded-full border-solid border-4 text-black text-sm font-semibold transform translate-y-5 -translate-x-5">
                        <div className="flex h-full justify-center items-center">
                            329K
                        </div>
                    </div>
                </div>
            </div>
            {/* End Banner */}

            {/* Flash Sale, Popular,... */}
            <div className="grid py-4 gap-4 grid-flow-row-dense grid-cols-2 justify-stretch lg:flex lg:gap-2">
                <div className="lg:w-1/4">
                    {/* Flash sale */}
                    <Typography variant="h5">FLASH SALE</Typography>
                    {flashSale ?
                        flashSale.map((flashSaleItem) => (
                            <div key={flashSaleItem.id} className="flex p-1 my-1 justify-between items-center shadow-lg">
                                <div className="group relative flex">
                                    <img
                                        src={flashSaleItem.imageURL}
                                        alt={flashSaleItem.foodName}
                                        className="h-28 w-44 object-cover group-hover:opacity-40"
                                    />
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
                                            <IconButton
                                                variant="text"
                                                className="bg-white rounded-full"
                                                onClick={() => navigate(`/food-details/${flashSaleItem.id}`)}
                                            >
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
                                <div className="w-2/5">
                                    <Typography variant="h6" className="pointer-events-none">{flashSaleItem.foodName}</Typography>
                                    <Typography color="blue" className="pb-2 relative w-fit pointer-events-none">
                                        {flashSaleItem.price}.000
                                        <span className="absolute font-normal top-0 -right-2 text-xs">
                                            đ
                                        </span>
                                    </Typography>
                                </div>
                            </div>
                        ))
                        : <Spinner></Spinner>
                    }
                </div>
                {/* Best seller */}
                <div className="lg:w-1/4">
                    <Typography variant="h5">BÁN CHẠY NHẤT</Typography>
                    {bestSeller ?
                        bestSeller.map((bestSellerItem) => (
                            <div key={bestSellerItem.id} className="flex p-1 my-1 justify-between items-center shadow-lg">
                                <div className="group relative flex">
                                    <img
                                        src={bestSellerItem.imageURL}
                                        alt={bestSellerItem.foodName}
                                        className="h-28 w-44 object-cover group-hover:opacity-40"
                                    />
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
                                            <IconButton
                                                variant="text"
                                                className="bg-white rounded-full"
                                                onClick={() => navigate(`/food-details/${bestSellerItem.id}`)}
                                            >
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
                                <div className="w-2/5">
                                    <Typography variant="h6" className="pointer-events-none">{bestSellerItem.foodName}</Typography>
                                    <Typography color="blue" className="pb-2 relative w-fit pointer-events-none">
                                        {bestSellerItem.price}.000
                                        <span className="absolute font-normal top-0 -right-2 text-xs">
                                            đ
                                        </span>
                                    </Typography>
                                </div>
                            </div>
                        ))
                        : <Spinner></Spinner>
                    }
                </div>
                {/* Most review */}
                <div className="lg:w-1/4">
                    <Typography variant="h5">ĐÁNH GIÁ HÀNG ĐẦU</Typography>
                    {bestRating ?
                        bestRating.map((bestRatingItem) => (
                            <div key={bestRatingItem.id} className="flex p-1 my-1 justify-between items-center shadow-lg">
                                <div className="group relative flex">
                                    <img
                                        src={bestRatingItem.imageURL}
                                        alt={bestRatingItem.foodName}
                                        className="h-28 w-44 object-cover group-hover:opacity-40"
                                    />
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
                                            <IconButton
                                                variant="text"
                                                className="bg-white rounded-full"
                                                onClick={() => navigate(`/food-details/${bestRatingItem.id}`)}
                                            >
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
                                <div className="w-2/5">
                                    <Typography variant="h6" className="pointer-events-none">{bestRatingItem.foodName}</Typography>
                                    <Typography color="blue" className="pb-2 relative w-fit pointer-events-none">
                                        {bestRatingItem.price}.000
                                        <span className="absolute font-normal top-0 -right-2 text-xs">
                                            đ
                                        </span>
                                    </Typography>
                                </div>
                            </div>
                        ))
                        : <Spinner></Spinner>
                    }
                </div>
                {/* New Food */}
                <div className="lg:w-1/4">
                    <Typography variant="h5">SẢN PHẨM MỚI</Typography>
                    {newFood ?
                        newFood.map((newFoodItem) => (
                            <div key={newFoodItem.id} className="flex p-1 my-1 justify-between items-center shadow-lg">
                                <div className="group relative flex">
                                    <img
                                        src={newFoodItem.imageURL}
                                        alt={newFoodItem.foodName}
                                        className="h-28 w-44 object-cover group-hover:opacity-40"
                                    />
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
                                            <IconButton
                                                variant="text"
                                                className="bg-white rounded-full"
                                                onClick={() => navigate(`/food-details/${newFoodItem.id}`)}
                                            >
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
                                <div className="w-2/5">
                                    <Typography variant="h6" className="pointer-events-none">{newFoodItem.foodName}</Typography>
                                    <Typography color="blue" className="pb-2 relative w-fit pointer-events-none">
                                        {newFoodItem.price}.000
                                        <span className="absolute font-normal top-0 -right-2 text-xs">
                                            đ
                                        </span>
                                    </Typography>
                                </div>
                            </div>
                        ))
                        : <Spinner></Spinner>
                    }
                </div>
            </div>
            {/* End Flash Sale, Popular,... */}
        </div>
    );
};

export default HomePage;