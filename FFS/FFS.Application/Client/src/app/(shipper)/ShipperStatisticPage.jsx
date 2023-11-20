import BarChart from "./BarChart";
const ShipperStatisticPage = () => {
  const chartData = {
    labels: ["Label 1", "Label 2", "Label 3"],
    values: [10, 20, 30],
  };

  return (
    <div className="h-[80vh]">
      <h2>Thu nhập năm 2023</h2>
      <BarChart data={chartData} />
    </div>
  );
};

export default ShipperStatisticPage;
