import { useEffect, useRef } from "react";
import Chart from "chart.js/auto";
import PropTypes from "prop-types";

const BarChart = ({ data }) => {
  const chartRef = useRef(null);

  useEffect(() => {
    // Ensure the chartRef is not null before attempting to create the chart
    if (chartRef.current) {
      const ctx = chartRef.current.getContext("2d");

      new Chart(ctx, {
        type: "bar",
        data: {
          labels: data.labels,
          datasets: [
            {
              label: "Chart Title",
              data: data.values,
              backgroundColor: "rgba(75, 192, 192, 0.2)",
              borderColor: "rgba(75, 192, 192, 1)",
              borderWidth: 1,
            },
          ],
        },
      });
    }
  }, [data]);

  return <canvas ref={chartRef} width="400" height="400"></canvas>;
};
BarChart.propTypes = {
  data: PropTypes.shape({
    labels: PropTypes.arrayOf(PropTypes.string.isRequired),
    values: PropTypes.arrayOf(PropTypes.number.isRequired),
  }).isRequired,
};
export default BarChart;
