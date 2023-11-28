import { useEffect, useState } from "react";
import axios from "../../shared/api/axiosConfig";
import { toast } from "react-toastify";
import { Button, Input, Typography } from "@material-tailwind/react";
import Pagination from "../../shared/components/Pagination";
import { Link } from "react-router-dom";

const PostManagePage = () => {
	const [postList, setPostList] = useState([]);
	const [totalPage, setTotalPage] = useState(0);
	const [dataSearch, setDateSearch] = useState({
		pageNumber: 1,
		pageSize: 15,
		status: 0,
		username: "",
		title: "",
	});

	const GetPosts = async () => {
		try {
			await axios
				.post(`/api/Admin/GetPosts`, dataSearch)
				.then((res) => {
					console.log(res);
					setPostList(res.data);
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

	const handleOnChange = async (e) => {
		dataSearch[e.target.name] = e.target.value;
		setDateSearch(dataSearch);

		await GetPosts();
	};

	const changePage = async (page) => {
		dataSearch.pageNumber = page;
		setDateSearch(dataSearch);
		await GetPosts();
	};

	useEffect(() => {
		GetPosts();
	}, []);

	return (
		<>
			<div className="relative overflow-x-auto">
				<div className="mb-4 flex flex-col gap-8 md:flex-row md:items-center">
					<Typography variant="h4" color="blue-gray">
						Danh sách bài viết
					</Typography>
				</div>

				<table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
					<thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
						<tr>
							<th scope="col" className="px-6 py-3">
								Tiêu đề
							</th>
							<th scope="col" className="px-6 py-3">
								Người đăng bài
							</th>
							<th scope="col" className="px-6 py-3">
								Ngày đăng
							</th>
							<th scope="col" className="px-6 py-3">
								Trạng thái
							</th>
							<th scope="col" className="px-6 py-3"></th>
						</tr>
						<tr>
							<th scope="col" className="px-6 py-3">
								<div className="w-full">
									<Input
										variant="static"
										placeholder="Tiêu đề"
										onChange={handleOnChange}
										name="title"
									/>
								</div>
							</th>
							<th scope="col" className="px-6 py-3">
								<div className="w-full">
									<Input
										variant="static"
										placeholder="Người đăng bài"
										onChange={handleOnChange}
										name="username"
									/>
								</div>
							</th>
							<th scope="col" className="px-6 py-3"></th>
							<th scope="col" className="px-6 py-3"></th>
							<th scope="col" className="px-6 py-3"></th>
						</tr>
					</thead>
					<tbody>
						{postList.map((post) => (
							<tr
								key={post.id}
								className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
							>
								<th
									scope="row"
									className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
								>
									{post.Title}
								</th>
								<td className="px-6 py-4">{post.UserName}</td>
								<td className="px-6 py-4">{post.CreatedAt}</td>
								<td className="px-6 py-4">
									{post.StatusCode === 1 && (
										<span className="font-bold text-primary">
											{post.Status}
										</span>
									)}
									{post.StatusCode === 2 && (
										<span className="font-bold text-green-500">
											{post.Status}
										</span>
									)}
									{post.StatusCode === 3 && (
										<span className="font-bold text-red-900">
											{post.Status}
										</span>
									)}
								</td>
								<th scope="col" className="px-6 py-3">
									<div className=" flex justify-evenly items-center">
										<Button
											variant="outlined"
											size="sm"
											className="border-primary hover:bg-primary hover:text-white"
										>
											xem
										</Button>
										<Button
											variant="outlined"
											size="sm"
											className="border-primary hover:bg-primary hover:text-white"
										>
											Chấp thuận
										</Button>
										<Button
											size="sm"
											variant="outlined"
											className="border-primary hover:bg-primary hover:text-white"
										>
											Từ chối
										</Button>
									</div>
								</th>
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

export default PostManagePage;
