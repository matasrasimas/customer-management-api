import {useNavigate} from "react-router-dom";

const GoBackButton = () => {
    const navigate = useNavigate();

    return (
        <button
            onClick={() => navigate(-1)}
            className="absolute left-15 font-bold text-2xl cursor-pointer hover:text-blue-500"
        >
            Atgal
        </button>
    );
};

export default GoBackButton;