import { yupResolver } from "@hookform/resolvers/yup";
import {
  Button,
  Dialog,
  DialogBody,
  DialogFooter,
  DialogHeader,
  Input,
  Typography,
} from "@material-tailwind/react";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as yup from "yup";
import axios from "../../../../../shared/api/axiosConfig";
import UploadImage from "../../../../../shared/components/form/UploadImage";
import ErrorText from "../../../../../shared/components/text/ErrorText";
import CookieService from "../../../../../shared/helper/cookieConfig";
import Editor from "./Editor";

const schema = yup.object({
  title: yup.string().required("Hãy viết tiêu đề của bạn!"),
  // content: yup.string().required("Hãy viết nội dung của bạn!"),
  image: yup.string().required("Hãy thêm ảnh!"),
});

const AddPost = ({ reloadPost }) => {
  const [open, setOpen] = useState(false);
  const handleOpen = () => setOpen(!open);
  const [editorContent, setEditorContent] = useState("");
  const userId = CookieService.getToken("fu_foody_id");
  const {
    register,
    setValue,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
    mode: "onSubmit",
  });

  const handleEditorChange = (content) => {
    setEditorContent(content);
  };
  const onSubmit = async (data) => {
    try {
      const newPost = {
        title: data.title,
        content: editorContent,
        image: data.image,
        userId: userId,
      };

      axios
        .post("/api/Post/CreatePost", newPost)
        .then(() => {
          toast.success("Thêm bài viết mới thành công!");
          reloadPost();
          setOpen(false);
        })
        .catch(() => {
          toast.error("Thêm bài viết mới thất bại!");
          setOpen(false);
        });
    } catch (error) {
      console.error("Error add post: ", error);
    }
  };

  return (
    <>
      <button
        onClick={handleOpen}
        className="text-white bg-primary hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-5 py-2.5 text-center"
      >
        + Thêm bài viết mới
      </button>
      <Dialog open={open} size="lg" handler={handleOpen}>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="flex items-center justify-between">
            <DialogHeader className="flex flex-col items-start">
              <Typography className="mb-1" variant="h4">
                Đăng bài viết
              </Typography>
            </DialogHeader>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              fill="currentColor"
              className="mr-3 h-5 w-5"
              onClick={handleOpen}
              color="black"
            >
              <path
                fillRule="evenodd"
                d="M5.47 5.47a.75.75 0 011.06 0L12 10.94l5.47-5.47a.75.75 0 111.06 1.06L13.06 12l5.47 5.47a.75.75 0 11-1.06 1.06L12 13.06l-5.47 5.47a.75.75 0 01-1.06-1.06L10.94 12 5.47 6.53a.75.75 0 010-1.06z"
                clipRule="evenodd"
              />
            </svg>
          </div>
          <DialogBody className="h-[30rem] overflow-scroll">
            <div className="w-full mb-4">
              <Input
                label="Tiêu đề của bạn"
                type="text"
                {...register("title")}
                className="rounded-none"
              />
              {errors.title && (
                <ErrorText text={errors.title.message}></ErrorText>
              )}
            </div>
            <Editor value={editorContent} onChange={handleEditorChange} />
            {errors.content && (
              <ErrorText text={errors.content.message}></ErrorText>
            )}
            <UploadImage name="image" onChange={setValue}></UploadImage>
          </DialogBody>
          <DialogFooter className="space-x-2">
            <Button variant="text" color="deep-orange" onClick={handleOpen}>
              cancel
            </Button>
            <Button
              className="rounded-none"
              variant="gradient"
              color="deep-orange"
              type="submit"
            >
              TẠO MỚI
            </Button>
          </DialogFooter>
        </form>
      </Dialog>
    </>
  );
};

export default AddPost;
