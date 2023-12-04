import { yupResolver } from "@hookform/resolvers/yup";
import {
    Dialog,
    IconButton,
    Input,
    Textarea,
    Tooltip,
} from "@material-tailwind/react";
import * as yup from "yup";
import propTypes from "prop-types";
import { useForm } from "react-hook-form";
import { useState } from "react";
import axios from "../../../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import ErrorText from "../../../../shared/components/text/ErrorText";

const schema = yup.object({
    code: yup.string().required("Hãy ghi mã ưu đãi!"),
    desc: yup.string().required("Hãy nhập mô tả chi tiết mã giảm giá!"),
    percent: yup.number().max(100, "Giá trị tối đa là 100%!").typeError("Hãy nhập phần trăm giảm giá!").positive("Giá trị phải lớn hơn 0").required("Hãy nhập phần trăm giảm giá!"),
    price: yup.number().positive("Giá trị phải lớn hơn 0").typeError("Hãy nhập giá trị đơn hàng tối thiếu!").required("Hãy nhập giá trị đơn hàng tối thiếu!"),
    quantity: yup.number().positive("Giá trị phải lớn hơn 0").typeError("Hãy nhập số lượng!").required("Hãy nhập số lượng!"),
    date: yup.date().min(new Date(), 'Thời gian hết hạn phải lớn hơn thời gian hiện tại').required("Hãy nhập ngày hết hạn")
});

const UpdateDiscount = ({ reload, discountData }) => {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm({
        resolver: yupResolver(schema),
        mode: "onSubmit",
    });

    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

    const onSubmit = async (data) => {
        try {
            const newDiscount = {
                id: discountData.id,
                code: data.code,
                description: data.desc,
                percent: data.percent,
                conditionPrice: data.price,
                quantity: data.quantity,
                expired: data.date
            };
            axios
                .put(`/api/Discount/UpdateDiscount?id=${discountData.id}`, newDiscount)
                .then(() => {
                    toast.success("Cập nhật ưu đãi mới thành công!");
                    reload();
                    setOpen(false);
                })
                .catch((error) => {
                    if (error.response && error.response.status === 400) {
                        // Handle the specific error when the discount code already exists
                        toast.error(error.response.data);
                    } else {
                        // Handle other errors
                        toast.error("Cập nhật ưu đãi mới thất bại!");
                    }
                    setOpen(false);
                });
        } catch (error) {
            console.error("Error occur: ", error);
        }
    };

    return (
        <div>
            <Tooltip content="Cập nhật ưu đãi">
                <IconButton variant="text" onClick={handleOpen}>
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="20"
                        height="20"
                        viewBox="0 0 512 512"
                    >
                        <path d="M471.6 21.7c-21.9-21.9-57.3-21.9-79.2 0L362.3 51.7l97.9 97.9 30.1-30.1c21.9-21.9 21.9-57.3 0-79.2L471.6 21.7zm-299.2 220c-6.1 6.1-10.8 13.6-13.5 21.9l-29.6 88.8c-2.9 8.6-.6 18.1 5.8 24.6s15.9 8.7 24.6 5.8l88.8-29.6c8.2-2.7 15.7-7.4 21.9-13.5L437.7 172.3 339.7 74.3 172.4 241.7zM96 64C43 64 0 107 0 160V416c0 53 43 96 96 96H352c53 0 96-43 96-96V320c0-17.7-14.3-32-32-32s-32 14.3-32 32v96c0 17.7-14.3 32-32 32H96c-17.7 0-32-14.3-32-32V160c0-17.7 14.3-32 32-32h96c17.7 0 32-14.3 32-32s-14.3-32-32-32H96z" />
                    </svg>
                </IconButton>
            </Tooltip>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form
                    className="form bg-white rounded px-4 py-4 mb-4"
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <p className="font-bold text-2xl text-center mb-4">Sửa ưu đãi</p>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="text"
                            defaultValue={discountData.code}
                            label="Mã ưu đãi"
                            {...register("code")}
                        ></Input>
                        {errors.code && <ErrorText text={errors.code.message}></ErrorText>}
                    </div>
                    <div className="mb-4">
                        <Textarea
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="text"
                            label="Mô tả"
                            {...register("desc")}
                        ></Textarea>
                        {errors.desc && <ErrorText text={errors.desc.message}></ErrorText>}
                    </div>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            type="number"
                            defaultValue={discountData.percent}
                            label="Phần trăm giảm giá"
                            {...register("percent")}
                        ></Input>
                        {errors.percent && (
                            <ErrorText text={errors.percent.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="number"
                            defaultValue={discountData.conditionPrice}
                            label="Giá trị đơn hàng"
                            {...register("price")}
                        ></Input>
                        {errors.price && (
                            <ErrorText text={errors.price.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="number"
                            defaultValue={discountData.quantity}
                            label="Số lượng"
                            {...register("quantity")}
                        ></Input>
                        {errors.quantity && (
                            <ErrorText text={errors.quantity.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="datetime-local"
                            defaultValue={discountData.expired}
                            label="Ngày hết hạn"
                            {...register("date")}
                        ></Input>
                        {errors.date && (
                            <ErrorText text={errors.date.message}></ErrorText>
                        )}
                    </div>
                    <button
                        type="submit"
                        className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center"
                    >
                        Submit
                    </button>
                </form>
            </Dialog>
        </div>
    );
};

UpdateDiscount.propTypes = {
    reload: propTypes.any.isRequired,
    discountData: propTypes.any.isRequired,
};

export default UpdateDiscount;
