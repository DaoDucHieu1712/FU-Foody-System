import { useState, useEffect } from 'react';
import axios from "../../shared/api/axiosConfig";
import propTypes from "prop-types";
import { Bar, Line } from 'react-chartjs-2';
import Chart from 'chart.js/auto';

const LineChartDetailFood = ({ storeId }) => {
  const [chart, setChart] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(`/api/Store/GetFoodDetailStatistics/${storeId}`);
        const data = response;
        if (Array.isArray(data)) {
          setChart(data);
        } else {
          console.error("Invalid data format received from the API");
        }
      } catch (error) {
        console.error("Error fetching data from API:", error);
      }
    };
    fetchData();
  }, [storeId]);

  console.log(chart);

  if (!chart || chart.length === 0) {
    return <div>Loading...</div>; // or some other loading state
  }

  const data = {
    labels: chart.map((entry) => `${entry.foodName} (${entry.rateAverage} sao)`),
    datasets: [
      {
        barThickness: 80,
        label: 'Số lượng đã bán',
        data: chart.map((entry) => entry.quantityOfSell),
        type: 'bar',
        backgroundColor: 'rgba(75, 192, 192, 0.2)',
        borderColor: 'rgba(75, 192, 192, 1)',
        borderWidth: 1,
      },
      {
        label: 'Số lượt đánh giá',
        data: chart.map((entry) => entry.ratingCount),
        type: 'line',
        fill: false,
        backgroundColor: 'rgba(255, 159, 64, 0.2)',
        borderColor: 'rgba(255, 159, 64, 1)',
        borderWidth: 1,
      },
    ],
  };

  const options = {
    maintainAspectRatio: false,
    scales: {
      x: {
        title: {
          display: true,
          text: 'Sản phẩm (đánh giá)',
          font: {
            size: 12,
            style: 'normal',
            lineHeight: 1.2,
          },
          padding: { top: 20, left: 0, right: 0, bottom: 0 },
        },
      },
      y: {
        suggestedMax: Math.max(
          ...chart.map((entry) => entry.quantityOfSell),
          ...chart.map((entry) => entry.ratingCount)
        ) + 5,
        title: {
          display: true,
          text: 'Số lượng',
          font: {
            size: 12,
            style: 'normal',
            lineHeight: 1.2,
          },
          padding: { top: 0, left: 30, right: 0, bottom: 0 },
          position: 'left',
        },
      },
    },
    legend: {
      labels: {
        fontSize: 25,
      },
    },
  };

  return (
    <div>
      <Line data={data} height={400} options={options} />
    </div>
  );
};

LineChartDetailFood.propTypes = {
  storeId: propTypes.any.isRequired,
};

export default LineChartDetailFood;
