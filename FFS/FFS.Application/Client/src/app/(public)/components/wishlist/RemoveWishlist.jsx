import React, { useState } from "react";
import { Button, Dialog, DialogHeader, DialogBody, DialogFooter, IconButton,Typography } from "@material-tailwind/react";
import axios from "../../../../shared/api/axiosConfig";
import { toast } from "react-toastify";

const RemoveWishlist = ({ wishlistId, reloadWishlist }) => {
  const [open, setOpen] = useState(false);

  const handleOpen = () => setOpen(!open);

  const handleDelete = async () => {
    try {
      
      await axios.delete(`/api/Wishlist/RemoveFromWishlist/${wishlistId}`);
      toast.success("Xóa yêu thích món ăn thành công!");
      // Close the dialog after a successful delete
      handleOpen();
      // Reload the inventory list
      reloadWishlist();
    } catch (error) {
      toast.error("Lỗi xảy ra khi xóa yêu thích món ăn:", error);
    }
  };

  return (
    <>
    <button onClick={handleOpen} class="align-middle select-none font-sans font-bold text-center uppercase transition-all disabled:opacity-50 disabled:shadow-none disabled:pointer-events-none text-xs py-3 border text-gray-900 hover:opacity-75  active:opacity-[0.85] block w-full rounded-none px-0 border-orange-100" type="button">Xóa</button>
      {/* <IconButton variant="text" onClick={handleOpen}>
        <i className="fas fa-trash" />
      </IconButton> */}

      <Dialog open={open} size="sm" handler={handleOpen}>
        <div className="flex items-center justify-between">
          <DialogHeader className="flex flex-col items-start">
            <Typography className="mb-1" variant="h4">
              Xác nhận xóa yêu thích món ăn 
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
          <p className="text-sm text-black">
            Bạn có chắc chắn muốn xóa yêu thích món ăn này không?
          </p>
        </DialogBody>
        <DialogFooter className="space-x-2">
          <Button variant="text" color="deep-orange" onClick={handleOpen}>
            Hủy
          </Button>
          <Button variant="gradient" color="deep-orange" onClick={handleDelete}>
            Xóa
          </Button>
        </DialogFooter>
      </Dialog>
    </>
  );
};

export default RemoveWishlist;
