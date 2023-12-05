import { Checkbox, Dialog, Textarea, Typography } from "@material-tailwind/react";
import { useState } from "react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import propTypes from "prop-types";
import { FaExclamationCircle } from "react-icons/fa";
import CheckLogin from "./CheckLogin";



const ReportStore = ({ uId, sId }) => {

    const [open, setOpen] = useState(false);
    const [selectedReasons, setSelectedReasons] = useState([]);
    const [otherReason, setOtherReason] = useState('');
    const [isOtherReasonChecked, setIsOtherReasonChecked] = useState(false);
    const [isLogged, setIsLogged] = useState(true);

    const handleCheckboxChange = (event) => {
        const value = event.target.value;
        if (event.target.checked) {
            setSelectedReasons([...selectedReasons, value]);
        } else {
            setSelectedReasons(selectedReasons.filter(reason => reason !== value));
        }
    };


    const handleOpen = () => {
        if (uId != null || uId != undefined) {
            setOpen((cur) => !cur)
            setIsLogged(true);
        } else {
            setIsLogged((cur) => !cur);
            setTimeout(() => {
                setIsLogged(true);
            }, 5000);
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
                reportType: 1,
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
            <Typography className="text-orange-900 font-semibold cursor-pointer hover:underline hover:text-orange-700" onClick={handleOpen}> <span className="flex items-center">
                <FaExclamationCircle className="ml-2" />Báo cáo
            </span></Typography>
            {isLogged ? null : <CheckLogin></CheckLogin>}
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <div className="bg-white rounded px-4 py-4">
                    <Typography variant="h5" color="black">Báo cáo cửa hàng</Typography>
                    <div className="flex flex-col">
                        <Checkbox label="Món ăn không đúng với mô tả" value="Món ăn không đúng với mô tả" onChange={handleCheckboxChange} />
                        <Checkbox label="Đồ ăn không đảm bảo vệ sinh" value="Đồ ăn không đảm bảo vệ sinh" onChange={handleCheckboxChange} />
                        <Checkbox label="Cửa hàng không phản hồi tin nhắn" value="Cửa hàng không phản hồi tin nhắn" onChange={handleCheckboxChange} />
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