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
                <h2 className="profile-username">User1</h2>
                <div className="profile-buttons">
                    <button className="profile-btn">Bewerken</button>
                    <button className="profile-btn" onClick={handleLogout}>Uitloggen <FaSignOutAlt /></button>
                </div>
            </div>
        </>
    );
};

export default Profile;
