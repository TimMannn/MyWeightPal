import { useState, useEffect, Fragment } from "react";
import axios from "axios";
import { useNavigate, Link } from "react-router-dom";
import "./Profile.css";
import {Button, Container, Navbar} from "react-bootstrap";
import {FaSignOutAlt, FaUserAlt, FaArrowLeft} from "react-icons/fa";
import {toast} from "react-toastify";

const Profile = () => {
    const apiUrl = import.meta.env.VITE_API_URL;
    const navigate = useNavigate();
    const [data, setData] = useState([]);

    useEffect(() => {
        getData();
    }, []);

    const getData = async () => {
        const token = localStorage.getItem("token");
        console.log("Sending Token:", token);
        axios
            .get(`${apiUrl}/api/User/me`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((result) => {
                console.log(result.data);
                setData(result.data);
            })
            .catch((error) => {
                console.error("Error fetching Users Data:", error);
                toast.error("Failed to fetch User");
            });
    };

    const handleLogout = () => {
        const token = localStorage.getItem("token");
        axios
            .post(
                `${apiUrl}/api/Account/logout`,
                {},
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                },
            )
            .then((response) => {
                if (response.status === 200) {
                    localStorage.removeItem("token");
                    navigate("/Login");
                } else {
                    toast.error("Error logging out");
                }
            })
            .catch((error) => {
                console.error("Error:", error);
                toast.error("Error logging out");
            });
    };

    return (
        <>
            <Navbar bg="primary" variant="dark" expand="lg">
                <Container fluid>
                    <Navbar.Brand>MyWeightPal</Navbar.Brand>
                    
                        <Button variant="light" className="profile-button2">
                            <FaUserAlt size={20} />
                        </Button>
 
                </Container>
            </Navbar>


            <Button className="btn back-btn" onClick={() => navigate('/gewicht')}>
                {" "}
                <FaArrowLeft /> Terug{" "}
            </Button>
            
            <div className="profile-container">
                <div className="profile-image-placeholder" />
                <h2 className="profile-username">{data.userName}</h2>
                <div className="profile-buttons">
                    <button className="profile-btn">Bewerken</button>
                    <button className="profile-btn" onClick={handleLogout}>Uitloggen <FaSignOutAlt /></button>
                </div>
            </div>
        </>
    );
};

export default Profile;
