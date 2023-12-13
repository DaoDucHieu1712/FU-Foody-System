import { Link } from "react-router-dom";

const AccessDenied = () => {
	return (
		<>
			<div className="w-full h-full relative">
				<img
					className="w-full h-screen object-cover"
					src="/src/assets/403.png"
					alt="not-found"
				/>
				<Link
					to="/"
					className="absolute top-[75%] right-[50%] z-10 px-4 py-2 bg-primary text-white cursor-pointer hover:opacity-80"
				>
					Quay về trang chủ
				</Link>
			</div>
		</>
	);
};

export default AccessDenied;
