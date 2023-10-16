import { useState } from 'react';
import propTypes from "prop-types";
import { Dialog, Textarea } from "@material-tailwind/react";
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";
import ErrorText from '../../../../shared/components/text/ErrorText';
import axios from 'axios';
import { toast } from 'react-toastify';

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

const UpdateLocation = ({ item , reload, wardList}) => {
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
    console.log(item);
    console.log(wardList);

    const onSubmit = async (data) => {
        try {
                const newLocation = {
                    address: data.address + "-" + data.ward + "-Thạch Thất-Hà Nội",
                    description: data.description || null,
                    receiver: data.receiver,
                    phoneNumber: data.phoneNumber
                };
            const response = await axios.put(`https://localhost:7025/api/Location/UpdateLocation/${data.id}`, newLocation);
                if (response.status == 200) {
                    toast.success("Cập nhật địa chỉ thành công!");
                    reload;
                }
            } catch (error) {
                console.error("Error update location: ", error);
            }
    }

    return (
        <>
            <p className="text-blue-500 font-semibold cursor-pointer hover:underline hover:text-blue-600" onClick={handleOpen}>Cập nhật</p>
            <Dialog
                size="xs"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form className="bg-white rounded px-40 py-20 mb-4" onSubmit={handleSubmit(onSubmit)}>
                    <p className="font-bold text-2xl text-center mb-4">Chỉnh sửa địa chỉ</p>
                    <div className="mb-4">
                        <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Thành phố Hà Nội" readOnly></input>
                    </div>
                    <div className="mb-4">
                        <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Thành phố Hà Nội" readOnly></input>
                    </div>
                    <div className="mb-4">
                        <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Thành phố Hà Nội" readOnly></input>
                    </div>
                    <div className="mb-4">
                        <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Huyện Thạch Thất" readOnly></input>
                    </div>
                    <div className="inline-block relative mb-4">
                        <select
                            className="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-tight focus:outline-none focus:shadow-outline"
                            {...register("ward")}
                            onChange={(e) => setValue('ward', e.target.value)}
                        >
                            <option value="">Chọn xã</option>
                            {wardList.map((ward) => (
                                <option
                                    key={ward.code}
                                    value={ward.name}
                                >
                                    {ward.name}
                                </option>
                            ))}
                        </select>
                        {errors.select && <ErrorText text={errors.ward.message}></ErrorText>}
                        <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                            <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" /></svg>
                        </div>
                    </div>
                    <div className="mb-4">
                        <textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            placeholder="Địa chỉ cụ thể...(Trọ, Thôn,...)"
                            type="text"
                            onChange={(e) => setValue('address', e.target.value)}
                            // defaultValue={updateData.address}
                            {...register("address")}
                        ></textarea>
                        {errors.address && (
                            <ErrorText text={errors.address.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            size="md" 
                            label="Textarea Medium"
                            type="text"
                            onChange={(e) => setValue('description', e.target.value)}
                        // value={updateData.description}
                        // {...register("description")}
                        ></Textarea>
                        {/* {errors.description && (
                        <ErrorText text={errors.description.message}></ErrorText>
                    )} */}
                    </div>
                    <button type="submit" className="text-white bg-orange-500 hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Submit</button>
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