import { useState } from 'react';
import propTypes from "prop-types";
import { Dialog, Input, Option, Select, Textarea } from "@material-tailwind/react";
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";
import ErrorText from '../../../../shared/components/text/ErrorText';
import axios from "../../../../shared/api/axiosConfig";
import { toast } from 'react-toastify';
import Cookies from "universal-cookie";

const cookies = new Cookies();

const regexPhoneNumber = /(0[3|5|7|8|9])+([0-9]{8})\b/g;
const schema = yup
    .object({
        ward: yup
            .string()
            .required("Hãy chọn xã!"),
        address: yup
            .string()
            .required("Hãy ghi thêm thông tin!"),
        description: yup
            .string()
            .nullable(),
        phoneNumber: yup
            .string()
            .matches(regexPhoneNumber, "Vui lòng nhập đúng định dạng số điện thoại!")
            .required("Hãy nhập số điện thoại!"),
        receiver: yup
            .string()
            .required("Hãy nhập tên người nhận!")
    });

const UpdateLocation = ({ item, reload, wardList }) => {
    const {
        register,
        setValue,
        handleSubmit,
        formState: { errors },
    } = useForm({
        resolver: yupResolver(schema),
        mode: "onSubmit",
    });

    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

    const trueAddress = item.address.split('-');

    const onSubmit = async (data) => {
        var email = cookies.get("fu_foody_email");
        alert("Hello");
        try {
            const newLocation = {
                email: email,
                address: data.address + "-" + data.ward + "-Thạch Thất-Hà Nội",
                description: data.description || null,
                receiver: data.receiver,
                phoneNumber: data.phoneNumber
            };
            axios
                .put(`/api/Location/UpdateLocation/${item.id}`, newLocation)
                .then(() => {
                    toast.success("Cập nhật địa chỉ thành công!");
                    reload();
                    setOpen(false);
                })
                .catch((error) => {
                    toast.error("Xóa địa chỉ thất bại!");
                    setOpen(false);
                    console.log(error);
                });
        } catch (error) {
            console.error("Error update location: ", error);
        }
    }

    return (
        <>
            <p className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600" onClick={handleOpen}>Cập nhật</p>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form className="bg-white rounded px-4 py-4 mb-4" onSubmit={handleSubmit(onSubmit)}>
                    <p className="font-bold text-2xl text-center mb-4">Chỉnh sửa địa chỉ</p>
                    <p></p>
                    <div className="mb-4">
                        <Input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="text"
                            label="Người nhận"
                            defaultValue={item.receiver}
                            {...register("receiver")}></Input>
                        {errors.receiver && (
                            <ErrorText text={errors.receiver.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="text"
                            label="Số điện thoại"
                            defaultValue={item.phoneNumber}
                            {...register("phoneNumber")}></Input>
                        {errors.phoneNumber && (
                            <ErrorText text={errors.phoneNumber.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none"
                            type="text"
                            placeholder="Thành phố Hà Nội"></input>
                    </div>
                    <div className="mb-4">
                        <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Huyện Thạch Thất" readOnly></input>
                    </div>
                    <div className="inline-block relative mb-4">
                        <Select
                            className="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-tight focus:outline-none focus:shadow-outline"
                            {...register("ward")}
                            onChange={(e) => setValue('ward', e)}
                            label='Chọn xã'
                            value={trueAddress[1]}
                        >
                            {wardList.map((ward) => (
                                <Option
                                    key={ward.code}
                                    value={ward.name}
                                >
                                    {ward.name}
                                </Option>
                            ))}
                        </Select>
                        {errors.select && <ErrorText text={errors.ward.message}></ErrorText>}
                    </div>
                    <div className="mb-4">
                        <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            size="md"
                            label="Địa chỉ cụ thể (Trọ, Thôn)"
                            defaultValue={trueAddress[0]}
                            {...register("address")}></Textarea>
                        {errors.address && (
                            <ErrorText text={errors.address.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            size="md"
                            label="Thông tin ghi chú thêm"
                            defaultValue={item.description}
                            {...register("description")}></Textarea>
                        {errors.description && (
                            <ErrorText text={errors.description.message}></ErrorText>
                        )}
                    </div>
                    <button type="submit" className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Cập nhật địa chỉ</button>
                </form>
            </Dialog>
        </>
    );
};
UpdateLocation.propTypes = {
    item: propTypes.any.isRequired,
    reload: propTypes.any.isRequired,
    wardList: propTypes.any.isRequired,
};
export default UpdateLocation;