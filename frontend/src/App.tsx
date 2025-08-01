import './App.css'
import Login from './components/Login'
import Register from './components/Register'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Home from './components/Home'
// import ProtectRoutes from './components/ProtectRoutes'
function App() {

  return (
    <>    
    <Router>
      <Routes>
        {/* <Route element={<ProtectRoutes/>}> */}
        <Route path="/home" element={<Home />} />
        {/* </Route> */}
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </Router>
    </>
  )
}

export default App
