import { Checkbox, Dialog, Textarea, Typography,MenuItem } from "@material-tailwind/react";
import { useState } from "react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import propTypes from "prop-types";


const ReportShipper = ({ uId, sId }) => {

    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);
    const [selectedReasons, setSelectedReasons] = useState([]);
    const [otherReason, setOtherReason] = useState('');
    const [isOtherReasonChecked, setIsOtherReasonChecked] = useState(false);

    const handleCheckboxChange = (event) => {
        const value = event.target.value;
        if (event.target.checked) {
            setSelectedReasons([...selectedReasons, value]);
        } else {
            setSelectedReasons(selectedReasons.filter(reason => reason !== value));
        }
    };

    const handleOtherReasonChange = (event) => {
        setOtherReason(event.target.value);
    };

    const handleOtherReasonCheckboxChange = (event) => {
        setIsOtherReasonChecked(event.target.checked);
    };

    const onSubmit = async () => {
        try {
            let reasons = selectedReasons.join(', ');
            if (isOtherReasonChecked && otherReason.trim() !== '' && reasons !== '') {
                reasons += `, ${otherReason}`;
            } else if (isOtherReasonChecked && otherReason.trim() !== '' && reasons === '') {
                reasons += `${otherReason}`;
            } else if (reasons === '') {
                toast.warning("Vui lòng chọn ít nhất một lý do!");
                setOpen(false);
                return;
            }

            const newReport = {
                userId: uId,
                targetId: sId,
                reportType: 2,
                description: reasons
            };
            axios
                .post(`/api/Report`, newReport)
                .then(() => {
                    toast.success("Báo cáo thành công!");
                    setSelectedReasons([]);
                    setIsOtherReasonChecked(false);
                    setOtherReason('');
                    setOpen(false);
                })
                .catch((error) => {
                    toast.error("Báo cáo thất bại!");
                    setSelectedReasons([]);
                    setIsOtherReasonChecked(false);
                    setOtherReason('');
                    setOpen(false);
                    console.log(error);
                });
        } catch (error) {
            console.error("Error report store: ", error);
        }
    }

    return (
        <>
            {/* <Typography className="text-orange-900 font-semibold cursor-pointer hover:underline hover:text-orange-700" onClick={handleOpen}>Báo cáo</Typography> */}

            <MenuItem onClick={handleOpen}>Báo cáo</MenuItem>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <div className="bg-white rounded px-4 py-4">
                    <Typography variant="h5" color="black">Báo cáo nhân viên giao hàng</Typography>
                    <div className="flex flex-col">
                        <Checkbox label="Giao hàng chậm" value="Giao hàng chậm" onChange={handleCheckboxChange} />
                        <Checkbox label="Xúc phạm khách hàng" value="Xúc phạm khách hàng" onChange={handleCheckboxChange} />
                        <Checkbox label="Nhân viên cáu gắt khi gặp khách hàng" value="Nhân viên cáu gắt khi gặp khách hàng" onChange={handleCheckboxChange} />
                        <Checkbox
                            label="Lý do khác"
                            value="Ly do khac"
                            onChange={handleOtherReasonCheckboxChange}
                        />
                        {isOtherReasonChecked && (
                            <Textarea
                                label="Nhập lý do khác"
                                value={otherReason}
                                onChange={handleOtherReasonChange}
                                className="pt-4"
                            />
                        )}
                    </div>
                    <button onClick={onSubmit} className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Gửi báo cáo</button>
                </div>
            </Dialog>
        </>
    );
};

ReportShipper.propTypes = {
    uId: propTypes.any.isRequired,
    sId: propTypes.any.isRequired
};

export default ReportShipper;