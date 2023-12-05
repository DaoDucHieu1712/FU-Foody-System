import { Button, Carousel, Typography } from "@material-tailwind/react";
import { useNavigate } from "react-router-dom";
import BestRatingHome from "./components/HomePage/BestRatingHome";
import BestSellerHome from "./components/HomePage/BestSellerHome";
import FlashSaleHome from "./components/HomePage/FlashSaleHome";
import NewestFoodHome from "./components/HomePage/NewestFoodHome";
import RecommendList from "./components/HomePage/RecommendList";
import StoreSpecial from "./components/HomePage/StoreSpecial";

const HomePage = () => {
	const navigate = useNavigate();

	return (
		<div>
			{/* Carousel */}
			<div className="py-4 grid grid-flow-row-dense grid-rows-4 grid-cols-1 lg:grid-rows-2 lg:grid-cols-3 gap-2 ">
				<div className="row-span-2 lg:col-span-2">
					<Carousel
						autoplay
						loop
						autoplayDelay={7000}
						transition={{ duration: 2 }}
						className=" bg-gray-50"
					>
						<div className="flex justify-around items-center overflow-hidden">
							<div>
								<Typography variant="h3" className="pointer-events-none">
									Siêu đại tiệc
								</Typography>
								<Typography className="w-48 md:w-auto pb-5 pointer-events-none">
									Giảm tối đa 20% cho các bạn đến quán...
								</Typography>
								<Button
									className="flex items-center gap-2 text-white font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900"
									onClick={() => navigate(`/food-list`)}
								>
									Xem ngay
									<svg
										xmlns="http://www.w3.org/2000/svg"
										height="1.5em"
										viewBox="0 0 512 512"
										fill="white"
									>
										<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
									</svg>
								</Button>
							</div>
							<div className="relative pointer-events-none">
								<img
									src="https://png.pngtree.com/thumb_back/fh260/background/20230724/pngtree-the-table-is-filled-with-several-dishes-image_10193167.jpg"
									alt="image 1"
									className="h-56 w-64 pt-5 object-fill"
								/>
								<div className="absolute top-0 md:right-0 h-12 w-12 bg-primary rounded-full text-white font-bold transform -translate-x-1/2 md:translate-x-1/2">
									<div className="flex h-full justify-center items-center">
										115K
									</div>
								</div>
							</div>
						</div>
						<div className="flex justify-around items-center overflow-hidden">
							<div>
								<Typography variant="h3" className="pointer-events-none">
									Siêu đại tiệc
								</Typography>
								<Typography className="w-48 md:w-auto pb-5 pointer-events-none">
									Giảm tối đa 20% cho các bạn đến quán...
								</Typography>
								<Button
									className="flex items-center gap-2 text-white font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900"
									onClick={() => navigate(`/food-list`)}
								>
									Xem ngay
									<svg
										xmlns="http://www.w3.org/2000/svg"
										height="1.5em"
										viewBox="0 0 512 512"
										fill="white"
									>
										<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
									</svg>
								</Button>
							</div>
							<div className="relative pointer-events-none">
								<img
									src="https://png.pngtree.com/thumb_back/fh260/background/20230902/pngtree-kimchi-tofu-boiled-on-a-stove-top-image_13181822.jpg"
									alt="image 1"
									className="h-56 w-64 pt-5 object-fill"
								/>
								<div className="absolute top-0 md:right-0 h-12 w-12 bg-primary rounded-full text-white font-bold transform -translate-x-1/2 md:translate-x-1/2">
									<div className="flex h-full justify-center items-center">
										115K
									</div>
								</div>
							</div>
						</div>
						<div className="flex justify-around items-center overflow-hidden">
							<div>
								<Typography variant="h3" className="pointer-events-none">
									Siêu đại tiệc
								</Typography>
								<Typography className="w-48 md:w-auto pb-5 pointer-events-none">
									Giảm tối đa 20% cho các bạn đến quán...
								</Typography>
								<Button
									className="flex items-center gap-2 text-white font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900"
									onClick={() => navigate(`/food-list`)}
								>
									Xem ngay
									<svg
										xmlns="http://www.w3.org/2000/svg"
										height="1.5em"
										viewBox="0 0 512 512"
										fill="white"
									>
										<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
									</svg>
								</Button>
							</div>
							<div className="relative pointer-events-none">
								<img
									src="https://png.pngtree.com/thumb_back/fh260/background/20230727/pngtree-spicy-korean-oxtail-tofu-soup-image_10192729.jpg"
									alt="image 1"
									className="h-56 w-64 pt-5 object-fill"
								/>
								<div className="absolute top-0 md:right-0 h-12 w-12 bg-primary rounded-full text-white font-bold transform -translate-x-1/2 md:translate-x-1/2">
									<div className="flex h-full justify-center items-center">
										115K
									</div>
								</div>
							</div>
						</div>
					</Carousel>
				</div>
				<div className="bg-black">
					<div className="flex justify-around items-center">
						<div>
							<Typography
								variant="paragraph"
								color="yellow"
								className="text-xs pointer-events-none"
							>
								Giảm giá
							</Typography>
							<Typography
								color="white"
								className="pb-2 font-semibold pointer-events-none"
							>
								Cơm trộn
							</Typography>
							<Button
								size="sm"
								className="flex items-center gap-1 text-white text-center font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900"
								onClick={() => navigate(`/food-list`)}
							>
								Mua ngay
								<svg
									xmlns="http://www.w3.org/2000/svg"
									height="1.5em"
									viewBox="0 0 512 512"
									fill="white"
								>
									<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
								</svg>
							</Button>
						</div>
						<div className="relative pointer-events-none">
							<img
								src="https://daotaobeptruong.vn/wp-content/uploads/2017/11/cach-lam-com-tron-han-quoc.jpg"
								alt="image 1"
								className="h-32 w-48 py-3 object-fill"
							/>
							<div className="absolute top-0 right-2 h-6 w-fit px-2 bg-yellow-300 rounded-sm text-black text-sm font-semibold transform translate-y-1">
								<div className="flex h-full justify-center items-center">
									10% OFF
								</div>
							</div>
						</div>
					</div>
				</div>
				<div className="bg-gray-50">
					<div className="flex justify-around">
						<div className="pointer-events-none">
							<img
								src="https://images.foody.vn/res/g92/914115/prof/s640x400/foody-upload-api-foody-mobile-seatalk_img_15845918-200319114117.jpg"
								alt="image 1"
								className="h-32 w-48 py-1 object-fill"
							/>
						</div>
						<div>
							<Typography variant="h6" className="w-36 pointer-events-none">
								Trà TMORE{" "}
							</Typography>
							<Typography
								color="blue"
								className="pb-2 relative w-fit pointer-events-none"
							>
								25.000
								<span className="absolute font-normal top-0 -right-2 text-xs">
									đ
								</span>
							</Typography>
							<Button
								size="sm"
								className="flex items-center gap-1 text-white text-center font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900 "
								onClick={() => navigate(`/food-list`)}
							>
								Mua ngay
								<svg
									xmlns="http://www.w3.org/2000/svg"
									height="1.5em"
									viewBox="0 0 512 512"
									fill="white"
								>
									<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
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
						<Typography variant="h6" className="font-semibold uppercase">
							Có thể bạn sẽ thích !!
						</Typography>
					</div>
					<p
						onClick={() => navigate(`/food-list`)}
						className="flex gap-2 items-center font-medium text-blue-600 dark:text-blue-500 cursor-pointer hover:underline"
					>
						Xem tất cả
						<svg
							xmlns="http://www.w3.org/2000/svg"
							height="1em"
							viewBox="0 0 512 512"
							fill="rgb(30 136 229 / var(--tw-text-opacity))"
						>
							<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
						</svg>
					</p>
				</div>
				<RecommendList />
			</div>
			{/* End Deal Price */}

			{/* Banner */}
			<div className="flex justify-around h-72 w-full bg-gray-50 items-center">
				<div>
					<p className="px-2 py-1 w-fit text-xs text-white bg-blue-500 pointer-events-none">
						Up to 20%
					</p>
					<Typography variant="h3" className="pointer-events-none">
						Khai trương quán
					</Typography>
					<Typography className="pb-5 pointer-events-none">
						Giảm tối đa 20% cho các bạn đến quán...
					</Typography>
					<Button
						className="flex items-center gap-2 text-white text-center font-bold rounded-sm bg-primary cursor-pointer hover:bg-orange-900"
						onClick={() => navigate(`/food-list`)}
					>
						Xem ngay
						<svg
							xmlns="http://www.w3.org/2000/svg"
							height="1.5em"
							viewBox="0 0 512 512"
							fill="white"
						>
							<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
						</svg>
					</Button>
				</div>
				<div className="relative pointer-events-none">
					<img
						src="https://vi.alongwalker.co/img/post_images/a83d99a88f97c4e6087946099a9ff415.jpg"
						alt="image 1"
						className="h-72 w-96 object-cover"
					/>
					<div className="absolute top-0 h-20 w-20 bg-primary rounded-full border-solid border-4 text-black text-sm font-semibold transform translate-y-5 -translate-x-5">
						<div className="flex h-full justify-center items-center">329K</div>
					</div>
				</div>
			</div>
			{/* End Banner */}

			<StoreSpecial></StoreSpecial>

			{/* Flash Sale, Popular,... */}
			<div className="grid py-4 gap-4 grid-flow-row-dense grid-cols-2 justify-stretch lg:flex lg:gap-2">
				<div className="lg:w-1/4">
					{/* Flash sale */}
					<FlashSaleHome></FlashSaleHome>
				</div>
				{/* Best seller */}
				<div className="lg:w-1/4">
					<BestSellerHome></BestSellerHome>
				</div>
				{/* Most review */}
				<div className="lg:w-1/4">
					<BestRatingHome></BestRatingHome>
				</div>
				{/* New Food */}
				<div className="lg:w-1/4">
					<NewestFoodHome></NewestFoodHome>
				</div>
			</div>
			{/* End Flash Sale, Popular,... */}
		</div>
	);
};

export default HomePage;
