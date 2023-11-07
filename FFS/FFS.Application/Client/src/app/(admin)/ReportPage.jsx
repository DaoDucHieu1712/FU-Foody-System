import { useState } from "react";
import Pagination from "../../shared/components/Pagination";
import { Input } from "@material-tailwind/react";

const ReportPage = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const test = (page) => {
    console.log(page);
    setCurrentPage(page);
  };

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
                Thời gian tạo
              </th>
            </tr>
            <tr>
              <th scope="col" className="px-6 py-3">
                <div className="w-full">
                  <Input variant="static" placeholder="Người báo cáo" />
                </div>
              </th>
              <th scope="col" className="px-6 py-3">
                Báo cáo
              </th>
              <th scope="col" className="px-6 py-3">
                <Input variant="static" placeholder="Nội dung báo cáo" />
              </th>
              <th scope="col" className="px-6 py-3"></th>
            </tr>
          </thead>
          <tbody>
            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
              <th
                scope="row"
                className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
              >
                Apple MacBook Pro 17"
              </th>
              <td className="px-6 py-4">Silver</td>
              <td className="px-6 py-4">Laptop</td>
              <td className="px-6 py-4">Laptop</td>
            </tr>
          </tbody>
        </table>
        <div className="mt-4 flex justify-center">
          <Pagination
            className="mt-4"
            totalPage="15"
            currentPage={currentPage}
            handleClick={test}
          ></Pagination>
        </div>
      </div>
    </>
  );
};

export default ReportPage;
