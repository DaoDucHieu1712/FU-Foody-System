import { useState, useEffect } from "react";
import PieChartOrder from "./PiechartOrder";
import axios from "../../shared/api/axiosConfig";
import { Option, Select } from "@material-tailwind/react";
import CookieService from "../../shared/helper/cookieConfig";
import BarChartRevenue from "./BarChartRevenue";
import LineChartDetailFood from "./LineChartDetailFood";

const uId = CookieService.getToken("fu_foody_id");
const StoreDashboardPage = () => {
	const [apiData, setApiData] = useState(null);
	// const [apiRevenueData, setApiRevenueData] = useState(null);
	const currentYear = new Date().getFullYear();
	const [storeId, setStoreId] = useState(0);
	const [selectedYear, setSelectedYear] = useState(currentYear.toString());

	const GetStoreByUid = async () => {
		try {
			await axios
				.get(`/api/Store/GetStoreByUid?uId=${uId}`)
				.then((response) => {
					setStoreId(response.id);
					console.log(response.id);
				})
				.catch((error) => {
					console.log(error);
				});
		} catch (error) {
			console.log("Get Store By Uid error: " + error);
		}
	};

	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await axios.get(
					`/api/Store/OrderStatistic/${storeId}`
				);
				const data = response;
				setApiData(data);
			} catch (error) {
				console.error("Error fetching data from API:", error);
			}
		};

		GetStoreByUid();
		fetchData();
	}, [storeId, selectedYear]);

	if (!apiData) {
		return <div>Loading...</div>;
	}

	return (
		<>
			<div className="flex flex-wrap justify-center items-center">
				<div className="m-4 p-4 bg-gray-200 rounded-lg">
					<h3 className="text-center text-lg font-bold mb-2">
						Thống kê đơn hàng
					</h3>
					<PieChartOrder storeid={storeId}></PieChartOrder>
					<h5 className="text-gray-500 text-center">
						Tổng số:{" "}
						<span className="font-bold text-primary">{apiData.totalOrder}</span>{" "}
						đơn hàng
					</h5>
				</div>
			</div>

			<div className="m-4 p-4 bg-gray-100 rounded-lg">
				<h3 className="text-center text-lg font-bold mb-2">Thống kê doanh thu cửa hàng</h3>
				<div className="flex items-center justify-center mx-auto w-4/12 whitespace-nowrap m-10">
					<span className="mr-2">Chọn năm </span>
					<Select
						value={selectedYear.toString()}
						onChange={(e) => setSelectedYear(e)}
						className="mt-2"
					>
						<Option value="2020">2020</Option>
						<Option value="2021">2021</Option>
						<Option value="2022">2022</Option>
						<Option value="2023">2023</Option>
					</Select>
				</div>

				<BarChartRevenue storeId={storeId} year={selectedYear}></BarChartRevenue>
			</div>

            <div className="m-4 p-4 bg-gray-100 rounded-lg">
				<h3 className="text-center text-lg font-bold mb-2">Thống kê món ăn</h3>
					<LineChartDetailFood storeId={storeId}></LineChartDetailFood> 
			</div>
		</>
	);
};

export default StoreDashboardPage;
