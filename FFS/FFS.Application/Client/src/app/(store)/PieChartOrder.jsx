import { useState, useEffect } from 'react'
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import axios from "../../shared/api/axiosConfig";
import propTypes from "prop-types";

import { Pie } from 'react-chartjs-2';

ChartJS.register(ArcElement, Tooltip, Legend);



const PieChartOrder = ({storeid}) => {
    const [chart, setChart] = useState({})
    useEffect(() => {
      const fetchData = async () => {
              try {
                  const response = await axios.get(`/api/Store/OrderStatistic/${storeid}`);
                  const data = await response;
                  setChart(data);
              } catch (error) {
                  console.error("Error fetching data from API:", error);
              }
          };
      fetchData()
    }, [storeid])

  var data = {
    labels: chart?.ordersStatistic?.map((statistic) => {
        switch (statistic.orderStatus) {
            case 1:
                return "Đang chờ";
            case 2:
                return "Đang giao";
            case 3:
                return "Đã giao";
            case 4:
                return "Đã hủy";
            default:
                return "Unknown";
        }
    }),
    datasets: [{
      label: `Số lượng`,
      data: chart?.ordersStatistic?.map(x => x.numberOfOrder),
      backgroundColor: [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(255, 159, 64, 0.2)',
      ],
      borderColor: [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(255, 159, 64, 1)', 
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

PieChartOrder.propTypes = {
	storeid: propTypes.any.isRequired,
};
export default PieChartOrder