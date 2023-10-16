import { useState } from 'react';
import propTypes from "prop-types";
import { Button, Dialog, DialogBody, DialogFooter, DialogHeader } from "@material-tailwind/react";
import axios from 'axios';
import { toast } from 'react-toastify';


const DeleteLocation = ({ id, reload }) => {

    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

    console.log(id);

    const onSubmit = async () => {
        try {
            const response = await axios.delete(`https://localhost:7025/api/Location/DeleteLocation/${id}`);
            if (response.status == 200) {
                toast.success("Xóa địa chỉ thành công!");
                reload();
                setOpen(false);
            }
        } catch (error) {
            console.error("Error update location: ", error);
            toast.error("Xóa địa chỉ thất bại!");
        }
    }

    return (
        <>
            <p className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600" onClick={handleOpen}>Xóa</p>
            <Dialog
                size="xs"
                open={open}
                handler={handleOpen}
                className="bg-white shadow-none"
            >
                <DialogHeader>Xác nhận xóa</DialogHeader>
                <DialogBody divider>
                    Bạn có muốn xóa địa chỉ này không?
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
DeleteLocation.propTypes = {
    id: propTypes.any.isRequired,
    reload: propTypes.any.isRequired,
};
export default DeleteLocation;