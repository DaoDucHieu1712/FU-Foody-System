import { Button, Dialog, DialogBody, DialogFooter, DialogHeader } from "@material-tailwind/react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const CheckLogin = () => {
    const navigate = useNavigate();
    const [open, setOpen] = useState(true);

    const handleOpen = () => {
        setOpen((cur) => !cur);
    }

    return (
        <div>
            <Dialog
                size="xs"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <div className='bg-white p-1 mb-4 rounded-xl shadow-lg'>
                    <DialogHeader className='flex flex-col'>
                        Đăng nhập hệ thống
                    </DialogHeader>
                    <DialogBody className='font-semibold text-sm text-center'>
                        Vui lòng đăng nhập để thực hiện chức năng này!
                    </DialogBody>
                    <DialogFooter>
                        <Button variant="outlined" onClick={() => navigate(`/login`)}>
                            <span>Đăng nhập</span>
                        </Button>
                        <Button variant="text" onClick={handleOpen}>
                            <span>Hủy</span>
                        </Button>
                    </DialogFooter>
                </div>
            </Dialog>
        </div>
    );
};

export default CheckLogin;