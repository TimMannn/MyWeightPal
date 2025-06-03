import { useState, useEffect, Fragment } from "react";
import axios from "axios";
import { useNavigate, Link } from "react-router-dom";
import "./Profile.css";
import {Button, Col, Container, Modal, Navbar, Row} from "react-bootstrap";
import {FaSignOutAlt, FaUserAlt, FaArrowLeft} from "react-icons/fa";
import {toast} from "react-toastify";


const Profile = () => {
    const apiUrl = import.meta.env.VITE_API_URL;
    const navigate = useNavigate();
    const [data, setData] = useState([]);

    const [selectedImage, setSelectedImage] = useState(null);
    const [imageUrl, setImageUrl] = useState("");

    const [editUser, setEditUser] = useState("");
    const [showEdit, setShowEdit] = useState(false);
    const handleCloseEdit = () => setShowEdit(false);
    const handleShowEdit = () => {
        setEditUser(data.userName);
        setShowEdit(true);
    };


    useEffect(() => {
        getData();
    }, []);

    const [animateHinge, setAnimateHinge] = useState(false);

    const handleAnnuleerClick = () => {
        setAnimateHinge(true);
    };

    const handleAnimationEnd = () => {
        if (animateHinge) {
            handleCloseEdit();
            setAnimateHinge(false);
        }
    };
    
    const handleImageChange = (e) => {
        setSelectedImage(e.target.files[0]);
    };

    const uploadToCloudinary = async () => {
        const formData = new FormData();
        formData.append("file", selectedImage);
        formData.append("upload_preset", "jouw_upload_preset"); // pas aan
        formData.append("cloud_name", "jouw_cloud_name"); // pas aan

        try {
            const response = await axios.post("https://api.cloudinary.com/v1_1/jouw_cloud_name/image/upload", formData);
            return response.data.secure_url;
        } catch (error) {
            console.error("Upload naar Cloudinary mislukt", error);
            toast.error("Upload mislukt");
            return null;
        }
    };



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

    const handleSaveChanges = async () => {
        let finalImageUrl = data.imageUrl; // huidige profielfoto

        if (selectedImage) {
            const uploadedUrl = await uploadToCloudinary();
            if (uploadedUrl) {
                finalImageUrl = uploadedUrl;
            }
        }

        const token = localStorage.getItem("token");
        axios.put(`${apiUrl}/api/User`, {
            userName: editUser,
            imageUrl: finalImageUrl,
        }, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        }).then(() => {
            toast.success("Gegevens succesvol bijgewerkt!");
            handleCloseEdit();
            getData(); // vernieuw data op de pagina
        }).catch((err) => {
            console.error(err);
            toast.error("Bijwerken mislukt");
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
                {data.imageUrl && (
                    <img src={data.imageUrl} alt="Profielfoto" className="profile-picture" />
                )}
                <div className="profile-image-placeholder" />
                <h2 className="profile-username">{data.userName}</h2>
                <div className="profile-buttons">
                    <button className="profile-btn" onClick={handleShowEdit}>Bewerken</button>
                    <button className="profile-btn" onClick={handleLogout}>Uitloggen <FaSignOutAlt /></button>
                </div>
            </div>

            <Modal show={showEdit} onHide={handleCloseEdit}>
                <Modal.Header closeButton>
                    <Modal.Title>Gegevens Bewerken</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col>
                            Gebruikersnaam wijzigen:
                            <input
                                type="text"
                                className="form-control"
                                placeholder="Voeg je nieuwe gebruikersnaam in."
                                value={editUser}
                                onChange={(e) => setEditUser(e.target.value)}
                                required
                            />
                        </Col>
                    </Row>
                    <Row className="mt-3">
                        <Col>
                            Profiel foto bewerken:
                            <input
                                type="file"
                                accept="image/*"
                                className="form-control"
                                onChange={(e) => handleImageChange(e)}
                            />
                        </Col>
                    </Row>

                </Modal.Body>
                <Modal.Footer className="menu-footer">
                    <Button
                        className={`btn menu-btn animate__animated ${animateHinge ? "animate__hinge animate__slower" : ""}`}
                        onClick={handleAnnuleerClick}
                        onAnimationEnd={handleAnimationEnd}
                    >
                        Annuleer
                    </Button>
                    <Button className="btn menu-btn" onClick={handleSaveChanges}>
                        Wijzigingen opslaan
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default Profile;
