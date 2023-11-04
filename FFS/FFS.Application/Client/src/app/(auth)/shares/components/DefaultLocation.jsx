
import { Button, Dialog, DialogBody, DialogFooter, DialogHeader } from '@material-tailwind/react';
import axios from "../../../../shared/api/axiosConfig";
import propTypes from 'prop-types';
import { useState } from 'react';
import { toast } from 'react-toastify';
import Cookies from "universal-cookie";

const cookies = new Cookies();

const DefaultLocation = ({ item, reload }) => {
    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);
    const onSubmit = async () => {
        try {
            var email = cookies.get("fu_foody_email");
            axios
                .put(`/api/Location/UpdateDefaultLocation/${item.id}?email=${email}`,)
                .then(() => {
                    toast.success("Cập nhật mặc định thành công!");
                    reload();
                    setOpen(false);
                })
                .catch((error) => {
                    toast.error("Cập nhật mặc định thất bại!");
                    setOpen(false);
                    console.log(error);
                });
        } catch (error) {
            console.error("Error update location: ", error);
            toast.error("Cập nhật mặc định thất bại!");
        }
    }
    return (
        <>
            <p className="text-gray-400 text-center font-semibold border-solid border-2 border-gray-400 h-auto w-36 cursor-pointer hover:text-gray-600 hover:border-gray-600" onClick={handleOpen}>Đặt làm mặc định</p>
            <Dialog
                size="xs"
                open={open}
                handler={handleOpen}
                className="bg-white shadow-none"
            >
                <DialogHeader>Xác nhận cập nhật mặc định</DialogHeader>
                <DialogBody divider>
                    Bạn có muốn đặt địa chỉ này làm mặc định không?
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

DefaultLocation.propTypes = {
    item: propTypes.any.isRequired,
    reload: propTypes.any.isRequired,
};

export default DefaultLocation;