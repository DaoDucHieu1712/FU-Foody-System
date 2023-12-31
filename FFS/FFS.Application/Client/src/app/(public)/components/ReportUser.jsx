import { Checkbox, Dialog, Textarea, Typography, } from "@material-tailwind/react";
import { useState } from "react";
import axios from "../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import propTypes from "prop-types";
import { FaExclamationCircle } from "react-icons/fa";
import CheckLogin from "./CheckLogin";


const ReportUser = ({ uId, sId }) => {

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

    const handleOtherReasonChange = (event) => {
        setOtherReason(event.target.value);
    };

    const handleOtherReasonCheckboxChange = (event) => {
        setIsOtherReasonChecked(event.target.checked);
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
                reportType: 3,
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
                    <Typography variant="h5" color="black">Báo cáo người dùng</Typography>
                    <div className="flex flex-col">
                        <Checkbox label="Người dùng xúc phạm ngôn từ" value="Người dùng xúc phạm ngôn từ" onChange={handleCheckboxChange} />
                        <Checkbox label="Người dùng không nghe máy, nhận hàng" value="Người dùng không nghe máy, nhận hàng" onChange={handleCheckboxChange} />
                        <Checkbox label="Người dùng review sai sự thật" value="Người dùng review sai sự thật" onChange={handleCheckboxChange} />
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

ReportUser.propTypes = {
    uId: propTypes.any.isRequired,
    sId: propTypes.any.isRequired
};

export default ReportUser;