const LastestPost = () => {
    return (
      <>
        <div className="Blog_newest border p-4">
              <h1 className="text-lg font-bold uppercase">Blog mới nhất</h1>
              <div className="flex mb-4 mt-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
              <div className="flex mb-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
              <div className="flex mb-4">
                {/* Image */}
                <div className="w-1/3">
                  <img
                    src="https://lavenderstudio.com.vn/wp-content/uploads/2017/03/chup-san-pham.jpg"
                    className="w-full h-auto"
                  />
                </div>
                {/* Title and Timestamp */}
                <div className="w-2/3 px-4">
                  <h2 className="text-sm font-bold">
                    Top 5 món ăn truyền thống đốn tim gen Z
                  </h2>
                  <p className="text-sm text-gray-500">12 Nov, 2023</p>
                </div>
              </div>
            </div>
      </>
    );
  };
  
  export default LastestPost;
  