
const Location = () => {
    return (
        <div className="w-full">
            <form id="location" className="bg-white rounded px-40 py-20 mb-4">
                <p className="font-bold text-2xl text-center mb-4">Thêm địa chỉ mới</p>
                <div className="mb-4">
                    <input id="name" className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Họ và tên" type="text"></input>
                </div>
                <div className="mb-4">
                    <input id="name" className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Số điện thoại" type="tel"></input>
                </div>
                <div className="mb-4">
                    <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Thành phố Hà Nội" readOnly></input>
                </div>
                <div className="mb-4">
                    <input className="shadow appearance-none border rounded w-full py-2 px-3 leading-tight pointer-events-none" type="text" placeholder="Huyện Thạch Thất" readOnly></input>
                </div>
                <div className="inline-block relative mb-4">
                    <select id="commune" className="block appearance-none w-full bg-white border border-gray-400 hover:border-gray-500 px-4 py-2 pr-8 rounded shadow leading-tight focus:outline-none focus:shadow-outline">
                        <option>Xã Tân xã</option>
                    </select>
                    <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                        <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" /></svg>
                    </div>
                </div>
                <div className="mb-4">
                    <textarea id="name" className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" placeholder="Địa chỉ cụ thể..." type="text"></textarea>
                </div>
                <button type="submit" className="text-white bg-orange-500 hover:bg-orange-600 focus:ring-4 focus:outline-none font-medium rounded-lg text-sm w-full px-5 py-2.5 text-center">Submit</button>
            </form>
        </div>
    );
};

export default Location;