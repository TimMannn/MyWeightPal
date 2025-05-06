import {
    BrowserRouter as Router,
    Route,
    Routes,
    Navigate,
} from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import Gewicht from "./Gewicht";


function App() {
    return (
  <Router>
      <div className="App" >
          <Routes>
              {/* Standaard route */}
              <Route path="/" element={<Navigate to="/gewicht" />} />

              {/* Route voor gewicht */}
              <Route path="/gewicht" element={<Gewicht />} />
          </Routes>
      </div>
  </Router>
    );
}

export default App
