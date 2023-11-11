import {
  Card,
  CardHeader,
  Typography,
  Button,
  CardBody,
  CardFooter,
  IconButton,
  Input,
} from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import CookieService from "../../shared/helper/cookieConfig";
import AddDiscount from "./components/Discount/AddDiscount";
import UpdateDiscount from "./components/Discount/UpdateDiscount";
import DeleteDiscount from "./components/Discount/DeleteDiscount";

const TABLE_HEAD = [
  "Id",
  "Mã ưu đãi",
  "Phần trăm giảm giá (%)",
  "Áp dụng với đơn hàng",
  "Số lượng",
  "Ngày hết hạn",
  "",
];

const Discount = () => {
  const backgroundColors = ["bg-gray-50", "bg-gray-200"];
  const [discountList, setDiscountList] = useState([]);
  const [discountFilter, setDiscountFilter] = useState("");
  const [pageNumber, setPageNumber] = useState(1);
  const pageSize = 10;
  const [totalPages, setTotalPages] = useState(0);
  const [storeId, setStoreId] = useState(0);
  const uId = CookieService.getToken("fu_foody_id");

  const GetStoreByUid = async () => {
    try {
      await axios
        .get(`/api/Store/GetStoreByUid?uId=${uId}`)
        .then((response) => {
          setStoreId(response.id);
          reloadList();
        })
        .catch((error) => {
          console.log(error);
        });
    } catch (error) {
      console.log("Get Store By Uid error: " + error);
    }
  };

  const reloadList = async () => {
    const dataPost = {
      uId: uId,
      DiscountName: discountFilter,
      PageNumber: pageNumber,
      PageSize: pageSize,
    };
    try {
      await axios
        .get(`/api/Discount/ListDiscountByStore`, dataPost)
        .then((res) => {
          console.log(res);
          setDiscountList(res);
        })
        .catch((error) => {
          toast.error("Có lỗi xảy ra!");
          console.log(error);
        });
    } catch (error) {
      console.log("Discount error: " + error);
    }
  };

  useEffect(() => {
    GetStoreByUid();
  }, []);

  useEffect(() => {
    reloadList();
  }, [pageNumber, discountFilter]);

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setPageNumber(newPage);
    }
  };

  return (
    <div>
      <Card className="h-full w-full px-2 py-2">
        <CardHeader floated={false} shadow={false} className="rounded-none">
          <div className="mb-4 flex flex-col justify-between gap-8 md:flex-row md:items-center">
            <Typography variant="h4" color="blue-gray">
              Danh sách ưu đãi
            </Typography>
            <AddDiscount reload={reloadList} storeId={storeId}></AddDiscount>

            <div className="w-full shrink-0 gap-2 px-2 py-2 md:w-max">
              <div className="w-full md:w-72">
                <Input
                  label="Tìm kiếm"
                  icon={
                    <svg
                      width="20"
                      height="20"
                      viewBox="0 0 20 20"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        d="M9.0625 15.625C12.6869 15.625 15.625 12.6869 15.625 9.0625C15.625 5.43813 12.6869 2.5 9.0625 2.5C5.43813 2.5 2.5 5.43813 2.5 9.0625C2.5 12.6869 5.43813 15.625 9.0625 15.625Z"
                        stroke="#191C1F"
                        strokeWidth="1.5"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                      />
                      <path
                        d="M13.7031 13.7031L17.5 17.5"
                        stroke="#191C1F"
                        strokeWidth="1.5"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                      />
                    </svg>
                  }
                  value={discountFilter}
                  onChange={(e) => setDiscountFilter(e.target.value)}
                />
              </div>
            </div>
          </div>
        </CardHeader>
        <CardBody className="px-0">
          <table className="w-full min-w-max table-auto text-center">
            <thead>
              <tr>
                {TABLE_HEAD.map((head) => (
                  <th
                    key={head}
                    className="border-y border-blue-gray-100 bg-blue-gray-50/50 p-4"
                  >
                    <Typography
                      variant="small"
                      color="blue-gray"
                      className="font-normal leading-none opacity-70"
                    >
                      {head}
                    </Typography>
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {discountList ? (
                discountList.map((discount, index) => (
                  <tr
                    key={discount.id}
                    className={
                      backgroundColors[index % backgroundColors.length]
                    }
                  >
                    <td>
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-bold"
                      >
                        {discount.id}
                      </Typography>
                    </td>
                    <td>
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal max-w-xs truncate"
                      >
                        {discount.code}
                      </Typography>
                    </td>
                    <td>
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal max-w-xs truncate"
                      >
                        {discount.conditionPrice}
                      </Typography>
                    </td>
                    <td>
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal"
                      >
                        {discount.quantity}
                      </Typography>
                    </td>
                    <td>
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="font-normal"
                      >
                        {discount.expired}
                      </Typography>
                    </td>
                    <td>
                      <div className="h-full flex justify-center items-center">
                        <UpdateDiscount
                          reload={reloadList}
                          foodData={discount}
                          storeId={storeId}
                        ></UpdateDiscount>
                        <DeleteDiscount
                          reload={reloadList}
                          id={discount.id}
                        ></DeleteDiscount>
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={TABLE_HEAD.length}>
                    <p>Chưa có ưu đãi nào!</p>
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </CardBody>
        <CardFooter className="flex items-center justify-between border-t border-blue-gray-50 p-4">
          <Button
            variant="outlined"
            size="sm"
            onClick={() => handlePageChange(pageNumber - 1)}
          >
            Previous
          </Button>
          <div className="flex items-center gap-2">
            {Array.from({ length: totalPages }, (_, i) => (
              <IconButton
                key={i}
                variant={i + 1 === pageNumber ? "outlined" : "text"}
                size="sm"
                onClick={() => handlePageChange(i + 1)}
              >
                {i + 1}
              </IconButton>
            ))}
          </div>
          <Button
            variant="outlined"
            size="sm"
            onClick={() => handlePageChange(pageNumber + 1)}
          >
            Next
          </Button>
        </CardFooter>
      </Card>
    </div>
  );
};

export default Discount;
