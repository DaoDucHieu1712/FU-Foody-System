import moment from "moment";
import "moment/locale/vi";
moment.locale("vi");

const isStoreOpen = (timeStart, timeEnd) => {
    const currentTime = moment();
    const openingTime = moment(timeStart, "HH:mm");
    const closingTime = moment(timeEnd, "HH:mm");

    // Check if the current time is between the opening and closing hours
    return currentTime.isBetween(openingTime, closingTime);
};

export default isStoreOpen;