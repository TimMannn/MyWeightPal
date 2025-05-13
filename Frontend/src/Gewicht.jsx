import { useState, useEffect, Fragment } from "react";
import { useNavigate } from "react-router-dom";
import "./Gewicht.css";
import 'animate.css';
import axios from "axios";
import { Navbar, Container, Button, Table, Modal, Row, Col } from "react-bootstrap";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import ReactApexChart from "react-apexcharts";
import { FaPen } from "react-icons/fa";
import { IoIosAddCircle } from "react-icons/io";
import { MdDelete } from "react-icons/md";
import Confetti from './Confetti';



const Gewicht = () => {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [data, setData] = useState([]);
    const [dataDoelGewicht, setDataDoelGewicht] = useState([]);
    const [chartData, setChartData] = useState({ series: [], options: {} });
    const navigate = useNavigate();
    
    
    const [Gewicht, setGewicht] = useState("");
    const [showAdd, setShowAdd] = useState(false);
    const handleCloseAdd = () => setShowAdd(false);
    const handleShowAdd = () => setShowAdd(true);

    
    const [editID, setEditID] = useState("");
    const [editGewicht, setEditGewicht] = useState("");
    const [showEdit, setShowEdit] = useState(false);
    const handleCloseEdit = () => setShowEdit(false);
    const handleShowEdit = () => setShowEdit(true);


    const [DoelGewicht, setDoelGewicht] = useState("");
    const [showDoelAdd, setShowDoelAdd] = useState(false);
    const handleCloseDoelAdd = () => setShowDoelAdd(false);
    const handleShowDoelAdd = () => setShowDoelAdd(true);


    const [editDoelID, setEditDoelID] = useState("");
    const [editDoelGewicht, setEditDoelGewicht] = useState("");
    const [editDoelDatumBehaald, setEditDoelDatumBehaald] = useState(null)
    const [showDoelEdit, setShowDoelEdit] = useState(false);
    const handleCloseDoelEdit = () => setShowDoelEdit(false);
    const handleShowDoelEdit = () => setShowDoelEdit(true);

    
    const [selectedItem, setSelectedItem] = useState(null);
    const [confettiActive, setConfettiActive] = useState(false);

    
    const [isGewichtVandaagIngevuld, setIsGewichtVandaagIngevuld] = useState(false);
    const [isDoelgewichtActief, setIsDoelgewichtActief] = useState(false);

    useEffect(() => {
        const today = new Date().toLocaleDateString("sv-SE");

        // Check of er vandaag al gewicht is ingevoerd
        const existingWeight = data.find(item =>
            new Date(item.datum).toLocaleDateString("sv-SE") === today
        );
        setIsGewichtVandaagIngevuld(!!existingWeight);

        // Check of er een actief doelgewicht is
        if (dataDoelGewicht.length > 0) {
            const laatsteDoel = dataDoelGewicht[dataDoelGewicht.length - 1];
            setIsDoelgewichtActief(laatsteDoel.datumbehaald === null);
        } else {
            setIsDoelgewichtActief(false);
        }
    }, [data, dataDoelGewicht]);


    const [animateHinge, setAnimateHinge] = useState(false);
    
    const handleAnnuleerClick = () => {
        setAnimateHinge(true);
    };

    const handleAnimationEnd = () => {
        if (animateHinge) {
            handleCloseDoelEdit();
            handleCloseDoelAdd();
            handleCloseEdit();
            handleCloseAdd();
            setAnimateHinge(false);
        }
    };

    const [animationClass, setAnimationClass] = useState("");
    const [isUpdating, setIsUpdating] = useState(false);

    const triggerOutAndUpdate = () => {
        handleCloseDoelEdit();
        setAnimationClass("animate__bounceOutLeft");
        setIsUpdating(true);
        
        setTimeout(() => {
            handleDoelUpdate();
        }, 1000); 
    };

    const triggerOutAndSave = () => {
        handleCloseDoelAdd();
        setAnimationClass("animate__bounceOutLeft");
        setIsUpdating(true);

        setTimeout(() => {
            handleDoelSave();
        }, 1000);
    };



    useEffect(() => {
        getDataDoelGewicht();
        getData();
    }, []);

    useEffect(() => {
        updateChartData();
    }, [data, dataDoelGewicht]);

    const getData = async () => {
        try {
            const result = await axios.get(`${apiUrl}/api/Gewicht/gewicht`);
            console.log("Data responds: ", result.data);
            setData(result.data);
            return result.data;
        } catch (error) {
            console.error("Error met ophalen van gewicht:", error);
            toast.error("Gewicht ophalen mislukt!");
        }
    };

    const getDataDoelGewicht = () => {
        axios.get(`${apiUrl}/api/Gewicht/doelgewicht`)
            .then((result) => {
                console.log("DataDoelGewicht responds: ", result.data);
                setDataDoelGewicht(result.data);
            })
            .catch((error) => {
                console.error("Error met ophalen van doelgewicht:", error);
                toast.error("Doelgewicht ophalen mislukt!");
            });
    };

    const updateChartData = () => {
        const laatsteDoelGewicht = dataDoelGewicht.length > 0 ? dataDoelGewicht[dataDoelGewicht.length - 1] : null;
        const datumBehaald = laatsteDoelGewicht ? laatsteDoelGewicht.datumbehaald : null;
        
        const annotations = datumBehaald === null ? [
            {
                y: laatsteDoelGewicht ? laatsteDoelGewicht.doelgewicht : null,
                borderColor: '#33cc66',
                strokeDashArray: 0,
                strokeWidth: 4,
                label: {
                    borderColor: '#33cc66',
                    style: {
                        color: '#fff',
                        background: '#33cc66'
                    },
                    text: laatsteDoelGewicht ? `Doel: ${laatsteDoelGewicht.doelgewicht} kg` : 'Geen doelgewicht ingesteld'
                }
            }
        ] : [];

        // Stel de chart data in met de juiste annotaties
        setChartData({
            series: [
                {
                    name: "Gewicht",
                    data: data.map(item => ({
                        x: new Date(item.datum).toLocaleDateString("sv-SE"), // Oplossing voor UTC probleem
                        y: item.gewicht
                    }))
                }
            ],
            options: {
                chart: {
                    type: "area",
                    height: 350,
                    zoom: { enabled: true }
                },
                xaxis: {
                    type: "category",
                    title: { text: "Datum" }
                },
                yaxis: {
                    title: { text: "Gewicht (kg)" },
                    min: Math.min(...data.map(d => d.gewicht), dataDoelGewicht.length > 0 ? dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht : Infinity) - 0.5,
                    max: Math.max(...data.map(d => d.gewicht), dataDoelGewicht.length > 0 ? dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht : -Infinity) + 0.5
                },
                tooltip: {
                    x: { format: "yyyy-MM-dd" }
                },
                annotations: {
                    yaxis: annotations
                }
            }
        });
    };


    const handleDuplicateCheck = () => {
        const today = new Date().toLocaleDateString("sv-SE");
        
        const existingWeight = data.find(item => new Date(item.datum).toLocaleDateString("sv-SE") === today);

        if (existingWeight) {
            setSelectedItem(existingWeight);
            setEditGewicht(existingWeight.gewicht);
            setEditID(existingWeight.id);
            handleShowEdit();
        } else {
            handleShowAdd();
        }
    };
    
    const handleExistingDoelgewichtCheck = () => {
        if (dataDoelGewicht.length > 0)
        {
            const laatsteDoel = dataDoelGewicht[dataDoelGewicht.length - 1];
            
            if(laatsteDoel.datumbehaald === null)
            {
                const huidigdoel = parseFloat(dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht);
                const doelgewichtid = parseFloat(dataDoelGewicht[dataDoelGewicht.length - 1].id);
                setEditDoelGewicht(huidigdoel);
                setEditDoelID(doelgewichtid);
                handleShowDoelEdit();
            }
            else{
                handleShowDoelAdd();
            }
        }
        else{
            handleShowDoelAdd();
        }
    };
    
    const handleNotSameWeight = () => {
        if(data.length > 0)
        {
            const vorigeGewicht = parseFloat(data[data.length - 1].gewicht);
            const nieuwDoel = parseFloat(DoelGewicht);
            if (nieuwDoel === vorigeGewicht)
            {
                toast.error("Doel mag niet hetzelfde zijn als je huidige gewicht!")
            }
            else {
                triggerOutAndSave()
            }
        }
        else{
            triggerOutAndSave()
        }
    };

    const handleNotSameWeightEdit = () => {
        if(data.length > 0)
        {
            const vorigeGewicht = parseFloat(data[data.length - 1].gewicht);
            const nieuwDoel = parseFloat(editDoelGewicht);
            if (nieuwDoel === vorigeGewicht)
            {
                toast.error("Doel mag niet hetzelfde zijn als je huidige gewicht!")
            }
            else {
                triggerOutAndUpdate()
            }
        }
        else{
            triggerOutAndUpdate()
        }
    };

    const handleSave = () => {
        const axiosData = {
            gewicht: Gewicht,
            datum: new Date().toLocaleDateString("sv-SE")
        };

        axios
            .post(`${apiUrl}/api/Gewicht/gewicht`, axiosData)
            .then((response) => {
                if (response.status === 200) {
                    getData();
                    setGewicht("");
                    handleCloseAdd();
                    toast.success("Gewicht toegevoegd!");
                    
                    if (dataDoelGewicht.length > 0 && data.length > 0) {
                        const huidigGewicht = parseFloat(Gewicht);
                        const vorigeGewicht = parseFloat(data[data.length - 1].gewicht);
                        const doelGewicht = parseFloat(dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht);
                        
                        const doelIsAankomen = doelGewicht > vorigeGewicht;
                        
                        const doelBehaald = doelIsAankomen
                            ? huidigGewicht >= doelGewicht 
                            : huidigGewicht <= doelGewicht;

                        if (doelBehaald) {
                            setConfettiActive(true);
                            handleDoelGewichtBehaald();
                            setTimeout(() => setConfettiActive(false), 10000);
                        }
                    }
                } else {
                    toast.error(`Gewicht toevoegen mislukt: ${response.data.message}`);
                }
            })
            .catch((error) => {
                const errorMessages = error.response?.data?.messages || [
                    error.response?.data?.message || "Gewicht toevoegen mislukt!",
                ];
                errorMessages.forEach((msg) => toast.error(msg));
            });
    };

    const handleDoelGewichtBehaald = () => {
        const laatsteDoel = dataDoelGewicht[dataDoelGewicht.length - 1];
        
        const huidigeDatum = new Date().toLocaleDateString("sv-SE");
        const data = {
            id: laatsteDoel.id,
            doelgewicht: laatsteDoel.doelgewicht,
            datumBehaald: huidigeDatum,
        };

        axios
            .put(`${apiUrl}/api/Gewicht/doelgewicht${laatsteDoel.id}`, data)
            .then((response) => {
                if (response.status === 200) {
                    getDataDoelGewicht();
                } 
                else {
                    toast.error("ging nie best");
                }
            })
            .catch((error) => {
                console.error("Error details:", error.response);
                const errorMessages = error.response?.data?.messages || [
                    error.response?.data?.message || "Error doelgewicht eindDatum",
                ];
                errorMessages.forEach((msg) => toast.error(msg));
            });
    }



    const handleDoelSave = () => {
        const data = {
            doelgewicht: DoelGewicht,
            datum: new Date().toLocaleDateString("sv-SE")
        };

        axios
            .post(`${apiUrl}/api/Gewicht/doelgewicht`, data)
            .then((response) => {
                if (response.status === 200) {
                    getDataDoelGewicht();
                    setDoelGewicht("");
                    handleCloseDoelAdd();
                    setAnimationClass("animate__bounceInRight");
                    setIsUpdating(false);
                } 
            })
            .catch((error) => {
                const errorMessages = error.response?.data?.messages || [
                    error.response?.data?.message || "Gewicht toevoegen mislukt!",
                ];
                errorMessages.forEach((msg) => toast.error(msg));
                setIsUpdating(false);
            });
    };
    
    
     
    const handleEdit = (ID) => {
        handleShowEdit();
        axios
            .get(`${apiUrl}/api/Gewicht/gewicht${ID}`)
            .then((result) => {
                console.log(result.data);
                setEditGewicht(result.data.gewicht);
                setEditID(ID);
            })
            .catch((error) => {
                console.error("Error bij het ophalen van gewicht:", error);
                toast.error("Gewicht niet gevonden!");
            })
    };

    const handleUpdate = async () => {
        const dataToSend = {
            id: editID,
            gewicht: editGewicht,
        };

        if (editGewicht.trim() === "") {
            toast.error("Voer een nieuw gewicht in!");
            return;
        }

        try {
            const response = await axios.put(`${apiUrl}/api/Gewicht/gewicht${editID}`, dataToSend);

            if (response.status === 200) {
                const nieuweData = await getData();

                setEditGewicht("");
                setEditID("");
                handleCloseEdit();
                toast.success("Gewicht is succesvol bewerkt!");

                if (dataDoelGewicht.length > 0 && nieuweData.length > 1) {
                    const huidigGewicht = parseFloat(editGewicht);
                    const vorigeGewicht = parseFloat(nieuweData[nieuweData.length - 2].gewicht);
                    const doelGewicht = parseFloat(dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht);

                    const doelIsAankomen = doelGewicht > vorigeGewicht;

                    const doelBehaald = doelIsAankomen
                        ? huidigGewicht >= doelGewicht
                        : huidigGewicht <= doelGewicht;

                    if (doelBehaald) {
                        setConfettiActive(true);
                        handleDoelGewichtBehaald();
                        setTimeout(() => setConfettiActive(false), 10000);
                    }
                }
            } else {
                toast.error(`Gewicht bewerken mislukt: ${response.data.message}`);
            }
        } catch (error) {
            console.error("Error details:", error.response);
            const errorMessages = error.response?.data?.messages || [
                error.response?.data?.message || "Error updating gewicht",
            ];
            errorMessages.forEach((msg) => toast.error(msg));
        }
    };

    const handleDoelUpdate = () => {
        const data = {
            id: editDoelID,
            doelgewicht: editDoelGewicht,
        };

        axios
            .put(`${apiUrl}/api/Gewicht/doelgewicht${editDoelID}`, data)
            .then((response) => {
                if (response.status === 200) {
                    getDataDoelGewicht();
                    setEditDoelGewicht("");
                    setEditDoelID("");
                    setAnimationClass("animate__bounceInRight");
                    setIsUpdating(false);
                } else {
                    toast.error(`Gewicht bewerken mislukt: ${response.data.message}`);
                    setIsUpdating(false);
                }
            })
            .catch((error) => {
                console.error("Error details:", error.response);
                const errorMessages = error.response?.data?.messages || [
                    error.response?.data?.message || "Error updating gewicht",
                ];
                errorMessages.forEach((msg) => toast.error(msg));
                setIsUpdating(false);
            });
    };
    
    const handleDelete = (ID) => {
        const clear = () => {
            setEditGewicht("");
            setEditID("");
        };
        
        if (window.confirm(`Weet je zeker dat je het gewicht wilt verwijderen?`)) {
            axios
                .delete(`${apiUrl}/api/Gewicht/gewicht${ID}`)
                .then((result) => {
                    if (result.status === 200) {
                        clear();
                        handleCloseEdit();
                        toast.success("Gewicht is succesvol verwijderd!");
                        getData();
                    }
                })
                .catch((error) => {
                    toast.error("Gewicht verwijderen mislukt!");
                    console.log(error);
                });
        }
    };

    const handleDoelDelete = (ID) => {
        const clear = () => {
            setEditDoelGewicht("");
            setEditDoelID("");
        };

        if (window.confirm(`Weet je zeker dat je het doelgewicht wilt verwijderen?`)) {
            axios
                .delete(`${apiUrl}/api/Gewicht/doelgewicht${ID}`)
                .then((result) => {
                    if (result.status === 200) {
                        clear();
                        handleCloseDoelEdit();
                        toast.success("Doelgewicht is succesvol verwijderd!");
                        getDataDoelGewicht();
                    }
                })
                .catch((error) => {
                    toast.error("Doelgewicht verwijderen mislukt!");
                    console.log(error);
                });
        }
    };

    return (
        <Fragment>
            <ToastContainer />
            <Navbar bg="primary" variant="dark" expand="lg">
                <Container>
                    <Navbar.Brand href="#home">MyWeightPal</Navbar.Brand>
                </Container>
            </Navbar>
            
            <Confetti show={confettiActive} />
            
            <Container fluid>
                <div className="gewichttoevoegen">
                    <Button className="btn gewichttoevoegen-btn" onClick={handleDuplicateCheck}>
                        {isGewichtVandaagIngevuld ? "Gewicht bewerken" : "Gewicht toevoegen"} <IoIosAddCircle />
                    </Button>
                    <Button className="btn doelgewichttoevoegen-btn" onClick={handleExistingDoelgewichtCheck}>
                        {isDoelgewichtActief ? "Doelgewicht bewerken" : "Doelgewicht toevoegen"} <IoIosAddCircle />
                    </Button>
                </div>
            </Container>
            <br />

            <Container fluid>
                <br />
                <div className={`animate__animated ${animationClass}`}>
                    <h3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Doelgewicht: {dataDoelGewicht.length > 0
                        ? `${dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht} kg`
                        : "Er is nog geen doelgewicht ingesteld"}
                    </h3>
                </div>
                <ReactApexChart
                    key={chartData.series.length}
                    options={chartData.options}
                    series={chartData.series}
                    type="area"
                    height={350}
                />
            </Container>

            <Container className="custom-table-container">
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
                        data
                            .slice()
                            .sort((a, b) => new Date(b.datum) - new Date(a.datum))
                            .map((item, index) => (
                                <tr key={index}>
                                    <td>{item.gewicht} kg</td>
                                    <td>{new Date(item.datum).toLocaleDateString("nl-NL")}</td> {/* Oplossing voor juiste datumweergave */}
                                    <td>
                                        <Button className="btn edit-btn" onClick={() => {
                                            setSelectedItem(item);
                                            handleEdit(item.id);
                                        }}>
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

            {/* Pop-up gewicht toevoegen */}
            <Modal show={showAdd} onHide={handleCloseAdd}>
                <Modal.Header closeButton>
                    <Modal.Title>Nieuw gewicht toevoegen.</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col>
                            <input
                                type="number"
                                className="form-control"
                                placeholder="Voer je gewicht in."
                                value={Gewicht}
                                onChange={(e) => setGewicht(e.target.value)}
                                min="0"
                                max="300"
                                required
                            />
                        </Col>
                    </Row>
                </Modal.Body>
                <Modal.Footer>
                    <Button
                        className={`animate__animated ${animateHinge ? "animate__hinge animate__slower" : ""}`}
                        onClick={handleAnnuleerClick}
                        onAnimationEnd={handleAnimationEnd}
                    >
                        Annuleer
                    </Button>
                    <Button onClick={handleSave}>Opslaan</Button>
                </Modal.Footer>
            </Modal>

            {/*Pop-up gewicht bewerken*/}
            <Modal show={showEdit} onHide={handleCloseEdit}>
                <Modal.Header closeButton>
                    <Modal.Title>Gewicht Bewerken</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col>
                            <input
                                type="number"
                                className="form-control"
                                placeholder="Voeg je nieuwe gewicht in."
                                value={editGewicht}
                                onChange={(e) => setEditGewicht(e.target.value)}
                                min="0"
                                max="300"
                                required
                            />
                        </Col>
                        <Button  className="btn delete-btn"
                                 id="delete-button"
                                 onClick={() => {
                                     handleDelete(selectedItem.id);
                                 }}>
                            <MdDelete size={18}/>
                        </Button>
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
                    <Button className="btn menu-btn" onClick={handleUpdate}>
                        Wijzigingen opslaan
                    </Button>
                </Modal.Footer>
            </Modal>

            {/*Pop-up doelgewicht toevoegen*/}
            <Modal show={showDoelAdd} onHide={handleCloseDoelAdd}>
                <Modal.Header closeButton>
                    <Modal.Title>Nieuw doelgewicht toevoegen.</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col>
                            <input
                                type="number"
                                className="form-control"
                                placeholder="Voer je doelgewicht in."
                                value={DoelGewicht}
                                onChange={(e) => setDoelGewicht(e.target.value)}
                                min="0"
                                max="200"
                                required
                            />
                        </Col>
                    </Row>
                </Modal.Body>
                <Modal.Footer>
                    <Button
                        className={`animate__animated ${animateHinge ? "animate__hinge animate__slower" : ""}`}
                        onClick={handleAnnuleerClick}
                        onAnimationEnd={handleAnimationEnd}
                    >
                        Annuleer
                    </Button>
                    <Button onClick={handleNotSameWeight} disabled={isUpdating}>Opslaan</Button>
                </Modal.Footer>
            </Modal>

            {/*Pop-up doelgewicht bewerken*/}
            <Modal show={showDoelEdit} onHide={handleCloseDoelEdit}>
                <Modal.Header closeButton>
                    <Modal.Title>Doelgewicht</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col>
                            <label>Huidig doel:</label>
                            <input
                                type="number"
                                className="form-control"
                                placeholder="Voeg je nieuwe gewicht in."
                                value={editDoelGewicht}
                                onChange={(e) => setEditDoelGewicht(e.target.value)}
                                min="0"
                                max="200"
                                required
                            />
                        </Col>
                        <Button  className="btn deletemargin-btn"
                                 id="delete-button"
                                 onClick={() => {handleDoelDelete(editDoelID);
                                 }}>
                            <MdDelete size={18}/>
                        </Button>
                    </Row>
                    <div className="menu-footer">
                        <Button
                            className={`btn menu-btn animate__animated ${animateHinge ? "animate__hinge animate__slower" : ""}`}
                            onClick={handleAnnuleerClick}
                            onAnimationEnd={handleAnimationEnd}
                        >
                            Annuleer
                        </Button>
                        <Button className="btn menu-btn" onClick={handleNotSameWeightEdit} disabled={isUpdating}>
                            Wijzigingen opslaan
                        </Button>
                    </div>
                </Modal.Body>
                <Modal.Footer className="d-flex justify-content-center">
                    <h3>Behaalde doelen</h3>
                    <Container className="custom2-table-container">
                        <Table striped bordered hover className="custom2-table">
                            <thead className="header-row">
                            <tr>
                                <th>Doelgewicht</th>
                                <th>Start datum</th>
                                <th>Datum gehaald</th>
                            </tr>
                            </thead>
                            <tbody>
                            {dataDoelGewicht.length > 0 ? (
                                dataDoelGewicht
                                    .filter((item) => item.datumbehaald)
                                    .slice()
                                    .sort((a, b) => b.id - a.id)
                                    .map((item, index) => (
                                        <tr key={index}>
                                            <td>{item.doelgewicht} kg</td>
                                            <td>{new Date(item.datum).toLocaleDateString("nl-NL")}</td> {/* Oplossing voor juiste datumweergave */}
                                            <td>{new Date(item.datumbehaald).toLocaleDateString("nl-NL")}</td>
                                        </tr>
                                    ))
                            ) : (
                                <tr>
                                    <td colSpan="3">Er is nog geen doel behaald, maar geef niet op!</td>
                                </tr>
                            )}
                            </tbody>
                        </Table>
                    </Container>
                </Modal.Footer>
            </Modal>
        </Fragment>
    );
};

export default Gewicht;