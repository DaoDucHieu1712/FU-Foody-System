import { css } from "@emotion/react";
import { BeatLoader } from "react-spinners";

const override = css`
  display: block;
  margin: 0 auto;
  border-color: red; // Customize the color of the spinner
`;

const Loading = () => {
  return (
    <div className="flex items-center justify-center h-screen">
      <BeatLoader css={override} size={15} color={"#36D7B7"} loading={true} />
    </div>
  );
};

export default Loading;
