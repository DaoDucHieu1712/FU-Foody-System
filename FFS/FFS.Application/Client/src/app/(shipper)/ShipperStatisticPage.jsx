// import BarChart from "./BarChart";
import { useEffect, useState } from "react";
import { Option, Select } from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { useSelector } from "react-redux";
import propTypes from "prop-types";
import BarChart from "./BarChart";
const ShipperStatisticPage = () => {
  const userProfile = useSelector((state) => state.auth.userProfile);
  const [statisticData, setStatisticData] = useState({
    today: 0,
    thisWeek: 0,
    thisMonth: 0,
    thisYear: 0,
  });
  const currentYear = new Date().getFullYear();
  const [selectedYear, setSelectedYear] = useState(currentYear.toString());


  const shipperId = userProfile?.id;
  useEffect(() => {
    const fetchStatisticData = async () => {
      try {
        const response = await axios.get(`api/Order/GetNumberOfOrder/${shipperId}`);
        setStatisticData(response);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchStatisticData();
  }, [shipperId]);

  return (
    <div className="min-h-screen bg-gray-100 p-8">
    <div className="max-w-4xl mx-auto">
      <h2 className="text-xl font-semibold mb-6">Đơn hàng đã hoàn thành</h2>
      <div className="grid grid-cols-1 md:grid-cols-4 gap-3">
        <StatisticCard label="Hôm nay" value={statisticData.today} />
        <StatisticCard label="Tuần này" value={statisticData.thisWeek} />
        <StatisticCard label="Tháng này" value={statisticData.thisMonth} />
        <StatisticCard label="Năm nay" value={statisticData.thisYear} />
      </div>
      <h2 className="text-xl font-semibold mb-6 mt-10">Thống kê doanh thu</h2>
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
      <BarChart shipperId={shipperId} year={selectedYear}></BarChart>
    </div>
  </div>
  );
};

const StatisticCard = ({ label, value }) => {
  return (
    <div className="flex-1 p-4 border border-gray-300 rounded-lg m-2 shadow-md text-center">
      <h3 className="text-xl font-semibold mb-2 text-primary">{label}</h3>
      <p className="text-sm font-medium">Tổng số: {value} đơn hàng</p>
    </div>
  );
};
StatisticCard.propTypes = {
  label: propTypes.any.isRequired,
  value: propTypes.any.isRequired,
};
export default ShipperStatisticPage;
