import { useEffect, useState } from "react";
import Pagination from "../../shared/components/Pagination";
import { Input, Typography, Button } from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";
const ReportPage = () => {
  const [dataSearch, setDateSearch] = useState({
    pageNumber: 1,
    pageSize: 15,
    description: "",
    usernameReport: "",
  });

  const handleExportExcel = () => {
    const fileDownloadUrl = `https://localhost:7025/api/Admin/ExportReport`;

    window.location.href = fileDownloadUrl;
  };

  const [reportList, setReportList] = useState([]);
  const [totalPage, setTotalPage] = useState(0);

  const changePage = async (page) => {
    dataSearch.pageNumber = page;
    setDateSearch(dataSearch);
    await getReport();
  };
  const handleOnChange = async (e) => {
    dataSearch[e.target.name] = e.target.value;
    setDateSearch(dataSearch);

    await countGetReport();
    await getReport();
  };

  const getReport = async () => {
    try {
      await axios
        .post(`/api/Admin/GetReports`, dataSearch)
        .then((res) => {
          setReportList(res);
        })
        .catch((error) => {
          toast.error("Có lỗi xảy ra!");
          console.log(error);
        });
    } catch (error) {
      console.log("Food error: " + error);
    }
  };

  const countGetReport = async () => {
    try {
      await axios
        .post(`/api/Admin/CountGetReports`, dataSearch)
        .then((res) => {
          const totalPages = Math.ceil(res / dataSearch.pageSize);
          setTotalPage(totalPages);
        })
        .catch((error) => {
          toast.error("Có lỗi xảy ra!");
          console.log(error);
        });
    } catch (error) {
      console.log("Food error: " + error);
    }
  };

  useEffect(() => {
    getReport();
    countGetReport();
  }, []);

  return (
    <>
      <div className="relative overflow-x-auto">
        <div className="mb-4 flex flex-col gap-8 md:flex-row md:items-center">
          <Typography variant="h4" color="blue-gray">
            Danh sách báo cáo
          </Typography>
          <div className="flex gap-5">
            <Button
              className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
              onClick={handleExportExcel}
            >
              Xuất Excel
            </Button>
          </div>
        </div>

        <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
              <th scope="col" className="px-6 py-3">
                Người báo cáo
              </th>
              <th scope="col" className="px-6 py-3">
                Báo cáo
              </th>
              <th scope="col" className="px-6 py-3">
                Nội dung báo cáo
              </th>
              <th scope="col" className="px-6 py-3">
                Loại báo cáo
              </th>
              <th scope="col" className="px-6 py-3">
                Thời gian tạo
              </th>
            </tr>
            <tr>
              <th scope="col" className="px-6 py-3">
                <div className="w-full">
                  <Input
                    variant="static"
                    placeholder="Người báo cáo"
                    onChange={handleOnChange}
                    name="usernameReport"
                  />
                </div>
              </th>
              <th scope="col" className="px-6 py-3">
                Báo cáo
              </th>
              <th scope="col" className="px-6 py-3">
                <Input
                  variant="static"
                  placeholder="Nội dung báo cáo"
                  onChange={handleOnChange}
                  name="description"
                />
              </th>
              <th scope="col" className="px-6 py-3"></th>
              <th scope="col" className="px-6 py-3"></th>
            </tr>
          </thead>
          <tbody>
            {reportList.map((report) => (
              <tr
                key={report.Id}
                className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
              >
                <th
                  scope="row"
                  className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                >
                  {report.userReport}
                </th>
                <td className="px-6 py-4">{report.userTarget}</td>
                <td className="px-6 py-4">{report.Description}</td>
                <td scope="col" className="px-6 py-4">
                  {report.reportType === "Report Store" && (
                    <span className="font-bold text-primary">Báo cáo cửa hàng</span>
                  )}
                  {report.reportType === "Report Shipper" && (
                    <span className="font-bold text-green-500">Báo cáo nhân viên giao hàng</span>
                  )}
                  {report.reportType === "Report Customer" && (
                    <span className="font-bold text-red-900">Báo cáo người dùng</span>
                  )}
                </td>
                <td className="px-6 py-4">{report.CreatedAt}</td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="mt-4 flex justify-center">
          <Pagination
            className="mt-4"
            totalPage={totalPage}
            currentPage={dataSearch.pageNumber}
            handleClick={changePage}
          ></Pagination>
        </div>
      </div>
    </>
  );
};

export default ReportPage;
