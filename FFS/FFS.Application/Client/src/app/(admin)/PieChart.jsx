import { useEffect, useRef } from "react";
import Chart from "chart.js/auto";
import PropTypes from "prop-types";

const PieChart = ({ data }) => {
  const chartRef = useRef(null);

  useEffect(() => {
    // Ensure the chartRef is not null before attempting to create the chart
    if (chartRef.current) {
      const ctx = chartRef.current.getContext("2d");

      new Chart(ctx, {
        type: "pie",
        data: {
          labels: data.labels,
          datasets: [
            {
              label: "Số lượng",
              data: data.values,
              backgroundColor: [
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                // Add more colors as needed
              ],
              borderColor: [
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                // Add more colors as needed
              ],
              borderWidth: 1,
            },
          ],
        },
      });
    }
  }, [data]);

  return <canvas  id="pieChart" ref={chartRef} width="200" height="200"></canvas>;
};
PieChart.propTypes = {
  data: PropTypes.shape({
    labels: PropTypes.arrayOf(PropTypes.string.isRequired),
    values: PropTypes.arrayOf(PropTypes.number.isRequired),
  }).isRequired,
};
export default PieChart;
