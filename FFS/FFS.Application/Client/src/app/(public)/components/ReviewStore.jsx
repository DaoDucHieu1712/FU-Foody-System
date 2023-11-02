import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from "yup";
import { Button, Dialog, DialogBody, DialogFooter, DialogHeader, Rating, Textarea, Typography } from '@material-tailwind/react';
import propTypes from 'prop-types';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import ErrorText from '../../../shared/components/text/ErrorText';
import { toast } from 'react-toastify';
import axios from "../../../shared/api/axiosConfig";

const schema = yup
    .object({
        reviewstore: yup
            .string()
            .required("Hãy ghi phản hồi về cửa hàng của bạn!"),
        reviewshipper: yup
            .string()
            .required("Hãy ghi phản hồi về shipper của bạn!")
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

    const [star, setStar] = useState(5);
    const [open, setOpen] = useState(false);
    const [openThank, setOpenThank] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);
    const handleOpenThank = () => setOpenThank((cur) => !cur);

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
                    setOpen(false);
                    setOpenThank(true);
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
            <Button className="bg-primary cursor-pointer hover:bg-orange-700" onClick={handleOpen}>Đánh giá cửa hàng</Button>
            <Dialog
                size="md"
                open={open}
                handler={handleOpen}
                className="bg-transparent shadow-none"
            >
                <form onSubmit={handleSubmit(onSubmit)} className='bg-white p-4 mb-4 rounded'>
                    <Typography variant='h5'>ĐÁNH GIÁ CỬA HÀNG</Typography>
                    <hr className="h-px my-2 bg-gray-200 border-0 dark:bg-gray-700" />
                    <Typography variant='h6' className='py-2'>Đánh giá</Typography>
                    <div className='flex gap-2 items-center'>
                        <Rating value={star} onChange={(value) => setStar(value)}></Rating>
                        <Typography variant='paragraph' className='font-semibold'>{star} Sao</Typography>
                    </div>
                    <Typography variant='h6' className='py-2'>Phản hồi về cửa hàng</Typography>
                    <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        size="md"
                        label="Phản hồi của bạn về cửa hàng"
                        {...register("reviewstore")}
                    ></Textarea>
                    {errors.reviewstore && (
                        <ErrorText text={errors.reviewstore.message}></ErrorText>
                    )}
                    <Typography variant='h6' className='py-2'>Phản hồi về shipper</Typography>
                    <Textarea className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        size="md"
                        label="Phản hồi của bạn về shipper"
                        {...register("reviewshipper")}
                    ></Textarea>
                    {errors.reviewshipper && (
                        <ErrorText text={errors.reviewshipper.message}></ErrorText>
                    )}
                    <DialogFooter>
                        <Button
                            variant="text"
                            color="red"
                            onClick={handleOpen}
                            className="mr-1"
                        >
                            <span>Hủy</span>
                        </Button>
                        <Button type='submit' className='w-fit bg-primary'>ĐÁNH GIÁ</Button>
                    </DialogFooter>
                </form>
            </Dialog>
            <Dialog
                size="sm"
                open={openThank}
                handler={handleOpenThank}
                className="bg-transparent shadow-none"
            >
                <div className='bg-white p-1 mb-4 rounded-xl'>
                    <DialogHeader className='text-green-600'>Đánh giá cửa hàng thành công</DialogHeader>
                    <DialogBody className='font-semibold text-xl'>
                        Cảm ơn bạn đã đánh giá cửa hàng
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

ReviewStore.propTypes = {
    idUser: propTypes.any.isRequired,
    idStore: propTypes.any.isRequired
};

export default ReviewStore;