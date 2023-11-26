import { Dialog, Typography } from "@material-tailwind/react";
import propTypes from "prop-types";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const ViewLikePost = ({ likeNumber, likedBy }) => {
    const navigate = useNavigate();
    const [isHovered, setIsHovered] = useState(false);
    const [open, setOpen] = useState(false);
    const handleOpen = () => setOpen((cur) => !cur);

    const handleMouseOver = () => {
        setIsHovered(true);
    };

    const handleMouseLeave = () => {
        setIsHovered(false);
    };

    return (
        <>
            <span
                className="relative ml-2 text-gray-500 cursor-pointer"
                onMouseOver={handleMouseOver}
                onMouseLeave={handleMouseLeave}
                onClick={handleOpen}
            >
                {likeNumber} lượt thích
                {isHovered && (
                    <div className="absolute w-max max-h-40 bg-white p-3 rounded shadow-md mt-1">
                        <ul>
                            {likedBy ?
                                (likedBy
                                    .filter((user) => user.isLike === true)
                                    .slice(0, 5)
                                    .map((user) => (
                                        <li key={user.userId}>{user.username}</li>
                                    )))
                                : null
                            }
                            {likedBy && likedBy.filter((user) => user.isLike === true).length > 5 && (
                                <li>và {likedBy.filter((user) => user.isLike === true).length - 5} người khác ...</li>
                            )}
                        </ul>
                    </div>
                )}
            </span>
            <Dialog
                size="xs"
                open={open}
                handler={handleOpen}
                className="max-h-96 bg-transparent shadow-none overflow-y-auto"
            >
                <div className="bg-white p-5">
                    {likedBy ?
                        (likedBy
                            .filter((user) => user.isLike === true)
                            .slice(0, 9)
                            .map((user) => (
                                <li className="flex justify-between items-center p-1" key={user.userId}>
                                    <div className="flex gap-2 items-center">
                                        <img
                                            src={user.avartar}
                                            alt={user.username}
                                            className="h-10 w-10 object-cover rounded-full"
                                        />
                                        {user.username}
                                    </div>
                                    <Typography variant="h6" onClick={() => navigate(`/user-detail/${user.userId}`)} className="py-1 px-2 rounded bg-primary text-white cursor-pointer">Xem thông tin</Typography>
                                </li>
                            )))
                        : null
                    }
                </div>
            </Dialog>
        </>
    );
};

ViewLikePost.propTypes = {
    likeNumber: propTypes.any.isRequired,
    likedBy: propTypes.any.isRequired,
}

export default ViewLikePost;