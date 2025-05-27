import {
    BrowserRouter as Router,
    Route,
    Routes,
    Navigate,
} from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import Gewicht from "./Gewicht";
import Login from "./Login";
import Register from "./Register";
import Profile from "./Profile";


function App() {
    const isAuthenticated = () => {
        return localStorage.getItem("token") !== null;
    };
    
    return (
  <Router>
      <div className="App" >
          <Routes>
              {/* Standaard route */}
              <Route path="/" element={<Navigate to="/login" />} />

              {/* Route voor inloggen */}
              <Route path="/login" element={<Login />} />

              {/* Route voor registreren */}
              <Route path="/register" element={<Register />} />

              {/* Route voor gewicht */}
              <Route
                  path="/gewicht"
                  element={isAuthenticated() ? <Gewicht /> : <Navigate to="/login" />}
              />

              {/* Route voor Profile */}
              <Route
                  path="/profile"
                  element={isAuthenticated() ? <Profile /> : <Navigate to="/login" />}
              />
          </Routes>
      </div>
  </Router>
    );
}

export default App
