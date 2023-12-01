import { useState, useEffect } from 'react'
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import axios from "../../shared/api/axiosConfig";


import { Pie } from 'react-chartjs-2';

ChartJS.register(ArcElement, Tooltip, Legend);



const PieChart = () => {
    const [chart, setChart] = useState({})
    useEffect(() => {
      const fetchData = async () => {
              try {
                  const response = await axios.get("/api/Admin/AccountsStatistic");
                  const data = await response;
                  console.log(response);
                  setChart(data);
              } catch (error) {
                  console.error("Error fetching data from API:", error);
              }
          };
      fetchData()
    }, [])

  console.log("chart", chart);
  var data = {
    labels: chart?.accountsStatistic?.map((statistic) => {
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
    datasets: [{
      label: `Số lượng`,
      data: chart?.accountsStatistic?.map(x => x.numberOfAccount),
      backgroundColor: [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
      ],
      borderColor: [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
      ],
      borderWidth: 1
    }]
  };

  var options = {
    maintainAspectRatio: false,
    scales: {
    },
    legend: {
      labels: {
        fontSize: 25,
      },
    },
  }

  return (
    <div>
      <Pie
        data={data}
        height={400}
        options={options}

      />
    </div>
  )
}

export default PieChart