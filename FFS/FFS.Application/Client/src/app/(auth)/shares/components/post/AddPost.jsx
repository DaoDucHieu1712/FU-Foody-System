import React, { useState } from "react";
import {
  Button,
  Dialog,
  DialogHeader,
  DialogBody,
  DialogFooter,
  Input,
  Typography,
} from "@material-tailwind/react";
import Editor from "./Editor";

const AddPost = () => {
  const [open, setOpen] = useState(false);
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");

  const handleOpen = () => setOpen(!open);

  const handleSubmit = (e) => {
    e.preventDefault();
    // Handle submission logic here
    // Example: Send title and content to server
    console.log("Title:", title);
    console.log("Content:", content);
    handleOpen(); // Close the dialog after submission
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
        <form onSubmit={handleSubmit}>
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
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                className="rounded-none"
              />
            </div>
            <Editor value={content} onChange={setContent} />
          </DialogBody>
          <DialogFooter className="space-x-2">
            <Button variant="text" color="deep-orange" onClick={handleOpen}>
              cancel
            </Button>
            <Button className="rounded-none" variant="gradient" color="deep-orange" type="submit">
              TẠO MỚI
            </Button>
          </DialogFooter>
        </form>
      </Dialog>
    </>
  );
};

export default AddPost;
