import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";
import { Button, Dialog, DialogBody, DialogFooter, DialogHeader, Rating, Textarea, Typography } from '@material-tailwind/react';
import propTypes from 'prop-types';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import ErrorText from '../../../shared/components/text/ErrorText';
import { toast } from 'react-toastify';
import axios from "../../../shared/api/axiosConfig";
import UploadImage from '../../../shared/components/form/UploadImage';
import { useNavigate } from 'react-router-dom';

const schema = yup
    .object({
        reviewfood: yup
            .string()
            .required("Hãy ghi phản hồi về món ăn của bạn!"),
        imageURL: yup.string().nullable(),
    });

const ReviewFood = ({ idUser, idFood }) => {
    const navigate = useNavigate();
    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors },
    } = useForm({
        resolver: yupResolver(schema),
        mode: "onSubmit",
    });
    const isReviewedStored = localStorage.getItem(`foodReview_${idFood}`); // Check if the food has been reviewed
    const [star, setStar] = useState(5);
    const [open, setOpen] = useState(false);
    const [openThank, setOpenThank] = useState(false);
    const [isReviewed, setIsReviewed] = useState(!!isReviewedStored); // Initialize with the stored value

    const handleOpen = () => setOpen((cur) => !cur);
    const handleOpenThank = () => setOpenThank((cur) => !cur);

    const onSubmit = async (data) => {
        try {
            const newRating = {
                userId: idUser,
                foodId: idFood,
                rate: star,
                content: data.reviewfood,
                images: [
                    {
                        url: data.imageURL,
                        commentId: 0
                    }
                ]
            };
            axios
                .post(`/api/Food/RatingFood`, newRating)
                .then(() => {
                    setOpen(false);
                    setOpenThank(true);
                    setIsReviewed(true);
                    localStorage.setItem(`foodReview_${idFood}`, true); // Store the information in local storage
                })
                .catch((error) => {
                    toast.error("Đánh giá thất bại!");
                    console.log(error);
                });
        } catch (error) {
            console.error("Error add rating: ", error);
        }
    }

    return (
        <div>
            <Button className={`bg-primary cursor-pointer hover:bg-orange-700 ${isReviewed ? 'hidden' : ''}`} onClick={handleOpen}>
                Đánh giá món ăn
            </Button>
            <Button
                className={`bg-primary cursor-pointer hover:bg-orange-700 ${!isReviewed ? 'hidden' : ''}`}
                onClick={() => {
                    // Redirect to the Review Page using React Router
                    navigate(`/food-details/${idFood}`); // Replace `idStore` with the appropriate variable
                }}
            >
                Xem đánh giá
            </Button>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form onSubmit={handleSubmit(onSubmit)} className='bg-white p-4 mb-4 rounded'>
                    <Typography variant='h5'>ĐÁNH GIÁ MÓN ĂN</Typography>
                    <hr className="h-px my-2 bg-gray-200 border-0 dark:bg-gray-700" />
                    <Typography variant='h6' className='py-2'>Đánh giá</Typography>
                    <div className='flex gap-2 items-center'>
                        <Rating value={star} onChange={(value) => setStar(value)}></Rating>
                        <Typography variant='paragraph' className='font-semibold'>{star} Sao</Typography>
                    </div>
                    <Typography variant='h6' className='py-2'>Phản hồi về món ăn</Typography>
                    <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        size="md"
                        label="Phản hồi của bạn về món ăn"
                        {...register("reviewfood")}
                    ></Textarea>
                    {errors.reviewfood && (
                        <ErrorText text={errors.reviewfood.message}></ErrorText>
                    )}
                    <Typography variant='h6' className='py-2'>Ảnh phản hồi của bạn về món ăn (Không bắt buộc)</Typography>
                    <UploadImage name="imageURL" onChange={setValue}></UploadImage>
                    <DialogFooter>
                        <Button
                            variant="text"
                            color="red"
                            onClick={handleOpen}
                            className="mr-1"
                        >
                            <span>Hủy</span>
                        </Button>
                        <Button onClick={handleOpenThank} type='submit' className='w-fit bg-primary'>ĐÁNH GIÁ</Button>
                    </DialogFooter>
                </form>
            </Dialog>
            <Dialog
                size="sm"
                open={openThank}
                handler={handleOpenThank}
                className="bg-transparent shadow-none"
            >
                <div className='bg-white p-1 mb-4 rounded-xl shadow-lg ring ring-indigo-600/50'>
                    <DialogHeader className='text-green-600 flex flex-col'>
                        <svg xmlns="http://www.w3.org/2000/svg" className="text-green-600 w-28 h-28" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth="1">
                            <path strokeLinecap="round" strokeLinejoin="round" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                        </svg>
                        Đánh giá món ăn thành công
                    </DialogHeader>
                    <DialogBody className='font-semibold text-xl text-center'>
                        Cảm ơn bạn đã đánh giá món ăn
                    </DialogBody>
                    <DialogFooter>
                        <Button variant="gradient" color="green" onClick={handleOpenThank}>
                            <span>OK</span>
                        </Button>
                    </DialogFooter>
                </div>
            </Dialog>
        </div>
    );
};

ReviewFood.propTypes = {
    idUser: propTypes.any.isRequired,
    idFood: propTypes.any.isRequired
};

export default ReviewFood;