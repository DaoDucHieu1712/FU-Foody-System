import { useEffect, useState } from "react";
import React from "react";
import axios from "../../../../../shared/api/axiosConfig";
import AddInventory from "../inventory/AddInventory";
import UpdateInventory from "../inventory/UpdateInventory";
import DeleteInventory from "../inventory/DeleteInventory";

import {
  Card,
  CardHeader,
  Input,
  Typography,
  Button,
  CardBody,
  CardFooter,
  Tabs,
  TabsHeader,
  Tab,
  IconButton,
} from "@material-tailwind/react";
import FormatDateString from "../../../../../shared/components/format/FormatDate";
import ArrowRight from "../../../../../shared/components/icon/ArrowRight";
import ArrowLeft from "../../../../../shared/components/icon/ArrowLeft";

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
  "",
];

const Inventory = () => {
  const [inventory, setInventory] = useState([]);
  const [foodNameFilter, setFoodNameFilter] = useState("");
  const [storeID] = useState(1); // Set the storeID to 1
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(4);
  const [totalPages, setTotalPages] = useState(1);
  const [active, setActive] = React.useState(1);

  const getItemProps = (index) => ({
    variant: active === index ? "filled" : "text",
    onClick: () => {
      setActive(index);
      setPageNumber(index); 
    },
    className: `rounded-full ${active === index ? "bg-primary" : ""}`,
  });

  const next = () => {
    if (active < totalPages) {
      setActive(active + 1);
      setPageNumber(pageNumber + 1);
    }
  };

  const prev = () => {
    if (active > 1) {
      setActive(active - 1);
      setPageNumber(pageNumber - 1); 
    }
  };

  const fetchInventory = async () => {
    try {
      const response = await axios.get("/api/Inventory/GetInventories", {
        params: {
          StoreId: storeID,
          FoodName: foodNameFilter,
          PageNumber: pageNumber,
          PageSize: pageSize,
        },
      });
      setInventory(response.entityInventory);
      setTotalPages(response.metadata.totalPages);
    } catch (error) {
      console.error("Error fetching inventory data:", error);
    }
  };

  useEffect(() => {
    fetchInventory();
  }, [storeID, foodNameFilter, pageNumber, pageSize]);

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
          <AddInventory reloadInventory={reloadInventory}></AddInventory>
        </div>

        <hr className="h-px my-4 bg-gray-200 border-0 dark:bg-gray-700" />
      </div>

      <Card className="h-full w-full" shadow={false} floated={false}>
        <CardHeader floated={false} shadow={false} className="rounded-none">
          <div className="flex flex-col items-center justify-between gap-4 md:flex-row mt-3">
            <div className="ExportExcel">
              <button className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-5 py-2.5 text-center">
                Export Excel
              </button>
            </div>

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
        <CardBody className="p-0 mt-3">
          
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
                        color="black"
                        className="font-normal leading-none opacity-70"
                      >
                        {head}
                      </Typography>
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {inventory.map(
                  (
                    {
                      id,
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
                    const isLast = index === inventory.length - 1;
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
                          <DeleteInventory
                            inventoryId={id}
                            reloadInventory={reloadInventory}
                          />
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
            variant="text"
            className="flex items-center gap-2 rounded-full"
            onClick={prev}
            disabled={active === 1}
          >
            <ArrowLeft /> Previous
          </Button>
          <div className="flex items-center gap-2">
            {Array.from({ length: totalPages }, (_, index) => (
              <IconButton key={index + 1} {...getItemProps(index + 1)}>
                {index + 1}
              </IconButton>
            ))}
          </div>
          <Button
            variant="text"
            className="flex items-center gap-2 rounded-full"
            onClick={next}
            disabled={active === totalPages}
          >
            Next <ArrowRight />
          </Button>
        </CardFooter>
      </Card>
    </>
  );
};
export default Inventory;