import { useEffect, useState } from "react";
import axios from "axios";
import AddInventory from "../inventory/AddInventory";
import {
  Card,
  CardHeader,
  Input,
  Typography,
  Button,
  CardBody,
  Chip,
  CardFooter,
  Tabs,
  TabsHeader,
  Tab,
  Avatar,
  IconButton,
  Tooltip,
} from "@material-tailwind/react";

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
  "Id",
  "Tên món ăn",
  "Ngày tạo",
  "Ngày chỉnh sửa",
  "Số lượng",
  "Action",
];

const Inventory = () => {
  const [inventory, setInventory] = useState([]);
  const [foodNameFilter, setFoodNameFilter] = useState("");
  const [storeID] = useState(1); // Set the storeID to 1

  useEffect(() => {
    // Fetch inventory data from the API
    fetch(
      `https://localhost:7025/api/Inventory/GetInventories?StoreId=${storeID}&FoodName=${foodNameFilter}`
    )
      .then((response) => response.json())
      .then((data) => setInventory(data));
  }, [storeID, foodNameFilter]);
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
                {inventory.map(
                  (
                    { foodId, foodName, quantity, createdAt, updatedAt },
                    index
                  ) => {
                    const isLast = index === inventory.length - 1;
                    const classes = isLast
                      ? "p-4"
                      : "p-4 border-b border-blue-gray-50";

                    return (
                      <tr key={foodId}>
                        <td className={classes}>
                          <div className="flex items-center gap-3">
                            <Typography
                              variant="small"
                              color="blue-gray"
                              className="font-bold"
                            >
                              {foodId}
                            </Typography>
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
                            {createdAt}
                          </Typography>
                        </td>
                        <td className={classes}>
                          <Typography
                            variant="small"
                            color="blue-gray"
                            className="font-normal"
                          >
                            {updatedAt}
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
                          <Tooltip content="Edit Inventory">
                            <IconButton variant="text">
                              <i className="fas fa-pencil" />
                              {/* <PencilIcon className="h-4 w-4" /> */}
                            </IconButton>
                          </Tooltip>
                        </td>
                      </tr>
                    );
                  }
                )}
              </tbody>
            </table>
          </CardBody>
          <CardFooter className="flex items-center justify-between border-t border-blue-gray-50 p-4">
            <Button variant="outlined" size="sm">
              Previous
            </Button>
            <div className="flex items-center gap-2">
              <IconButton variant="outlined" size="sm">
                1
              </IconButton>
              <IconButton variant="text" size="sm">
                2
              </IconButton>
              <IconButton variant="text" size="sm">
                3
              </IconButton>
              <IconButton variant="text" size="sm">
                ...
              </IconButton>
              <IconButton variant="text" size="sm">
                8
              </IconButton>
              <IconButton variant="text" size="sm">
                9
              </IconButton>
              <IconButton variant="text" size="sm">
                10
              </IconButton>
            </div>
            <Button variant="outlined" size="sm">
              Next
            </Button>
          </CardFooter>
        </Card>
      </div>
    </>
  );
};
export default Inventory;
