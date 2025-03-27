import { useEffect, useState } from "react";
import ReactConfetti from "react-confetti";

const Confetti = ({ show }) => {
    const [windowDimension, setWindowDimension] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });

    useEffect(() => {
        const detectSize = () => {
            setWindowDimension({
                width: window.innerWidth,
                height: window.innerHeight,
            });
        };

        window.addEventListener("resize", detectSize);
        return () => {
            window.removeEventListener("resize", detectSize);
        };
    }, []);

    return show ? (
        <ReactConfetti
            width={windowDimension.width}
            height={windowDimension.height}
        />
    ) : null;
};

export default Confetti;
