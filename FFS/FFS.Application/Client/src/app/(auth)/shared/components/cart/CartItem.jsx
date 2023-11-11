import propTypes from "prop-types";
import DeleteIcon from "../../../../../shared/components/icon/DeleteIcon";
import { useDispatch } from "react-redux";
import { cartActions } from "../../cartSlice";

const CartItem = ({ item }) => {
  const dispatch = useDispatch();

  const IncrementHandler = () => {
    dispatch(cartActions.increaseItemQuantity(item));
    console.log("+", item);
  };

  const DescrementHandler = () => {
    dispatch(cartActions.decreaseItemQuantity(item));
    console.log("-", item);
  };

  const RemoveHandler = () => {
    dispatch(cartActions.removeItem(item));
  };
  return (
    <>
      <tr key={item.foodId}>
        <td className="p-4 border-b border-blue-gray-50 flex items-center gap-x-2">
          <div onClick={RemoveHandler}>
            <DeleteIcon></DeleteIcon>
          </div>
          <img src={item.img} alt="" className="w-[70px]" />
          <span>{item.foodName}</span>
        </td>
        <td className="p-4 border-b border-blue-gray-50">{item.price} đ</td>
        <td className="p-4 border-b border-blue-gray-50">
          <div className="flex items-center justify-between border p-2">
            <span
              className="text-gray-500 text-3xl font-medium cursor-pointer"
              onClick={DescrementHandler}
            >
              -
            </span>
            <p>{item.quantity}</p>
            <span
              className="text-3xl font-medium cursor-pointer"
              onClick={IncrementHandler}
            >
              +
            </span>
          </div>
        </td>
        <td className="p-4 border-b border-blue-gray-50">
          {item.quantity * item.price} đ
        </td>
      </tr>
    </>
  );
};

CartItem.propTypes = {
  item: propTypes.any.isRequired,
};

export default CartItem;
