import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";
import { Button, Rating, Textarea, Typography } from '@material-tailwind/react';
import propTypes from 'prop-types';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import ErrorText from '../../../shared/components/text/ErrorText';
import { toast } from 'react-toastify';
import axios from "../../../shared/api/axiosConfig";

const schema = yup
    .object({
        review: yup
            .string()
            .required("Hãy ghi phản hồi của bạn!")
    });

const ReviewStore = ({ idUser, idStore }) => {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm({
        resolver: yupResolver(schema),
        mode: "onSubmit",
    });

    console.log(idUser + " " + idStore);
    const [star, setStar] = useState(5);

    const onSubmit = async (data) => {
        try {
            const newRating = {
                userId: idUser,
                storeId: idStore,
                rate: data.star,
                content: data.review
            };
            axios
                .get(`/api/Store/RatingStore`, newRating)
                .then(() => {
                    toast.success("Đánh giá thành công!");
                })
                .catch((error) => {
                    toast.success("Đánh giá thất bại!");
                    console.log(error);
                });
        } catch (error) {
            console.error("Error add rating: ", error);
        }
    }
    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <Typography variant='h5'>ĐÁNH GIÁ CỬA HÀNG</Typography>
                <hr className="h-px my-2 bg-gray-200 border-0 dark:bg-gray-700" />
                <Typography variant='h6' className='py-2'>Đánh giá</Typography>
                <div className='flex gap-2 items-center'>
                    <Rating value={star} onChange={(value) => setStar(value)}></Rating>
                    <Typography variant='paragraph' className='font-semibold'>{star} Sao</Typography>
                </div>
                <Typography variant='h6' className='py-2'>Phản hồi</Typography>
                <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                    size="md"
                    label="Phản hồi của bạn về cửa hàng"
                    {...register("review")}
                ></Textarea>
                {errors.review && (
                    <ErrorText text={errors.review.message}></ErrorText>
                )}
                <Button type='submit' className='w-fit bg-primary'>ĐÁNH GIÁ</Button>
            </form>

        </div>
    );
};

ReviewStore.propTypes = {
    idUser: propTypes.any.isRequired,
    idStore: propTypes.any.isRequired
};

export default ReviewStore;