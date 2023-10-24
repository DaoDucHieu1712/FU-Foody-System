import { useEffect, useState } from "react";
import React from "react";
import axios from "axios";
import AddInventory from "../inventory/AddInventory";
import UpdateInventory from "../inventory/UpdateInventory";
import DeleteInventory from "../inventory/DeleteInventory";
import {Card,CardHeader,Input,Typography,Button,CardBody,Chip,CardFooter,Tabs,TabsHeader,Tab,Avatar,IconButton,Tooltip,
} from "@material-tailwind/react";
import FormatDateString from "../../../../../shared/components/format/FormatDate";

const TABS = [
  {
    label: "All",
    value: "all",
  },
  {
    label: "Monitored",
    value: "monitored",
  },
  {
    label: "Unmonitored",
    value: "unmonitored",
  },
];
const TABLE_HEAD = [
  "Ảnh món ăn",
  "Tên món ăn",
  "Ngày tạo",
  "Ngày chỉnh sửa",
  "Phân loại",
  "Số lượng",
  "Action",
];



const Inventory = () => {
  const [inventory, setInventory] = useState([]);
  const [foodNameFilter, setFoodNameFilter] = useState("");
  const [storeID] = useState(1); // Set the storeID to 1
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [totalPages, setTotalPages] = useState(1);

  const fetchInventory = async () => {
    try {
      const response = await axios.get(
        "https://localhost:7025/api/Inventory/GetInventories",
        {
          params: {
            StoreId: storeID,
            FoodName: foodNameFilter,
            PageNumber: pageNumber,
            PageSize: pageSize,
          },
        }
      );
      setInventory(response.data);
      console.log(response);
      // Extract pagination data from the response headers
      const paginationHeader = response.headers["x-pagination"];
      console.log(paginationHeader);
      if (paginationHeader) {
        const paginationData = JSON.parse(paginationHeader);
        setTotalPages(paginationData.TotalPages);
      }
    } catch (error) {
      console.error("Error fetching inventory data:", error);
    }
  };

  useEffect(() => {
    fetchInventory();
  }, [storeID, foodNameFilter, pageNumber, pageSize]);

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setPageNumber(newPage);
    }
  };

  // Filter inventory items by foodName
  const filteredInventory = inventory.filter((item) => {
    return item.foodName.toLowerCase().includes(foodNameFilter.toLowerCase());
  });

  const reloadInventory = async () => {
    await fetchInventory();
  };

  return (
    <>
      <div className="w-full h-auto">
        <div className="flex items-center justify-between">
          <p className="px-5 mx-5 mt-2 font-bold text-lg pointer-events-none">
            Tồn kho
          </p>
          <AddInventory></AddInventory>
        </div>

        <hr className="h-px my-4 bg-gray-200 border-0 dark:bg-gray-700" />
      </div>

      <div>
        <Card className="h-full w-full">
          <CardHeader floated={false} shadow={false} className="rounded-none">
            <div className="flex flex-col items-center justify-between gap-4 md:flex-row mt-3">
              <Tabs value="all" className="w-full md:w-max">
                <TabsHeader>
                  {TABS.map(({ label, value }) => (
                    <Tab key={value} value={value}>
                      &nbsp;&nbsp;{label}&nbsp;&nbsp;
                    </Tab>
                  ))}
                </TabsHeader>
              </Tabs>
              <div className="w-full md:w-72">
                <Input
                  label="Tên món ăn"
                  icon={<i className="fas fa-search" />}
                  value={foodNameFilter}
                  onChange={(e) => setFoodNameFilter(e.target.value)}
                />
              </div>
            </div>
          </CardHeader>
          <CardBody className="px-0">
            <table className="mt-4 w-full min-w-max table-auto text-left">
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
                {filteredInventory.map(
                  (
                    { id,
                      foodId,
                      imageURL,
                      foodName,
                      quantity,
                      createdAt,
                      updatedAt,
                      categoryName,
                    },
                    index
                  ) => {
                    const isLast = index === filteredInventory.length - 1;
                    const classes = isLast
                      ? "p-4"
                      : "p-4 border-b border-blue-gray-50";

                    return (
                      <tr key={id}>
                        <td className={classes}>
                          <div className="flex items-center gap-3">
                            <img
                              className="h-20 w-20 object-cover object-center"
                              src={imageURL}
                              alt="nature image"
                            />
                          </div>
                        </td>
                        <td className={classes}>
                          <Typography
                            variant="small"
                            color="blue-gray"
                            className="font-normal"
                          >
                            {foodName}
                          </Typography>
                        </td>

                        <td className={classes}>
                          <Typography
                            variant="small"
                            color="blue-gray"
                            className="font-normal"
                        
                          >
                            {FormatDateString(createdAt)}
                          </Typography>
                        </td>
                        <td className={classes}>
                          <Typography
                            variant="small"
                            color="blue-gray"
                            className="font-normal"
                          >
                            {FormatDateString(updatedAt)}
                          </Typography>
                        </td>
                        <td className={classes}>
                          <Typography
                            variant="small"
                            color="blue-gray"
                            className="font-normal"
                          >
                            {categoryName}
                          </Typography>
                        </td>
                        <td className={classes}>
                          <Typography
                            variant="small"
                            color="blue-gray"
                            className="font-normal"
                          >
                            {quantity}
                          </Typography>
                        </td>
                        <td className={classes}>
                          <UpdateInventory
                            foodId={foodId}
                            foodName={foodName}
                            quantity={quantity}
                            reloadInventory={reloadInventory}
                          />
                          <DeleteInventory inventoryId={id} reloadInventory={reloadInventory} />
                        </td>
                      </tr>
                    );
                  }
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
    </>
  );
};
export default Inventory;
