import { useState } from "react";
import axios from "axios";
import { useNavigate, Link } from "react-router-dom";
import "./Login.css";

const Login = () => {
    const apiUrl = import.meta.env.VITE_API_URL;
    
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post(
                `${apiUrl}/api/Account/login`,
                {
                    userName,
                    password,
                },
            );

            console.log("Login Response:", response);

            localStorage.setItem("token", response.data.token);
            console.log("Token:", response.data.token)
            setMessage("Login successful");
            navigate("/Gewicht");
        } catch (error) {
            console.error("Error details:", error.response);
            setMessage("Username or password is incorrect");
        }
    };

    return (
        <div className="login-background">
            <div className="login-container">
                <form className="login-form" onSubmit={handleLogin}>
                    <div className="login-header">
                        <h2>Login</h2>
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
                    <button type="submit" id="login-button" className="btn login-btn btn-primary">
                        Login
                    </button>
                    {message && <p className="message">{message}</p>}
                    <p>
                        Do not have an account? <Link to="/register">Register here</Link>
                    </p>
                </form>
            </div>
        </div>
    );
};

export default Login;
