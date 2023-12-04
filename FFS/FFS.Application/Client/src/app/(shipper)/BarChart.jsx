import { useState, useEffect } from 'react';
import axios from '../../shared/api/axiosConfig';
import propTypes from 'prop-types';
import { Chart as ChartJS, BarElement, LinearScale, CategoryScale } from 'chart.js';
import { Bar } from 'react-chartjs-2';

ChartJS.register(BarElement, LinearScale, CategoryScale);


const BarChart = ({ shipperId, year }) => {
    const [chart, setChart] = useState([]);
  
    useEffect(() => {
      const fetchData = async () => {
        try {
          const response = await axios.get(
            `/api/Order/GetRevenueShipperPerMonth/${shipperId}/${year}`
          );
          const data = response;
          setChart(data);
        } catch (error) {
          console.error("Error fetching data from API:", error);
        }
      };
      fetchData();
    }, [shipperId, year]);
  
    if (!chart || chart.length === 0) {
      return <div>Loading...</div>; // or some other loading state
    }
  
    var data = {
      labels: chart.map((entry) => entry.month),
      datasets: [
        {
          label: `Doanh thu`,
          data: chart.map((entry) => entry.revenue),
          backgroundColor: 'rgba(255, 159, 64, 0.5)',
          borderColor: 'rgba(255, 159, 64, 1)',
          borderWidth: 1,
        },
      ],
    };
  
    var options = {
      maintainAspectRatio: false,
      scales: {
        x: {
          type: 'category', // Specify the scale type explicitly
        },
        y: {
          type: 'linear', // Specify the scale type explicitly
          suggestedMax: Math.max(...chart.map((entry) => entry.revenue)) + 50000,
          title: {
            display: true,
            text: '(VND)',
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
        <Bar data={data} height={400} options={options} />
      </div>
    );
  };
  
  BarChart.propTypes = {
    shipperId: propTypes.any.isRequired,
    year: propTypes.any.isRequired,
  };
  
  export default BarChart;