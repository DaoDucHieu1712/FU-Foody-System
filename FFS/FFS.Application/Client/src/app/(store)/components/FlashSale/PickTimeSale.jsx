import { yupResolver } from "@hookform/resolvers/yup";
import { Button, Dialog, Input, Typography } from "@material-tailwind/react";
import { useState } from "react";
import { useForm } from "react-hook-form";
import * as yup from "yup";
import ErrorText from "../../../../shared/components/text/ErrorText";
import propTypes from "prop-types";

const schema = yup.object({
    dateStart: yup.date().typeError("Hãy nhập ngày bắt đầu!").min(new Date(), 'Thời gian bắt đầu phải lớn hơn thời gian hiện tại!').max(yup.ref('dateEnd'), 'Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc!').required("Hãy nhập ngày bắt đầu!"),
    dateEnd: yup.date().typeError("Hãy nhập ngày kết thúc!").min(new Date(), 'Thời gian kết thúc phải lớn hơn thời gian hiện tại').min(yup.ref('dateStart'), 'Thời gian kết thúc phải lớn hơn thời gian bắt đầu!').required("Hãy nhập ngày kết thúc!")
});

const PickTimeSale = ({ getDate }) => {
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

    const onSubmit = (data) => {
        getDate(data.dateStart.toISOString(), data.dateEnd.toISOString());
        setOpen(false);
    };

    return (
        <div>
            <div className="flex gap-3 items-center">
                <Typography className="font-semibold">Lựa chọn khung thời gian:</Typography>
                <Button color="white" className="flex gap-2 text-orange-500 border-solid border-2 border-orange-500" onClick={handleOpen}>
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        height="1.2em"
                        viewBox="0 0 448 512"
                    >
                        <path
                            d="M152 24c0-13.3-10.7-24-24-24s-24 10.7-24 24V64H64C28.7 64 0 92.7 0 128v16 48V448c0 35.3 28.7 64 64 64H384c35.3 0 64-28.7 64-64V192 144 128c0-35.3-28.7-64-64-64H344V24c0-13.3-10.7-24-24-24s-24 10.7-24 24V64H152V24zM48 192h80v56H48V192zm0 104h80v64H48V296zm128 0h96v64H176V296zm144 0h80v64H320V296zm80-48H320V192h80v56zm0 160v40c0 8.8-7.2 16-16 16H320V408h80zm-128 0v56H176V408h96zm-144 0v56H64c-8.8 0-16-7.2-16-16V408h80zM272 248H176V192h96v56z"
                            fill="orange"
                        />
                    </svg>
                    Lựa chọn khung giờ
                </Button>
            </div>
            <Dialog
                size="sm"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form
                    className="form bg-white rounded-3xl p-6"
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <p className="font-bold text-2xl text-center mb-8">Chọn khung giờ Flash Sale của Shop</p>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="datetime-local"
                            label="Ngày bắt đầu"
                            {...register("dateStart")}
                        ></Input>
                        {errors.dateStart && (
                            <ErrorText text={errors.dateStart.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Input
                            className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="datetime-local"
                            label="Ngày kết thúc"
                            {...register("dateEnd")}
                        ></Input>
                        {errors.dateEnd && (
                            <ErrorText text={errors.dateEnd.message}></ErrorText>
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

PickTimeSale.propTypes = {
    getDate: propTypes.any.isRequired
}

export default PickTimeSale;