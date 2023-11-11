import { useEffect, useState } from "react";
import Pagination from "../../shared/components/Pagination";
import { Input } from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";
const ReportPage = () => {
  const [dataSearch, setDateSearch] = useState({
    pageNumber: 1,
    pageSize: 15,
    description: "",
    usernameReport: "",
  });

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
              <>
                <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                  <th
                    scope="row"
                    className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                  >
                    {report.userReport}
                  </th>
                  <td className="px-6 py-4">{report.userTarget}</td>
                  <td className="px-6 py-4">{report.Description}</td>
                  <td className="px-6 py-4">{report.reportType}</td>
                  <td className="px-6 py-4">{report.CreatedAt}</td>
                </tr>
              </>
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
