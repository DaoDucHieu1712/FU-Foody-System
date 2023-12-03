const FormatPriceHelper = (price) => {
    const priceString = price.toString();

    const parts = priceString.split('.');
    const integerPart = parts[0];
    const decimalPart = parts.length > 1 ? '.' + parts[1] : '';

    const formattedIntegerPart = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, ',');

    const formattedPrice = formattedIntegerPart + decimalPart;

    return formattedPrice;
};

export default FormatPriceHelper;