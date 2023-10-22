import React from "react";
import { Button,
    Dialog,
    DialogHeader,
    DialogBody,
    DialogFooter,
    Input,
    Textarea,Select, Option, Typography} from "@material-tailwind/react";
import { useState } from "react";
const AddInventory= () => {
    const [open, setOpen] = React.useState(false);
    const handleOpen = () => setOpen(!open);
    return (
        <>
        <button onClick={handleOpen} type="submit" className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-5 py-2.5 text-center">+ Thêm tồn kho mới</button>
       
    <Dialog open={open} size="sm" handler={handleOpen}>
        <div className="flex items-center justify-between">
          <DialogHeader className="flex flex-col items-start">
            {" "}
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
            {/* <Typography className="-mb-1" color="blue-gray" variant="h6">
              Tên món ăn
            </Typography> */}
             <div className="w-full">
                <Select label="Chọn món ăn">
                    <Option>Material Tailwind HTML</Option>
                    <Option>Material Tailwind React</Option>
                    <Option>Material Tailwind Vue</Option>
                    <Option>Material Tailwind Angular</Option>
                    <Option>Material Tailwind Svelte</Option>
                </Select>
            </div>
           <Input label="Số lượng" type="number"/>
           
          </div>
        </DialogBody>
        <DialogFooter className="space-x-2">
          <Button variant="text" color="gray" onClick={handleOpen}>
            cancel
          </Button>
          <Button variant="gradient" className="bg-primary" onClick={handleOpen}>
            TẠO MỚI
          </Button>
        </DialogFooter>
      </Dialog>
        </>
    )
};
export default AddInventory;