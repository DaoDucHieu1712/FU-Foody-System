import { Button, Input } from "@material-tailwind/react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import AuthServices from "../(public)/shared/auth.service";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import CookieService from "../../shared/helper/cookieConfig";
import { useDispatch } from "react-redux";
import { setAccessToken } from "../../redux/auth";
import { cartActions } from "./shared/cartSlice";
import { comboActions } from "./shared/comboSlice";
const schema = yup.object({
	email: yup
		.string()
		.email("Email không hợp lệ !")
		.required("Email không thể để trống !"),
	oldPassword: yup
		.string()
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
			"Mật khẩu phải chứa ít nhất 1 chữ thường , 1 chữ hoa và 1 ký tự đặc biệt"
		)
		.required("Vui lòng điền mật khẩu cũ !"),
	newPassword: yup
		.string()
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
			"Mật khẩu phải chứa ít nhất 1 chữ thường , 1 chữ hoa và 1 ký tự đặc biệt"
		)
		.required("Vui lòng điền mật khẩu mới!"),
	confirmPassword: yup
		.string()
		.matches(
			/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
			"Mật khẩu phải chứa ít nhất 1 chữ thường , 1 chữ hoa và 1 ký tự đặc biệt"
		)
		.required("Vui lòng điền xác nhận mật khẩu mới !"),
});

const ChangePasswordPage = () => {
	const navigate = useNavigate();
	const dispatch = useDispatch();
	const { register, handleSubmit } = useForm({
		resolver: yupResolver(schema),
		mode: "onChange",
	});

	const onSubmitHandler = async (data) => {
		console.log(data);
		AuthServices.changePassword(data)
			.then((res) => {
				console.log(res);
				toast.success("Đổi mật khẩu thành công !");
				CookieService.removeToken("fu_foody_token"); // Remove the user token
				CookieService.removeToken("fu_foody_role");
				CookieService.removeToken("fu_foody_id");
				CookieService.removeToken("fu_foody_email");
				dispatch(setAccessToken(null));
				dispatch(cartActions.clearCart());
				dispatch(comboActions.clearCart());
				window.location.href = "/login";
			})
			.catch((err) => {
				toast.error(err.response.data);
			});
	};

	return (
		<>
			<div className="">
				<form
					className="p-10 w-[500px]"
					onSubmit={handleSubmit(onSubmitHandler)}
				>
					<div className="heading">
						<h1 className="font-semibold text-xl uppercase mb-8 text-primary">
							Đổi mật khẩu
						</h1>
						<div className="flex flex-col gap-y-3">
							<div className="flex flex-col gap-y-2">
								<span>Email</span>
								<Input label="Email" {...register("email")} />
							</div>
							<div className="flex flex-col gap-y-2">
								<span>Mật khẩu cũ</span>
								<Input
									type="password"
									label="Mật khẩu cũ"
									{...register("oldPassword")}
								/>
							</div>
							<div className="flex flex-col gap-y-2">
								<span>Mật khẩu mới</span>
								<Input
									type="password"
									label="Mật khẩu mới"
									{...register("newPassword")}
								/>
							</div>
							<div className="flex flex-col gap-y-2">
								<span>Xác nhận mật khẩu mới</span>
								<Input
									type="password"
									label="Xác nhận mật khẩu mới"
									{...register("confirmPassword")}
								/>
							</div>
							<div className="flex flex-col gap-y-2">
								<Button type="submit" className="bg-primary w-full">
									Cập nhật
								</Button>
							</div>
						</div>
					</div>
				</form>
			</div>
		</>
	);
};

export default ChangePasswordPage;
