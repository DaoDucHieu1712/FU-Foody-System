import { useState } from 'react';
import propTypes from "prop-types";
import { Button, Dialog, DialogBody, DialogFooter, DialogHeader, IconButton, Tooltip } from "@material-tailwind/react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from 'react-toastify';


const DeleteFood = ({ id, reload }) => {

    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

    const onSubmit = async () => {
        try {
            axios
                .put(`/api/Food/DeleteFood/${id}`)
                .then(() => {
                    toast.success("Xóa món ăn thành công!");
                    reload();
                    setOpen(false);
                })
                .catch((error) => {
                    toast.error("Xóa món ăn thất bại!");
                    setOpen(false);
                    console.log(error);
                });
        } catch (error) {
            console.error("Error delete food: ", error);
        }
    }

    return (
        <>
            <Tooltip content="Xóa món ăn">
                <IconButton variant="text" onClick={handleOpen}>
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="20"
                        height="20"
                        viewBox="0 0 448 512"
                    >
                        <path
                            d="M135.2 17.7L128 32H32C14.3 32 0 46.3 0 64S14.3 96 32 96H416c17.7 0 32-14.3 32-32s-14.3-32-32-32H320l-7.2-14.3C307.4 6.8 296.3 0 284.2 0H163.8c-12.1 0-23.2 6.8-28.6 17.7zM416 128H32L53.2 467c1.6 25.3 22.6 45 47.9 45H346.9c25.3 0 46.3-19.7 47.9-45L416 128z"
                        />
                    </svg>
                </IconButton>
            </Tooltip>
            <Dialog
                size="xs"
                open={open}
                handler={handleOpen}
                className="bg-white shadow-none"
            >
                <DialogHeader>Xác nhận xóa</DialogHeader>
                <DialogBody divider>
                    Bạn có muốn xóa món ăn này không?
                </DialogBody>
                <DialogFooter>
                    <Button
                        variant="text"
                        color="red"
                        onClick={handleOpen}
                        className="mr-1"
                    >
                        <span>Quay lại</span>
                    </Button>
                    <Button variant="gradient" color="green" onClick={onSubmit}>
                        <span>Xác nhận</span>
                    </Button>
                </DialogFooter>
            </Dialog>
        </>
    );
};
DeleteFood.propTypes = {
    id: propTypes.any.isRequired,
    reload: propTypes.any.isRequired,
};
export default DeleteFood;