import { useState, useEffect, Fragment } from "react";
import { useNavigate } from "react-router-dom";
import "./Gewicht.css";
import axios from "axios";
import { Navbar, Container, Nav, Button, Col, Modal, Table } from "react-bootstrap";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import ReactApexChart from "react-apexcharts";
import { FaPen } from "react-icons/fa";
import { IoIosAddCircle } from "react-icons/io";

const Gewicht = () => {
    
    const [data, setData] = useState([]);
    const [dataDoelGewicht, setDataDoelGewicht] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        getDataDoelGewicht();
        getData();
    }, []);

    const getData = () => {
        axios.get(`https://localhost:7209/api/Gewicht/gewicht`)
            .then((result) => {
                console.log("Data responds: ", result.data);
                setData(result.data);
            })
            .catch((error) => {
                console.error("Error met ophalen van gewicht:", error);
                toast.error("Gewicht ophalen mislukt!");
            });
    };

    const getDataDoelGewicht = () => {
        axios.get("https://localhost:7209/api/Gewicht/doelgewicht")
            .then((result) => {
                console.log("DataDoelGewicht responds: ", result.data);
                setDataDoelGewicht(result.data);
            })
            .catch((error) => {
                console.error("Error met ophalen van doelgewicht:", error);
                toast.error("Doelgewicht ophalen mislukt!");
            });
    };
    
    const chartData = {
        series: [
            {
                name: "Gewicht",
                data: data.map(item => ({
                    x: new Date(item.datum).toISOString().split("T")[0],
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
                yaxis: [
                    {
                        y: dataDoelGewicht.length > 0 ? dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht : null,
                        borderColor: '#33cc66',
                        strokeDashArray: 0,
                        strokeWidth: 4,
                        label: {
                            borderColor: '#33cc66',
                            style: {
                                color: '#fff',
                                background: '#33cc66'
                            },
                            text: 'Doelgewicht'
                        }
                    }
                ]
            }
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
            <Container fluid>
                <div className="gewichttoevoegen">
                    <Button className="btn gewichttoevoegen-btn" >
                        Gewicht toevoegen <IoIosAddCircle />
                    </Button>
                    <Button className="btn doelgewichttoevoegen-btn" >
                        Doelgewicht toevoegen <IoIosAddCircle />
                    </Button>
                </div>
            </Container>
            <br />

            <Container fluid>
                <br />
                <div>
                    <h3>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Doelgewicht: {dataDoelGewicht.length > 0
                        ? `${dataDoelGewicht[dataDoelGewicht.length - 1].doelgewicht} kg`
                        : "Er is nog geen doelgewicht ingesteld"}
                    </h3>
                </div>
                <ReactApexChart options={chartData.options} series={chartData.series} type="area" height={350} />
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
                                <td>{item.gewicht } kg</td>
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