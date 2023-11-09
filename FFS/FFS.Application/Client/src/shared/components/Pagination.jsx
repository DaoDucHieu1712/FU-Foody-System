import { Button, IconButton } from "@material-tailwind/react";
import propTypes from "prop-types";
import ArrowLeft from "./icon/ArrowLeft";
import ArrowRight from "./icon/ArrowRight";

const Pagination = ({ totalPage, handleClick, currentPage }) => {
  console.log(currentPage);
  const pageNumbers = Array.from(
    { length: totalPage },
    (_, index) => index + 1
  );
  const next = () => {
    if (currentPage === totalPage) return;

    handleClick(currentPage + 1);
  };

  const prev = () => {
    if (currentPage === 1) return;

    handleClick(currentPage - 1);
  };

  return (
    <div className="flex items-center gap-4">
      <Button
        variant="text"
        className="flex items-center gap-2"
        onClick={prev}
        disabled={currentPage == 1}
      >
        <ArrowLeft strokeWidth={2} className="h-4 w-4" /> Previous
      </Button>
      <div className="flex items-center gap-2">
        {pageNumbers.map((page) => (
          <IconButton
            key={page}
            onClick={() => handleClick(page)}
            variant={currentPage == page ? "filled" : "text"}
            className={currentPage == page ? "gray" : ""}
          >
            {page}
          </IconButton>
        ))}
      </div>
      <Button
        variant="text"
        className="flex items-center gap-2"
        onClick={next}
        disabled={currentPage == totalPage}
      >
        Next
        <ArrowRight strokeWidth={2} className="h-4 w-4" />
      </Button>
    </div>
  );
};

Pagination.propTypes = {
  totalPage: propTypes.any,
  handleClick: propTypes.any,
  currentPage: propTypes.any,
};

export default Pagination;
