import {
  Button,
  IconButton,
  Input,
  Option,
  Radio,
  Select,
  Spinner,
  Tooltip,
  Typography,
} from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";

const filter = [
  { id: "", name: "Tất cả" },
  { id: 1, name: "Tên A-Z" },
  { id: 2, name: "Tên Z-A" },
  { id: 3, name: "Đánh giá hàng đầu" },
];

const StoreList = () => {
  const [storeList, setStoreList] = useState([]);
  const [storeNameFilter, setStoreNameFilter] = useState("");
  const [storeFilter, setStoreFilter] = useState("");
  const [pageNumber, setPageNumber] = useState(1);
  const pageSize = 12;
  const [totalPages, setTotalPages] = useState(0);
  const [category, setCategory] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState("");

  const GetListCategory = async () => {
    try {
      axios
        .get("/api/Category/ListTop8PopularCategory")
        .then((response) => {
          setCategory([{ id: "", categoryName: "Tất cả" }, ...response]);
        })
        .catch((error) => {
          console.log(error);
          toast.error("Lấy phân loại thất bại!");
        });
    } catch (error) {
      console.error("Category: " + error);
    }
  };

  const GetListAllStore = async () => {
    try {
      axios
        .get(`/api/Store/ListAllStore`, {
          params: {
            CategoryName: selectedCategory,
            Search: storeNameFilter,
            FilterStore: storeFilter,
            PageNumber: pageNumber,
            PageSize: pageSize,
          },
        })
        .then((response) => {
          setStoreList(response.storeDTOs);
          setTotalPages(response.metadata.totalPages);
        })
        .catch((error) => {
          console.log(error);
          toast.error("Lấy phân loại thất bại!");
        });
    } catch (error) {
      console.error("Category: " + error);
    }
  };

  const handleFoodListByCategory = async (e) => {
    setSelectedCategory(e.target.value);
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setPageNumber(newPage);
    }
  };

  useEffect(() => {
    GetListCategory();
  }, []);

  useEffect(() => {
    GetListAllStore();
  }, [storeNameFilter, storeFilter, selectedCategory, pageNumber]);

  return (
    <div className="flex gap-5">
      <div className="flex w-60 flex-col">
        <Typography variant="h6">DANH MỤC PHỔ BIẾN</Typography>
        {category ? (
          category.map((category) => (
            <Radio
              key={category.id}
              label={category.categoryName}
              checked={selectedCategory == category.categoryName}
              onChange={handleFoodListByCategory}
              value={category.categoryName}
            />
          ))
        ) : (
          <Spinner></Spinner>
        )}
        <hr className="h-px my-2 bg-gray-200 border-0 dark:bg-gray-700" />
      </div>
      <div className="w-full">
        <div className="flex justify-between">
          <div className="w-96">
            <Input
              label="Tìm kiếm cửa hàng"
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
              value={storeNameFilter}
              onChange={(e) => setStoreNameFilter(e.target.value)}
            />
          </div>
          <div className="flex items-center">
            <Typography className="w-24">Bộ lọc: </Typography>
            <Select
              className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
              onChange={(e) => {
                setStoreFilter(e);
              }}
              label="Chọn loại"
            >
              {filter.map((filterItem) => (
                <Option key={filterItem.id} value={filterItem.id.toString()}>
                  {filterItem.name}
                </Option>
              ))}
            </Select>
          </div>
        </div>
        <hr className="h-px my-2 bg-gray-300 border-0 dark:bg-gray-700" />
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 my-3">
          {storeList && storeList.length != 0 ? (
            storeList.map((store) => (
              <div key={store.id} className="rounded border relative">
                <a href={`/store/detail/${store.id}`}>
                  <img
                    className="w-full h-40 object-cover"
                    src={store.avatarURL}
                    alt="Store"
                  />
                </a>

                <div
                  className="text-sm absolute bg-green-600 text-white rounded-full h-4 w-4  border border-white"
                  style={{ top: "-6px", left: "-6px" }}
                ></div>

                <div className="px-2 py-2">
                  <a
                    href={`/store/detail/${store.id}`}
                    className="font-semibold text-base inline-block hover:text-orange-600 transition duration-500 ease-in-out"
                    style={{
                      display: "-webkit-box",
                      WebkitLineClamp: 1,
                      WebkitBoxOrient: "vertical",
                      overflow: "hidden",
                    }}
                  >
                    {store.storeName}
                  </a>
                  <p
                    className="text-gray-500 text-sm"
                    style={{
                      display: "-webkit-box",
                      WebkitLineClamp: 1,
                      WebkitBoxOrient: "vertical",
                      overflow: "hidden",
                    }}
                  >
                    {store.address}
                  </p>
                </div>
              </div>
            ))
          ) : (
            <Typography variant="h5" className="mt-5 ml-5">
              Không có cửa hàng nào!
            </Typography>
          )}
        </div>
        <div className="flex items-center justify-between border-t border-blue-gray-50 p-4">
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
        </div>
      </div>
    </div>
  );
};

export default StoreList;
