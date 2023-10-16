import { Button, Input } from "@material-tailwind/react";

const ChangePasswordPage = () => {
  return (
    <>
      <div className="flex justify-center items-center my-10">
        <form className="shadow-xl p-10 w-[500px]">
          <div className="heading">
            <h1 className="font-semibold text-xl uppercase text-center mb-8">
              Đổi mật khẩu
            </h1>
            <div className="flex flex-col gap-y-3">
              <div className="flex flex-col gap-y-2">
                <span>Email</span>
                <Input label="Email" />
              </div>
              <div className="flex flex-col gap-y-2">
                <span>Mật khẩu cũ</span>
                <Input type="password" label="Mật khẩu cũ" />
              </div>
              <div className="flex flex-col gap-y-2">
                <span>Mật khẩu mới</span>
                <Input type="password" label="Mật khẩu mới" />
              </div>
              <div className="flex flex-col gap-y-2">
                <span>Xác nhận mật khẩu mới</span>
                <Input type="password" label="Xác nhận mật khẩu mới" />
              </div>
              <div className="flex flex-col gap-y-2">
                <Button className="bg-primary w-full">Cập nhật</Button>
              </div>
            </div>
          </div>
        </form>
      </div>
    </>
  );
};

export default ChangePasswordPage;
