import axios from "axios";
import React from "react";
import { toast } from "react-toastify";
import ErrorText from "../../../../../shared/components/text/ErrorText";
import {
  Input,
  Typography,
  Button,
  Dialog,
  DialogHeader,
  DialogBody,
  DialogFooter,
  IconButton,
} from "@material-tailwind/react";

const UpdateInventory = ({ foodName, quantity, foodId, reloadInventory }) => {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(!open);
  const [updatedQuantity, setUpdatedQuantity] = React.useState(quantity);
  const [errors, setErrors] = React.useState({});

  const handleUpdate = async () => {
    try {
        if (updatedQuantity < 0) {
          setErrors({ quantity: "Số lượng tồn kho phải > 0" });
          return;
        }
      
      // Use the foodId in the update request
      const storeId = 1;

      // Send the update request
      await axios.put(
        `https://localhost:7025/api/Inventory/UpdateInventoryByStoreAndFoodId/update/${storeId}/${foodId}/${updatedQuantity}`
      );
      toast.success("Cập nhật kho thành công !");
      // Close the dialog after a successful update
      handleOpen();
      // Reload the inventory list
      reloadInventory();
    } catch (error) {
      toast.error("Lỗi xảy ra khi cập nhật kho:", error);
    }
  };

  return (
    <>
      <IconButton variant="text" onClick={handleOpen}>
        <i className="fas fa-pencil" />
      </IconButton>

      <Dialog open={open} size="sm" handler={handleOpen}>
        <div className="flex items-center justify-between">
          <DialogHeader className="flex flex-col items-start">
            <Typography className="mb-1" variant="h4">
              Chỉnh sửa kho món ăn
            </Typography>
          </DialogHeader>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            fill="currentColor"
            className="mr-3 h-5 w-5"
            onClick={handleOpen}
          >
            <path
              fillRule="evenodd"
              d="M5.47 5.47a.75.75 0 011.06 0L12 10.94l5.47-5.47a.75.75 0 111.06 1.06L13.06 12l5.47 5.47a.75.75 0 11-1.06 1.06L12 13.06l-5.47 5.47a.75.75 0 01-1.06-1.06L10.94 12 5.47 6.53a.75.75 0 010-1.06z"
              clipRule="evenodd"
            />
          </svg>
        </div>
        <DialogBody>
          <div className="grid gap-6">
            <div className="w-full">
              <Input label="Tên món ăn" type="text" value={foodName} readOnly />
            </div>
            <Input
              label="Số lượng"
              type="number"
              value={updatedQuantity}
              onChange={(e) => setUpdatedQuantity(e.target.value)}
            />
          </div>
          {errors.quantity && (
            <p className="text-sm text-red-500">{errors.quantity}</p>
          )}
        </DialogBody>
        <DialogFooter className="space-x-2">
          <Button variant="text" color="deep-orange" onClick={handleOpen}>
            Hủy
          </Button>
          <Button variant="gradient" color="deep-orange" onClick={handleUpdate}>
            Cập nhật
          </Button>
        </DialogFooter>
      </Dialog>
    </>
  );
};

export default UpdateInventory;
