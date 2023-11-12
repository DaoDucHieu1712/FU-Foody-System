import axios from "../../../../../shared/api/axiosConfig";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { useForm, Controller } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import propTypes from "prop-types";
import ErrorText from "../../../../../shared/components/text/ErrorText";
import {
  Button,
  Dialog,
  DialogHeader,
  DialogBody,
  DialogFooter,
  Input,
  Select,
  Option,
  Typography,
} from "@material-tailwind/react";

const schema = yup.object({
  selectedFood: yup.string().required("Hãy chọn món ăn!"),
  quantity: yup
    .number()
    .positive("Số lượng tồn kho phải lớn hơn 0 !")
    .default(null)
    .typeError("Hãy nhập số lượng !"),
});

const AddInventory = ({ storeId, reloadInventory }) => {
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(!open);
  const [foodList, setFoodList] = useState([]);
  const {
    control, // Use control from react-hook-form
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  useEffect(() => {
    axios
      .get(`/api/Food/GetFoodByStoreId/${storeId}`)
      .then((response) => {
        setFoodList(response);
      })
      .catch((error) => {
        console.error("Error fetching food items: " + error);
      });
  }, []);

  const onSubmit = async (data) => {
    try {
      const response = await axios.get(
        `/api/Inventory/CheckExistingInventory/${storeId}/${data.selectedFood}`
      );

      if (response) {
        alert("Món ăn này đã tồn tại trong kho !");
      } else {
        // Proceed with creating the new inventory entry
        await axios.post(
          `/api/Inventory/CreateInventory`,
          {
            storeId: storeId,
            foodId: data.selectedFood,
            quantity: data.quantity,
          }
        );
        toast.success("Tạo kho món ăn thành công!");
        handleOpen();

        reloadInventory();
      }
    } catch (error) {
      toast.error("Lỗi khi tạo kho món ăn !");
    }
  };

  return (
    <>
      <button
        onClick={handleOpen}
        type="submit"
        className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-5 py-2.5 text-center"
      >
        + Thêm tồn kho mới
      </button>

      <Dialog open={open} size="sm" handler={handleOpen}>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="flex items-center justify-between">
            <DialogHeader className="flex flex-col items-start">
              <Typography className="mb-1" variant="h4">
                TẠO KHO MÓN ĂN
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
                <Controller
                  name="selectedFood"
                  control={control}
                  render={({ field }) => (
                    <Select label="Choose food" {...field}>
                      {foodList.map((food) => (
                        <Option
                          key={food.id.toString()}
                          value={food.id.toString()}
                        >
                          {food.foodName}
                        </Option>
                      ))}
                    </Select>
                  )}
                />
                {errors.selectedFood && (
                  <ErrorText text={errors.selectedFood.message}></ErrorText>
                )}
              </div>
              <Controller
                name="quantity"
                control={control}
                render={({ field }) => (
                  <Input label="Số lượng" type="number" {...field} />
                )}
              />
            </div>
            {errors.quantity && (
              <ErrorText text={errors.quantity.message}></ErrorText>
            )}
          </DialogBody>
          <DialogFooter className="space-x-2">
            <Button variant="text" color="deep-orange" onClick={handleOpen}>
              cancel
            </Button>
            <Button variant="gradient" color="deep-orange" type="submit">
              TẠO MỚI
            </Button>
          </DialogFooter>
        </form>
      </Dialog>
    </>
  );
};

AddInventory.propTypes = {
  reloadInventory: propTypes.any.isRequired,
  storeId: propTypes.any.isRequired,
};

export default AddInventory;
