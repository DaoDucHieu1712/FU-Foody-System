import { useState, useEffect } from 'react';
import axios from '../../shared/api/axiosConfig';
import propTypes from 'prop-types';
import { Chart as ChartJS, BarElement } from 'chart.js';
import { Bar } from 'react-chartjs-2';

ChartJS.register(BarElement);

const BarChart = ({ year }) => {
  const [chart, setChart] = useState({});

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(`/api/Admin/ReportsStatistic/${year}`);
        const data = await response;
        setChart(data);
      } catch (error) {
        console.error('Error fetching data from API:', error);
      }
    };
    fetchData();
  }, [year]);

  console.log('chart', chart);

  const reportsStatistic = chart?.reportsStatistic ?? [];

  const data = {
    labels: reportsStatistic.map((x) => x.month),
    datasets: [
      {
        label: `Số lượng`,
        data: reportsStatistic.map((x) => x.numberOfReport),
        backgroundColor: ['rgb(255, 159, 64, 0.5)'],
        borderColor: ['rgb(255, 159, 64, 0.5)'],
        borderWidth: 1,
      },
    ],
  };

  const options = {
    maintainAspectRatio: false,
    scales: {
      y: {
        suggestedMax: Math.max(...reportsStatistic.map((entry) => entry.numberOfReport)) + 2.5,
        title: {
          display: true,
          text: '(báo cáo)',
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
  year: propTypes.any.isRequired,
};

export default BarChart;
