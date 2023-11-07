import axios from "../../../../../shared/api/axiosConfig";
import React from "react";
import { toast } from "react-toastify";

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
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import propTypes from "prop-types";
import ErrorText from "../../../../../shared/components/text/ErrorText";

const schema = yup.object({
  quantity: yup
    .number()
    .positive("Số lượng tồn kho phải lớn hơn 0 !")
    .default(null)
    .typeError("Hãy nhập số lượng !"),
});

const UpdateInventory = ({ foodName, quantity, foodId, reloadInventory }) => {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(!open);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const onSubmit = async (data) => {
    try {
      // Use the foodId in the update request
      const storeId = 1;

      // Send the update request with the entered quantity
      await axios.put(
        `/api/Inventory/UpdateInventoryByStoreAndFoodId/update/${storeId}/${foodId}/${data.quantity}`
      );
      toast.success("Cập nhật kho thành công!");
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
        <form onSubmit={handleSubmit(onSubmit)}>
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
                <Input
                  label="Tên món ăn"
                  type="text"
                  value={foodName}
                  readOnly
                />
              </div>
              <Input
                label="Số lượng"
                type="number"
                defaultValue={quantity || null}
                {...register("quantity")}
              />
            </div>
            {errors.quantity && (
              <ErrorText text={errors.quantity.message}></ErrorText>
            )}
          </DialogBody>
          <DialogFooter className="space-x-2">
            <Button variant="text" color="deep-orange" onClick={handleOpen}>
              Hủy
            </Button>
            <Button variant="gradient" color="deep-orange" type="submit">
              Cập nhật
            </Button>
          </DialogFooter>
        </form>
      </Dialog>
    </>
  );
};
UpdateInventory.propTypes = {
  foodId: propTypes.any.isRequired,
  reloadInventory: propTypes.any.isRequired,
  foodName: propTypes.any.isRequired,
  quantity: propTypes.any.isRequired,
};
export default UpdateInventory;
