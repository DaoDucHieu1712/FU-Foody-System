import { yupResolver } from "@hookform/resolvers/yup";
import {
    Button,
    Dialog,
    Input,
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
    percent: yup.number().max(100, "Giá trị tối đa là 100%!").typeError("Hãy nhập phần trăm giảm giá!").positive("Giá trị phải lớn hơn 0").required("Hãy nhập phần trăm giảm giá!"),
    price: yup.number().positive("Giá trị phải lớn hơn 0").typeError("Hãy nhập giá trị đơn hàng tối thiếu!").required("Hãy nhập giá trị đơn hàng tối thiếu!"),
    quantity: yup.number().positive("Giá trị phải lớn hơn 0").typeError("Hãy nhập số lượng!").required("Hãy nhập số lượng!"),
    date: yup.date().typeError("Hãy nhập ngày hết hạn!").min(new Date(), 'Thời gian hết hạn phải lớn hơn thời gian hiện tại').required("Hãy nhập ngày hết hạn")
});

const AddDiscount = ({ reload, storeId }) => {
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
            const dataPost = {
                code: data.code,
                percent: data.percent,
                conditionPrice: data.price,
                quantity: data.quantity,
                expired: data.date,
                storeId: storeId,
            };
            axios
                .post("/api/Discount/CreateDiscount", dataPost)
                .then(() => {
                    toast.success("Thêm ưu đãi mới thành công!");
                    reload();
                    setOpen(false);
                })
                .catch(() => {
                    toast.error("Thêm ưu đãi mới thất bại!");
                    setOpen(false);
                });
        } catch (error) {
            console.error("Error occur: ", error);
        }
    };

    return (
        <div>
            <Button
                className=" text-white text-center font-bold bg-primary cursor-pointer hover:bg-orange-900"
                onClick={handleOpen}
            >
                Thêm ưu đãi
            </Button>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form
                    className="form bg-white rounded px-4 py-4"
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <p className="font-bold text-2xl text-center mb-4">Thêm ưu đãi mới</p>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="text"
                            label="Mã ưu đãi"
                            {...register("code")}
                        ></Input>
                        {errors.code && <ErrorText text={errors.code.message}></ErrorText>}
                    </div>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            type="number"
                            label="Phần trăm giảm giá (%)"
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
                            label="Áp dụng giá trị đơn hàng (nghìn VND)"
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
                        Thêm
                    </button>
                </form>
            </Dialog>
        </div>
    );
};
AddDiscount.propTypes = {
    reload: propTypes.any.isRequired,
    storeId: propTypes.any.isRequired,
};
export default AddDiscount;
