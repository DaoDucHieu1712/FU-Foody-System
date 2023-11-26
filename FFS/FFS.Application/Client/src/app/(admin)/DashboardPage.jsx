import { useState, useEffect } from "react";
import PieChart from "./Piechart"
import axios from "../../shared/api/axiosConfig";
const DashboardPage = () => {
  const [apiData, setApiData] = useState(null);
  const [apiPostData, setApiPostData] = useState(null);


  useEffect(() => {
    // Fetch data from the API
    const fetchData = async () => {
      try {
        const response = await axios.get("/api/Admin/AccountsStatistic");
        const data = await response;
        console.log(response);
        setApiData(data);
      } catch (error) {
        console.error("Error fetching data from API:", error);
      }
    };

    const fetchDataPost = async () => {
      try {
        const response = await axios.get("/api/Admin/PostsStatistic");
        const data = await response;
        console.log(response);
        setApiPostData(data);
      } catch (error) {
        console.error("Error fetching data from API:", error);
      }
    };

    fetchData();
    fetchDataPost();
  }, []);

  // Render loading state if data is still being fetched
  if (!apiData) {
    return <div>Loading...</div>;
  }

  // Process API data to match the PieChart component's expected format
  const chartData = {
    labels: apiData.accountsStatistic.map((statistic) => {
      switch (statistic.userType) {
        case 1:
          return "Đang chờ duyệt";
        case 2:
          return "Đang hoạt động";
          case 3:
            return "Từ chối yêu cầu";
        default:
          return "Unknown";
      }
    }),
    values: apiData.accountsStatistic.map((statistic) => statistic.numberOfAccount),
  };

  const chartDataPost = {
    labels: apiPostData.postsStatistic.map((statistic) => {
      switch (statistic.postStatus) {
        case 1:
          return "Đang chờ duyệt";
        case 2:
          return "Đang hoạt động";
          case 3:
            return "Từ chối yêu cầu";
        default:
          return "Unknown";
      }
    }),
    values: apiPostData.postsStatistic.map((statistic) => statistic.numberOfPosts),
  };

  return (
    <>
      <h3 className="text-center bg-gray-500 text-white p-2 m-2">Thống kê tài khoản</h3>
      <PieChart data={chartData} width={200} height={200}/>
      <h5 className="text-gray-500">
  Tổng số: <span className="font-bold text-primary">{apiData.totalAccount}</span> tài khoản
</h5>

<h3 className="text-center bg-gray-500 text-white p-2 m-2">Thống kê bài viết</h3>
      <PieChart data={chartDataPost} width={200} height={200}/>
      <h5 className="text-gray-500">
  Tổng số: <span className="font-bold text-primary">{apiPostData.totalPost}</span> bài viết
</h5>
    </>
  );
};

export default DashboardPage;
