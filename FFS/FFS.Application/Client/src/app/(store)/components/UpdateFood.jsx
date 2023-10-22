import { yupResolver } from "@hookform/resolvers/yup";
import { Dialog, IconButton, Input, Option, Select, Textarea, Tooltip } from "@material-tailwind/react";
import * as yup from "yup";
import propTypes from "prop-types";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import axios from "axios";
import { toast } from "react-toastify";
import ErrorText from "../../../shared/components/text/ErrorText";

const schema = yup
    .object({
        category: yup
            .number()
            .required("Hãy chọn loại!"),
        name: yup
            .string()
            .required("Hãy ghi tên món ăn!"),
        description: yup
            .string()
            .required("Hãy ghi mô tả món ăn!"),
        price: yup
            .number()
            .positive("Giá tiền phải lớn hơn 0")
            .required("Hãy nhập giá món ăn!")
    });

const UpdateFood = ({ reload, foodData }) => {
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
    const [category, setCategory] = useState([]);
    const handleOpen = () => setOpen((cur) => !cur);

    const ListCaegory = async () => {
        try {
            const response = await axios.get('https://localhost:7025/api/Category/ListCategory');
            const categories = response.data || [];
            setCategory(categories.data.result);
        } catch (error) {
            console.error("Category: " + error);
        }
    }

    useEffect(() => {
        ListCaegory();
    }, []);

    const onSubmit = async (data) => {
        try {
            const newFood = {
                foodName: data.name,
                description: data.description,
                price: data.price,
                categoryId: data.category
            };
            const response = await axios.post(`https://localhost:7025/api/Food/UpdateFood/${foodData.id}`, newFood);
            if (response.status == 200) {
                toast.success("Cập nhật món ăn thành công!");
                reload();
                setOpen(false);
            }
        } catch (error) {
            console.error("Error update food: ", error);
        }
    }

    return (
        <div>
            <Tooltip content="Cập nhật món ăn">
                <IconButton variant="text" onClick={handleOpen}>
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="20"
                        height="20"
                        viewBox="0 0 512 512"
                    >
                        <path
                            d="M471.6 21.7c-21.9-21.9-57.3-21.9-79.2 0L362.3 51.7l97.9 97.9 30.1-30.1c21.9-21.9 21.9-57.3 0-79.2L471.6 21.7zm-299.2 220c-6.1 6.1-10.8 13.6-13.5 21.9l-29.6 88.8c-2.9 8.6-.6 18.1 5.8 24.6s15.9 8.7 24.6 5.8l88.8-29.6c8.2-2.7 15.7-7.4 21.9-13.5L437.7 172.3 339.7 74.3 172.4 241.7zM96 64C43 64 0 107 0 160V416c0 53 43 96 96 96H352c53 0 96-43 96-96V320c0-17.7-14.3-32-32-32s-32 14.3-32 32v96c0 17.7-14.3 32-32 32H96c-17.7 0-32-14.3-32-32V160c0-17.7 14.3-32 32-32h96c17.7 0 32-14.3 32-32s-14.3-32-32-32H96z"
                        />
                    </svg>
                </IconButton>
            </Tooltip>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form className="form bg-white rounded px-4 py-4 mb-4" onSubmit={handleSubmit(onSubmit)}>
                    <p className="font-bold text-2xl text-center mb-4">Thêm món ăn mới</p>
                    <div className="mb-4">
                        <Input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="text"
                            label="Tên món ăn"
                            defaultValue={foodData.foodName}
                            {...register("name")}></Input>
                        {errors.name && (
                            <ErrorText text={errors.name.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            size="md"
                            label="Mô tả món ăn"
                            defaultValue={foodData.description}
                            {...register("description")}></Textarea>
                        {errors.description && (
                            <ErrorText text={errors.description.message}></ErrorText>
                        )}
                    </div>
                    <div className="mb-4">
                        <Input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight"
                            type="number"
                            label="Giá tiền"
                            defaultValue={foodData.price}
                            {...register("price")}></Input>
                        {errors.price && (
                            <ErrorText text={errors.price.message}></ErrorText>
                        )}
                    </div>
                    <div className="inline-block relative mb-4">
                        <Select
                            className="block appearance-none w-full bg-white px-4 py-2 pr-8 shadow leading-tight focus:outline-none focus:shadow-outline"
                            {...register("category")}
                            onChange={(e) => setValue('category', e)}
                            label='Chọn loại'
                            value={foodData.categoryId}
                        >
                            {category.map((category) => (
                                <Option
                                    key={category.id}
                                    value={category.id}
                                >
                                    {category.categoryName}
                                </Option>
                            ))}
                        </Select>
                        {errors.category && (
                            <ErrorText text={errors.category.message}></ErrorText>
                        )}
                    </div>
                    <button type="submit" className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Submit</button>
                </form>
            </Dialog>
        </div>
    );
};

UpdateFood.propTypes = {
    reload: propTypes.any.isRequired,
    foodData: propTypes.any.isRequired
};

export default UpdateFood;