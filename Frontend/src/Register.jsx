import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./Register.css";

const Register = () => {
    const apiUrl = import.meta.env.VITE_API_URL;
    
    const [userName, setUserName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            // 1. Registreer gebruiker
            await axios.post(`${apiUrl}/api/Account/register`, {
                userName,
                email,
                password,
                confirmPassword,
            });

            // 2. Login direct na registratie
            const loginResponse = await axios.post(`${apiUrl}/api/Account/login`, {
                userName,
                password,
            });

            const token = loginResponse.data.token;
            localStorage.setItem("token", token); // Optioneel: ook save userId/username als je dat terugkrijgt

            // 3. Maak profiel aan in jouw eigen UserModel
            await axios.post(
                `${apiUrl}/api/User`,
                {
                    userName,
                    profileImageUrl: null, // Of standaardwaarde
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            // 4. Redirect naar gewicht-pagina
            navigate("/Gewicht");
        } catch (error) {
            console.error("Fout bij registratie", error);
            setMessage("Registratie mislukt");
        }
    };

    
    return (
        <div className="register-background">
            <div className="register-container">
                <form className="register-form" onSubmit={handleRegister}>
                    <div className="register-header">
                        <h2>Register</h2>
                    </div>
                    <div className="form-group">
                        <label htmlFor="username">Username</label>
                        <input
                            type="text"
                            id="username"
                            placeholder="Username"
                            value={userName}
                            onChange={(e) => setUserName(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="email">Email</label>
                        <input
                            type="email"
                            id="email"
                            placeholder="Email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">Password</label>
                        <input
                            type="password"
                            id="password"
                            placeholder="Password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>
                    <div style={{ textAlign: 'left' }}>
                        <ul>
                            <li>At least one uppercase letter</li>
                            <li>At least one lowercase letter</li>
                            <li>At least one numeral (0-9)</li>
                            <li>At least one symbol (!@#^*_?{ }-)</li>
                            <li>Minimum 6 characters</li>
                        </ul>
                    </div>
                    <div className="form-group">
                        <label htmlFor="confirmPassword">Confirm Password</label>
                        <input
                            type="password"
                            id="confirmPassword"
                            placeholder="Confirm Password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit" id="register-button" className="btn register-btn btn-primary">
                        Register
                    </button>
                    {message && <p className="message">{message}</p>}
                </form>
            </div>
        </div>
    );
};

export default Register;
