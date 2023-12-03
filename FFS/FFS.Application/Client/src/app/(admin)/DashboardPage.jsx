import { useState, useEffect } from "react";
import PieChartAccount from "./PieChartAccount";
import PieChartPost from "./PieChartPost";
import VerticalBarChart from "./VerticalBarChart";
import axios from "../../shared/api/axiosConfig";
import { Option, Select } from "@material-tailwind/react";

const DashboardPage = () => {
	const [apiData, setApiData] = useState(null);
	const [apiPostData, setApiPostData] = useState(null);
	const [apiReportData, setApiReportData] = useState(null);
	const currentYear = new Date().getFullYear();
	const [selectedYear, setSelectedYear] =useState(currentYear.toString());
	const fetchDataReport = async (year) => {
		try {
			const response = await axios.get(`/api/Admin/ReportsStatistic/${year}`);
			const data = response; // Use response.data to get the actual data
			setApiReportData(data);
		} catch (error) {
			console.error("Error fetching data from API:", error);
		}
	};

	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await axios.get("/api/Admin/AccountsStatistic");
				const data = response;
				setApiData(data);
			} catch (error) {
				console.error("Error fetching data from API:", error);
			}
		};

		const fetchDataPost = async () => {
			try {
				const response = await axios.get("/api/Admin/PostsStatistic");
				const data = response;
				setApiPostData(data);
			} catch (error) {
				console.error("Error fetching data from API:", error);
			}
		};

		fetchData();
		fetchDataPost();
		fetchDataReport(selectedYear);
	}, [selectedYear]);

	if (!apiData) {
		return <div>Loading...</div>;
	}

	return (
		<>
		
			<div className="flex flex-wrap justify-center items-center">
				<div className="m-4 p-4 bg-gray-200 rounded-lg">
					<h3 className="text-center text-lg font-bold mb-2">
						Thống kê tài khoản
					</h3>
					<PieChartAccount></PieChartAccount>
					<h5 className="text-gray-500 text-center">
						Tổng số:{" "}
						<span className="font-bold text-primary">
							{apiData.totalAccount}
						</span>{" "}
						tài khoản
					</h5>
				</div>

				<div className="m-4 p-4 bg-gray-200 rounded-lg">
					<h3 className="text-center text-lg font-bold mb-2">
						Thống kê bài viết
					</h3>
					<PieChartPost></PieChartPost>
					<h5 className="text-gray-500 text-center">
						Tổng số:{" "}
						<span className="font-bold text-primary">
							{apiPostData ? apiPostData.totalPost : 0}
						</span>{" "}
						bài viết
					</h5>
				</div>
			</div>

			<div className="m-4 p-4 bg-gray-100 rounded-lg">
				<h3 className="text-center text-lg font-bold mb-2">Thống kê báo cáo</h3>
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

				<VerticalBarChart year={selectedYear}></VerticalBarChart>

				<h5 className="text-gray-500 text-center">
					Tổng số:{" "}
					<span className="font-bold text-primary">
						{apiReportData ? apiReportData.totalReportYear : 0}
					</span>{" "}
					báo cáo
				</h5>
			</div>
		</>
	);
};

export default DashboardPage;
