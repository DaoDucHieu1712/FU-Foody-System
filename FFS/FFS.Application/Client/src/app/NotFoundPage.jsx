// NotFound.jsx
const NotFoundPage = () => {
  const handleBackBtn = () => {
    window.location.href = "/";
  };
  return (
    <div className="w-full h-full relative">
      <img
        className="w-full h-screen"
        src="/src/assets/404.png"
        alt="not-found"
      />
      <p
        className="absolute top-[65%] right-[50%] z-10 px-4 py-2 bg-primary text-white cursor-pointer hover:opacity-80"
        onClick={handleBackBtn}
      >
        Quay về trang chủ
      </p>
    </div>
  );
};

export default NotFoundPage;
