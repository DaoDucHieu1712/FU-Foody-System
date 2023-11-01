import { Checkbox, Dialog, Textarea, Typography } from "@material-tailwind/react";
import { useState } from "react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import propTypes from "prop-types";


const ReportStore = ({ uId, sId }) => {

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
                description: reasons
            };
            axios
                .put(`/api/Store/ReportStore`, newReport)
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
            <Typography className="text-orange-900 font-semibold cursor-pointer hover:underline hover:text-orange-700" onClick={handleOpen}>Báo cáo</Typography>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <div className="bg-white rounded px-4 py-4">
                    <Typography variant="h5" color="black">Báo cáo cửa hàng</Typography>
                    <div className="flex flex-col">
                        <Checkbox label="Chưa nhận được đơn hàng" value="Chưa nhận được đơn hàng" onChange={handleCheckboxChange} />
                        <Checkbox label="Đơn hàng giao thiếu/ giao nhầm" value="Đơn hàng giao thiếu/ giao nhầm" onChange={handleCheckboxChange} />
                        <Checkbox label="Đơn hàng không đúng với mô tả" value="Đơn hàng không đúng với mô tả" onChange={handleCheckboxChange} />
                        <Checkbox label="Đơn hàng không đảm bảo vệ sinh" value="Đơn hàng không đảm bảo vệ sinh" onChange={handleCheckboxChange} />
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

ReportStore.propTypes = {
    uId: propTypes.any.isRequired,
    sId: propTypes.any.isRequired
};

export default ReportStore;