import { useState, useEffect, Fragment } from "react";
import { useNavigate } from "react-router-dom";
import "./Gewicht.css";
import axios from "axios";
import { Navbar, Container, Nav, Button, Col, Modal, Table } from "react-bootstrap";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { FaPen } from "react-icons/fa";
import { IoIosAddCircle } from "react-icons/io";




const Gewicht = () => {
    
    const [data, setData] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        getData();
    }, []);

    const getData = () => {
        axios.get(`https://localhost:7209/api/Gewicht`)
            .then((result) => {
                console.log(result.data);
                setData(result.data);
            })
            .catch((error) => {
                console.error("Error met ophalen van gewicht:", error);
                toast.error("Gewicht ophalen mislukt!");
            });
    };

    return (
        <Fragment>
            <ToastContainer />
            <Navbar bg="primary" variant="dark" expand="lg">
                <Container>
                    <Navbar.Brand href="#home">MyWeightPal</Navbar.Brand>
                </Container>
            </Navbar>
            <Container fluid>
                <row className="gewichttoevoegen">
                    <Button className="btn gewichttoevoegen-btn" >
                        Gewicht toevoegen <IoIosAddCircle />
                    </Button>
                </row>
            </Container>
            <br />
            <Container>
                <Table striped bordered hover className="custom-table">
                    <thead className="header-row">
                    <tr>
                        <th>Gewicht</th>
                        <th>Datum</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
                    {data.length > 0 ? (
                        data.map((item, index) => (
                            <tr key={index}>
                                <td>{item.gewicht }</td>
                                <td>{new Date(item.datum).toISOString().split("T")[0]}</td>
                                <td>
                                    <Button
                                        className="btn edit-btn"
                                        id="edit-button"
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            //handleEdit(item.id);
                                        }}
                                    >
                                        <FaPen size={16} />
                                    </Button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="3">Er is nog geen gewicht toegevoegd.</td>
                        </tr>
                    )}
                    </tbody>
                </Table>
            </Container>
        </Fragment>
    );
};

export default Gewicht;