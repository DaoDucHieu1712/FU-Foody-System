import axios from "../../../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { Typography } from "@material-tailwind/react";

const StoreSpecial = () => {
	const [storeList, setStoreList] = useState([]);

	const GetTopStore = async () => {
		try {
			axios
				.get("/api/Store/GetTop10Store")
				.then((response) => {
					setStoreList(response);
				})
				.catch((error) => {
					console.log(error);
					toast.error("Lấy cửa hàng thất bại!");
				});
		} catch (error) {
			console.error("Store: " + error);
		}
	};
	useEffect(() => {
		GetTopStore();
	}, []);

	return (
		<>
			<div className="py-4">
				<div className="bg-white p-4 rounded shadow">
					<div className="flex px-4 pb-2 justify-between">
						<div className="flex items-center">
							<Typography variant="h5">Cửa hàng</Typography>
						</div>
						<a
							href="/store-list"
							className="flex gap-2 items-center font-medium text-orange-900 dark:text-blue-500 cursor-pointer hover:underline"
						>
							Xem tất cả
							<svg
								xmlns="http://www.w3.org/2000/svg"
								height="1em"
								viewBox="0 0 512 512"
								fill="rgb(230 81 0 / var(--tw-text-opacity)"
							>
								<path d="M502.6 278.6c12.5-12.5 12.5-32.8 0-45.3l-128-128c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L402.7 224 32 224c-17.7 0-32 14.3-32 32s14.3 32 32 32l370.7 0-73.4 73.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l128-128z" />
							</svg>
						</a>
					</div>
					<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5 gap-4 my-3">
						{storeList && storeList.length !== 0 ? (
							storeList.map((store) => (
								<div
									key={store.id}
									className=" rounded border relative hover:scale-80 hover:shadow-lg"
								>
									<a href={`/store/detail/${store.id}`}>
										<img
											className="w-full h-40 object-cover"
											src={store.avatarURL}
											alt="Store"
										/>
									</a>

									<div
										className="text-sm absolute bg-green-600 text-white rounded-full h-4 w-4 border border-white"
										style={{ top: "-6px", left: "-6px" }}
									></div>

									<div className="px-2 py-2">
										<a
											href={`/store/detail/${store.id}`}
											className="font-semibold text-base inline-block hover:text-orange-600 transition duration-500 ease-in-out"
											style={{
												display: "-webkit-box",
												WebkitLineClamp: 1,
												WebkitBoxOrient: "vertical",
												overflow: "hidden",
											}}
										>
											{store.storeName}
										</a>
										<p
											className="text-gray-500 text-sm"
											style={{
												display: "-webkit-box",
												WebkitLineClamp: 1,
												WebkitBoxOrient: "vertical",
												overflow: "hidden",
											}}
										>
											{store.address}
										</p>
									</div>
								</div>
							))
						) : (
							<Typography variant="h5" className="mt-5 ml-5">
								Không có cửa hàng nào!
							</Typography>
						)}
					</div>
				</div>
			</div>
		</>
	);
};
export default StoreSpecial;
