import copy from 'clipboard-copy';
import { useState } from 'react';
import propTypes from "prop-types";

const DiscountCopyStore = ({ codeToCopy }) => {
    const [isCopied, setIsCopied] = useState(false);

    const handleCopyClick = async () => {
        try {
            await copy(codeToCopy);
            setIsCopied(true);
            setTimeout(() => {
                setIsCopied(false);
            }, 5000);
        } catch (error) {
            console.error('Không thể sao chép vào clipboard: ', error);
        }
    };

    return (
        <div>
            <button className='flex items-center gap-2' onClick={handleCopyClick} disabled={isCopied}>
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    height="16"
                    width="14"
                    viewBox="0 0 448 512">
                    <path
                        d="M208 0H332.1c12.7 0 24.9 5.1 33.9 14.1l67.9 67.9c9 9 14.1 21.2 14.1 33.9V336c0 26.5-21.5 48-48 48H208c-26.5 0-48-21.5-48-48V48c0-26.5 21.5-48 48-48zM48 128h80v64H64V448H256V416h64v48c0 26.5-21.5 48-48 48H48c-26.5 0-48-21.5-48-48V176c0-26.5 21.5-48 48-48z"
                    />
                </svg>
                {isCopied ? 'Đã sao chép!' : 'Sao chép'}
            </button>
        </div>
    );
};

DiscountCopyStore.propTypes = {
    codeToCopy: propTypes.any.isRequired
};

export default DiscountCopyStore;