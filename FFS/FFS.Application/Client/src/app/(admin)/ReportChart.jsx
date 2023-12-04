import React, { useEffect, useRef, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import Chart from "chart.js/auto";
const ReportChart = () => {
	const [chart, setChart] = useState(null);
    const chartRef = useRef(null);
	useEffect(() => {
		const fetchData = async () => {
			try {
				const response = await axios.get("/api/Report/GetTotalReportsByType");
				const data = response;

				const chartData = {
					labels: data.map((item) => `${item.year}-${item.month}`),
					datasets: [
						{
							label: "Báo cáo cửa hàng",
							data: data
								.filter((item) => item.reportType === 1)
								.map((item) => item.totalReports),
							backgroundColor: "rgba(75, 192, 192, 0.2)",
							borderColor: "rgba(75, 192, 192, 1)",
							borderWidth: 1,
						},
						{
							label: "Báo cáo người giao hàng",
							data: data
								.filter((item) => item.reportType === 2)
								.map((item) => item.totalReports),
							backgroundColor: "rgba(255, 99, 132, 0.2)",
							borderColor: "rgba(255, 99, 132, 1)",
							borderWidth: 1,
						},
						{
							label: "Báo cáo người dùng",
							data: data
								.filter((item) => item.reportType === 3)
								.map((item) => item.totalReports),
							backgroundColor: "rgba(54, 162, 235, 0.2)",
							borderColor: "rgba(54, 162, 235, 1)",
							borderWidth: 1,
						},
					],
				};

				// Ensure the chartRef is not null before creating the chart
				if (chartRef.current) {
					const ctx = chartRef.current.getContext("2d");

					new Chart(ctx, {
						type: "bar",
						data: chartData,
						options: {
							scales: {
								y: {
									beginAtZero: true,
								},
							},
						},
					});
				}
			} catch (error) {
				console.error("Error fetching data from API:", error);
			}
		};

		fetchData();
	}, []);

	return (
		<canvas id="reportChart" ref={chartRef} width="400" height="200"></canvas>
	);
};

export default ReportChart;
