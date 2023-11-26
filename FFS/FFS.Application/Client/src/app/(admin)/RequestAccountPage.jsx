import Pagination from "../../shared/components/Pagination";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { Button, Input, Option, Select } from "@material-tailwind/react";
import axios from "../../shared/api/axiosConfig";
import { Link } from "react-router-dom";

const RequestAccountPage = () => {
	const [dataSearch, setDateSearch] = useState({
		pageNumber: 1,
		pageSize: 5,
		username: "",
		email: "",
		role: "",
		status: 0,
	});

	const [userList, setUserList] = useState([]);
	const [roleList, setRoleList] = useState([]);

	const [totalPage, setTotalPage] = useState(0);
	const [roleSelect, setRoleSelect] = useState("");
	const [statusSelect, setStatusSelect] = useState("");

	const statusList = [
		{
			id: 1,
			name: "Chờ duyệt",
			value: "Pending",
		},
		{
			id: 2,
			name: "Chấp thuận",
			value: "Accept",
		},
		{
			id: 3,
			name: "Từ chối",
			value: "Reject",
		},
	];

	const changePage = async (page) => {
		dataSearch.pageNumber = page;
		setDateSearch(dataSearch);
		await getRequestAccount();
	};
	const handleOnChange = async (e) => {
		dataSearch[e.target.name] = e.target.value;
		setDateSearch(dataSearch);

		await getRequestAccount();
	};

	const getRequestAccount = async () => {
		try {
			await axios
				.post(`/api/Admin/GetRequestAccount`, dataSearch)
				.then((res) => {
					console.log(res);
					setUserList(res.data);
					const totalPages = Math.ceil(res.total / dataSearch.pageSize);
					setTotalPage(totalPages);
				})
				.catch((error) => {
					toast.error("Có lỗi xảy ra!");
					console.log(error);
				});
		} catch (error) {
			console.log("Food error: " + error);
		}
	};

	const getRoles = async () => {
		try {
			await axios
				.get(`/api/Admin/GetRoles`)
				.then((res) => {
					console.log(res);
					setRoleList(res);
				})
				.catch((error) => {
					toast.error("Có lỗi xảy ra!");
					console.log(error);
				});
		} catch (error) {
			console.log("Food error: " + error);
		}
	};

	const handleClickOption = (id, name) => {
		console.log(name);
		dataSearch.role = id;
		setDateSearch(dataSearch);
		setRoleSelect(name);
		getRequestAccount();
	};

	const handleClickBtn = async (id, username, action) => {
		var txt = "";
		if (action == "Accept") {
			txt = `Bạn có chắc chắn duyệt tài khoản ${username}!`;
		} else {
			txt = `Bạn có chắc chắn hủy duyệt tài khoản ${username}!`;
		}
		const check = confirm(txt);
		if (check) {
			const data = {
				id: id,
				username: username,
				action: action,
			};
			try {
				await axios
					.post(`/api/Admin/ApproveUser`, data)
					.then((res) => {
						toast.success(res);
						getRequestAccount();
					})
					.catch((error) => {
						toast.error("Có lỗi xảy ra!");
						console.log(error);
					});
			} catch (error) {
				console.log("Food error: " + error);
			}
		}
	};

	const handleClickStatus = (id, name) => {
		dataSearch.status = id;
		setDateSearch(dataSearch);
		setStatusSelect(name);
		getRequestAccount();
	};

	useEffect(() => {
		getRequestAccount();
		getRoles();
	}, []);

	return (
		<>
			<div className="relative overflow-x-auto">
				<table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
					<thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
						<tr>
							<th scope="col" className="px-6 py-3">
								STT
							</th>
							<th scope="col" className="px-6 py-3">
								Avatar
							</th>
							<th scope="col" className="px-6 py-3">
								Username
							</th>
							<th scope="col" className="px-6 py-3">
								Email
							</th>
							<th scope="col" className="px-6 py-3">
								Vai trò đăng kí
							</th>
							<th scope="col" className="px-6 py-3">
								Trạng thái
							</th>
							<th scope="col" className="px-6 py-3 text-center">
								Thao tác
							</th>
						</tr>
						<tr>
							<th scope="col" className="px-6 py-3"></th>
							<th scope="col" className="px-6 py-3"></th>
							<th scope="col" className="px-6 py-3">
								<Input
									variant="static"
									placeholder="Username"
									onChange={handleOnChange}
									name="username"
								/>
							</th>
							<th scope="col" className="px-6 py-3">
								<div className="w-full">
									<Input
										variant="static"
										placeholder="Email"
										onChange={handleOnChange}
										name="email"
									/>
								</div>
							</th>
							<th scope="col" className="px-6 py-3">
								<div className="w-full">
									<Select variant="static" name="role" value={roleSelect}>
										<Option
											value=""
											onClick={() => handleClickOption("", "Tất cả")}
										>
											Tất cả
										</Option>
										{roleList.map((role) => {
											return (
												<Option
													key={role.id}
													value={role.id}
													onClick={() => handleClickOption(role.id, role.name)}
												>
													{role.name}
												</Option>
											);
										})}
									</Select>
								</div>
							</th>
							<th scope="col" className="px-6 py-3">
								<div className="w-full">
									<Select variant="static" name="status" value={statusSelect}>
										<Option
											value=""
											onClick={() => handleClickStatus(0, "Tất cả")}
										>
											Tất cả
										</Option>
										{statusList.map((status) => {
											return (
												<Option
													key={status.id}
													value={status.value}
													onClick={() =>
														handleClickStatus(status.id, status.name)
													}
												>
													{status.name}
												</Option>
											);
										})}
									</Select>
								</div>
							</th>
							<th scope="col" className="px-6 py-3"></th>
						</tr>
					</thead>
					<tbody>
						{userList.map((user, index) => (
							<tr
								key={user.Id}
								className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
							>
								<th
									scope="row"
									className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
								>
									{index + 1}
								</th>
								<td className="px-6 py-4">
									<img src={user.Avatar} height={100} width={100} />
								</td>
								<td className="px-6 py-4">{user.UserName}</td>
								<td className="px-6 py-4">{user.Email}</td>
								<td className="px-6 py-4">{user.Role}</td>
								<td scope="col" className="px-6 py-3">
									{user.Status === "Pending" && (
										<span className="font-bold text-primary">Đợi duyệt</span>
									)}
									{user.Status === "Accept" && (
										<span className="font-bold text-green-500">Chấp thuận</span>
									)}
									{user.Status === "Reject" && (
										<span className="font-bold text-red-900">Từ chối</span>
									)}
								</td>

								<td className="px-6 py-3">
									<div className=" flex justify-evenly items-center">
										<Link to={`/admin/application/${user.Id}`}>
											<Button
												variant="outlined"
												size="sm"
												className="border-primary hover:bg-primary hover:text-white"
											>
												xem
											</Button>
										</Link>
										<Button
											variant="outlined"
											size="sm"
											className="border-primary hover:bg-primary hover:text-white"
											onClick={() =>
												handleClickBtn(user.Id, user.UserName, "Accept")
											}
										>
											Chấp thuận
										</Button>
										<Button
											size="sm"
											variant="outlined"
											className="border-primary hover:bg-primary hover:text-white"
											onClick={() =>
												handleClickBtn(user.Id, user.UserName, "Reject")
											}
										>
											Từ chối
										</Button>
									</div>
								</td>
							</tr>
						))}
					</tbody>
				</table>
				<div className="mt-4 flex justify-center">
					<Pagination
						className="mt-4"
						totalPage={totalPage}
						currentPage={dataSearch.pageNumber}
						handleClick={changePage}
					></Pagination>
				</div>
			</div>
		</>
	);
};

export default RequestAccountPage;
